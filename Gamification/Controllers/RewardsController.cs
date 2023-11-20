using Gamification.Model;
using Gamification.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gamification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RewardsController : Controller
    {
        public List<Reward> Rewards { get; set; }
        private IRewardRepository _rewardRepository { get; set; }

        public RewardsController(IRewardRepository rewardRepository)
        {
            _rewardRepository = rewardRepository;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<List<Reward>> Get()
        {
            return await _rewardRepository.GetAllReward(); 
        }

        [HttpPost]
        public async Task<List<Reward>> Post([FromBody] Reward reward)
        {
            await _rewardRepository.AddReward(reward);
            return await _rewardRepository.GetAllReward(); 
        }
    }
}
