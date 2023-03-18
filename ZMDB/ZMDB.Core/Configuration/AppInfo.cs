using Microsoft.Extensions.Configuration;

namespace ZMDB.Core.Configuration
{
    public interface IAppInfo
    {
        /// <summary>
        /// Get application name. e.g. '@zmdb/skeleton'.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the application short name. e.g. 'skeleton'.
        /// </summary>
        string ShortName { get; }

        string ClusterId { get; set; }

        /// <summary>
        /// Get environment. e.g. 'Development'. (based on ASPNET_ENVIRONMENT, which can be mapped).
        /// </summary>
        string Environment { get; }

        /// <summary>
        /// Get git short commit hash. e.g. 'abc123'
        /// </summary>
        string GitCommit { get; }

        /// <summary>
        /// Get application version. e.g. '0.0.1-rc1'
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Get whether the app is dockerized or not.
        /// </summary>
        bool IsDockerized { get; }

        /// <summary>
        /// Gets which service type is this app responsible of e.g. web, silo, etc...
        /// </summary>
        string ServiceType { get; set; }
    }

    public class AppInfo : IAppInfo
    {
        public string Name { get; set; }
        public string ShortName { get; }
        public string ClusterId { get; set; }
        public string Environment { get; set; }
        public string GitCommit { get; set; }
        public string Version { get; set; }
        public bool IsDockerized { get; set; }
        public string ServiceType { get; set; }

        private static readonly Dictionary<string, string> EnvironmentMapping = new Dictionary<string, string>
        {
            ["Development"] = "dev",
            ["Staging"] = "staging",
            ["Production"] = "prod",
        };

        public AppInfo()
        {
        }

        public AppInfo(IConfiguration config)
        {
            Name = config.GetValue("AppInfo.AppName", "app");
            Version = config.GetValue("AppInfo.Version", "local");
            GitCommit = config.GetValue("AppInfo.GitCommit", "-");
            Environment = config.GetValue<string>("ASPNETCORE_ENVIRONMENT");
            IsDockerized = config.GetValue<bool>("DOCKER");
            ServiceType = config.GetValue("AppInfo.ServiceType", "dotnet");
            ShortName = Name.Split('/').Last();

            if (string.IsNullOrEmpty(Environment))
                throw new InvalidOperationException("Environment is not set. Please specify the environment via 'ASPNETCORE_ENVIRONMENT'");

            ClusterId = $"{Name}-{Version}";

            Environment = MapEnvironmentName(Environment);
        }

        public static string MapEnvironmentName(string environment)
        {
            if (environment == null) throw new ArgumentNullException(nameof(environment));

            EnvironmentMapping.TryGetValue(environment, out var env);
            return env;
        }
    }
}
