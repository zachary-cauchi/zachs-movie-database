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
        Task<Movie> GetMovie(int id);
        Task SetMovie(int id, Movie movie);
    }
}
