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
            services.AddDbContextPool<MovieDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MoviesStorage.ConnectionString")));

            return services;
        }
    }
}
