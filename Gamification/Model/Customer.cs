namespace Gamification.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public long PhoneNumber { get; set; }
        public ICollection<Reward> Rewards { get; set; }

    }
}
