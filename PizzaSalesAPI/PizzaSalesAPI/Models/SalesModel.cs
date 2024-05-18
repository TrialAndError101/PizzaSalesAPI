using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaSalesAPI.Models
{
    public class SalesModel
    {
        public int order_details_id { get; set; } = 0;
        public int order_id { get; set; } = 0;
        public string pizza_id { get; set; } = string.Empty;
        public int quantity { get; set; } = 0;
    }
}
