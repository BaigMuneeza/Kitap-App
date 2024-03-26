using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitapRepositories
{
    [Serializable]
    public class LoginModel
    {
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }
    }
}
