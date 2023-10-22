using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MovieAppAPI.Authorization
{
    public class OwnAccountAuhtorizationHandler : AuthorizationHandler<OwnAccountRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OwnAccountAuhtorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnAccountRequirement requirement)
        {
            string? queryUserId = _httpContextAccessor.HttpContext?.GetRouteValue("userId").ToString();
            string? loggedInUser = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(!string.IsNullOrEmpty(queryUserId) && !string.IsNullOrEmpty(loggedInUser) && queryUserId == loggedInUser)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
