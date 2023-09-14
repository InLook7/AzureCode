using Microsoft.AspNetCore.Components.Forms;

namespace AzureTask.Blazor.Services.Interfaces;

public interface IUploadService
{
    Task<string> UploadFileAsync(IBrowserFile file, string email);
}