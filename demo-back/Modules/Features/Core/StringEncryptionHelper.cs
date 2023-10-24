
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace back.Core;

public static class StringEncryptionHelper
{
    private static readonly byte[] Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("miClaveSecreta123"));
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");



    public static string EncryptToken(string token)
    {
        byte[] encryptedBytes;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(token);
                    }

                    encryptedBytes = memoryStream.ToArray();
                }
            }
        }

        string encryptedString = Convert.ToBase64String(encryptedBytes);
        string urlSafeEncryptedString = HttpUtility.UrlEncode(encryptedString);
        return urlSafeEncryptedString;
    }
    public static string DecryptToken(string encryptedToken)
{
    try
    {
        string encryptedString = HttpUtility.UrlDecode(encryptedToken);
        byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
        string decryptedToken;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cryptoStream))
                    {
                        decryptedToken = reader.ReadToEnd();
                    }
                }
            }

            return decryptedToken;
        }
    }
    catch (Exception ex)
    {
        throw new Exception("Error al descifrar el token", ex);
    }
}

public static IResult DecryptEndpoint(Models.Token Otoken)
{
    try
    {
        var token = DecryptToken(Otoken.encryptedToken);
        return Results.Ok(token);
    }
    catch (Exception ex)
    {
        return Results.NotFound("Error al descifrar el token: " + ex.Message);
    }
}

}