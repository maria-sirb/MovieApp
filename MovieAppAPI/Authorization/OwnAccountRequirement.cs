using Microsoft.AspNetCore.Authorization;

namespace MovieAppAPI.Authorization
{
    public class OwnAccountRequirement : IAuthorizationRequirement
    {
        public OwnAccountRequirement() { }
    }
}
