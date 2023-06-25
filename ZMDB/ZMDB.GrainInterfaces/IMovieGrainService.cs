using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.GrainInterfaces
{
    public interface IMovieGrainService
    {
        Task<Movie> GetMovie(long id);
        Task SetMovie(long id, Movie movie);
    }
}
