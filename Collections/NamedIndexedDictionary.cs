namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 인덱싱을 지원하는 Dictionary 타입의 확장 클래스입니다.
    /// </summary>
    /// <typeparam name="T">사전에 있는 값의 형식입니다.</typeparam>
    public class NamedIndexedDictionary<T> : IndexedDictionary<string, T>
    {
        /// <summary>
        /// 키에 대한 대소문자 구별 여부입니다.
        /// </summary>
        private bool isCaseInsensitive = false;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public NamedIndexedDictionary()
        {
        }

        /// <summary>
        /// 키에 대한 대소문자 구별 여부 옵션을 포함하는 생성자 입니다.
        /// </summary>
        /// <param name="caseInsensitive">키에 대한 대소문자 구별 여부입니다.</param>
        public NamedIndexedDictionary(bool caseInsensitive)
        {
            isCaseInsensitive = caseInsensitive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseInsensitive">키에 대한 대소문자 구별 여부입니다.</param>
        /// <param name="duplicateKeys">중복키 입력시 기존 아이템을 삭제하는 옵션입니다.</param>
        public NamedIndexedDictionary(bool caseInsensitive, bool duplicateKeys)
            : this(caseInsensitive)
        {
            isDuplicateKeys = duplicateKeys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseInsensitive">키에 대한 대소문자 구별 여부입니다.</param>
        /// <param name="duplicateKeys">중복키 입력시 기존 아이템을 삭제하는 옵션입니다.</param>
        /// <param name="errorOnInvalidRemove">예외가 발생하면 해당 아이템을 삭제하는 옵션입니다.</param>
        public NamedIndexedDictionary(bool caseInsensitive, bool duplicateKeys, bool errorOnInvalidRemove)
            : this(caseInsensitive, duplicateKeys)
        {
            isErrorOnInvalidRemove = errorOnInvalidRemove;
        }

        /// <summary>
        /// 키에 대한 대소문자 구별 여부를 조회합니다.
        /// </summary>
        public bool CaseInsensitive
        {
            get { return isCaseInsensitive; }
        }

        /// <summary>
        /// 키 타입을 조회합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>키 값입니다.</returns>
        protected override string TransformKey(string key)
        {
            if (isCaseInsensitive == true)
            {
                return key.ToUpperInvariant();
            }

            return key;
        }
    }
}
