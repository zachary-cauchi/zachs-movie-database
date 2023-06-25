using Serilog;
using System.Net.Sockets;
using ZMDB.BackHost.Extensions;
using ZMDB.BackHost.Configurations;
using ZMDB.MovieServer;
using ZMDB.Core.Configuration;
using ZMDB.Core.Extensions;
using ZMDB.Core.GrainFilters;
using Microsoft.Extensions.Logging.Configuration;
using ZMDB.Core.StartupTasks;
using ZMDB.MovieContracts;

namespace ZMDB.BackHost
{
    public class Program
    {
        public static Task Main(String[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("app-info.json");
            builder.Services.Configure<PreloadMoviesOptions>(builder.Configuration.GetSection(PreloadMoviesOptions.PreloadMovies));
            List<SiloPersistenceOptions> siloPersistenceOptions = new ();
            builder.Configuration.GetSection("SiloConfiguration:Persistences").Bind(siloPersistenceOptions);

            // Load all information about the app ahead of any services or post-configuration that may need it.
            IAppInfo appInfo = new AppInfo(builder.Configuration);
            Console.Title = $"{appInfo.Name} - {appInfo.Environment}";
            builder.Services.AddSingleton<IAppInfo>(appInfo);
            
            builder.Host.UseSerilog((ctx, loggerConfig) =>
            {
                loggerConfig.Enrich.FromLogContext()
                    .ReadFrom.Configuration(ctx.Configuration)
                    .Enrich.WithMachineName()
                    .Enrich.WithDemystifiedStackTraces()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext:l}] {Message:lj}{NewLine}{Exception}");

                loggerConfig.WithAppInfo(appInfo);
            });

            builder.Host.UseOrleans((ctx, siloBuilder) =>
            {
                siloBuilder
                .ConfigureLogging(loggerBuilder =>
                {
                    loggerBuilder
                        .AddConfiguration(ctx.Configuration)
                        .AddSerilog();
                })
                .InitZMDBSilos(new AppSiloBuilderContext()
                {
                    AppInfo = appInfo,
                    HostBuilderContext = ctx,
                    SiloOptions = new AppSiloOptions()
                    {
                        SiloPort = GetAvailablePort(11111, 12000),
                        GatewayPort = ctx.Configuration.GetValue("orleans:gatewayPort", 30001)
                    },
                    SiloPersistenceOptionsList = siloPersistenceOptions
                })
                .AddIncomingGrainCallFilter<LoggingIncomingCallFilter>()
                .AddOutgoingGrainCallFilter<LoggingOutgoingCallFilter>()
                .AddStartupTask<MovieGrainPreloaderStartupTask>()
                .UseDashboard(x => x.HostSelf = true);
            });

            //builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.ConfigureAuth0();
            builder.Services.AddControllersWithViews();

            builder.Services.UseMovieServices();

            // Configure the DBContexts
            builder.Services.AddMovieDbContexts(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Map("/dashboard", x => x.UseOrleansDashboard());

            return app.RunAsync();
        }

        private static int GetAvailablePort(int start, int end)
        {
            for (var port = start; port < end; ++port)
            {
                var listener = TcpListener.Create(port);
                listener.ExclusiveAddressUse = true;
                try
                {
                    listener.Start();
                    return port;
                }
                catch (SocketException)
                {
                }
                finally
                {
                    listener.Stop();
                }
            }

            throw new InvalidOperationException();
        }
    }
}