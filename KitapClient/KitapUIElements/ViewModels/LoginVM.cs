using System;
using KitapUIElements.Views;
using KitapUIElements.Utilities;
using System.Collections.Generic;
using KitapRepositories;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using KitapCache;
using System.Windows.Markup;

namespace KitapUIElements.ViewModels
{
    public class LoginVM : ViewModelBase
    {
        private LoginModel _customer;
        private Dictionary<string, object> CacheData;
        private bool loggedIn;
        private bool isCustomer;

        public LoginModel Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
            }
        }

        public ICommand LoginCommand { get; }
        public LoginVM()
        {
            _customer = new LoginModel();
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }


        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Customer.LoginUsername) || string.IsNullOrWhiteSpace(Customer.LoginPassword))
            {
                validData = false;
            }

            else
            {
                validData = true;
            }

            return validData;
        }
        private void ExecuteLoginCommand(object obj)
        {
            string key = "CustomerModel";
            string keyPending = "Pending";

            if (GUIHandler.Instance.CacheManager == null)
            {
                MessageBox.Show("Unexpected error");
                return;
            }

            List<CustomerModel> loginData = GUIHandler.Instance.CacheManager.GetCachedData<List<CustomerModel>>(key);
            List<CustomerModel> PendingData = GUIHandler.Instance.CacheManager.GetCachedData<List<CustomerModel>>(keyPending);

            if (PendingData != null)
            {
                foreach (CustomerModel customerModel in PendingData)
                {

                    if (customerModel.Username == Customer.LoginUsername && customerModel.Password == Customer.LoginPassword)
                    {
                        MessageBox.Show("Waiting for Admin approval");
                        return;
                    }
                }
            }

            if (loginData == null)
            {
                MessageBox.Show("There is a problem! Try again later."); 
                return;
            }

            foreach (CustomerModel customerModel in loginData)
            {
                if (customerModel.Username == Customer.LoginUsername && customerModel.Password == Customer.LoginPassword) 
                {
                    loggedIn = true;

                    if (customerModel.UserType == 1)
                    {
                        isCustomer = true;
                        break;
                    }

                    else
                    {
                        isCustomer = false;
                        break;
                    }
                }
            }

            if (loggedIn)
            {
                try
                {
                    GUIHandler.Instance.ClientManager.SendAckToServer("LoggedIn");
                    MessageBox.Show("Login successful!");
                    while (loggedIn)
                    {
                        if (GUIHandler.Instance.CacheManager.HasProductData)
                        {
                            if (isCustomer)
                            {
                                MainAppWindow MainAppWindow = GUIHandler.Instance.CreateNewWindow<MainAppWindow>();
                                Application.Current.MainWindow.Close();
                                MainAppWindow.Show();
                                return;
                            }

                            else
                            {     
                                AdminDashboard AdminPanel = GUIHandler.Instance.CreateNewWindow<AdminDashboard>();
                                Application.Current.MainWindow.Close();
                                AdminPanel.Show();
                                return;
                            }

                        }
                    }
                }

                catch (Exception e) { Console.WriteLine(e); }
            }
            else
                MessageBox.Show("Invalid username or password, Try again or Sign up!");
        }
    }
}