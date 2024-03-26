using KitapRepositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    
    public partial class ManageCustomersView : UserControl
    {
        private ObservableCollection<CustomerModel> _pendingCustomers;
        public ObservableCollection<CustomerModel> PendingCustomers
        {
            get { return _pendingCustomers; }
            set { _pendingCustomers = value; }
        }
        public ObservableCollection<CustomerModel> Selection { get; } = new ObservableCollection<CustomerModel>();
        public ManageCustomersView()
        {

            DataContext = this;
            PendingCustomers = new ObservableCollection<CustomerModel>(GUIHandler.Instance.CacheManager.GetCachedData<List<CustomerModel>>("Pending"));
            InitializeComponent();
            
        }

        private void ApproveRequest(object sender, RoutedEventArgs e)
        {
            IList selectedRows = Selection.ToList();
            CustomerModel selectedRow = (CustomerModel)selectedRows[0];
            GUIHandler.Instance.ClientManager.SendObjectToServer(selectedRow);
            view.DeleteRow(view.FocusedRowHandle);
            MessageBox.Show("Request Approved");
        }
    }
}
