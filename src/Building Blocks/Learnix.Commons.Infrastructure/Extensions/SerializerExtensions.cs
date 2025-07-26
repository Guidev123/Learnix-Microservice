using Newtonsoft.Json;

namespace Learnix.Commons.Infrastructure.Extensions
{
    public static class SerializerExtensions
    {
        public static readonly JsonSerializerSettings Instance = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
        };
    }
}