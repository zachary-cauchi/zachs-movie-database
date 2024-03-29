﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDB.MovieContracts.Interfaces;

namespace ZMDB.MovieContracts.Movie
{
    [Table("Genres")]
    public class Genre : AuditableObject
    {

        [JsonProperty(PropertyName = "Id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; } = String.Empty;

    }
}
