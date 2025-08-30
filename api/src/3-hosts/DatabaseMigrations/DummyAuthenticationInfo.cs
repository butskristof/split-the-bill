using SplitTheBill.Application.Common.Authentication;

namespace SplitTheBill.DatabaseMigrations;

internal sealed class DummyAuthenticationInfo : IAuthenticationInfo
{
    public string UserId => "DATABASE_MIGRATIONS";
}
