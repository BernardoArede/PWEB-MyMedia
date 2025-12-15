using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMedia.RCL.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public List<OrderItemModel> Items { get; set; } = new();
    }

    public class OrderItemModel
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Product Product { get; set; } 
    
    }
}
