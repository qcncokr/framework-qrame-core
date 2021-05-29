using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using Qrame.CoreFX.ExtensionMethod;

namespace Qrame.CoreFX.Cryptography
{
    public static class AES256
    {
        private static Encoding encoding = Encoding.Default;
        public static DateTime? KMSKeyFault = null;
        public static bool IsKMSSecretKey = false;
        public static byte[] SecretKey = new byte[32];

        public static void SettingSecretKey(byte[] secretKey)
        {
            SecretKey = secretKey;
        }

        public static string BytesToBitHexa(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        public static byte[] HexaToBytes(string hex)
        {
            int length = hex.Length;

            byte[] result = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                result[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return result;
        }

        private static void SecretKeyVarify(byte[] testSecretKey)
        {
            if (testSecretKey != null)
            {
                SecretKey = testSecretKey;
            }

            if (SecretKey == null)
            {
                throw new Exception("AES256 비밀키를 설정하지 못했습니다");
            }
        }

        public static byte[] Encrypt(byte[] message, CryptographyResultType resultType, byte[] testSecretKey = null)
        {
            SecretKeyVarify(testSecretKey);

            byte[] result = null;
            if (message == null)
            {
                return result;
            }
            else
            {
                byte[] buffer = null;
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = SecretKey;
                    aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };

                    var encrypt = aes.CreateEncryptor();

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        cs.Write(message, 0, message.Length);
                        cs.FlushFinalBlock();

                        buffer = ms.ToArray();
                    }
                }

                switch (resultType)
                {
                    case CryptographyResultType.Hexa:
                        result = encoding.GetBytes(BytesToBitHexa(buffer));
                        break;
                    case CryptographyResultType.Base64:
                        result = encoding.GetBytes(Convert.ToBase64String(buffer));
                        break;
                    case CryptographyResultType.Binary:
                        result = buffer;
                        break;
                }
            }

            return result;
        }

        public static string Encrypt(string message, byte[] testSecretKey = null)
        {
            SecretKeyVarify(testSecretKey);

            string result = "";
            if (string.IsNullOrEmpty(message) == true)
            {
                return result;
            }
            else
            {
                byte[] buffer = null;
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = SecretKey;
                    aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };

                    var encrypt = aes.CreateEncryptor();

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] messageBytes = encoding.GetBytes(message);
                        cs.Write(messageBytes, 0, messageBytes.Length);
                        cs.FlushFinalBlock();

                        buffer = ms.ToArray();
                    }
                }

                result = BytesToBitHexa(buffer);
            }

            return result;
        }
        
        public static byte[] EncryptByte(string message, byte[] testSecretKey = null)
        {
            SecretKeyVarify(testSecretKey);

            byte[] result = null;
            if (string.IsNullOrEmpty(message) == true)
            {
                return result;
            }
            else
            {
                byte[] buffer = null;
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = SecretKey;
                    aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };

                    var encrypt = aes.CreateEncryptor();

                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] messageBytes = encoding.GetBytes(message);
                        cs.Write(messageBytes, 0, messageBytes.Length);
                        cs.FlushFinalBlock();

                        buffer = ms.ToArray();
                    }
                }

                result = buffer;
            }

            return result;
        }

        public static byte[] Decrypt(byte[] message, CryptographyResultType resultType, byte[] testSecretKey = null)
        {
            SecretKeyVarify(testSecretKey);

            byte[] decryptData = null;
            if (message == null)
            {
                return decryptData;
            }
            else
            {
                switch (resultType)
                {
                    case CryptographyResultType.Hexa:
                        decryptData = HexaToBytes(encoding.GetString(message));
                        break;
                    case CryptographyResultType.Base64:
                        decryptData = Convert.FromBase64String(encoding.GetString(message));
                        break;
                    case CryptographyResultType.Binary:
                        decryptData = message;
                        break;
                }

                byte[] buffer = new byte[decryptData.Length];
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = SecretKey;
                    aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    var decrypt = aes.CreateDecryptor();
                    using (var ms = new MemoryStream(decryptData))
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        cs.Read(buffer, 0, buffer.Length);
                    }
                }

                decryptData = EmptyLTrim(buffer);
            }

            return decryptData;
        }

        public static string Decrypt(string message, byte[] testSecretKey = null)
        {
            SecretKeyVarify(testSecretKey);

            string result = "";
            if (string.IsNullOrEmpty(message) == true)
            {
                return result;
            }
            else
            {
                byte[] encryptData = HexaToBytes(message);

                byte[] buffer = new byte[encryptData.Length];
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = SecretKey;
                    aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    var decrypt = aes.CreateDecryptor();
                    using (var ms = new MemoryStream(encryptData))
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        cs.Read(buffer, 0, buffer.Length);
                    }
                }

                buffer = EmptyLTrim(buffer);

                result = encoding.GetString(buffer);
            }

            return result;
        }

        public static string DecryptByte(byte[] message, byte[] testSecretKey = null)
        {
            SecretKeyVarify(testSecretKey);

            string result = null;
            if (message == null)
            {
                return result;
            }
            else
            {
                byte[] encryptData = message;

                byte[] buffer = new byte[encryptData.Length];
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = SecretKey;
                    aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    var decrypt = aes.CreateDecryptor();
                    using (var ms = new MemoryStream(encryptData))
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        cs.Read(buffer, 0, buffer.Length);
                    }
                }

                buffer = EmptyLTrim(buffer);

                result = encoding.GetString(buffer);
            }

            return result;
        }

        /// <summary>
        /// 복호화시 발생하는 끝의 16 바이트에서 null 값을 확인하여 삭제 처리
        /// </summary>
        /// <param name="buffer">AES256 복호화 바이트 배열</param>
        /// <returns>null 값 삭제 처리된 AES256 복호화 바이트 배열</returns>
        private static byte[] EmptyLTrim(byte[] buffer)
        {
            int cryptoBuffer = 16;
            if (buffer.Length > cryptoBuffer)
            {
                byte[] tempDecrypt = new byte[buffer.Length - cryptoBuffer];
                Buffer.BlockCopy(buffer, 0, tempDecrypt, 0, buffer.Length - cryptoBuffer);

                byte[] nullCheck = new byte[cryptoBuffer];
                Buffer.BlockCopy(buffer, buffer.Length - cryptoBuffer, nullCheck, 0, cryptoBuffer);

                int length = nullCheck.Length;
                for (int i = 0; i < cryptoBuffer; i++)
                {
                    int findIndex = nullCheck.Find(new byte[1] { 0x00 });

                    if (findIndex == -1)
                    {
                        break;
                    }

                    nullCheck = nullCheck.Replace(new byte[1] { 0x00 }, new byte[0]);
                }

                buffer = tempDecrypt.Combine(nullCheck);
            }
            return buffer;
        }
    }
}
