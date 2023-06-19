namespace InvoicesManager.Views
{
    public partial class SignUpView : Page
    {
        public SignUpView()
        {
            InitializeComponent();
        }

        private void Bttn_SignIn_Click(object sender, RoutedEventArgs e)
            => EnvironmentsVariable.MainWindowInstance.ViewMirror.Content = new SignInView();

        private void Bttn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            //check if the data is vaild
            if (!CheckUserInput())
            {
                MessageBox.Show("Check your input");
                return;
            }

            dynamic userData = new
            {
                username = Tb_Username.Text,
                password = Tb_Password01.Password,
                firstName = Tb_FirstName.Text,
                lastName = Tb_LastName.Text,
                email = Tb_UserEmail.Text,
            };

            UserSystem us = new UserSystem();
            if (!us.Create(userData))
                return;
               
            us.Logout();
            
            EnvironmentsVariable.MainWindowInstance.ViewMirror.Content = new Page();
        }

        private bool CheckUserInput()
            => CheckUsername() &&  CheckPassword() && CheckFirstName() && CheckLastName() && CheckEmail();

        private bool CheckUsername()
            => !String.IsNullOrEmpty(Tb_Username.Text);
        private bool CheckFirstName()
            => !String.IsNullOrEmpty(Tb_FirstName.Text);
        private bool CheckLastName()
            => !String.IsNullOrEmpty(Tb_LastName.Text);
        private bool CheckEmail()
            => !String.IsNullOrEmpty(Tb_UserEmail.Text);
        private bool CheckPassword()
            => !String.IsNullOrEmpty(Tb_Password01.Password) && Tb_Password01.Password.Equals(Tb_Password02.Password);

    }
}
