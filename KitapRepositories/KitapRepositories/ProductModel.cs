using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitapRepositories
{
    [Serializable]
    public class ProductModel
    {
        public int BookId { get; set; }
        public string BookGenre { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string CoverImage { get; set; }
        public int ProductStock { get; set; }
    }
}
