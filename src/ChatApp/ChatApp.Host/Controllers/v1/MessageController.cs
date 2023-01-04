namespace ChatApp.Host.Controllers.v1;

using ChatApp.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiVersion("1.0")]
public class MessageController : ApiController
{
    private readonly ILogger<MessageController> _logger;

    public MessageController(ILogger<MessageController> logger) => _logger = logger;

    [HttpPost]
    [Route("send")]
    public IActionResult Send([FromBody] RequestMessage request)
    {
        _logger.LogWarning(request?.Text);
        return Ok();
    }
}
