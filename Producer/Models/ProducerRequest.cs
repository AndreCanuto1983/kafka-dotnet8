namespace Producer.Models
{
    public class ProducerRequest
    {
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Value { get; set; }
    }
}