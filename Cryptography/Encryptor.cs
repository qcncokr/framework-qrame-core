using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Qrame.CoreFX.ExtensionMethod;

namespace Qrame.CoreFX.Cryptography
{
    /// <summary>    
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 암호화 변환 작업을 적용할 클래스 입니다.
    /// </summary>
    public class Encryptor
    {
        private CryptoTransformer transformer;
        private byte[] initialVector;
        private byte[] encryptionKey;

        /// <summary>
        /// <see cref="Encryptor"/> 클래스의 인스턴스 생성시 데이터에 암호화를 적용할 알고리즘을 선택합니다.
        /// </summary>
        /// <param name="algorithmID">데이터에 암호화를 적용할 알고리즘에 대한 열거자입니다.</param>
        public Encryptor(Encryption algorithmID)
        {
            transformer = new CryptoTransformer(algorithmID);
        }

        public string GenerateKey()
        {
            return transformer.GenerateKey().BytesToHex();
        }

        public string GenerateIV()
        {
            return transformer.GenerateIV().BytesToHex();
        }

        /// <summary>
        /// 문자열을 암호화합니다.
        /// </summary>
        /// <param name="data">암호화할 문자열입니다.</param>
        /// <returns>암호화된 문자열을 반환합니다.</returns>
        public string Encrypt(string data)
        {
            string result = "";
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            this.Key = transformer.GenerateKey();
            this.IV = transformer.GenerateIV();

            transformer.IV = this.IV;
            ICryptoTransform transform = transformer.GetEncryptServiceProvider(this.Key);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(byteData, 0, byteData.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();

                result = memoryStream.ToArray().BytesToHex();
            }

            return result;
        }

        /// <summary>
        /// 대상 데이터에 암호화 작업을 수행합니다.
        /// </summary>
        /// <param name="data">암호화할 문자열입니다.</param>
        /// <param name="key">알고리즘에 적용할 암호키입니다.</param>
        /// <param name="iv">알고리즘에 적용할 초기화 벡터키입니다.</param>
        /// <returns>암호화된 문자열을 반환합니다.</returns>
        public string Encrypt(string data, string key, string iv)
        {
            string result = "";
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            this.Key = key.HexToBytes();
            this.IV = iv.HexToBytes();

            transformer.IV = this.IV;
            ICryptoTransform transform = transformer.GetEncryptServiceProvider(this.Key);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(byteData, 0, byteData.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();

                result = memoryStream.ToArray().BytesToHex();
            }

            return result;
        }

        /// <summary>
        /// 대상 데이터에 암호화 작업을 수행합니다.
        /// </summary>
        /// <code>
        /// Encryptor encryptor = new Encryptor(Encryption.Des);
        /// encryptor.IV = "aaaaaaaa".ToBytes(Encoding.UTF8);
        /// pin = Convert.ToBase64String(encryptor.Encrypt("1".ToBytes(Encoding.UTF8), "aaaaaaaa".ToBytes(Encoding.UTF8)));
        /// </code>
        /// <param name="bytesData">암호화 작업을 수행할 데이터입니다.</param>
        /// <param name="bytesKey">대칭 알고리즘에 사용할 비밀 키입니다.</param>
        /// <returns>암호화 작업을 완료한 데이터입니다.</returns>
        public byte[] Encrypt(byte[] bytesData, byte[] bytesKey)
        {
            transformer.IV = initialVector;

            ICryptoTransform transform = transformer.GetEncryptServiceProvider(bytesKey);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream encryptStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                try
                {
                    encryptStream.Write(bytesData, 0, bytesData.Length);
                }
                catch (Exception exception)
                {
                }

                encryptionKey = transformer.Key;
                initialVector = transformer.IV;

                encryptStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// 대칭 알고리즘에 대한 초기화 벡터를 가져오거나, 설정합니다.
        /// </summary>
        public byte[] IV
        {
            get
            {
                return initialVector;
            }
            set
            {
                initialVector = value;
            }
        }

        /// <summary>
        /// 대칭 알고리즘에 대한 비밀 키를 가져옵니다
        /// </summary>
        public byte[] Key
        {
            get
            {
                return encryptionKey;
            }
            set
            {
                encryptionKey = value;
            }
        }
    }
}
