namespace ChatApp.Host.Producer.Interfaces;

using Confluent.Kafka;

public interface IKafkaProducer<TKey, TValue>
{
    Task ProduceAsync(string topic, Message<TKey, TValue> message);
}
