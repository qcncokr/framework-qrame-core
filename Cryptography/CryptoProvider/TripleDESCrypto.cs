using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Qrame.CoreFX.ExtensionMethod;

namespace Qrame.CoreFX.Cryptography.CryptoProvider
{
    /// <summary>
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 TripleDES 알고리즘을 적용하여 기본 암호화, 복호화 변환 작업을 적용할 클래스 입니다.
    /// </summary>
    public static class TripleDESCrypto
    {
        /// <summary>
        /// 알고리즘에 사용할 기본 초기화 키값입니다.
        /// </summary>
        public static string DefaultKey = "EA81AA1D5FC1EC53E84F30AA746139EEBAFF8A9B76638895";

        /// <summary>
        /// 알고리즘에 사용할 기본 초기화 벡터값입니다.
        /// </summary>
        public static string DefaultIV = "87AF7EA221F3FFF5";
        private static TripleDESCryptoServiceProvider des3;

        /// <summary>
        /// 기본 생성자입니다.
        /// </summary>
        static TripleDESCrypto()
        {
            des3 = new TripleDESCryptoServiceProvider();
            des3.Mode = CipherMode.CBC;
        }

        /// <summary>
        /// 알고리즘에 적용할 임의의 암호키를 생성합니다.
        /// </summary>
        /// <returns>바이트 시퀀스로 인코딩된 값을 문자열로 변환합니다.</returns>
        public static string GenerateKey()
        {
            des3.GenerateKey();
            return des3.Key.ToHex();
        }

        /// <summary>
        /// 알고리즘에 적용할 대칭 알고리즘에 대한 초기화 벡터를 생성합니다.
        /// </summary>
        /// <returns>바이트 시퀀스로 인코딩된 값을 문자열로 변환합니다.</returns>
        public static string GenerateIV()
        {
            des3.GenerateIV();
            return des3.IV.ToHex();
        }

        /// <summary>
        /// 문자열을 암호화합니다.
        /// </summary>
        /// <param name="data">암호화할 문자열입니다.</param>
        /// <param name="key">알고리즘에 적용할 암호키입니다.</param>
        /// <param name="iv">알고리즘에 적용할 초기화 벡터키입니다.</param>
        /// <returns>암호화된 문자열을 반환합니다.</returns>
        public static string Encrypt(string data, string key, string iv)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            byte[] byteKey = key.HexToBytes();
            byte[] byteIV = iv.HexToBytes();

            using (MemoryStream stream = new MemoryStream())
            using (CryptoStream encStream = new CryptoStream(stream, des3.CreateEncryptor(byteKey, byteIV), CryptoStreamMode.Write))
            {
                encStream.Write(byteData, 0, byteData.Length);
                encStream.FlushFinalBlock();
                encStream.Close();

                return stream.ToArray().ToHex();
            }
        }

        /// <summary>
        /// 문자열을 복호화합니다.
        /// </summary>
        /// <param name="data">복호화할 문자열입니다.</param>
        /// <param name="key">알고리즘에 적용할 암호키입니다.</param>
        /// <param name="iv">알고리즘에 적용할 초기화 벡터키입니다.</param>
        /// <returns>복호화된 문자열을 반환합니다.</returns>
        public static string Decrypt(string data, string key, string iv)
        {
            byte[] byteData = data.HexToBytes();
            byte[] byteKey = key.HexToBytes();
            byte[] byteIV = iv.HexToBytes();

            using (MemoryStream stream = new MemoryStream())
            using (CryptoStream encStream = new CryptoStream(stream, des3.CreateDecryptor(byteKey, byteIV), CryptoStreamMode.Write))
            {
                encStream.Write(byteData, 0, byteData.Length);
                encStream.FlushFinalBlock();
                encStream.Close();

                return Encoding.ASCII.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 문자열을 암호화합니다.
        /// </summary>
        /// <param name="data">암호화할 문자열입니다.</param>
        /// <returns>암호화된 문자열을 반환합니다.</returns>ㅕ
        public static string Encrypt(string data)
        {
            return Encrypt(data, DefaultKey, DefaultIV);
        }

        /// <summary>
        /// 문자열을 복호화합니다.
        /// </summary>
        /// <param name="data">복호화할 문자열입니다.</param>
        /// <returns>복호화된 문자열을 반환합니다.</returns>
        public static string Decrypt(string data)
        {
            return Decrypt(data, DefaultKey, DefaultIV);
        }
    }
}
