using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IdentityResourceApi;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
        {
            return Task.CompletedTask;
        }

        var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer)?.Value.Split(' ');
        if (scopes != null && scopes.Any(s => s == requirement.Scope))
        {
            context.Succeed(requirement);

            foreach (var otherRequirement in context.Requirements)
            {
                if (otherRequirement.GetType() == typeof(RolesAuthorizationRequirement))
                {
                    context.Succeed(otherRequirement);
                }
            }
        }
        
        return Task.CompletedTask;
    }
}
