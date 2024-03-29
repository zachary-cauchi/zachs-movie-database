﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using ZMDB.MovieContracts.Interfaces;

namespace ZMDB.MovieContracts.Movie
{
    [Table("Movies")]
    public class Movie : AuditableObject
    {

        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "genres")]
        public IList<Genre> Genres { get; set; } = new List<Genre>();

        [JsonProperty(PropertyName = "rate")]
        public string Rate { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "length")]
        public string Length { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "img")]
        public string Img { get; set; } = String.Empty;
    }
}
