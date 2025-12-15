namespace MyMedia.RCL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public byte[]? Image { get; set; }
        public string? ImageMimeType { get; set; }
        public bool IsActive { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int AvailabilityModeId { get; set; }
        public AvailabilityMode? AvailabilityMode { get; set; }

    }
}
