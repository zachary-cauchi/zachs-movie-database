using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.Core.Configuration;
using ZMDB.GrainInterfaces;
using ZMDB.Grains.Grains;
using ZMDB.MovieContracts.Movie;

namespace ZMDB.Core.StartupTasks
{
    public class MovieGrainPreloaderStartupTask : IStartupTask
    {
        private readonly IGrainFactory _grainFactory;
        private readonly ILogger<MovieGrainPreloaderStartupTask> _logger;
        private readonly PreloadMoviesOptions _preloadMoviesOptions;

        public MovieGrainPreloaderStartupTask(
            IGrainFactory grainFactory,
            IOptions<PreloadMoviesOptions> options,
            ILogger<MovieGrainPreloaderStartupTask> logger
        )
        {
            _grainFactory = grainFactory;
            _preloadMoviesOptions = options.Value;
            _logger = logger;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {

            _logger.LogDebug("Checking if movies are to be preloaded.");

            if ( _preloadMoviesOptions == null || !_preloadMoviesOptions.PreloadGrains )
            {
                _logger.LogInformation("Skipping movie grain preloading.");
                return;
            }

            if ( _preloadMoviesOptions.MoviesSource != MoviesPreloaderSource.FILE )
            {
                _logger.LogError("Unsupported movie grain source type ({sourceType})", _preloadMoviesOptions.MoviesSource);
                return;
            }

            string filePath = _preloadMoviesOptions.SourcePath;
            JObject rawMoviesJson = JObject.Parse(File.ReadAllText(filePath));

            if (!rawMoviesJson.ContainsKey("movies"))
            {
                _logger.LogError("Could not preload movie grains ({filePath})", filePath);

                throw new FormatException("root token \"movies\" not found.");
            }

            var movies = 
                from m in rawMoviesJson["movies"]
                select m.ToObject<Movie>();

            _logger.LogDebug("Found {count} movies to load.", movies.Count());

            foreach ( var movie in movies )
            {
                await _grainFactory.GetGrain<IMovieGrain>(movie.Id).SetMovie(movie);
            }

            _logger.LogInformation("Preloaded {count} movies.", movies.Count());

            return;
        }
    }
}
