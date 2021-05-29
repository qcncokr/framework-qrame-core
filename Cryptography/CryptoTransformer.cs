using System.Security.Cryptography;

using Qrame.CoreFX.ExtensionMethod;

namespace Qrame.CoreFX.Cryptography
{
    /// <summary>    
    /// 닷넷 프레임워크 기반의 응용 프로그램에서 기본 암호화 변환 작업을 적용할 클래스 입니다.
    /// 적용 가능한 알고리즘은 DES, TripleDES, AES, RC2를 지원합니다.
    /// </summary>
    internal class CryptoTransformer
    {
        /// <summary>
        /// 데이터에 암호화, 복호화를 적용할 알고리즘에 대한 열거자입니다.
        /// </summary>
        private Encryption algorithmID;
        
        /// <summary>
        /// 대칭 알고리즘에 대한 초기화 벡터값 입니다.
        /// </summary>
        private byte[] initialVector;
        
        /// <summary>
        /// 대칭 알고리즘에 대한 비밀 키를 입니다.
        /// </summary>
        private byte[] encryptionKey;

        /// <summary>
        /// <see cref="CryptoTransformer"/> 클래스의 인스턴스 생성시 데이터에 암호화, 복호화를 적용할 알고리즘을 선택합니다.
        /// </summary>
        /// <param name="algorithmID">데이터에 암호화, 복호화를 적용할 알고리즘에 대한 열거자입니다.</param>
        public CryptoTransformer(Encryption encryption)
        {
            algorithmID = encryption;
        }

        /// <summary>
        /// 복호화에 필요한 기본 암호화 변환 작업을 구현한 제공자를 가져옵니다.
        /// </summary>
        /// <param name="bytesKey">대칭 알고리즘에 사용할 비밀 키입니다.</param>
        /// <param name="bytesIV">대칭 알고리즘에 대한 초기화 벡터입니다.</param>
        /// <returns>기본 암호화 변환 작업을 구현한 제공자입니다.</returns>
        internal ICryptoTransform GetDecryptServiceProvider(byte[] bytesKey, byte[] bytesIV)
        {
            initialVector = bytesIV;
            switch (algorithmID)
            {
                case Encryption.Des:
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;
                    des.Key = bytesKey;
                    des.IV = initialVector;
                    return des.CreateDecryptor();

                case Encryption.TripleDes:
                    TripleDES des3 = new TripleDESCryptoServiceProvider();
                    des3.Mode = CipherMode.CBC;
                    return des3.CreateDecryptor(bytesKey, initialVector);

                case Encryption.Rc2:
                    RC2 rc2 = new RC2CryptoServiceProvider();
                    rc2.Mode = CipherMode.CBC;
                    return rc2.CreateDecryptor(bytesKey, initialVector);

                case Encryption.Rijndael:
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;
                    return rijndael.CreateDecryptor(bytesKey, initialVector);
            }

            return null;
        }

        /// <summary>
        /// 암호화에 필요한 기본 암호화 변환 작업을 구현한 제공자를 가져옵니다.
        /// </summary>
        /// <param name="bytesKey">대칭 알고리즘에 사용할 비밀 키입니다.</param>
        /// <returns>기본 암호화 변환 작업을 구현한 제공자입니다.</returns>
        internal ICryptoTransform GetEncryptServiceProvider(byte[] bytesKey)
        {
            switch (algorithmID)
            {
                case Encryption.Des:
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;

                    if (bytesKey == null)
                    {
                        encryptionKey = des.Key;
                    }
                    else
                    {
                        des.Key = bytesKey;
                        encryptionKey = des.Key;
                    }

                    if (initialVector == null)
                    {
                        initialVector = des.IV;
                    }
                    else
                    {
                        des.IV = initialVector;
                    }

                    return des.CreateEncryptor();

                case Encryption.TripleDes:
                    TripleDES des3 = new TripleDESCryptoServiceProvider();
                    des3.Mode = CipherMode.CBC;

                    if (bytesKey == null)
                    {
                        encryptionKey = des3.Key;
                    }
                    else
                    {
                        des3.Key = bytesKey;
                        encryptionKey = des3.Key;
                    }

                    if (initialVector == null)
                    {
                        initialVector = des3.IV;
                    }
                    else
                    {
                        des3.IV = initialVector;
                    }
                    return des3.CreateEncryptor();

                case Encryption.Rc2:
                    RC2 rc2 = new RC2CryptoServiceProvider();
                    rc2.Mode = CipherMode.CBC;

                    if (bytesKey == null)
                    {
                        encryptionKey = rc2.Key;
                    }
                    else
                    {
                        rc2.Key = bytesKey;
                        encryptionKey = rc2.Key;
                    }

                    if (initialVector == null)
                    {
                        initialVector = rc2.IV;
                    }
                    else
                    {
                        rc2.IV = initialVector;
                    }
                    return rc2.CreateEncryptor();

                case Encryption.Rijndael:
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;

                    if (bytesKey == null)
                    {
                        encryptionKey = rijndael.Key;
                    }
                    else
                    {
                        rijndael.Key = bytesKey;
                        encryptionKey = rijndael.Key;
                    }

                    if (initialVector == null)
                    {
                        initialVector = rijndael.IV;
                    }
                    else
                    {
                        rijndael.IV = initialVector;
                    }
                    return rijndael.CreateEncryptor();
            }

            return null;
        }

        public byte[] GenerateKey()
        {
            switch (algorithmID)
            {
                case Encryption.Des:
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;

                    des.GenerateKey();
                    this.Key = des.Key;
                    break;
                case Encryption.TripleDes:
                    TripleDES des3 = new TripleDESCryptoServiceProvider();
                    des3.Mode = CipherMode.CBC;

                    des3.GenerateKey();
                    this.Key = des3.Key;
                    break;
                case Encryption.Rc2:
                    RC2 rc2 = new RC2CryptoServiceProvider();
                    rc2.Mode = CipherMode.CBC;

                    rc2.GenerateKey();
                    this.Key = rc2.Key;
                    break;
                case Encryption.Rijndael:
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;

                    rijndael.GenerateKey();
                    this.Key = rijndael.Key;
                    break;
            }

            return this.Key;
        }

        public byte[] GenerateIV()
        {
            switch (algorithmID)
            {
                case Encryption.Des:
                    DES des = new DESCryptoServiceProvider();
                    des.Mode = CipherMode.CBC;

                    des.GenerateIV();
                    this.IV = des.IV;
                    break;
                case Encryption.TripleDes:
                    TripleDES des3 = new TripleDESCryptoServiceProvider();
                    des3.Mode = CipherMode.CBC;

                    des3.GenerateIV();
                    this.IV = des3.IV;
                    break;
                case Encryption.Rc2:
                    RC2 rc2 = new RC2CryptoServiceProvider();
                    rc2.Mode = CipherMode.CBC;

                    rc2.GenerateIV();
                    this.IV = rc2.IV;
                    break;
                case Encryption.Rijndael:
                    Rijndael rijndael = new RijndaelManaged();
                    rijndael.Mode = CipherMode.CBC;

                    rijndael.GenerateIV();
                    this.IV = rijndael.IV;
                    break;
            }

            return this.IV;
        }

        /// <summary>
        /// 대칭 알고리즘에 대한 초기화 벡터를 가져오거나 설정합니다.
        /// </summary>
        internal byte[] IV
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
        internal byte[] Key
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
