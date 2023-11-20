namespace Gamification.Model
{
    public class Tier
    {
        public int TeirId { get; set; }
        public string Name { get; set; }
        public int PointThreshold { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}
