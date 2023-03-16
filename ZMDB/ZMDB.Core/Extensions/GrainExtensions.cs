// ReSharper disable once CheckNamespace
using Orleans;

namespace Orleans
{
    public static class GrainExtensions
    {
        /// <summary>
        /// Returns the primary key of the grain of any type as a string.
        /// </summary>
        /// <param name="grain"></param>
        /// <returns></returns>
        public static string GetPrimaryKeyAny(this Grain grain)
        {
            return grain.GetPrimaryKeyString()
                   ?? (grain.IsPrimaryKeyBasedOnLong()
                       ? grain.GetPrimaryKeyLong().ToString()
                       : grain.GetPrimaryKey().ToString());
        }
    }
}
