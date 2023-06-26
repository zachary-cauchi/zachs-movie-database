using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ZMDB.Grains.Grains;
using ZMDB.MovieContracts.DbContexts;
using ZMDB.MovieContracts.Movie;
using ZMDB.MovieServer.Services;

namespace ZMDB.MovieServer.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly MoviesService _movieService;
        private readonly MovieDbContext _movieDbContext;

        public MoviesController(
            MoviesService movieService,
            MovieDbContext movieDbContext
            )
        {
            _movieService = movieService;
            _movieDbContext = movieDbContext;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Movie>> Get([FromRoute] long id)
        {
            Movie? movie = await _movieService.GetMovie(id);

            if (movie == null || !movie.IsInitialised)
            {
                movie = await _movieDbContext.FindAsync<Movie>(id);

                if (movie == null)
                {
                    return NotFound();
                }

                await _movieService.SetMovie(id, movie);
            }

            return Ok(movie);
        }

        [HttpPost("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Set([FromRoute] long id, Movie movie)
        {
            Movie foundMovie = await _movieService.GetMovie(id);

            // TODO: Add database check for existing movie.

            if (foundMovie.IsInitialised)
            {
                return BadRequest($"A movie with id {id} already exists. Cannot create a new movie.");
            }

            if (movie == null)
            {
                return BadRequest("Request body empty");
            }

            movie.IsInitialised = true;
            movie.CreatedAt = DateTime.UtcNow;
            movie.UpdatedAt = DateTime.UtcNow;

            await _movieDbContext.AddAsync<Movie>(movie);
            await _movieDbContext.SaveChangesAsync();

            await _movieService.SetMovie(id, movie);

            return CreatedAtAction(nameof(Get), new { id }, movie);
        }
    }
}
