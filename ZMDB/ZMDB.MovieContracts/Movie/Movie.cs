using Newtonsoft.Json;

namespace ZMDB.MovieContracts.Movie
{
    public class Movie
    {

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "genres")]
        public IList<string> Genres { get; set; } = new List<string>();

        [JsonProperty(PropertyName = "rate")]
        public string Rate { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "length")]
        public string Length { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "img")]
        public string Img { get; set; } = String.Empty;
    }
}
