using Microsoft.AspNetCore.Authorization;

namespace Events.Api.Authorization.Requirements
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(IEnumerable<string> roles)
        {
            Roles = roles;
        }

        public IEnumerable<string> Roles { get; }
    }
}
