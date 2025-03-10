using SplitTheBillPocV1.Models;

namespace SplitTheBillPocV1.Tests.TestData;

internal static class Members
{
    internal static class Alice
    {
        internal static readonly Guid Id = new("B0102BEF-70FE-4DA1-83CE-4ADEB5463169");
        internal const string Name = nameof(Alice);

        internal static Member Entity() => new() { Id = Id, Name = Name };
    }

    internal static class Bob
    {
        internal static readonly Guid Id = new("A4D7FC3F-9350-48D7-8204-FD59A31F4902");
        internal const string Name = nameof(Bob);

        internal static Member Entity() => new() { Id = Id, Name = Name };
    }
}