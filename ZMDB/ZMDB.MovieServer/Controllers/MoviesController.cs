﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.Grains.Grains;
using ZMDB.MovieContracts.Movie;
using ZMDB.MovieServer.Services;

namespace ZMDB.MovieServer.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly MoviesService _movieService;

        public MoviesController(MoviesService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id}")]
        public async Task<Movie> Get([FromRoute] int id)
        {
            return await _movieService.GetMovie(id);
        }

        [HttpPost("{id}")]
        public async Task Set([FromRoute] int id, [FromBody] Movie movie)
        {
            await _movieService.SetMovie(id, movie).ConfigureAwait(false);
        }
    }
}