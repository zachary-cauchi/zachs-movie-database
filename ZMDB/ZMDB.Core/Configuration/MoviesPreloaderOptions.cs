using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDB.Core.Configuration
{
    public enum MoviesPreloaderSource
    {
        FILE
    }

    public class PreloadMoviesOptions
    {
        public const string PreloadMovies = "PreloadMovies";

        public bool PreloadGrains { get; set; } = false;
        public MoviesPreloaderSource MoviesSource { get; set; } = MoviesPreloaderSource.FILE;
        public string SourcePath { get; set; } = String.Empty;
    }
}
