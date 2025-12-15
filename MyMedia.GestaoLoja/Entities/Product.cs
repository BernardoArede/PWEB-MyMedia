using MyMedia.GestaoLoja.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.GestaoLoja.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Preços (Regra de negócio: Base + Margem = Final)
        [Column(TypeName = "decimal(10, 2)")]
        public decimal BasePrice { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; } // Preço Final calculado

        public int Stock { get; set; }

        // Imagem do Produto
        public byte[]? Image { get; set; } // Guardado na BD como varbinary(MAX)
        public string? ImageMimeType { get; set; } // Ex: "image/jpeg"

        public bool IsActive { get; set; } // Para ativar/desativar visibilidade

        // Chaves Estrangeiras
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int AvailabilityModeId { get; set; }
        public AvailabilityMode? AvailabilityMode { get; set; }

        // Relacionamento com o Fornecedor (User)
        public string? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public ApplicationUser? Supplier { get; set; }
    }
}
