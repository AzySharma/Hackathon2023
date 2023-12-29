namespace Gamification.Model
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaybleAmount { get; set; }
        public decimal Discount { get; set; }
        public string RewardCode { get; set; }
    }
}
