using System.Security.Claims;

namespace API_budget_finance.Context
{
    public class AuthenticatedUserContext(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
