namespace MyMedia.RCL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
