using SplitTheBill.Application.Common.Authentication;

namespace SplitTheBill.Migrations;

internal sealed class AuthenticationInfo : IAuthenticationInfo
{
    public string UserId => "MIGRATION";
}