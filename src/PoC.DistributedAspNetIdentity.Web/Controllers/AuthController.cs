using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoC.DistributedAspNetIdentity.Web.Services;
using PoC.DistributedAspNetIdentity.Web.Services.Dto;

namespace PoC.DistributedAspNetIdentity.Web.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersApiClient _usersApiClient;

        public AuthController(IUsersApiClient usersApiClient)
        {
            _usersApiClient = usersApiClient;
        }

        [HttpGet("check-session")]
        public ActionResult<UserResponse> CheckSession()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated) return Unauthorized();

            return Ok(new UserResponse
            {
                Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Email = User.FindFirstValue(ClaimTypes.Email),
                Name = User.FindFirstValue(ClaimTypes.Name),
                Surname = User.FindFirstValue(ClaimTypes.Surname),
            });
        }

        [HttpPost("login")]
        public async Task<UserResponse> Login([FromBody] CheckCredentialsRequest request, CancellationToken cancellationToken)
        {
            var response = await _usersApiClient.CheckCredentials(request, cancellationToken);

            await AuthenticateUser(response);

            return response;
        }

        [HttpPost("signup")]
        public async Task<UserResponse> Signup([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _usersApiClient.CreateUser(request, cancellationToken);

            await AuthenticateUser(response);

            return response;
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        // Source: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-6.0
        private async Task AuthenticateUser(UserResponse user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, 
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}