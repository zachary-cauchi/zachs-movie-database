using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.GrainInterfaces;
using ZMDB.MovieContracts.DbContexts;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.MovieServer.Services
{
    public class MoviesService
    {
        private readonly IMovieGrainClient _movieGrainClient;

        public MoviesService(
            IMovieGrainClient movieGrainClient
            )
        {
            _movieGrainClient = movieGrainClient;
        }

        public async Task<Movie> GetMovie(long id)
        {
            var result = await _movieGrainClient.Get(id).ConfigureAwait(false);

            return result;
        }

        public async Task SetMovie(long id, Movie movie)
        {
            await _movieGrainClient.Set(id, movie).ConfigureAwait(false);
        }
    }
}
