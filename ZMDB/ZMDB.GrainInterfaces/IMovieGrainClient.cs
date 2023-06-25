using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.GrainInterfaces
{
    public interface IMovieGrainClient
    {
        Task<Movie> Get(long id);
        Task Set(long id, Movie movie);
    }
}
