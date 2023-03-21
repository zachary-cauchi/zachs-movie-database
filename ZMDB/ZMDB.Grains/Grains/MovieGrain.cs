using Microsoft.AspNetCore.Connections.Features;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using ZMDB.GrainInterfaces;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.Grains.Grains
{
    public class MovieGrain : Grain, IMovieGrain
    {
        private readonly IPersistentState<Movie> _state;

        public MovieGrain(
            [PersistentState(
                stateName: "movie",
                storageName: "zmdb_movies_database")]
            IPersistentState<Movie> state
        )
        {
            _state = state;
        }

        public Task<Movie> GetMovie()
        {
            return Task.FromResult(_state.State);
        }

        public async Task SetMovie(Movie movie)
        {
            _state.State = movie;

            await _state.WriteStateAsync();
        }
    }
}