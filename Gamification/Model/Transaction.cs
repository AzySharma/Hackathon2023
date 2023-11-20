namespace Gamification.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public int PointEarned { get; set; }
        public Customer Customer { get; set; }
    }
}
