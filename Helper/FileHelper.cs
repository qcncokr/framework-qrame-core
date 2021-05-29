using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Qrame.CoreFX.Helper
{
  public static class FileHelper
  {
    public static void CopyFolder(string sourceFolder, string destFolder)
    {
      if (!Directory.Exists(destFolder))
        Directory.CreateDirectory(destFolder);

      string[] files = Directory.GetFiles(sourceFolder);
      string[] folders = Directory.GetDirectories(sourceFolder);

      foreach (string file in files)
      {
        string name = Path.GetFileName(file);
        string dest = Path.Combine(destFolder, name);
        File.Copy(file, dest);
      }

      // foreach 안에서 재귀 함수를 통해서 폴더 복사 및 파일 복사 진행 완료  
      foreach (string folder in folders)
      {
        string name = Path.GetFileName(folder);
        string dest = Path.Combine(destFolder, name);
        CopyFolder(folder, dest);
      }
    }

    public static void EncryptFile(string FilePath, string CryptographyKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
      string OutputFilePath = Path.GetTempFileName();
      using (Aes encryptor = Aes.Create())
      {
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(CryptographyKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using (FileStream fsOutput = new FileStream(OutputFilePath, FileMode.Create))
        {
          using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
          {
            using (FileStream fsInput = new FileStream(FilePath, FileMode.Open))
            {
              int data;
              while ((data = fsInput.ReadByte()) != -1)
              {
                cs.WriteByte((byte)data);
              }
            }
          }
        }
      }

      if (File.Exists(FilePath)) File.Delete(FilePath);
      File.Move(OutputFilePath, FilePath);
      if (File.Exists(OutputFilePath)) File.Delete(OutputFilePath);
    }

    public static void DecryptFile(string FilePath, string CryptographyKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ")
    {
      string OutputFilePath = Path.GetTempFileName();
      using (Aes encryptor = Aes.Create())
      {
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(CryptographyKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using (FileStream fsInput = new FileStream(FilePath, FileMode.Open))
        {
          using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
          {
            using (FileStream fsOutput = new FileStream(OutputFilePath, FileMode.Create))
            {
              int data;
              while ((data = cs.ReadByte()) != -1)
              {
                fsOutput.WriteByte((byte)data);
              }
            }
          }
        }
      }

      if (File.Exists(FilePath)) File.Delete(FilePath);
      File.Move(OutputFilePath, FilePath);
      if (File.Exists(OutputFilePath)) File.Delete(OutputFilePath);
    }
  }
}
