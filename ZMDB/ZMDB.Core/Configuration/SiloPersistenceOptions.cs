using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDB.Core.Configuration
{
    public enum SiloPersistenceTypes
    {
        UNDEFINED,
        POSTGRESQL
    }

    public class SiloPersistenceOptions
    {
        public const string SiloPersistence = "SiloPersistence";

        public SiloPersistenceTypes Type { get; set; } = SiloPersistenceTypes.UNDEFINED;

        public string Name { get; set; } = String.Empty;

        public string Invariant { get; set; } = String.Empty;

        public string ConnectionString { get; set; } = String.Empty;

        public string UseJsonFormat { get; set; } = String.Empty;
    }
}
