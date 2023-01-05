namespace ChatApp.Host.Controllers.v1;

using Models;
using Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiVersion("1.0")]
public class MessageController : ApiController
{
    private readonly ILogger<MessageController> _logger;
    private readonly IMessageService _messageService;

    public MessageController(
        ILogger<MessageController> logger,
        IMessageService messageService) => (_logger , _messageService) = (logger, messageService);

    [HttpPost]
    [Route("send")]
    public async Task<IActionResult> Send([FromBody] RequestMessage request)
    {
        var username = HttpContext?.User?.Identity?.Name;

        await _messageService.SendMessage(username, request.Text);

        return Ok();
    }
}
