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
        private MainWindow _mw;
        public SignUpView(MainWindow mw)
        {
            InitializeComponent();
            _mw = mw;
        }

        private void Bttn_SignIn_Click(object sender, RoutedEventArgs e)
            => _mw.ViewMirror.Content = new SignInView(_mw);

        private void Bttn_SignUp_Click(object sender, RoutedEventArgs e)
        {
            //check if the password is correct
            if (!Tb_Password01.Password.Equals(Tb_Password02.Password))
                return;

            UserSystem us = new UserSystem(_mw);
            us.Create(Tb_Username.Text, Tb_Password01.Password);
        }
    }
}
