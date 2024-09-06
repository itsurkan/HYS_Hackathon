using Microsoft.AspNetCore.Mvc;

namespace ZoomAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ZoomController(ZoomService zoomService) : ControllerBase
{
    [HttpGet("authorize")]
    public IActionResult Authorize()
    {
        var authorizationUrl = zoomService.GetAuthorizationUrl();
        return Redirect(authorizationUrl);
    }

    [HttpGet("oauth/callback")]
    public async Task<IActionResult> OAuthCallback([FromQuery] string code)
    {
        try
        {
            var accessToken = await zoomService.GetAccessTokenAsync(code);
            // Store the access token securely, e.g., in a database
            var joinUrl = await zoomService.CreateMeetingAsync(accessToken, "New Meeting", DateTime.UtcNow.AddHours(1), 30);
            return Ok("Zoom authentication successful!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}
