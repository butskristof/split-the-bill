namespace SplitTheBill.Application.Tests.Shared.TestData;

public static class TestValues
{
    public static IEnumerable<string?> EmptyStrings()
    {
        yield return null;
        yield return string.Empty;
        yield return " ";
        yield return "      ";
    }
}
