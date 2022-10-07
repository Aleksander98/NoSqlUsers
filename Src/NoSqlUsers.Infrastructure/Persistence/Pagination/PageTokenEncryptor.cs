using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.Model;
using MyEmployees.Infrastructure.Persistence.Json;
using Amazon.DynamoDBv2.DocumentModel;

namespace MyEmployees.Infrastructure.Persistence.Pagination;

public static class PageTokenEncryptor
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new ExclusiveStartKeyStringJsonConverter()
        }
    };

    public static string Encrypt(Dictionary<string, AttributeValue> exclusiveStartKey, string encryptionKey)
    {
        var json = JsonSerializer.Serialize(exclusiveStartKey, JsonSerializerOptions);

        return EncryptJson(encryptionKey, json);
    }

    public static Dictionary<string, AttributeValue>? Decrypt(string? encryptedPageToken, string decryptionKey)
    {
        if (encryptedPageToken is null)
        {
            return null;
        }

        var json = DecryptJson(decryptionKey, encryptedPageToken);
        var exclusiveStartKey = Document
            .FromJson(json)
            .ToAttributeMap();

        return exclusiveStartKey;   
    }
    
    private static string EncryptJson(string encryptionKey, string json)
    {
        var initVector = new byte[16]; // Should not be empty on production.
        byte[] resultArray;  
  
        using (var aes = CreateAes(encryptionKey, initVector))  
        {  
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
  
            using (var memoryStream = new MemoryStream())  
            {  
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))  
                {  
                    using (var streamWriter = new StreamWriter(cryptoStream))  
                    {  
                        streamWriter.Write(json);  
                    }  
  
                    resultArray = memoryStream.ToArray();  
                }  
            }  
        }  
  
        return Convert.ToBase64String(resultArray);
    }

    private static string DecryptJson(string decryptionKey, string encryptedJson)
    {
        var initVector = new byte[16]; // Should not be empty on production.
        var buffer = Convert.FromBase64String(encryptedJson);

        using var aes = CreateAes(decryptionKey, initVector);
        
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        
        return streamReader.ReadToEnd();
    }

    private static Aes CreateAes(string key, byte[] initVector)
    {
        var aes = Aes.Create();
        
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.IV = initVector;

        return aes;
    }
}