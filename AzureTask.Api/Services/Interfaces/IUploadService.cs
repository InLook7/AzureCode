namespace AzureTask.Api.Services.Interfaces;

public interface IUploadService
{
    Task UploadFileAsync(IFormFile file, string email);
}