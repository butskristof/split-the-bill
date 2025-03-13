using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.UnitTests.TestData;

internal static class Members
{
    internal static class Alice
    {
        internal static readonly Guid Id = new("C8FE2024-32FB-49D7-A92B-5E56D8AE8360");
        internal const string Name = nameof(Alice);

        internal static Member Entity() => new()
        {
            Id = Id,
            Name = Name
        };
    }

    internal static class Bob
    {
        internal static readonly Guid Id = new("347D012C-AFE9-404D-80B2-47E57AB3EACA");
        internal const string Name = nameof(Bob);

        internal static Member Entity() => new()
        {
            Id = Id,
            Name = Name
        };
    }

    internal static class Charlie
    {
        internal static readonly Guid Id = new("243C316A-1336-4247-89A7-CACCBF9C6E6E");
        internal const string Name = nameof(Charlie);

        internal static Member Entity() => new()
        {
            Id = Id,
            Name = Name
        };
    }
}