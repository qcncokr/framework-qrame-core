using System;
using System.Collections.Generic;

namespace Qrame.CoreFX.Collections
{
    /// <summary>
    /// 닷넷 응용 프로그램 구성 파일에서 설정값에 대한 컬렉션을 구성하는 클래스입니다.
    /// </summary>
    public class SettingsCollection : IEnumerable<ISetting>
    {
        /// <summary>
        /// 닷넷 응용 프로그램 구성 파일에서 설정값을 조회하기 위한 인터페이스 정의입니다.
        /// </summary>
        private ISettingsSource settingsSource;

        /// <summary>
        /// 인스턴스 생성시, 닷넷 응용 프로그램 구성 파일에서 설정값을 조회하기 위한 인터페이스를 포함하는 생성자 입니다.
        /// </summary>
        /// <param name="source">ISettingsSource를 구현한 타입입니다.</param>
        public SettingsCollection(ISettingsSource source)
        {
            settingsSource = source;
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 string 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 string 값입니다.</returns>
        public string this[string settingName]
        {
            get { return GetString(settingName); }
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 string 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 string 값입니다.</returns>
        public string this[string settingName, string defaultValue]
        {
            get { return GetString(settingName, defaultValue); }
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 bool 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 bool 값입니다.</returns>
        public bool this[string settingName, bool defaultValue]
        {
            get { return GetBoolean(settingName, defaultValue); }
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 decimal 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 decimal 값입니다.</returns>
        public decimal this[string settingName, decimal defaultValue]
        {
            get { return GetDecimal(settingName, defaultValue); }
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 int 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 int 값입니다.</returns>
        public int this[string settingName, int defaultValue]
        {
            get { return GetInt(settingName, defaultValue); }
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 DateTime 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 DateTime 값입니다.</returns>
        public DateTime this[string settingName, DateTime defaultValue]
        {
            get { return GetDate(settingName, defaultValue); }
        }

        /// <summary>
        /// 구성 항목이 존재 하는지 검사합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목이 존재하면 true이고, 그렇지 않으면 false입니다.</returns>
        public bool Contains(string settingName)
        {
            return Get(settingName).Value != null;
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 ISetting를 구현한 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>ISetting 구현한 타입입니다.</returns>
        protected ISetting Get(string settingName)
        {
            return settingsSource.Get(settingName);
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 bool 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 bool 값입니다.</returns>
        public bool GetBoolean(string settingName, bool defaultValue)
        {
            if (string.IsNullOrEmpty(settingName) == false && Contains(settingName) == true)
            {
                return Reflector.StringToTypedValue<bool>((string)GetValue(settingName));
            }

            return defaultValue;
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 bool 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 bool 값입니다.</returns>
        public bool GetBoolean(string settingName)
        {
            return GetBoolean(settingName, false);
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 decimal 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 decimal 값입니다.</returns>
        public decimal GetDecimal(string settingName, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(settingName) == false && Contains(settingName) == true)
            {
                return Reflector.StringToTypedValue<decimal>((string)GetValue(settingName));
            }

            return defaultValue;
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 decimal 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 decimal 값입니다.</returns>
        public decimal GetDecimal(string settingName)
        {
            return GetDecimal(settingName, 0M);
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 int 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 int 값입니다.</returns>
        public int GetInt(string settingName, int defaultValue)
        {
            if (string.IsNullOrEmpty(settingName) == false && Contains(settingName) == true)
            {
                return Reflector.StringToTypedValue<int>((string)GetValue(settingName));
            }

            return defaultValue;
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 int 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 int 값입니다.</returns>
        public int GetInt(string settingName)
        {
            return GetInt(settingName, 0);
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 DateTime 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 DateTime 값입니다.</returns>
        public DateTime GetDate(string settingName, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(settingName) == false && Contains(settingName) == true)
            {
                return Reflector.StringToTypedValue<DateTime>((string)GetValue(settingName));
            }

            return defaultValue;
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 DateTime 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 DateTime 값입니다.</returns>
        public DateTime GetDate(string settingName)
        {
            return GetDate(settingName, DateTime.MinValue);
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 string 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 string 값입니다.</returns>
        public string GetString(string settingName, string defaultValue)
        {
            if (string.IsNullOrEmpty(settingName) == false && Contains(settingName) == true)
            {
                return Reflector.StringToTypedValue<string>((string)GetValue(settingName));
            }

            return defaultValue;
        }

        /// <summary>
        /// 구성 항목명에 맞는 특정 string 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 string 값입니다.</returns>
        public string GetString(string settingName)
        {
            return GetString(settingName, null);
        }

        /// <summary>
        /// 구성 항목명에 맞는 object 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 object 타입을 반환합니다.</returns>
        public object GetValue(string settingName)
        {
            return GetValue(settingName, null);
        }

        /// <summary>
        /// 구성 항목명에 맞는 object 타입을 반환합니다.
        /// </summary>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 object 타입을 반환합니다.</returns>
        public object GetValue(string settingName, object defaultValue)
        {
            if (Contains(settingName) == true)
            {
                return Get(settingName).Value;
            }

            return defaultValue;
        }

        /// <summary>
        /// 구성 항목명에 맞는 제네릭 타입을 반환합니다.
        /// </summary>
        /// <typeparam name="T">구성 항목명에 맞는 제네릭 타입입니다.</typeparam>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <returns>구성 항목명에 맞는 제네릭 타입을 반환합니다.</returns>
        public T GetValue<T>(string settingName)
        {
            return GetValue<T>(settingName, default(T));
        }

        /// <summary>
        /// 구성 항목명에 맞는 제네릭 타입을 반환합니다.
        /// </summary>
        /// <typeparam name="T">구성 항목명에 맞는 제네릭 타입입니다.</typeparam>
        /// <param name="settingName">구성 항목명입니다.</param>
        /// <param name="defaultValue">구성 항목명에 맞는 값이 존재하지 않을 경우 반환할 기본값입니다.</param>
        /// <returns>구성 항목명에 맞는 제네릭 타입을 반환합니다.</returns>
        public T GetValue<T>(string settingName, T defaultValue)
        {
            ISetting setting = Get(settingName);

            if (setting.Value == null)
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(setting.Value, typeof(T));
        }

        /// <summary>
        /// 컬렉션을 반복하는 열거자를 반환합니다.
        /// </summary>
        /// <returns>ISetting를 구현한 열거자입니다.</returns>
        public IEnumerator<ISetting> GetEnumerator()
        {
            return settingsSource.GetEnumerator() as IEnumerator<ISetting>;
        }

        /// <summary>
        /// 컬렉션을 반복하는 열거자를 반환합니다.
        /// </summary>
        /// <returns>IEnumerator를 구현한 타입입니다.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return settingsSource.GetEnumerator();
        }
    }
}
