namespace Gamification.Model
{
    public class Reward
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PointRequired { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
