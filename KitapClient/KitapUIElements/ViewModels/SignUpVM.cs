using KitapRepositories;
using KitapUIElements.Utilities;
using KitapUIElements.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using KitapClientMananger;
using KitapCache;

namespace KitapUIElements.ViewModels
{
    public class SignUpVM : ViewModelBase
    {
        private CustomerModel _customer;
        public CustomerModel CustomerData
        {
            get { return _customer; }
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(CustomerData));
            }
        }

        public ICommand SignUpCommand { get; }
        public SignUpVM()
        {
            _customer = new CustomerModel();
            SignUpCommand = new RelayCommand(ExecuteSignUpCommand, CanExecuteSignUpCommand);
        }


        private bool CanExecuteSignUpCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(CustomerData.Username) || string.IsNullOrWhiteSpace(CustomerData.FirstName) || string.IsNullOrWhiteSpace(CustomerData.LastName) || string.IsNullOrWhiteSpace(CustomerData.Email) || CustomerData.Address == null)
            {
               validData = false;
            }

            else
            {
                validData = true;
                CustomerData.UserType = 1;
            }

            return validData;
        }
        private void ExecuteSignUpCommand(object obj)
        {
            string key = "CustomerModel";

            if (GUIHandler.Instance.CacheManager != null)
            {
                List<CustomerModel> loginData = GUIHandler.Instance.CacheManager.GetCachedData<List<CustomerModel>>(key);

                if (loginData != null)
                {
                    foreach (CustomerModel customerModel in loginData)
                    {
                        if (customerModel.Username == CustomerData.Username)
                        {
                            MessageBox.Show("Username already Taken. Try a new one");
                            return;
                        }
                    }
                    GUIHandler.Instance.ClientManager.SendObjectToServer(CustomerData);                 
                    GUIHandler.Instance.SetView<LoginView>(Application.Current.MainWindow);
                    MessageBox.Show(" Account has been sent for approval");
                }
                else
                {
                    MessageBox.Show("Can't access data.");
                }
            }
            else
            {
                MessageBox.Show("Unexpected error");
            }
        }
    }
}
