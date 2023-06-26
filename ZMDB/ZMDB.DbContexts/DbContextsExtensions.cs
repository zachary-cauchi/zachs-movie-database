using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.MovieContracts.DbContexts;

namespace ZMDB.MovieContracts
{
    public static class DbContextsExtensions
    {
        public static IServiceCollection AddMovieDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            string connString = configuration.GetSection("MoviesStorage")["ConnectionString"];

            if (connString == null)
            {
                throw new ArgumentNullException("'MoviesStorage.ConnectionString' is null. Cannot create db context without connection string.");
            }

            services.AddDbContextPool<MovieDbContext>(options =>
                options.UseNpgsql(connString));

            return services;
        }
    }
}
