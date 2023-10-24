namespace back.Core;
public class FueatureModule : IModule
{
    public IServiceCollection RegisterModules(IServiceCollection services)
    {

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
         endpoints.MapPost("/api/send-email", (Models.SendEmail oSendEmail) => { return SendGrid.SendEmailsync(oSendEmail); }).RequireAuthorization().WithTags("Enviar correos");
         endpoints.MapPost("/api/decrypt-token", (Models.Token Otoken) => { return StringEncryptionHelper.DecryptEndpoint(Otoken); }).WithTags("Desencriptar token");
         endpoints.MapPost("/api/upload", (IFormFile file) => UploadFile.Upload(file)).RequireAuthorization().WithTags("Subir archivo");
        endpoints.MapPost("/api/upload-files", (IFormFileCollection files) => UploadFile.UploadFiles(files)).RequireAuthorization().WithTags("Subir archivo");
      
        endpoints.MapGet("/api/getfile/{fileName}", (string fileName) => UploadFile.DownloadAndDecryptAsync(fileName)).RequireAuthorization().WithTags("Subir archivo");
        endpoints.MapGet("/api/getfiles/{fileNamesCsv}", (string fileNamesCsv) => UploadFile.DownloadAndDecryptFiles(fileNamesCsv)).RequireAuthorization().WithTags("Subir archivo");



        // local
         endpoints.MapPost("/api/uploadlocal", (IFormFile file) => UploadFile.UploadFileLocal(file)).WithTags("Subir archivo");
        endpoints.MapGet("/api/getfilelocal/{fileName}", (string fileName) => UploadFile.GetAndDecryptFileLocal(fileName)).WithTags("Subir archivo");
        //Archivos
        endpoints.MapGet("/api/getFilesfolder", () => ListFiles.ListNameFiles()).WithTags("Subir archivo");

        return endpoints;
    }
}