namespace ChatApp.Host.Services.Implementations;

using Models;
using Interfaces;
using Configuration;
using Producer.Interfaces;

using Microsoft.Extensions.Options;

using System.Web;

public class MessageService : IMessageService
{
	private readonly IKafkaProducer<string, Message> _producer;
	private readonly IIdProvider _idProvider;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly MessageLoggingConfig _messageLoggingConfig;

	public MessageService(
		IKafkaProducer<string, Message> producer,
		IOptionsMonitor<MessageLoggingConfig> messageLoggingConfig,
		IIdProvider idProvider,
		IDateTimeProvider dateTimeProvider)
	{
		_producer = producer;
		_idProvider = idProvider;
		_dateTimeProvider = dateTimeProvider;
		_messageLoggingConfig = messageLoggingConfig?.CurrentValue ?? throw new ArgumentNullException("Kafka message logging not configured.");
	}

	public async Task SendMessage(string username, string text)
	{
		var msg = BuildMessage(username, text);

        if (_messageLoggingConfig.IsEnabled)
		{
			var message = new Confluent.Kafka.Message<string, Message>()
			{
				Key = msg.Id,
				Value = msg
			};

			await _producer.ProduceAsync(_messageLoggingConfig.Topic, message);
		}
	}

	private Message BuildMessage(string username, string text)
	{
		var message = new Message()
		{
			Id = _idProvider.GenerateId(),
			UserName = username,
			SendDate = _dateTimeProvider.DateTimeNow(),
			Text = HtmlEscape(text)
        };

		return message;
	}

    private string HtmlEscape(string text)
    {
        string encodedString = HttpUtility.HtmlEncode(text);

        StringWriter stringWriter = new StringWriter();

        HttpUtility.HtmlDecode(encodedString, stringWriter);

        string decodedString = stringWriter.ToString();

        return decodedString;
    }
}