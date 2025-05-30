using System.Security.Authentication;
using System.Security.Claims;
using SplitTheBill.Application.Common.Authentication;

namespace SplitTheBill.Api.Common.Authentication;

internal sealed class ApiAuthenticationInfo : IAuthenticationInfo
{
    #region construction

    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiAuthenticationInfo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new AuthenticationException();
}
