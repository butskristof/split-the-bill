using SplitTheBillPocV3.Models;

namespace SplitTheBillPocV3.Tests.TestData;

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

    internal static class Charlie
    {
        internal static readonly Guid Id = new("98B9B416-E832-41C7-BE64-0C329BEE17BB");
        internal const string Name = nameof(Charlie);
        
        internal static Member Entity() => new() { Id = Id, Name = Name };
    }
}