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
        Task<Movie> Get(int id);
        Task Set(int id, Movie movie);
    }
}
