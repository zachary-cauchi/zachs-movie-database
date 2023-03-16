using Serilog;
using System.Net.Sockets;
using ZMDB.BackHost.Utils;
using ZMDB.BackHost.Extensions;
using ZMDB.GrainFilters;
using ZMDB.BackHost.Configurations;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace ZMDB.BackHost
{
    public class Program
    {
        public static Task Main(String[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            
            // Load all information about the app ahead of any services or post-configuration that may need it.
            IAppInfo appInfo = new AppInfo(builder.Configuration);
            Console.Title = $"{appInfo.Name} - {appInfo.Environment}";
            builder.Services.AddSingleton<IAppInfo>(appInfo);

            //builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.ConfigureAuth0();
            builder.Services.AddControllersWithViews();

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
                siloBuilder.InitZMDBSilos(new AppSiloBuilderContext()
                    {
                        AppInfo = appInfo,
                        HostBuilderContext = ctx,
                        SiloOptions = new AppSiloOptions()
                        {
                            SiloPort = GetAvailablePort(11111, 12000),
                            GatewayPort = ctx.Configuration.GetValue("orleans:gatewayPort", 30001)
                        }
                    })
                .AddIncomingGrainCallFilter<LoggingIncomingCallFilter>()
                .UseDashboard(x => x.HostSelf = true);
            });

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