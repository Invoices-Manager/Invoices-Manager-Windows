namespace InvoicesManager.Views
{
    public partial class SignInView : Page
    {
        public SignInView()
        {
            InitializeComponent();
        }

        private void Bttn_SignIn_Click(object sender, RoutedEventArgs e)
        {
            UserWebSystem us = new UserWebSystem();

            us.Login(Tb_Username.Text, Tb_Password.Password);
        }

        private void Bttn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            EnvironmentsVariable.MainWindowInstance.ViewMirror.Content = new SignUpView();
        }
    }
}
