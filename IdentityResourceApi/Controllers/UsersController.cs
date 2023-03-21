using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityResourceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly User[] Users = {
        new() { Name = "User 1", UserName = "user1@gmail.com" },
        new() { Name = "User 2", UserName = "user2@gmail.com" }
    };
    
    [HttpGet]
    [Authorize(Policy = "admin:users")]
    //[Authorize(Policy = "admin:users",Roles = "useradmin")]
    public IEnumerable<User> Index()
    {
        return Users;
    }
}