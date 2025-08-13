namespace ProductCrud.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string? ProductionDocuments { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
