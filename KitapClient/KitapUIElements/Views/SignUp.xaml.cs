using DevExpress.Xpf.Editors;
using KitapUIElements.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

    public partial class SignUp : Page
    {
        SignUpVM signUpObj = null;
        public SignUp()
        {
            InitializeComponent();
            signUpObj = new SignUpVM();
            this.DataContext = signUpObj;
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            signUpObj.CustomerData.Username = txtUser.Text;
        }

        private void passwordEdit_TextChanged(object sender, EditValueChangedEventArgs e)
        {
            signUpObj.CustomerData.Password = passwordEdit.Text;
        }
        private void txtFName_TextChanged(object sender, TextChangedEventArgs e)
        {
            signUpObj.CustomerData.FirstName = txtFName.Text;
        }

        private void txtLName_TextChanged(object sender, TextChangedEventArgs e)
        {
            signUpObj.CustomerData.LastName = txtLName.Text;
        }

        private void txtAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            signUpObj.CustomerData.Address = txtAddress.Text;
        }

        private void dxTextEdit_Validate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null) return;
            if (Regex.IsMatch(e.Value.ToString(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")) return;
            e.IsValid = false;
            e.ErrorContent = "Incorrect email format";
        }

        private void txtAddress_TextChanged(object sender, EditValueChangedEventArgs e)
        {
            signUpObj.CustomerData.Email = txtEmail.Text;
        }
    }
}
