namespace SplitTheBill.Application.Tests.Shared.TestData;

public static class TestUtilities
{
    public static string GenerateString(int length)
    {
        if (length < 0)
            throw new ArgumentException("Length cannot be negative", nameof(length));
        
        return new string('a', length);
    }
}