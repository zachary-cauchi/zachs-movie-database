using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<Movie> Get([FromRoute] long id)
        {
            return await _movieService.GetMovie(id);
        }

        [HttpPost("{id}")]
        public async Task Set([FromRoute] long id, [FromBody] Movie movie)
        {
            await _movieService.SetMovie(id, movie).ConfigureAwait(false);
        }
    }
}
