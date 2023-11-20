using Gamification.Model;

namespace Gamification.Repositories
{
    public interface IRewardRepository
    {
        public Task<Reward> AddReward(Reward reward);
        public Task DeleteReward(string id);
        public Task<Reward> EditReward(Reward reward);
        public Task<List<Reward>> GetAllReward();
        public Task<Reward> GetRewardByCode(string code);
        public Task<Reward> GetRewardById(string id);
    }
}
