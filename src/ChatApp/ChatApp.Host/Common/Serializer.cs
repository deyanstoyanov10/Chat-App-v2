namespace ChatApp.Host.Common;

using MessagePack;
using Confluent.Kafka;

public class Serializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
        => data == null ? new byte[1] : MessagePackSerializer.Serialize(data);
}