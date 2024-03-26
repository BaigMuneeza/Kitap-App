using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitapRepositories
{
    [Serializable]
    public class OrderModel
    {
        public int OrderId { get; set; }
        public ObservableCollection<CartElementModel> cart { get; set; }
        public string Username { get; set; }
        public string CustomerAddress { get; set; }

    }
}
