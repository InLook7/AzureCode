using Xunit;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using AzureTask.Api.Services;

namespace AzureTask.Test;

public class ApiTest
{
    [Fact]
    public async void UploadFileAsync_UploadFileWithNameTest_ReturnsDocx()
    {
        // Arrange
        UploadService uploadService = new UploadService(new BlobServiceClient("UseDevelopmentStorage=true"));
        string file = "test.docx";
        using var stream = new MemoryStream(File.ReadAllBytes(file).ToArray());
        var formFile = new FormFile(stream, 0, stream.Length, "streamFile", file.Split(@"\").Last());
        
        // Act
        await uploadService.UploadFileAsync(formFile, "test@gmail.com");

        // Assert
        var fileName = uploadService._blobServiceClient.GetBlobContainerClient("container").GetBlobClient(file).Name;
        Assert.Equal("test.docx", fileName);
    }
}