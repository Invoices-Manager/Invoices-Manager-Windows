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
    public partial class SignInView : Page
    {
        public SignInView()
        {
            InitializeComponent();
        }

        private void Bttn_SignIn_Click(object sender, RoutedEventArgs e)
        {
            UserSystem us = new UserSystem();

            us.Login(Tb_Username.Text, Tb_Password.Password);
        }

        private void Bttn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            EnvironmentsVariable.MainWindowInstance.ViewMirror.Content = new SignUpView();
        }
    }
}
