using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Qrame.CoreFX.ExtensionMethod;

namespace Qrame.CoreFX.Cryptography
{
    /// <summary>    
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 복호화 변환 작업을 적용할 클래스 입니다.
    /// </summary>
    public class Decryptor
    {
        private CryptoTransformer transformer;
        private byte[] initialVector;
        private byte[] encryptionKey;

        /// <summary>
        /// <see cref="Decryptor"/> 클래스의 인스턴스 생성시 데이터에 복호화를 적용할 알고리즘을 선택합니다.
        /// </summary>
        /// <param name="algorithmID">데이터에 복호화를 적용할 알고리즘에 대한 열거자입니다.</param>
        public Decryptor(Encryption algorithmID)
        {
            transformer = new CryptoTransformer(algorithmID);
        }

        /// <summary>
        /// 대상 데이터에 복호화 작업을 수행합니다.
        /// </summary>
        /// <param name="data">복호화 작업을 수행할 데이터입니다.</param>
        /// <returns>복호화 작업을 완료한 데이터입니다.</returns>
        public string Decrypt(string data)
        {
            byte[] byteData = data.HexToBytes();

            transformer.IV = this.IV;
            ICryptoTransform transform = transformer.GetDecryptServiceProvider(this.Key, this.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream decryptStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                decryptStream.Write(byteData, 0, byteData.Length);

                decryptStream.FlushFinalBlock();
                decryptStream.Close();

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 대상 데이터에 복호화 작업을 수행합니다.
        /// </summary>
        /// <param name="data">복호화 작업을 수행할 데이터입니다.</param>
        /// <param name="key">대칭 알고리즘에 사용할 비밀 키입니다.</param>
        /// <param name="iv">대칭 알고리즘에 사용할 비밀 키입니다.</param>
        /// <returns>복호화 작업을 완료한 데이터입니다.</returns>
        public string Decrypt(string data, string key, string iv)
        {
            byte[] byteData = data.HexToBytes();
            byte[] byteKey = key.HexToBytes();
            byte[] byteIV = iv.HexToBytes();

            transformer.IV = byteIV;
            ICryptoTransform transform = transformer.GetDecryptServiceProvider(byteKey, byteIV);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream decryptStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                decryptStream.Write(byteData, 0, byteData.Length);

                decryptStream.FlushFinalBlock();
                decryptStream.Close();

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// 대상 데이터에 복호화 작업을 수행합니다.
        /// <code>
        /// Decryptor decryptor = new Decryptor(Encryption.Des);
        /// decryptor.IV = "aaaaaaaa".ToBytes(Encoding.UTF8);
        /// pin = Encoding.UTF8.GetString(decryptor.Decrypt(Convert.FromBase64String(pin), decriptKey.ToBytes(Encoding.UTF8)));
        /// </code>
        /// </summary>
        /// <param name="bytesData">복호화 작업을 수행할 데이터입니다.</param>
        /// <param name="bytesKey">대칭 알고리즘에 사용할 비밀 키입니다.</param>
        /// <returns>복호화 작업을 완료한 데이터입니다.</returns>
        public byte[] Decrypt(byte[] bytesData, byte[] bytesKey)
        {
            transformer.IV = initialVector;

            ICryptoTransform transform = transformer.GetDecryptServiceProvider(bytesKey, transformer.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            using (CryptoStream decryptStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                decryptStream.Write(bytesData, 0, bytesData.Length);

                decryptStream.FlushFinalBlock();
                decryptStream.Close();

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
