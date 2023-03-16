using Microsoft.Extensions.Logging;
using Orleans;
using ZMDB.GrainInterfaces;

namespace ZMDB.Grains
{
    public class Movie : Grain, IMovie
    {
        private readonly ILogger _logger;

        public ValueTask<string> IMovie.SayHello(string greeting)
        {
            _logger.LogInformation(
                "SayHello message received: greeting = '{Greeting}'", greeting);

            return ValueTask.FromResult(
                $"""
                Client said: '{greeting}', so MovieGrain says: Hello!
                """);
        }
    }
}