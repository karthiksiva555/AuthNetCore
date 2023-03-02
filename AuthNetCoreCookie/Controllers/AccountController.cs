using System.Security.Claims;
using AuthNetCoreCookie.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthNetCoreCookie.Controllers;

[Route("/api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private List<User> _users = new()
    {
        new User("user1@test.com", "User 1", "user1"),
        new User("user2@test.com", "User 2", "user2"),
    };

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInRequest signInRequest)
    {
        var user = _users.FirstOrDefault(x => x.Email == signInRequest.Email &&
                                             x.Password == signInRequest.Password);
        if (user is null)
        {
            return BadRequest(new Response(false, "Invalid credentials."));
        }

        var claims = new List<Claim>
        {
            new (type: ClaimTypes.Email, value: signInRequest.Email),
            new (type: ClaimTypes.Name, value: user.Name)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            });
        return Ok(new Response(true, "Signed in successfully"));
    }
    
    [Authorize]
    [HttpGet("user")]
    public IActionResult GetUser()
    {
        var userClaims = User.Claims.Select(x => new UserClaim(x.Type, x.Value)).ToList();

        return Ok(userClaims);
    }

    [Authorize]
    [HttpGet("signout")]
    public async Task SignOutAsync()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}