using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using ZMDB.GrainInterfaces;
using ZMDB.Grains.GrainClients;

namespace ZMDB.Grains
{
    public static class AddGrainClientsExtentions
    {
        public static ISiloBuilder AddMovieGrainClients(this ISiloBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IMovieGrainClient, MovieGrainClient>();
            });

            return builder;
        }
    }
}
