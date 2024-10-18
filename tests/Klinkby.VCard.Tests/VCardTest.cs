using System.Diagnostics;

namespace Klinkby.VCard.Tests;

[Trait("Category", "Unit")]
public sealed class VCardTest
{
    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    
    public void WriteVCard(string expected, IVCardWriter testData)
    {
        using var sw = new StringWriter();
        Debug.Assert(testData != null, nameof(testData) + " != null");
        testData.WriteVCard(sw);
        var actual = sw.ToString();

        Assert.Equal(expected, actual);
    }
}