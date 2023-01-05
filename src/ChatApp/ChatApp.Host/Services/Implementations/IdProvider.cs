namespace ChatApp.Host.Services.Implementations;

using Services.Interfaces;

public class IdProvider : IIdProvider
{
    public string GenerateId()
        => Guid.NewGuid().ToString().Replace("-", "");
}