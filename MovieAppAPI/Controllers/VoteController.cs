using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using MovieAppAPI.Dtos;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.Repositories;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : Controller
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public VoteController(IVoteRepository voteRepository, IReviewRepository reviewRepository, IUserRepository userRepository, IMapper mapper)
        {
            _voteRepository = voteRepository;
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("{reviewId}/{userId}")]
        [ProducesResponseType(200, Type = typeof(VoteDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUserReviewVote(int userId, int reviewId)
        {
            var vote = _mapper.Map<VoteDto>(_voteRepository.GetVoteFromUserAndReview(userId, reviewId));
            return Ok(vote);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<VoteDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewVotes(int reviewId)
        {
            var votes = _mapper.Map<List<VoteDto>>(_voteRepository.GetReviewVotes(reviewId));
            return Ok(votes);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult CreateVote([FromQuery]int userId, [FromQuery]int reviewId, [FromBody]VoteDto voteCreate)
        {
            if(voteCreate== null)
                return BadRequest();
            if(_voteRepository.VoteExists(userId, reviewId))
                return BadRequest("User already voted this review.");
            var voteMap = _mapper.Map<Vote>(voteCreate);
            voteMap.User = _userRepository.GetUser(userId);
            voteMap.Review = _reviewRepository.GetReview(reviewId);
            if(!_voteRepository.CreateVote(voteMap))
            {
                return BadRequest("Something went wrong while saving vote.");
            }
            return NoContent();
        }

        [HttpPut("{voteId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        [Authorize]
        public IActionResult UpdateVote(int voteId, [FromBody]VoteDto voteUpdate)
        {
            if(voteUpdate == null)
                return BadRequest();
            if(voteUpdate.VoteId != voteId)
                return NotFound();
            if (!_voteRepository.VoteExists(voteId))
                return BadRequest();
            var voteMap = _mapper.Map<Vote>(voteUpdate);
            if(!_voteRepository.UpdateVote(voteMap))
            {
                return BadRequest("Something went wrong while saving.");
            }
            return NoContent();

        }
    }
}
