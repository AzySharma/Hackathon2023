using Gamification.Model;

namespace Gamification.Repositories
{
    public interface IRewardRepository
    {
        public Task<Reward> AddReward(Reward reward);
        public Task DeleteReward(Reward reward);
        public Task<Reward> EditReward(Reward reward);
        public Task<List<Reward>> GetAllReward();
        public Task<Reward> GetRewardByCode(string code);
    }
}
