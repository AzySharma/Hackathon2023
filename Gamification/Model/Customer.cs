namespace Gamification.Model
{
    public class Customer
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long PhoneNumber { get; set; }
        public ICollection<Reward> Rewards { get; set; }

    }
}
