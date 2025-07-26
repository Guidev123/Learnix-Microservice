using Confluent.Kafka;
using Learnix.Commons.Domain.DomainObjects;
using System.IO.Compression;
using System.Text.Json;

namespace Learnix.Commons.Infrastructure.Extensions
{
    public sealed class KafkaSerializerExtensions<T> : ISerializer<T>
        where T : IEvent
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(data);

            using var memoryStream = new MemoryStream();
            using var zipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Close();

            var buffer = memoryStream.ToArray();

            return buffer;
        }
    }
}