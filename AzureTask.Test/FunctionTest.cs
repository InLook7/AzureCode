using Xunit;
using AzureTask.Function;

namespace AzureTask.Test;

public class FunctionTest
{
    [Fact]
    public void GenerateSasToken_GenerateToken_ContainsSas()
    {
        // Arrange
        string tokenString;
        
        // Act
        tokenString = FunctionBlobStorageTrigger.GenerateSasToken("test.docx");

        // Assert
        Assert.Contains("?sv", tokenString);
    }
}