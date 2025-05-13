using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/{tenantSlug}/google")]
[ApiController]
public class GoogleAuthController : ControllerBase
{
    private readonly GoogleAuthService _googleAuthService;

    public GoogleAuthController(GoogleAuthService googleAuthService)
    {
        _googleAuthService = googleAuthService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginWithGoogle([FromRoute] string tenantSlug, [FromBody] GoogleLoginRequest request, CancellationToken cancellationToken)
    {

        string token = await _googleAuthService.AuthenticateWithGoogleAsync(request.IdToken, tenantSlug, cancellationToken);
        return Ok(new { token });

    }
}

public class GoogleLoginRequest
{
    public string IdToken { get; set; }

}
