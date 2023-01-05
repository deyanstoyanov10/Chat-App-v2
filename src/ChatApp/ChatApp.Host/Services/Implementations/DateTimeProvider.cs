namespace ChatApp.Host.Services.Implementations;

using Interfaces;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeNow()
        => DateTime.UtcNow;
}