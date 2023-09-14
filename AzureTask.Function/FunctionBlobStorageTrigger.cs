using System;
using System.IO;
using Azure.Storage;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;

namespace AzureTask.Function;

[StorageAccount("BlobConnectionString")]
public static class FunctionBlobStorageTrigger
{
    [FunctionName("SendEmail")]
    public static void Run(
        [BlobTrigger("container/{name}")] Stream myBlob, string name, BlobProperties properties, Uri uri, ILogger log)
    {
        var sasToken = GenerateSasToken(name);
        var sasUrl = uri.AbsoluteUri + "?" + sasToken;

        var message = GenerateEmailMessage(name, properties, sasUrl);
        
        using var client = new SmtpClient();
        client.Connect(Environment.GetEnvironmentVariable("Server"), 465, true);
        client.Authenticate(Environment.GetEnvironmentVariable("UsernameEmail"), Environment.GetEnvironmentVariable("PasswordEmail"));
        client.Send(message);
        client.Disconnect(true);
    }

    public static string GenerateSasToken(string name)
    {
        BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = Environment.GetEnvironmentVariable("ContainerName"),
            BlobName = name,
            ExpiresOn = DateTime.UtcNow.AddMinutes(60),
        };
        
        blobSasBuilder.SetPermissions(BlobSasPermissions.All);
        
        var sasToken = blobSasBuilder.ToSasQueryParameters
        (new StorageSharedKeyCredential(Environment.GetEnvironmentVariable("AccountName"), 
            Environment.GetEnvironmentVariable("AccountKey"))).ToString();

        return sasToken;
    }

    public static MimeMessage GenerateEmailMessage(string name, BlobProperties properties, string sasUrl)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(Environment.GetEnvironmentVariable("SenderName"), Environment.GetEnvironmentVariable("SenderEmail")));
        message.To.Add(new MailboxAddress("", properties.Metadata["email"]));
        message.Subject = Environment.GetEnvironmentVariable("Subject");
        message.Body = new TextPart("Plain")
        {
            Text = $"File \"{name}\" is successfully uploaded.\n{sasUrl}"
        };

        return message;
    }
}