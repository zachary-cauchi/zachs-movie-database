using Orleans;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.GrainInterfaces
{
    public interface IMovieGrain : IGrainWithIntegerKey
    {
        Task SetMovie(Movie movie);
        Task<Movie> GetMovie();
    }
}