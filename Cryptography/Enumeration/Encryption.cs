

namespace Qrame.CoreFX.Cryptography
{
    public enum CryptographyResultType
    {
        Hexa,
        Base64,
        Binary
    }

    /// <summary>    
    /// Qrame.CoreFX.Cryptography 어셈블리를 통해 데이터에 암호화, 복호화를 적용할 알고리즘에 대한 열거자입니다.
    /// </summary>
    public enum Encryption
    {
        /// <summary>
        /// DES(Data Encryption Standard)는 미국 정부에서 국가 표준으로 정한 암/복호화 알고리즘입니다.
        /// </summary>
        Des,

        /// <summary>
        /// Rivest가 개발한 가변 길이의 64비트 키를 가진 블럭 암호 알고리즘입니다.
        /// </summary>
        Rc2,

        /// <summary>
        /// Rijndael(AES)는 미국 정부에서 민감한 정보들을 암호화하는 데 사용되는 표준 암/복호화 알고리즘입니다.
        /// </summary>
        Rijndael,

        /// <summary>
        /// DES 알고리즘을 세 번 반복해서 사용하는 알고리즘입니다.
        /// </summary>
        TripleDes
    };

    /// <summary>    
    /// 단방향 문자열 데이터에 암호화를 적용할 알고리즘에 대한 열거자입니다.
    /// </summary>
    public enum ShaEncryption
    {
        /// <summary>
        /// MD5 해시 알고리즘입니다.
        /// </summary>
        MD5,

        /// <summary>
        /// 입력 데이터에 대한 SHA1 해시를 계산합니다.
        /// </summary>
        SHA1,

        /// <summary>
        /// 입력 데이터에 대한 SHA256 해시를 계산합니다.
        /// </summary>
        SHA256,

        /// <summary>
        /// 입력 데이터에 대한 SHA512 해시를 계산합니다.
        /// </summary>
        SHA512
    };
}
