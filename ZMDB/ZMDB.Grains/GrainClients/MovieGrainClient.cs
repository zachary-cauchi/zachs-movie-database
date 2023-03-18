using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.GrainInterfaces;
using ZMDB.Grains.Grains;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.Grains.GrainClients
{
    public class MovieGrainClient : IMovieGrainClient
    {
        private readonly IGrainFactory _grainFactory;

        public MovieGrainClient (
            IGrainFactory grainFactory
            )
        {
            _grainFactory = grainFactory;
        }

        public Task<Movie> Get(int id)
        {
            var grain = _grainFactory.GetGrain<IMovieGrain>(id);

            return grain.GetMovie();
        }

        public Task Set(int id, Movie movie)
        {
            movie.Id = id;

            var grain = _grainFactory.GetGrain<IMovieGrain>(id);

            return grain.SetMovie(movie);
        }
    }
}
