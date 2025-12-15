using MyMedia.GestaoLoja.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.GestaoLoja.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total { get; set; }

        public string Status { get; set; } = "Pendente"; 

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // Itens da encomenda
        public List<OrderItem>? Items { get; set; }
    }
}
