namespace MyMedia.GestaoLoja.Entities
{
    public class CompanySettings
    {
        public int Id { get; set; }

        public decimal ProfitMargin { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
