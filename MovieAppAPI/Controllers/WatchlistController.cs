using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MovieAppAPI.Dtos;
using MovieAppAPI.Interfaces;
using MovieAppAPI.Models;

namespace MovieAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : Controller
    {
        private readonly IWatchlistRepository _watchlistController;
        private readonly IMapper _mapper;
        public WatchlistController(IWatchlistRepository watchlistController, IMapper mapper)
        {
            _watchlistController = watchlistController;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserWatchlist(int userId)
        {
            var watchlist = _mapper.Map<List<MovieDto>>(_watchlistController.GetUserWatchlist(userId));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(watchlist);
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddWatchlistItem(WatchlistDto itemCreate)
        {
            if (itemCreate == null)
                return BadRequest();
            if (_watchlistController.WatchlistItemExists(itemCreate.UserId, itemCreate.MovieId))
                return BadRequest("User already has this movie in their watchlist.");
            if (!ModelState.IsValid)
                return BadRequest();
            var item = _mapper.Map<Watchlist>(itemCreate);
            if (!_watchlistController.AddWatchlistItem(item))
                return BadRequest("Something went wrong while saving item.");
            return Ok();
        }

        [HttpDelete("{userId}/{movieId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteWatchlistItem(int userId, int movieId)
        {
            if (!_watchlistController.WatchlistItemExists(userId, movieId))
                return NotFound();
            var item = _watchlistController.GetWatchlistItem(userId, movieId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_watchlistController.DeleteWatchlistItem(item))
                return BadRequest("Something went wrong while deleting item.");
            return NoContent();

        }

    }
}
