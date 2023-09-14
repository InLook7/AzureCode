using Azure.Storage.Blobs;
using AzureTask.Api.Services;
using AzureTask.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetValue<string>("AzureBlobStorageConnectionString")));
builder.Services.AddSingleton<IUploadService, UploadService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{}

app.MapControllers();

app.Run();