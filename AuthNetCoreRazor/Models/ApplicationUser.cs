using Microsoft.AspNetCore.Identity;

namespace AuthNetCoreRazor.Models;

public class ApplicationUser : IdentityUser
{
    public virtual string? FullName { get; set; }
    public virtual int Age { get; set; }
}