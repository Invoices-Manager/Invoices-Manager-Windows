using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            us.Create(userData);

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
