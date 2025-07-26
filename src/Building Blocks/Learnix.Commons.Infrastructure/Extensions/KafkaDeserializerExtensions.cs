using Confluent.Kafka;
using Learnix.Commons.Domain.DomainObjects;
using System.IO.Compression;
using System.Text.Json;

namespace Learnix.Commons.Infrastructure.Extensions
{
    public sealed class KafkaDeserializerExtensions<T> : IDeserializer<T>
        where T : IEvent
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            using var memoryStream = new MemoryStream(data.ToArray());
            using var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true);

            return JsonSerializer.Deserialize<T>(zipStream)
                ?? throw new InvalidDataException("Deserialized data is null.");
        }
    }
}