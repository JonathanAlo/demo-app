using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace back.Core;

public class SendGrid
{
    static ExampleContext dbContext = new();


    public static async Task<IResult> SendEmailsync(Models.SendEmail oSendEmail)
    {
        dbContext = new();

        IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();


        var key = configuration.GetConnectionString("SendGridKey");

        var client = new SendGridClient(key);


        var from = new EmailAddress("notificacion@proseso.mx", "PROSESO Docs");
        var subject = oSendEmail.Subject;
        var to = new EmailAddress(oSendEmail.To);
        var plainTextContent = $"{oSendEmail.Message}";
        var user = oSendEmail.User;
        var url = oSendEmail.Url;
        var button = oSendEmail.Button;
        var mensaje = oSendEmail.Message;
    
  


        string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "template.html");
        string htmlContent = File.ReadAllText(htmlFilePath);
        var defaultUser = "Usuario";

        htmlContent = htmlContent.Replace("{{message}}", mensaje);
        htmlContent = htmlContent.Replace("{{user}}", (user == null) ? defaultUser : user);
        htmlContent = htmlContent.Replace("{{url}}", url);
        htmlContent = htmlContent.Replace("{{button}}", button);


        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        var response = await client.SendEmailAsync(msg);

        return Results.Ok(response.StatusCode);
    }








    public static async Task<string> DownloadAndDecryptAsync(string fileName)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                     .AddJsonFile("appsettings.json")
                     .Build();
        string connectionString = configuration.GetConnectionString("StorageKey");
        var containerName = configuration.GetConnectionString("ContainerName");

        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient encryptedBlob = containerClient.GetBlobClient(fileName);
        BlobDownloadInfo downloadInfo = await encryptedBlob.DownloadAsync();

        using (MemoryStream memoryStream = new MemoryStream())
        {
            await downloadInfo.Content.CopyToAsync(memoryStream);
            byte[] encryptedData = memoryStream.ToArray();
            byte[] decryptedData = FileEncryptionHelper.DecryptFile(encryptedData);
            return Convert.ToBase64String(decryptedData);
        }
    }

}
