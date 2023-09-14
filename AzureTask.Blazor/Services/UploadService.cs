using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using AzureTask.Blazor.Services.Interfaces;

namespace AzureTask.Blazor.Services;

public class UploadService : IUploadService
{
    private readonly HttpClient _httpClient;

    public UploadService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> UploadFileAsync(IBrowserFile file, string email)
    {
        var fileContent = new StreamContent(file.OpenReadStream());
        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(fileContent, $"{file.Name}", file.Name);
        multipartContent.Add(new StringContent(email), "email");
        
        var response = await _httpClient.PostAsync("api/uploaddata/uploadfile/", multipartContent);
        var result = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.OK)
            return "File uploaded successfully!";
        return "File with the same name already exists!";
    }
}