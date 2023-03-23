using InvoicesManager.Classes;

namespace InvoicesManager.Views
{
    public partial class LogView : Page
    {
        public LogView()
            => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshBoard();
        }


        private void Bttn_BoardRefresh_Click(object sender, RoutedEventArgs e)
            => RefreshBoard();

        private void Bttn_OpenLogFolder_Click(object sender, RoutedEventArgs e)
        {
            //open the folder where the logs are stored with explorer
            Process.Start("explorer.exe", EnvironmentsVariable.PathLog);
        }

        private void Bttn_DeleteAllLogs_Click(object sender, RoutedEventArgs e)
        {
            //delete all files in the log folder and refresh the board (but ask for confirmation first)

            //leave if the user doesn't want to delete all logs
            if (MessageBox.Show("Are you sure you want to delete all logs?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            LoggerSystem.DeleteAllLogs();
            RefreshBoard();
        }


        private void Comb_Logs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void Dp_Search_Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Comb_Search_State_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Comb_Search_Prefix_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void Tb_Search_Message_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void Bttn_Search_Date_Clear_Click(object sender, RoutedEventArgs e)
            => Dp_Search_Date.SelectedDate = default;

        private void Bttn_Search_State_Clear_Click(object sender, RoutedEventArgs e)
            => Comb_Search_Prefix.SelectedIndex = -1;

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
            List<LogModel> logs = LoggerSystem.GetAllLogs();

            Dg_Logs.ItemsSource = logs;
        }
    }
}
