using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace back.Core;
//
static class ListFiles
{
    public static List<string> ListNameFiles()
    {
        List<string> nombresArchivos = new List<string>();

        try
        {
            string[] archivos = Directory.GetFiles("files");

            foreach (string archivo in archivos)
            {
                nombresArchivos.Add(Path.GetFileName(archivo));
            }
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("La carpeta especificada no existe.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("No tienes permiso para acceder a la carpeta.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocurrió un error: {ex.Message}");
        }

        return nombresArchivos;
    }
}