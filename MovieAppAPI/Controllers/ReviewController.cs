using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Dtos;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;
using MovieAppAPI.Repositories;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        public ReviewController(IReviewRepository reviewRepository, IUserRepository userRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
        [HttpGet("movie/{movieId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovieReviews(int movieId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsFromMovie(movieId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(reviews);
        }

        [HttpGet("average/{movieId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAverageMovieRating(int movieId)
        {
            var rating = _reviewRepository.GetAverageRatingFromMovie(movieId);
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(rating);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserReviews(int userId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsFromUser(userId));
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult CreateReview([FromQuery] int userId, [FromQuery] int movieId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest();
            if (_reviewRepository.ReviewExists(userId, movieId))
                return BadRequest("User already reviewed this movie.");
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.User = _userRepository.GetUser(userId);
            reviewMap.Movie = _movieRepository.GetMovie(movieId);
            reviewMap.Date = DateTime.Now;
            if (!_reviewRepository.CreateReview(reviewMap))
            {
                return BadRequest("Something went wrong while saving.");
            }

            return Ok();
        }

        [HttpGet("author/{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetReviewAuthor(int reviewId)
        {
            var user = _reviewRepository.GetReviewAuthor(reviewId);
            if (user != null && !String.IsNullOrEmpty(user.ImageName))
                user.ImageSource = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/images/{user.ImageName}";
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(user);
        }

        [HttpGet("reviewed/{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult GetReviewedMovie(int reviewId)
        {
            var movie = _reviewRepository.GetReviewedMovie(reviewId);
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(movie);
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto reviewUpdate)
        {
            if (reviewUpdate == null)
                return BadRequest();
            if (reviewUpdate.ReviewId != reviewId)
                return NotFound();
            if (!_reviewRepository.ReviewExists(reviewId))
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var reviewMap = _mapper.Map<Review>(reviewUpdate);
            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                return BadRequest("Something went wrong updating review.");
            }
            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();
            var reviewToDelete = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                return BadRequest("Something went wrong while deleting review.");
            }
            return NoContent();

        }

    }
}
