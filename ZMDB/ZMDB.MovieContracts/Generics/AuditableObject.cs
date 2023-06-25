﻿using Newtonsoft.Json;

namespace ZMDB.MovieContracts.Interfaces
{
    public abstract class AuditableObject
    {
        [JsonProperty(PropertyName = "IsInitialised")]
        public bool IsInitialised { get; set; } = false;

        [JsonProperty(PropertyName = "IsDeleted")]
        public bool IsDeleted { get; set; } = false;

        [JsonProperty(PropertyName = "CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonProperty(PropertyName = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [JsonProperty(PropertyName = "DeletedAt")]
        public DateTime? DeletedAt { get; set; } = null;
    }
}
