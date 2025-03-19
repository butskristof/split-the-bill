using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.Tests.Shared.TestData;

public static class Members
{
    public static class Alice
    {
        public static readonly Guid Id = new("C8FE2024-32FB-49D7-A92B-5E56D8AE8360");
        public const string Name = nameof(Alice);

        public static Member Entity() => new()
        {
            Id = Id,
            Name = Name
        };
    }

    public static class Bob
    {
        public static readonly Guid Id = new("347D012C-AFE9-404D-80B2-47E57AB3EACA");
        public const string Name = nameof(Bob);

        public static Member Entity() => new()
        {
            Id = Id,
            Name = Name
        };
    }

    public static class Charlie
    {
        public static readonly Guid Id = new("243C316A-1336-4247-89A7-CACCBF9C6E6E");
        public const string Name = nameof(Charlie);

        public static Member Entity() => new()
        {
            Id = Id,
            Name = Name
        };
    }

    public static class David
    {
        public static readonly Guid Id = new("66559461-E123-4233-9B57-4D8E715AA19F");
        public const string Name = nameof(David);

        public static Member Entity() => new()
        {
            Id = Id,
            Name = Name
        };
    }
    
    public static IEnumerable<Member> GetAllMembers()
    {
        yield return Alice.Entity();
        yield return Bob.Entity();
        yield return Charlie.Entity();
        yield return David.Entity();
    }
}