using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public() => Ok("Public endpoint");

    [Authorize]
    [HttpGet("private")]
    public IActionResult Private()
    {
        var username = User.Identity?.Name;
        return Ok($"Welocome {username}! .");
    }
}