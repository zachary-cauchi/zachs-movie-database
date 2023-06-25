using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.MovieContracts.DbContexts
{
    public class MovieDbContext : DbContext
    {
        public DbSet<ZMDB.MovieContracts.Movie.Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
        {
        }
    }
}
