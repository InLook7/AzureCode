using Microsoft.AspNetCore.Mvc;
using AzureTask.Api.Services.Interfaces;

namespace AzureTask.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadDataController : ControllerBase
{
    private readonly IUploadService _uploadService;
    
    public UploadDataController(IUploadService uploadService)
    {
        _uploadService = uploadService;
    }
    
    [HttpPost("uploadfile")]
    public async Task<IActionResult> UploadFile()
    {
        var file = Request.Form.Files[0];
        var email = Request.Form["email"].ToString();

        try
        {
            await _uploadService.UploadFileAsync(file, email);
            return Ok();
        }
        catch
        {
            return StatusCode(500);
        }
    }
}