using Microsoft.Extensions.DependencyInjection;
using ZMDB.MovieServer.Services;

namespace ZMDB.MovieServer
{
    public static class RegisterMovieServerExtensions
    {
        public static IServiceCollection UseMovieServices(this IServiceCollection services)
        {
            services.AddSingleton<MoviesService>();

            return services;
        }
    }
}
