using System.ComponentModel.DataAnnotations.Schema;

namespace MyMedia.GestaoLoja.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Auto-relacionamento para Subcategorias
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public Category? ParentCategory { get; set; }

        // Relacionamento inverso (opcional, mas útil)
        public ICollection<Category>? SubCategories { get; set; }
    }
}
