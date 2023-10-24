
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace back.Core;
//
static class UploadFile
{

   public static async Task<EncryptedFileInfo> Upload(IFormFile file)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string[] allowedContentTypes = { "application/pdf", "image/png", "image/jpeg" };
        long maxSizeBytes = 5242880;
        if (!allowedContentTypes.Contains(file.ContentType) || file.Length > maxSizeBytes)
        {
            throw new ArgumentException("El tipo de archivo no es compatible o pesa mas de 5 mb.");
        }
        else
        {
            byte[] encryptedFile = FileEncryptionHelper.EncryptFile(file);


            string fileName = Path.GetFileName(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string uniqueFileName = Guid.NewGuid().ToString();
            string connectionString = configuration.GetConnectionString("StorageKey");
            var containerName = configuration.GetConnectionString("ContainerName");

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobClient = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(uniqueFileName);


            await blobClient.UploadAsync(new MemoryStream(encryptedFile), true);
            var encryptedFileInfo = new EncryptedFileInfo
            {
                FileName = uniqueFileName,
                FileExtension = extension
            };
            return encryptedFileInfo;
        }
    }

    public static async Task<List<EncryptedFileInfo>> UploadFiles(IFormFileCollection files)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string[] allowedContentTypes = { "application/pdf", "image/png", "image/jpeg" };
        const long maxSizeBytes = 5242880;

        List<EncryptedFileInfo> encryptedFileInfos = new();

        foreach (var file in files)
        {
            if (!allowedContentTypes.Contains(file.ContentType) || file.Length > maxSizeBytes)
            {
                Console.WriteLine($"El tipo de archivo {file.FileName} no es compatible o pesa más de 5 MB.");
                continue; 
            }

            Console.WriteLine($"Comenzando el cifrado y carga del archivo {file.FileName}...");

            try
            {
                byte[] encryptedFile = FileEncryptionHelper.EncryptFile(file);

                string fileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string uniqueFileName = Guid.NewGuid().ToString();
                string connectionString = configuration.GetConnectionString("StorageKey");
                var containerName = configuration.GetConnectionString("ContainerName");

                var blobServiceClient = new BlobServiceClient(connectionString);
                var blobClient = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(uniqueFileName);

                await blobClient.UploadAsync(new MemoryStream(encryptedFile), true);
                var encryptedFileInfo = new EncryptedFileInfo
                {
                    FileName = uniqueFileName,
                    FileExtension = extension
                };

                encryptedFileInfos.Add(encryptedFileInfo);
                Console.WriteLine($"Archivo {file.FileName} cifrado y cargado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante el cifrado o la carga del archivo {file.FileName}: {ex.Message}");
            }
        }

        return encryptedFileInfos;
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


public static async Task<List<string>> DownloadAndDecryptFiles(string fileNamesCsv)
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

    string connectionString = configuration.GetConnectionString("StorageKey");
    var containerName = configuration.GetConnectionString("ContainerName");

    BlobServiceClient blobServiceClient = new(connectionString);
    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

    List<string> decryptedDataList = new();

    string[] fileNames = fileNamesCsv.Split(',');

    foreach (var fileName in fileNames)
    {
        BlobClient encryptedBlob = containerClient.GetBlobClient(fileName.Trim());
        BlobDownloadInfo downloadInfo = await encryptedBlob.DownloadAsync();

        using (MemoryStream memoryStream = new())
        {
            await downloadInfo.Content.CopyToAsync(memoryStream);
            byte[] encryptedData = memoryStream.ToArray();
            byte[] decryptedData = FileEncryptionHelper.DecryptFile(encryptedData);
            string decryptedDataString = Convert.ToBase64String(decryptedData);
            decryptedDataList.Add(decryptedDataString);
        }
    }

    return decryptedDataList;
}





public static async Task<EncryptedFileInfo> UploadFileLocal(IFormFile file)
    {

        string[] allowedContentTypes = { "application/pdf", "image/png", "image/jpeg" };
        long maxSizeBytes = 5242880;
        if (!allowedContentTypes.Contains(file.ContentType) || file.Length > maxSizeBytes)
        {
            throw new ArgumentException("El tipo de archivo no es compatible o pesa mas de 5 mb.");
        }
        else
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid() + fileExtension;
            var filePath = Path.Combine("files", fileName.ToString());

            using (var encryptedStream = new MemoryStream())
            {
                byte[] encryptedFile = FileEncryptionHelper.EncryptFile(file);
                await encryptedStream.WriteAsync(encryptedFile, 0, encryptedFile.Length);
                encryptedStream.Seek(0, SeekOrigin.Begin);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await encryptedStream.CopyToAsync(fileStream);
                }
            }

            var response = new EncryptedFileInfo
            {
                FileName = fileName,
                FileExtension = fileExtension
            };

            return response;
        }
    }



    public static async Task<string> GetAndDecryptFileLocal(string fileName)
    {

        var filePath = Path.Combine("files", fileName);

        if (!File.Exists(filePath))
        {
           throw new ArgumentException("Archivo no encontrado");
        }

        byte[] encryptedData;
        using (var fileStream = new FileStream(filePath, FileMode.Open))
        {
            encryptedData = new byte[fileStream.Length];
            await fileStream.ReadAsync(encryptedData, 0, (int)fileStream.Length);
        }
        byte[] decryptedData = FileEncryptionHelper.DecryptFile(encryptedData);
        var base64String = Convert.ToBase64String(decryptedData);
        return base64String;
    } 
}
class EncryptedFileInfo
{
    public string FileName { get; set; }
    public string FileExtension { get; set; }
}
