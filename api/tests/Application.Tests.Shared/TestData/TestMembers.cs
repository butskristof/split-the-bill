using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Tests.Shared.TestData;

public static class TestMembers
{
    public static Member Alice => new()
    {
        Id = new Guid("C8FE2024-32FB-49D7-A92B-5E56D8AE8360"),
        Name = nameof(Alice),
        Username = "alice",
        UserId = "alice",
    };

    public static Member Bob => new()
    {
        Id = new Guid("347D012C-AFE9-404D-80B2-47E57AB3EACA"),
        Name = nameof(Bob),
        Username = "bob",
        UserId = "bob",
    };

    public static Member Charlie => new()
    {
        Id = new Guid("243C316A-1336-4247-89A7-CACCBF9C6E6E"),
        Name = nameof(Charlie),
        Username = "charlie",
    };

    public static Member David => new()
    {
        Id =new Guid("66559461-E123-4233-9B57-4D8E715AA19F") ,
        Name = nameof(David),
        Username = "david",
    };

    public static IEnumerable<Member> GetAllMembers()
    {
        yield return Alice;
        yield return Bob;
        yield return Charlie;
        yield return David;
    }
}