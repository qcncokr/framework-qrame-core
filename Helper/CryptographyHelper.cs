using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Qrame.CoreFX.Helper
{
  public static class CryptographyHelper
  {
    public static string Encrypt(string encryptString, string cryptographyKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
      string EncryptionKey = cryptographyKey;
      byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);

      try
      {
        using (Aes encryptor = Aes.Create())
        {
          Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
          encryptor.Key = pdb.GetBytes(32);
          encryptor.IV = pdb.GetBytes(16);
          using (MemoryStream ms = new MemoryStream())
          {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
              cs.Write(clearBytes, 0, clearBytes.Length);
            }
            encryptString = Convert.ToBase64String(ms.ToArray());
          }
        }
      }
      catch (Exception)
      {
        encryptString = "";
      }

      return encryptString;
    }

    public static string Decrypt(string cipherText, string cryptographyKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
      string EncryptionKey = cryptographyKey;
      cipherText = cipherText.Replace(" ", "+");
      byte[] cipherBytes = Convert.FromBase64String(cipherText);

      try
      {
        using (Aes encryptor = Aes.Create())
        {
          Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
          encryptor.Key = pdb.GetBytes(32);
          encryptor.IV = pdb.GetBytes(16);
          using (MemoryStream ms = new MemoryStream())
          {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
              cs.Write(cipherBytes, 0, cipherBytes.Length);
            }
            cipherText = Encoding.Unicode.GetString(ms.ToArray());
          }
        }
      }
      catch (Exception)
      {
        cipherText = "";
      }

      return cipherText;
    }
  }
}
