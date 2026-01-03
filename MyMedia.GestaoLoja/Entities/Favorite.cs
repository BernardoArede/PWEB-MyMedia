using MyMedia.GestaoLoja.Data;
using MyMedia.GestaoLoja.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.GestaoLoja.Entities
{
    public class Favorite
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
