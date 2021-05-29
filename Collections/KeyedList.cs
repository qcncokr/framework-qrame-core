using System;
using System.Collections;
using System.Collections.Generic;

namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 인덱싱을 지원하는 List 타입의 확장 클래스입니다.
    /// </summary>
    /// <typeparam name="K">사전에 있는 키의 형식입니다.</typeparam>
    /// <typeparam name="V">사전에 있는 값의 형식입니다.</typeparam>
    [Serializable]
    public class KeyedList<K, V> : IDictionary<K, V>, IList<KeyValuePair<K, V>>
    {
        /// <summary>
        /// 키와 값의 컬렉션을 나타냅니다.
        /// </summary>
        private Dictionary<K, V> dictionary = new Dictionary<K, V>();

        /// <summary>
        /// 인덱스로 액세스할 수 있는 강력한 형식의 개체 목록을 나타냅니다.목록의 검색, 정렬 및 조작에 사용할 수 있는 메서드를 제공합니다.
        /// </summary>
        private List<KeyValuePair<K, V>> keyValueList = new List<KeyValuePair<K, V>>();

        /// <summary>
        /// KeyedList는 읽기 쓰기 가능한 컬렉션입니다.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// KeyedList의 전체 요소수를 반환합니다.
        /// </summary>
        public int Count
        {
            get { return keyValueList.Count; }
        }

        /// <summary>
        /// 특정 인덱스의 요소의 값을 가져오거나 설정합니다.
        /// </summary>
        /// <param name="index">Item을 관리 하는 0부터 시작하는 인덱스입니다.</param>
        /// <returns>KeyValuePair 타입입니다.</returns>
        public KeyValuePair<K, V> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException("특정 인덱스의 요소의 값을 가져오거나 설정할 때, 잘못된 인덱스 값이 부여되었습니다.");
                }

                return keyValueList[index];
            }
            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException("특정 인덱스의 요소의 값을 가져오거나 설정할 때, 잘못된 인덱스 값이 부여되었습니다.");
                }

                keyValueList[index] = value;
                dictionary[value.Key] = value.Value;
            }
        }

        /// <summary>
        /// 특정 키의 요소의 값을 가져오거나 설정합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>사전에 있는 값의 형식입니다.</returns>
        public virtual V this[K key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key] = value;
                    keyValueList[IndexOf(key)] = new KeyValuePair<K, V>(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        /// <summary>
        /// 키 역할을 담당하는 타입 컬렉션을 반환 합니다.
        /// </summary>
        public ICollection<K> Keys
        {
            get { return dictionary.Keys; }
        }

        /// <summary>
        /// 값 역할을 담당하는 타입 컬렉션을 반환 합니다.
        /// </summary>
        public ICollection<V> Values
        {
            get { return dictionary.Values; }
        }

        /// <summary>
        /// 키 역할을 담당하는 List 컬렉션을 반환 합니다.
        /// </summary>
        public List<K> OrderedKeys
        {
            get
            {
                List<K> result = new List<K>();

                foreach (KeyValuePair<K, V> pair in keyValueList)
                {
                    result.Add(pair.Key);
                }

                return result;
            }
        }

        /// <summary>
        /// 값 역할을 담당하는 List 컬렉션을 반환 합니다.
        /// </summary>
        public List<V> OrderedValues
        {
            get
            {
                List<V> result = new List<V>();

                foreach (KeyValuePair<K, V> pair in keyValueList)
                {
                    result.Add(pair.Value);
                }

                return result;
            }
        }

        /// <summary>
        /// 특정 인덱스의 키 타입을 반환합니다.
        /// </summary>
        /// <param name="index">Item을 관리 하는 0부터 시작하는 인덱스입니다.</param>
        /// <returns>사전에 있는 키의 형식입니다.</returns>
        public K GetKey(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException("특정 인덱스의 키 타입을 반환할 때, 잘못된 인덱스 값이 부여되었습니다.");
            }

            return keyValueList[index].Key;
        }

        /// <summary>
        /// 특정 인덱스의 값 타입을 반환합니다.
        /// </summary>
        /// <param name="index">Item을 관리 하는 0부터 시작하는 인덱스입니다.</param>
        /// <returns>사전에 있는 값의 형식입니다.</returns>
        public V GetValue(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException("특정 인덱스의 키 타입을 반환할 때, 잘못된 인덱스 값이 부여되었습니다.");
            }

            return keyValueList[index].Value;
        }

        /// <summary>
        /// KeyedList에서 특정 항목의 인덱스를 확인합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>목록에 있으면 Item의 인덱스이고, 그렇지 않으면 -1입니다.</returns>
        public int IndexOf(K key)
        {
            int Return = -1;

            for (int i = 0; i < keyValueList.Count; i++)
            {
                if (keyValueList[i].Key.Equals(key))
                {
                    Return = i;
                    break;
                }
            }

            return Return;
        }

        /// <summary>
        /// KeyedList에서 특정 항목의 인덱스를 확인합니다.
        /// </summary>
        /// <param name="pair">KeyValuePair 타입입니다.</param>
        /// <returns>목록에 있으면 Item의 인덱스이고, 그렇지 않으면 -1입니다.</returns>
        public int IndexOf(KeyValuePair<K, V> pair)
        {
            return IndexOf(pair.Key);
        }

        /// <summary>
        /// KeyedList 내부에서 요소들을 관리하는 Dictionary를 반환합니다.
        /// </summary>
        public Dictionary<K, V> ToDictionary
        {
            get { return dictionary; }
        }

        /// <summary>
        /// KeyedList 컬렉션을 초기화 합니다.
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
            keyValueList.Clear();
        }

        /// <summary>
        /// 키 역할을 담당하는 타입이 중복되는지 확인합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>중복 여부입니다.</returns>
        public bool ContainsKey(K key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 키 역할을 담당하는 타입이 중복되는지 확인합니다.
        /// </summary>
        /// <param name="pair">KeyValuePair 타입입니다.</param>
        /// <returns>중복 여부입니다.</returns>
        public bool Contains(KeyValuePair<K, V> pair)
        {
            return dictionary.ContainsKey(pair.Key);
        }

        /// <summary>
        /// 개체를 KeyedList의 끝 부분에 추가합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <param name="value">사전에 있는 값의 형식입니다. 참조 형식에 대해 값은 null이 될 수 있습니다.</param>
        public void Add(K key, V value)
        {
            dictionary.Add(key, value);
            keyValueList.Add(new KeyValuePair<K, V>(key, value));
        }

        /// <summary>
        /// 개체를 KeyedList의 끝 부분에 추가합니다.
        /// </summary>
        /// <param name="pair">KeyValuePair 타입입니다.</param>
        public void Add(KeyValuePair<K, V> pair)
        {
            Add(pair.Key, pair.Value);
        }

        /// <summary>
        /// 대상 배열의 지정된 인덱스부터 시작하여 전체 요소를 호환되는 1차원 배열에 복사합니다.
        /// </summary>
        /// <param name="keyValuePair">KeyValuePair 배열 타입입니다.</param>
        /// <param name="index">복사가 시작되는 0부터 시작하는 인덱스입니다.</param>
        public void CopyTo(KeyValuePair<K, V>[] keyValuePair, int index)
        {
            keyValueList.CopyTo(keyValuePair, index);
        }

        /// <summary>
        /// KeyedList의 지정된 인덱스에 요소를 삽입합니다.
        /// </summary>
        /// <param name="index">Value을 삽입해야 하는 0부터 시작하는 인덱스입니다.</param>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <param name="value">사전에 있는 값의 형식입니다. 참조 형식에 대해 값은 null이 될 수 있습니다.</param>
        public void Insert(int index, K key, V value)
        {
            if ((index < 0) || (index > Count))
            {
                throw new ArgumentOutOfRangeException("KeyedList의 지정된 인덱스에 요소를 삽입할 때, 잘못된 인덱스 값이 부여되었습니다.");
            }

            dictionary.Add(key, value);
            keyValueList.Insert(index, new KeyValuePair<K, V>(key, value));
        }

        /// <summary>
        /// KeyedList의 지정된 인덱스에 요소를 삽입합니다.
        /// </summary>
        /// <param name="index">Value을 삽입해야 하는 0부터 시작하는 인덱스입니다.</param>
        /// <param name="pair">KeyValuePair 타입입니다.</param>
        public void Insert(int index, KeyValuePair<K, V> pair)
        {
            if ((index < 0) || (index > Count))
            {
                throw new ArgumentOutOfRangeException("KeyedList의 지정된 인덱스에 요소를 삽입할 때, 잘못된 인덱스 값이 부여되었습니다.");
            }

            dictionary.Add(pair.Key, pair.Value);
            keyValueList.Insert(index, pair);
        }

        /// <summary>
        /// KeyedList에서 키 타입에 맞는 특정 개체를 제거합니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <returns>Item이 성공적으로 제거되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool Remove(K key)
        {
            bool isRemove = dictionary.Remove(key);

            if (isRemove == true)
            {
                keyValueList.RemoveAt(IndexOf(key));
            }

            return isRemove;
        }

        /// <summary>
        /// KeyedList에서 키 타입에 맞는 특정 개체를 제거합니다.
        /// </summary>
        /// <param name="pair">KeyValuePair 타입입니다.</param>
        /// <returns>Item이 성공적으로 제거되면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool Remove(KeyValuePair<K, V> pair)
        {
            return Remove(pair.Key);
        }

        /// <summary>
        /// KeyedList의 지정한 인덱스에서 요소를 제거합니다.
        /// </summary>
        /// <param name="index">제거할 요소의 0부터 시작하는 인덱스입니다.</param>
        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= Count))
            {
                throw new ArgumentOutOfRangeException("KeyedList의 지정한 인덱스에서 요소를 제거할 때, 잘못된 인덱스 값이 부여되었습니다.");
            }

            dictionary.Remove(keyValueList[index].Key);
            keyValueList.RemoveAt(index);
        }

        /// <summary>
        /// 지정된 키와 연결된 값을 가져옵니다.
        /// </summary>
        /// <param name="key">사전에 있는 키의 형식입니다.</param>
        /// <param name="value">사전에 있는 값의 형식입니다.</param>
        /// <returns>지정된 키와 연결된 값을 가져올 경우 true이고, 그렇지 않으면 false입니다.</returns>
        public bool TryGetValue(K key, out V value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// KeyValuePair 컬렉션의 열거자를 반환합니다.
        /// </summary>
        /// <returns>IEnumerator를 구현한 타입입니다.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return keyValueList.GetEnumerator();
        }

        /// <summary>
        /// KeyValuePair 컬렉션의 열거자를 반환합니다.
        /// </summary>
        /// <returns>IEnumerator를 구현한 타입입니다.</returns>
        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
        {
            return keyValueList.GetEnumerator();
        }
    }
}
