using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace Qrame.CoreFX.Cryptography.CryptoProvider
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 Secure Hash 알고리즘을 적용하여 변환 작업을 적용할 클래스 입니다.
    /// </summary>
    public sealed class ShaCrypto
    {
        /// <summary>
        /// 지정한 해시 알고리즘을 통해 해시 데이터를 생성합니다.
        /// </summary>
        /// <param name="data">해시 데이터를 생성할 문자열입니다.</param>
        /// <param name="sha">해시 알고리즘을 적용할 열거자입니다.</param>
        /// <returns>해시 문자열입니다.</returns>
        public static string GetHashData(string data, ShaEncryption sha = ShaEncryption.SHA256)
        {
            string result = "";
            switch (sha)
            {
                case ShaEncryption.MD5:
                    result = GetMD5HashData(data);
                    break;
                case ShaEncryption.SHA1:
                    result = GetSHA1HashData(data);
                    break;
                case ShaEncryption.SHA256:
                    result = GetSHA256HashData(data);
                    break;
                case ShaEncryption.SHA512:
                    result = GetSHA512HashData(data);
                    break;
            }
            return result;
        }
        /// <summary>
        /// 지정한 해시 알고리즘을 통해 입력된 문자열과 해시 데이터를 비교합니다.
        /// </summary>
        /// <param name="data">입력 문자열입니다.</param>
        /// <param name="data">해시 데이터 문자열입니다.</param>
        /// <param name="sha">해시 알고리즘을 적용할 열거자입니다.</param>
        /// <returns>입력된 문자열과 해시 데이터가 동일하면 true를, 아니면 false를 반환합니다.</returns>
        public static bool ValidateHashData(string data, string storedHashData, ShaEncryption sha)
        {
            bool result = false;
            switch (sha)
            {
                case ShaEncryption.MD5:
                    result = ValidateMD5HashData(data, storedHashData);
                    break;
                case ShaEncryption.SHA1:
                    result = ValidateSHA1HashData(data, storedHashData);
                    break;
                case ShaEncryption.SHA256:
                    result = ValidateSHA256HashData(data, storedHashData);
                    break;
                case ShaEncryption.SHA512:
                    result = ValidateSHA512HashData(data, storedHashData);
                    break;
            }
            return result;
        }
        private static string GetMD5HashData(string data)
        {
            MD5 md5 = MD5.Create();
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString("x2"));
            }
            return returnValue.ToString();
        }
        private static string GetSHA1HashData(string data)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString("x2"));
            }
            return returnValue.ToString();
        }
        private static string GetSHA256HashData(string data)
        {
            SHA256 shaM = new SHA256Managed();
            byte[] hashData = shaM.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString("x2"));
            }
            return returnValue.ToString();
        }
        private static string GetSHA512HashData(string data)
        {
            SHA512 shaM = new SHA512Managed();
            byte[] hashData = shaM.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString("x2"));
            }
            return returnValue.ToString();
        }

        private static bool ValidateMD5HashData(string data, string storedHashData)
        {
            string getHashdata = GetMD5HashData(data);
            if (string.Compare(getHashdata, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool ValidateSHA1HashData(string data, string storedHashData)
        {
            string getHashdata = GetSHA1HashData(data);
            if (string.Compare(getHashdata, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateSHA256HashData(string data, string storedHashData)
        {
            string getHashdata = GetSHA256HashData(data);
            if (string.Compare(getHashdata, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool ValidateSHA512HashData(string data, string storedHashData)
        {
            string getHashdata = GetSHA512HashData(data);
            if (string.Compare(getHashdata, storedHashData) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}