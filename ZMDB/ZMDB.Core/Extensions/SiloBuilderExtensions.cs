using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Diagnostics;
using System.Net;
using ZMDB.Core.Configuration;
using ZMDB.GrainInterfaces;
using ZMDB.Grains;
using ZMDB.Grains.GrainClients;

namespace ZMDB.Core.Extensions
{
    public enum StorageProviderType
    {
        Memory,
        SqlDb
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class AppSiloOptions
    {
        private string DebuggerDisplay => $"GatewayPort: '{GatewayPort}', SiloPort: '{SiloPort}'";

        public int GatewayPort { get; set; } = 30000;
        public int SiloPort { get; set; } = 11111;
        public StorageProviderType? StorageProviderType { get; set; }
    }

    public class AppSiloBuilderContext
    {
        public HostBuilderContext HostBuilderContext { get; set; }
        public IAppInfo AppInfo { get; set; }
        public AppSiloOptions SiloOptions { get; set; }
    }

    public static class SiloBuilderExtensions
    {
        private static StorageProviderType _defaultProviderType;

        public static ISiloBuilder InitZMDBSilos(this ISiloBuilder siloBuilder, AppSiloBuilderContext context)
        {
            siloBuilder
                .UseAppConfiguration(context)
                .UseStorage("moviesDatabase", context.AppInfo, context.HostBuilderContext, StorageProviderType.Memory, "movies")
                .AddMovieGrainClients();
            return siloBuilder;
        }

        private static ISiloBuilder UseAppConfiguration(this ISiloBuilder siloHost, AppSiloBuilderContext context)
        {
            _defaultProviderType = context.SiloOptions.StorageProviderType ?? StorageProviderType.Memory;

            var appInfo = context.AppInfo;
            siloHost
                .AddMemoryGrainStorageAsDefault()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = appInfo.ClusterId;
                    options.ServiceId = appInfo.Name;
                });

            siloHost.UseDevelopment(context);
            siloHost.UseDevelopmentClustering(context);

            return siloHost;
        }

        private static ISiloBuilder UseDevelopment(this ISiloBuilder siloHost, AppSiloBuilderContext context)
        {
            siloHost
                .ConfigureServices(services =>
                {
                    //services.Configure<GrainCollectionOptions>(options => { options.CollectionAge = TimeSpan.FromMinutes(1.5); });
                });

            return siloHost;
        }

        private static ISiloBuilder UseDevelopmentClustering(this ISiloBuilder siloHost, AppSiloBuilderContext context)
        {
            var siloAddress = IPAddress.Loopback;
            var siloPort = context.SiloOptions.SiloPort;
            var gatewayPort = context.SiloOptions.GatewayPort;

            return siloHost
                    .UseLocalhostClustering(siloPort: siloPort, gatewayPort: gatewayPort)
                ;
        }

        public static ISiloBuilder UseStorage(this ISiloBuilder siloBuilder, string storeProviderName, IAppInfo appInfo, HostBuilderContext context, StorageProviderType? storageProvider = null, string storeName = null)
        {
            storeName = string.IsNullOrWhiteSpace(storeName) ? storeProviderName : storeName;
            storageProvider ??= _defaultProviderType;

            switch (storageProvider)
            {
                case StorageProviderType.Memory:
                    siloBuilder.AddMemoryGrainStorage(storeProviderName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(storageProvider), $"Storage provider '{storageProvider}' is not supported.");
            }

            return siloBuilder;
        }
    }
}