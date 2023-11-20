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

        [HttpGet]
        [Route("id/{id}")]
        public async Task<Reward> GetById(string id)
        {
            return await _rewardRepository.GetRewardById(id);
        }

        [HttpGet]
        [Route("code/{code}")]
        public async Task<Reward> GetByCode(string code)
        {
            return await _rewardRepository.GetRewardByCode(code); 
        }

        [HttpPost]
        public async Task<List<Reward>> Post([FromBody] Reward reward)
        {
            await _rewardRepository.AddReward(reward);
            return await _rewardRepository.GetAllReward(); 
        }

        [HttpPut]
        public async Task<Reward> UpdateReward([FromBody] Reward reward)
        {
            return await _rewardRepository.EditReward(reward);
             
        }

        [HttpDelete("{id}")]
        public async Task DeleteReward(string id)
        {
            await _rewardRepository.DeleteReward(id);
        }
    }
}
