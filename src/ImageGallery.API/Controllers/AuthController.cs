using Microsoft.AspNetCore.Mvc;

namespace ImageGallery.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

    [HttpGet("login-url")]
    public IActionResult GetLoginUrl()
    {
        var supabaseUrl = _config["Supabase:AuthUrl"]?.TrimEnd('/');
        var redirectTo = $"{Request.Scheme}://{Request.Host}/login-callback";
        var url = $"{supabaseUrl}/authorize?provider=github&redirect_to={Uri.EscapeDataString(redirectTo)}";
        return Ok(new { url });
    }

    [HttpGet("me")]
    public IActionResult GetMe()
    {
        var userId = User.FindFirst("sub")?.Value;
        var email = User.FindFirst("email")?.Value;
        var name = User.FindFirst("user_metadata")?.Value;
        return Ok(new { userId, email, name });
    }
}
