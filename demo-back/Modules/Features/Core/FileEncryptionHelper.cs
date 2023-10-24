using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace back.Core;

public static class FileEncryptionHelper
{
    private static readonly byte[] Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("miClaveSecreta123"));
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");



    public static byte[] EncryptFile(IFormFile file)
    {
        using var inputStream = file.OpenReadStream();
        using var memoryStream = new MemoryStream();
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        inputStream.CopyTo(cryptoStream);
        cryptoStream.FlushFinalBlock();
        return memoryStream.ToArray();
    }



    public static byte[] DecryptFile(byte[] encryptedData)
    {
        using var memoryStream = new MemoryStream(encryptedData);
        using var outputStream = new MemoryStream();
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        cryptoStream.CopyTo(outputStream);
        outputStream.Position = 0;
        return outputStream.ToArray();
    }
}