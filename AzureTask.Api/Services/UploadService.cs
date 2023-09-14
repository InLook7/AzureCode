using Azure.Storage.Blobs;
using AzureTask.Api.Services.Interfaces;

namespace AzureTask.Api.Services;

public class UploadService : IUploadService
{
    public readonly BlobServiceClient _blobServiceClient;

    public UploadService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    
    public async Task UploadFileAsync(IFormFile file, string email)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("container");
        var blobClient = containerClient.GetBlobClient(file.Name);
        await blobClient.UploadAsync(file.OpenReadStream());
        await blobClient.SetMetadataAsync(new Dictionary<string,string> {{"email", email}});
    }
}