﻿using InvoicesManager.Core.Sort;

namespace InvoicesManager.Views
{
    public partial class LogView : Page
    {
        public LogView()
            => InitializeComponent();

        private List<LogModel> allDataGridLogs = new List<LogModel>();
        private DateTime filterDate = default;
        private LogStateEnum filterLogState = LogStateEnum.FilterPlaceholder;
        private LogPrefixEnum filterLogPrefix = LogPrefixEnum.FilterPlaceholder;
        private string filterMessage = string.Empty;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { MessageBox.Show(Application.Current.Resources["loadTimeMessage"] as string, Application.Current.Resources["loadTimeTitle"] as string, MessageBoxButton.OK, MessageBoxImage.Information); });
            
            //update the Comb_Logs
            UpdateCombLogs();
            //set default of "todays logs"
            Comb_Logs.SelectedIndex = 1;

            //load the state combobox
            LoadStateComb();

            //load the prefix combobox
            LoadPrefixComb();
        }

        private void LoadPrefixComb()
        {
            Comb_Search_Prefix.Items.Clear();

            foreach (LogPrefixEnum lpe in Enum.GetValues(typeof(LogPrefixEnum)))
                if (lpe != LogPrefixEnum.FilterPlaceholder)
                    Comb_Search_Prefix.Items.Add(lpe);
        }
        
        private void LoadStateComb()
        {
            Comb_Search_State.Items.Clear();

            foreach (LogStateEnum lse in Enum.GetValues(typeof(LogStateEnum)))
                if (lse != LogStateEnum.FilterPlaceholder)
                    Comb_Search_State.Items.Add(lse);
        }

        private void Bttn_BoardRefresh_Click(object sender, RoutedEventArgs e)
            => RefreshBoard();

        private void Bttn_OpenLogFolder_Click(object sender, RoutedEventArgs e)
        {
            //open the folder where the logs are stored with explorer
            Process.Start("explorer.exe", EnvironmentsVariable.PathLogs);
        }

        private void Bttn_DeleteAllLogs_Click(object sender, RoutedEventArgs e)
        {
            //delete all files in the log folder and refresh the board (but ask for confirmation first)

            //leave if the user doesn't want to delete all logs
            if (MessageBox.Show(Application.Current.Resources["deleteAllLogsMsg"] as string, Application.Current.Resources["deleteAllLogsTitle"] as string, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            LoggerSystem.DeleteAllLogs();
            RefreshBoard();
        }


        private void Comb_Logs_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => RefreshBoard();
        
        private void Dp_Search_Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            filterDate = Dp_Search_Date.SelectedDate ?? default;
            Sort();
        }

        private void Comb_Search_State_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Comb_Search_State.SelectedIndex == -1)
                filterLogState = LogStateEnum.FilterPlaceholder;
            else
                filterLogState = (LogStateEnum)Comb_Search_State.SelectedItem;
            
            Sort();
        }

        private void Comb_Search_Prefix_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Comb_Search_Prefix.SelectedIndex == -1)
                filterLogPrefix = LogPrefixEnum.FilterPlaceholder;
            else
                filterLogPrefix = (LogPrefixEnum)Comb_Search_Prefix.SelectedItem;
            
            Sort();
        }
        
        private void Tb_Search_Message_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterMessage = Tb_Search_Message.Text ?? String.Empty;
            Sort();
        }


        private void Bttn_Search_Date_Clear_Click(object sender, RoutedEventArgs e)
            => Dp_Search_Date.SelectedDate = default;

        private void Bttn_Search_State_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_State.SelectedIndex = -1;

        private void Bttn_Search_Prefix_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_Prefix.SelectedIndex = -1;

        private void Bttn_Search_Message_Clear_Click(object sender, RoutedEventArgs e)
            => Tb_Search_Message.Text = String.Empty;
        

        private void Dg_Logs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //happens sometimes, therefore this catch
            if (Dg_Logs.SelectedItem == null)
                return;

            var cellInfo = Dg_Logs.CurrentCell;
            var column = cellInfo.Column as DataGridBoundColumn;

            var element = new FrameworkElement() { DataContext = cellInfo.Item };
            BindingOperations.SetBinding(element, TagProperty, column.Binding);
            var cellValue = element.Tag;
            Clipboard.SetText(cellValue.ToString());
        }

        private void RefreshBoard()
        {
            //TODO: IMPROVE Loading time
            
            int index = Comb_Logs.SelectedIndex;
            string combText = Comb_Logs.SelectedItem.ToString() ?? "";

            Task.Run(() => 
            {
                List<LogModel> logs = new List<LogModel>();

                //check which kind of logs the user wants to see

                switch (index)
                {
                    //has noting selected
                    case -1:
                        break;

                    //wants to see all logs
                    case 0:
                        logs = LoggerSystem.GetAllLogs();
                        break;

                    //wants to see only the today logs
                    case 1:
                        logs = LoggerSystem.GetAllLogs(onlyToday: true);
                        break;

                    default:
                        logs = LoggerSystem.GetLogs(combText);
                        break;
                }

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                    => { Dg_Logs.ItemsSource = logs; }));

                //will be needed later for the sort
                allDataGridLogs = logs;

                //update the Comb_Logs
                UpdateCombLogs();
            });
        }

        private void UpdateCombLogs()
        {
            Task.Run(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                        => { Comb_Logs.ItemsSource = LoggerSystem.GetLogsChoices(); }));
            });
        }

        private void Sort()
        {
            Task.Run(() =>
            {
                LogsSortSystem _lss = new LogsSortSystem(allDataGridLogs, filterDate, filterLogState, filterLogPrefix, filterMessage);
                _lss.Sort();

                Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(()
                    => { Dg_Logs.ItemsSource = EnvironmentsVariable.FilteredLogs; }));

                RefreshBoard();
            });
        }
    }
}
