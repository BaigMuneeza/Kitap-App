using DevExpress.Xpf.Editors;
using KitapUIElements.ViewModels;
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

namespace KitapUIElements.Views
{

    public partial class LoginView : Page
    {
        LoginVM loginObj = null;
        public LoginView()
        {
            InitializeComponent();
            loginObj = new LoginVM();
            this.DataContext = loginObj;
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            loginObj.Customer.LoginUsername = txtUser.Text;
        }

        private void passwordEdit_TextChanged(object sender, EditValueChangedEventArgs e)
        {
            loginObj.Customer.LoginPassword = passwordEdit.Text;
        }

        private void btnGoToSignUp_Click(object sender, RoutedEventArgs e)
        {
            SignUp signUpVw = new SignUp();
            Application.Current.MainWindow.Content = signUpVw;
        }


    }
}
