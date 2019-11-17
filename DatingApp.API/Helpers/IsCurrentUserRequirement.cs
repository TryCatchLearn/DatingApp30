using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public class IsCurrentUserRequirement : IAuthorizationRequirement
    {
    }

    public class IsCurrentUserRequirementHandler : AuthorizationHandler<IsCurrentUserRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsCurrentUserRequirementHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCurrentUserRequirement requirement)
        {
            var userId = _httpContextAccessor.HttpContext.User?.Claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToString();

            var userIdFromRoute = _httpContextAccessor.HttpContext.Request.RouteValues.SingleOrDefault(x => x.Key == "userId").Value.ToString();

            if (userId == userIdFromRoute)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}