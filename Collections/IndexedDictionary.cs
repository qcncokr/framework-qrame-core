using System;
using System.Collections.Generic;
using Qrame.CoreFX.Exceptions;

namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 인덱싱을 지원하는 Dictionary 타입의 확장 클래스입니다.
    /// </summary>
    /// <typeparam name="T">사전에 있는 키의 형식입니다.</typeparam>
    /// <typeparam name="V">사전에 있는 값의 형식입니다.</typeparam>
    public class IndexedDictionary<T, V> : Dictionary<T, V>
    {
        /// <summary>
        /// Item을 관리 하는 컬렉션입니다.
        /// </summary>
        protected List<T> itemCollection = new List<T>();

        /// <summary>
        /// 중복키 입력시 기존 아이템을 삭제하고 새로 입력할 것인지 여부입니다.
        /// </summary>
        protected bool isDuplicateKeys = false;

        /// <summary>
        /// 특정 아이템에서 작업수행시 예외가 발생하면 해당 아이템을 삭제할 것인지 여부입니다.
        /// </summary>
        protected bool isErrorOnInvalidRemove = false;

        /// <summary>
        /// 기본 생성자 입니다. 
        /// </summary>
        public IndexedDictionary()
        {
            ValidateKeyType();
        }

        /// <summary>
        /// 중복키 입력시 기존 아이템을 삭제하는 옵션을 포함 하는 생성자 입니다.
        /// </summary>
        /// <param name="IsDuplicateKeys">중복키 입력시 기존 아이템을 삭제하는 옵션입니다.</param>
        public IndexedDictionary(bool IsDuplicateKeys)
        {
            isDuplicateKeys = IsDuplicateKeys;
        }

        /// <summary>
        /// 중복키 입력시 기존 아이템을 삭제하는 옵션과 예외가 발생하면 해당 아이템을 삭제하는 옵션을 포함 하는 생성자 입니다.
        /// </summary>
        /// <param name="IsDuplicateKeys">중복키 입력시 기존 아이템을 삭제하는 옵션입니다.</param>
        /// <param name="IsErrorOnInvalidRemove">예외가 발생하면 해당 아이템을 삭제하는 옵션입니다.</param>
        public IndexedDictionary(bool IsDuplicateKeys, bool IsErrorOnInvalidRemove)
            : this(IsDuplicateKeys)
        {
            isErrorOnInvalidRemove = IsErrorOnInvalidRemove;
        }

        /// <summary>
        /// 중복키 입력시 기존 아이템을 삭제하는 옵션을 조회합니다.
        /// </summary>
        public bool IsDuplicateKeys
        {
            get { return isDuplicateKeys; }
        }

        /// <summary>
        /// 예외가 발생하면 해당 아이템을 삭제하는 옵션을 조회합니다.
        /// </summary>
        public bool IsErrorOnInvalidRemove
        {
            get { return isErrorOnInvalidRemove; }
        }

        /// <summary>
        /// 열거자의 키 타입에 맞는 요소를 가져옵니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>사전에 있는 값의 형식입니다.</returns>
        public new V this[T key]
        {
            get
            {
                return base[TransformKey(key)];
            }
            set
            {
                if (!base.ContainsKey(TransformKey(key)))
                {
                    Add(key, value);
                }
                base[TransformKey(key)] = value;
            }
        }

        /// <summary>
        /// 열거자의 값 타입에 맞는 요소를 가져옵니다.
        /// </summary>
        /// <param name="index">Item을 관리 하는 0부터 시작하는 인덱스입니다.</param>
        /// <returns>사전에 있는 값의 형식입니다.</returns>
        public V this[int index]
        {
            get
            {
                return this[itemCollection[index]];
            }
            set
            {
                this[itemCollection[index]] = value;
            }
        }

        /// <summary>
        /// Dictionary 키에 숫자 데이터 타입을 사용하고 있는지 검증합니다.
        /// </summary>
        /// <exception cref="Qrame.CoreFX.Exceptions.IndexedDictionaryException">
        /// IndexedDictionary 컬렉션에 숫자 데이터 타입을 사용 할 경우 발생합니다.
        /// </exception>
        private static void ValidateKeyType()
        {
            if (typeof(T) == typeof(int))
            {
                throw new IndexedDictionaryException("숫자 데이터 타입은 사용 할 수 없습니다.");
            }
        }

        /// <summary>
        /// 키 타입을 조회합니다.(자식 클래스에서 옵션을 부여할 수 있도록 virtual로 선언합니다.)
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>사전에 있는 키의 형식입니다.</returns>
        protected virtual T TransformKey(T key)
        {
            return key;
        }

        /// <summary>
        /// 키 역할을 담당하는 타입이 중복되는지 확인합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>중복 여부입니다.</returns>
        public bool Contains(T key)
        {
            return base.ContainsKey(TransformKey(key));
        }

        /// <summary>
        /// 개체를 IndexedDictionary의 끝 부분에 추가합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <param name="item">사전에 있는 값의 형식입니다. 참조 형식에 대해 값은 null이 될 수 있습니다.</param>
        public virtual new void Add(T key, V item)
        {
            AddAt(-1, key, item);
        }

        /// <summary>
        /// 개체를 IndexedDictionary의 지정된 인덱스에 요소를 삽입합니다.
        /// </summary>
        /// <param name="index">Item을 삽입해야 하는 0부터 시작하는 인덱스입니다. -1일 경우 끝 부분에 추가합니다.</param>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <param name="item">사전에 있는 값의 형식입니다. 참조 형식에 대해 값은 null이 될 수 있습니다.</param>
        public virtual void AddAt(int index, T key, V item)
        {
            if (isDuplicateKeys == true && ContainsKey(key) == true)
            {
                Remove(key);
            }

            base.Add(TransformKey(key), item);

            if (index != -1)
            {
                itemCollection.Insert(index, key);
            }
            else
            {
                itemCollection.Add(key);
            }
        }

        /// <summary>
        /// IndexedDictionary의 지정한 인덱스에서 요소를 제거합니다.
        /// </summary>
        /// <param name="index">Item을 삭제해야 하는 0부터 시작하는 인덱스입니다.</param>
        public virtual void RemoveAt(int index)
        {
            if (isErrorOnInvalidRemove == true)
            {
                if (index < 0 || index >= itemCollection.Count)
                {
                    throw new Exception("IndexedDictionary의 지정한 인덱스에서 요소를 제거할 때, 잘못된 인덱스 값이 부여되었습니다.");
                }
            }

            T Key = itemCollection[index];

            Remove(Key);
        }

        /// <summary>
        /// IndexedDictionary에서 키 타입에 맞는 특정 개체를 제거합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        public new void Remove(T key)
        {
            bool IsContains = ContainsKey(key);

            if (isErrorOnInvalidRemove == true && IsContains == false)
            {
                throw new Exception("IndexedDictionary의 지정한 인덱스에서 요소를 제거할 때, 잘못된 인덱스 값이 부여되었습니다.");
            }
            else if (IsContains == false)
            {
                return;
            }

            itemCollection.Remove(TransformKey(key));

            base.Remove(TransformKey(key));
        }

        /// <summary>
        /// 키 역할을 담당하는 타입이 중복되는지 확인합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>키 역할을 담당하는 타입이 중복되면 true이고, 그렇지 않으면 false입니다.</returns>
        public new bool ContainsKey(T key)
        {
            return base.ContainsKey(TransformKey(key));
        }

        /// <summary>
        /// IndexedDictionary의 지정한 인덱스에서 요소를 조회합니다.
        /// </summary>
        /// <param name="index">Item을 관리 하는 0부터 시작하는 인덱스입니다.</param>
        /// <returns>지정한 인덱스에서 요소를 반환합니다.</returns>
        public T KeyAtIndex(int index)
        {
            return itemCollection[index];
        }

        /// <summary>
        /// IndexedDictionary에서 요소를 모두 제거합니다.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            itemCollection = new List<T>();
        }
    }
}
