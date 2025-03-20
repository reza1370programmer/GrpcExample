namespace GrpcExample.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public string ColorSet { get; set; } = string.Empty; //comma seprator string
        public DateTime CreatedAt { get; set; }
    }
}
