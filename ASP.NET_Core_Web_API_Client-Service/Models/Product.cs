using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_Web_API_Client_Service.Models
{
    public class Product
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Product()
        {
            ProductID = Guid.NewGuid().ToString();
        }
    }
}
