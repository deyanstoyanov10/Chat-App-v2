namespace ChatApp.Host.Services.Interfaces;

public interface IMessageService
{
    Task SendMessage(string username, string text);
}