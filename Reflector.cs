using System;
using System.Text;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;

using Qrame.CoreFX.Collections;
using Qrame.CoreFX.Exceptions;
using System.Runtime.Serialization.Formatters.Binary;

namespace Qrame.CoreFX
{
    /// <summary>
    /// 닷넷 Reflection 기능을 포괄 적으로 제공 하기위한 Helper 클래스 입니다. 
    /// 
    /// 주요 기능으로 다음과 같습니다.
    /// 대상 클래스의 정보를 관리하는 타입 정보 클래스 제공
    /// 어셈블리 정보를 관리하는 Dictionary
    /// 클래스 정보를 관리하는 Dictionary
    /// 클래스 타입의 인스턴스 관리
    /// 현재 도메인에서 있는 모든 어ㅣ셈ㅣ블리에서 특정 타입을 검색
    /// 특정 경로에 있는 어셈블리 파일 로드 및 클래스 타입을 반환
    /// 특정 타입의 프로퍼티, 필드, 메서드 호출 기능 제공
    /// </summary>
    public class Reflector
    {
        private static readonly BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// 어셈블리 정보를 관리하는 컬렉션입니다.
        /// </summary>
        public static Dictionary<string, Assembly> assemblyList = new Dictionary<string, Assembly>();

        /// <summary>
        /// 클래스 정보를 관리하는 컬렉션입니다.
        /// </summary>
        public static Dictionary<string, TypeDescription> typeList = new Dictionary<string, TypeDescription>();

        /// <summary>
        /// MemberAccess Binding Flags 상수값을 정의합니다. 기본적으로 타입의 모든 정보를 조회합니다.
        /// </summary>
        public const BindingFlags memberAccess = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;

        /// <summary>
        /// MemberAccess Binding Flags 상수값을 정의합니다. pubilc으로 선언된 타입의 정보를 조회합니다.
        /// </summary>
        public const BindingFlags memberPublicInstanceAccess = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

        /// <summary>
        /// 현재 실행중인 메서드의 StackTrace 메서드를 조회합니다.
        /// </summary>
        /// <returns>인덱스로 액세스할 수 있는 강력한 형식의 개체 목록을 나타냅니다.</returns>
        public List<string> GetTraceMethods()
        {
            List<string> traceMethods = new List<string>();

            StackTrace stackTrace = new StackTrace(true);
            StackFrame[] stackFrames = stackTrace.GetFrames();

            foreach (StackFrame stackFrame in stackFrames)
            {
                traceMethods.Add(stackFrame.GetMethod().Name);
            }

            return traceMethods;
        }

        /// <summary>
        /// 현재 도메인에 로드된 어셈블리들 내에 있는 클래스 타입의 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="typeName">인스턴스를 생성할 타입명입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CreateInstance(string typeName)
        {
            Type type = FindType(typeName);

            if (type != null)
            {
                return type.Assembly.CreateInstance(typeName, true);
            }

            return null;
        }

        /// <summary>
        /// 어셈블리 내에 있는 클래스 타입의 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="TargetAssembly">인스턴스를 생성할 클래스가 포함되어 있는 어셈블리 Type 입니다.</param>
        /// <param name="TypeName">인스턴스를 생성할 타입명입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CreateInstance(Assembly TargetAssembly, string TypeName)
        {
            Type type = null;

            foreach (Type eachType in TargetAssembly.GetTypes())
            {
                if (eachType.FullName.Equals(TypeName, StringComparison.OrdinalIgnoreCase))
                {
                    type = eachType;
                    break;
                }
            }

            if (type != null)
            {
                return type.Assembly.CreateInstance(TypeName, true);
            }

            return null;
        }

        /// <summary>
        /// 디렉토리 정보를 반환합니다.
        /// </summary>
        /// <param name="targetAssembly">디렉토리 정보를 반환할 어셈블리입니다.</param>
        /// <returns></returns>
        public static string GetAssemblyDirectory(Assembly targetAssembly)
        {
            string codeBase = targetAssembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 현재 도메인에 로드되어 있는 모든 어셈블리에서 특정 타입을 검색합니다.
        /// </summary>
        /// <param name="typeName">인스턴스를 생성할 타입명입니다.</param>
        /// <returns>클래스 형식, 인터페이스 형식, 배열 형식, 값 형식, 열거형 형식, 형식 매개 변수, 제네릭 형식 정의 및 개방형 생성 제네릭 형식이나 폐쇄형 생성 제네릭 형식에 대한 형식 선언을 나타냅니다.</returns>
        public static Type FindType(string typeName)
        {
            foreach (Assembly targetAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in targetAssembly.GetTypes())
                {
                    if (type.FullName.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 어셈블리에서 특정 타입을 검색합니다.
        /// </summary>
        /// <param name="targetAssembly">인스턴스를 생성할 클래스가 포함되어 있는 어셈블리 Type 입니다.</param>
        /// <param name="typeName">인스턴스를 생성할 타입명입니다.</param>
        /// <returns>클래스 형식, 인터페이스 형식, 배열 형식, 값 형식, 열거형 형식, 형식 매개 변수, 제네릭 형식 정의 및 개방형 생성 제네릭 형식이나 폐쇄형 생성 제네릭 형식에 대한 형식 선언을 나타냅니다.</returns>
        public static Type FindType(Assembly targetAssembly, string typeName)
        {
            foreach (Type type in targetAssembly.GetTypes())
            {
                if (type.FullName.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// 특정 경로에 있는 어셈블리 파일을 현재 도메인에 반환하며, 캐시에 저장하여
        /// 다음 번 조회시 AssemblyKey 값으로 캐시에서 조회 합니다.
        /// </summary>
        /// <param name="assemblyFile">인스턴스를 생성할 클래스가 포함되어 있는 어셈블리 파일 경로입니다.</param>
        /// <param name="assemblyKey">어셈블리를 조회 할 캐시 키 값입니다.</param>
        /// <returns>다시 사용 및 버전 지정이 가능한, 공용 언어 런타임 응용 프로그램의 자체 설명 빌딩 블록인 어셈블리를 나타냅니다.</returns>
        public static Assembly LoadAssembly(string assemblyFile, string assemblyKey)
        {
            Assembly targetAssembly;
            if (assemblyList.ContainsKey(assemblyKey) == false)
            {
                try
                {
                    targetAssembly = Assembly.LoadFrom(assemblyFile);
                    assemblyList.Add(assemblyKey, targetAssembly);

                    return targetAssembly;
                }
                catch (Exception e)
                {
                    throw new ReflectorException("어셈블리 정보를 찾을 수 없습니다 " + e.Message);
                }
            }

            return assemblyList[assemblyKey] as Assembly;
        }

        /// <summary>
        /// 특정 경로에 있는 어셈블리 파일을 현재 도메인에 반환하며, 어셈블리 내에서 클래스 타입을 반환합니다.
        /// 다음 번 조회시 AssemblyKey, ClassKey 값으로 캐시에서 조회 합니다.
        /// </summary>
        /// <param name="assemblyFile">인스턴스를 생성할 클래스가 포함되어 있는 어셈블리 파일 경로입니다.</param>
        /// <param name="assemblyKey">어셈블리를 조회 할 캐시 키 값입니다.</param>
        /// <param name="className">인스턴스를 생성할 클래스 타입 명입니다.</param>
        /// <param name="classKey">클래스를 조회 할 캐시 키 값입니다.</param>
        /// <returns>클래스의 타입과 정보를 관리하는 타입 정보 클래스입니다.</returns>
        public static TypeDescription LoadClassType(string assemblyFile, string assemblyKey, string className, string classKey)
        {
            if (typeList.ContainsKey(classKey) == false)
            {
                Assembly targetAssembly = LoadAssembly(assemblyFile, assemblyKey);

                foreach (Type type in targetAssembly.GetTypes())
                {
                    if (type.IsClass == true && type.FullName.EndsWith(className, StringComparison.CurrentCultureIgnoreCase))
                    {
                        TypeDescription typeDescription = new TypeDescription(type, Activator.CreateInstance(type));
                        typeList.Add(classKey, typeDescription);

                        return typeDescription;
                    }
                }

                throw (new ReflectorException("클래스의 인스턴스 정보를 확인 하지 못했습니다."));
            }

            return typeList[classKey] as TypeDescription;
        }

        /// <summary>
        /// 어셈블리 내에서 클래스 타입을 반환합니다.
        /// 다음 번 조회시 ClassKey 값으로 캐시에서 조회 합니다.
        /// </summary>
        /// <param name="loadAssembly">인스턴스를 생성할 클래스가 포함되어 있는 어셈블리 타입입니다.</param>
        /// <param name="className">인스턴스를 생성할 클래스 타입 명입니다.</param>
        /// <param name="classKey">클래스를 조회 할 캐시 키 값입니다.</param>
        /// <returns>클래스의 타입과 정보를 관리하는 타입 정보 클래스입니다.</returns>
        public static TypeDescription LoadClassType(Assembly loadAssembly, string className, string classKey)
        {
            if (typeList.ContainsKey(classKey) == false)
            {
                foreach (Type type in loadAssembly.GetTypes())
                {
                    if (type.IsClass == true)
                    {
                        if (type.FullName.EndsWith(className, StringComparison.CurrentCultureIgnoreCase))
                        {
                            TypeDescription typeDescription = new TypeDescription(type, Activator.CreateInstance(type));
                            typeList.Add(classKey, typeDescription);
                            return (typeDescription);
                        }
                    }
                }

                throw (new ReflectorException("클래스의 인스턴스 정보를 확인 하지 못했습니다."));
            }

            return typeList[classKey] as TypeDescription;
        }

        /// <summary>
        /// 현재 실행중인 어셈블리 내에서 클래스 타입을 반환합니다.
        /// 다음 번 조회시 ClassKey 값으로 캐시에서 조회 합니다.
        /// </summary>
        /// <param name="className">인스턴스를 생성할 클래스 타입 명입니다.</param>
        /// <param name="classKey">클래스를 조회 할 캐시 키 값입니다.</param>
        /// <returns>클래스의 타입과 정보를 관리하는 타입 정보 클래스입니다.</returns>
        public static TypeDescription LoadClassType(string className, string classKey)
        {
            if (typeList.ContainsKey(classKey) == false)
            {
                Assembly targetAssembly = Assembly.GetExecutingAssembly();

                foreach (Type type in targetAssembly.GetTypes())
                {
                    if (type.IsClass == true)
                    {
                        if (type.FullName.EndsWith(className, StringComparison.CurrentCultureIgnoreCase))
                        {
                            TypeDescription typeDescription = new TypeDescription(type, Activator.CreateInstance(type));
                            typeList.Add(classKey, typeDescription);

                            return typeDescription;
                        }
                    }
                }

                throw new ReflectorException("클래스의 인스턴스 정보를 확인 하지 못했습니다.");
            }

            return typeList[classKey] as TypeDescription;
        }

        /// <summary>
        /// 인스턴스 타입의 메서드를 조회합니다.
        /// </summary>
        /// <param name="instance">메서드를 조회 할 인스턴스 타입입니다.</param>
        /// <param name="methodName">조회할 메서드명입니다.</param>
        /// <returns>메서드 결과입니다.</returns>
        public static object GetMethod(object instance, string methodName)
        {
            return instance.GetType().GetMethod(methodName, memberAccess);
        }

        /// <summary>
        /// 인스턴스 타입의 속성값을 조회합니다.
        /// </summary>
        /// <param name="instance">속성값을 조회 할 인스턴스 타입입니다.</param>
        /// <param name="propertyName">조회할 속성명입니다.</param>
        /// <returns>속성값 결과입니다.</returns>
        public static object GetProperty(object instance, string propertyName)
        {
            return instance.GetType().GetProperty(propertyName, memberAccess).GetValue(instance, null);
        }

        /// <summary>
        /// 인스턴스 타입의 속성값을 설정합니다.
        /// </summary>
        /// <param name="instance">속성값을 조회 할 인스턴스 타입입니다.</param>
        /// <param name="propertyName">조회할 속성명입니다.</param>
        /// <param name="value">속성 설정값입니다.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            instance.GetType().GetProperty(propertyName, memberAccess).SetValue(instance, value, null);
        }

        /// <summary>
        /// 인스턴스 타입의 필드값을 설정합니다.
        /// </summary>
        /// <param name="instance">필드값을 설정 할 인스턴스 타입입니다.</param>
        /// <param name="property">설정할 필드명입니다.</param>
        /// <param name="value">필드 설정값입니다.</param>
        public static void SetField(object instance, string property, object value)
        {
            instance.GetType().GetField(property, memberAccess).SetValue(instance, value);
        }

        /// <summary>
        /// 인스턴스 타입의 필드값을 조회합니다.
        /// </summary>
        /// <param name="instance">필드값을 조회 할 인스턴스 타입입니다.</param>
        /// <param name="propertyName">조회할 필드명입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object GetField(object instance, string propertyName)
        {
            return instance.GetType().GetField(propertyName, memberAccess).GetValue(instance);
        }

        /// <summary>
        /// 인스턴스 타입의 Array, ICollection의 속성 또는 필드값을 조회합니다. 조회시 명칭을 '[', ']'으로 감싸야 합니다.
        /// </summary>
        /// <param name="instance">속성 또는 필드값을 조회 할 인스턴스 타입입니다.</param>
        /// <param name="propertyName">조회할 속성 또는 필드명입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        private static object GetPropertyInternal(object instance, string propertyName)
        {
            if (propertyName == "this")
            {
                return instance;
            }

            object result = null;
            string pureProperty = propertyName;
            string indexes = null;
            bool isCollection = false;

            // 속성명칭에 '[', ']' 단어가 포함되어 있으면 Array, ICollection의 필드값을 조회하는 것으로 간주합니다.
            if (propertyName.IndexOf("[") > -1)
            {
                pureProperty = propertyName.Substring(0, propertyName.IndexOf("["));
                indexes = propertyName.Substring(propertyName.IndexOf("["));
                isCollection = true;
            }

            MemberInfo member = instance.GetType().GetMember(pureProperty, memberAccess)[0];

            if (member.MemberType == MemberTypes.Property)
            {
                result = ((PropertyInfo)member).GetValue(instance, null);
            }
            else
            {
                result = ((FieldInfo)member).GetValue(instance);
            }

            if (isCollection)
            {
                indexes = indexes.Replace("[", "").Replace("]", "");

                if (result is Array)
                {
                    int Index = -1;
                    int.TryParse(indexes, out Index);
                    result = CallMethod(result, "GetValue", Index);
                }
                else if (result is ICollection)
                {
                    if (indexes.StartsWith("\""))
                    {
                        indexes = indexes.Trim('\"');
                        result = CallMethod(result, "get_Item", indexes);
                    }
                    else
                    {
                        int index = -1;
                        int.TryParse(indexes, out index);
                        result = CallMethod(result, "get_Item", index);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// 인스턴스 타입의 필드값을 설정합니다.
        /// </summary>
        /// <param name="instance">속성값을 조회 할 인스턴스 타입입니다.</param>
        /// <param name="propertyName">설정할 필드명입니다.</param>
        /// <param name="value">설정값입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        private static object SetPropertyInternal(object instance, string propertyName, object value)
        {
            if (propertyName == "this")
            {
                return instance;
            }

            object result = null;
            string pureProperty = propertyName;
            string indexes = null;
            bool isCollection = false;

            if (propertyName.IndexOf("[") > -1)
            {
                pureProperty = propertyName.Substring(0, propertyName.IndexOf("["));
                indexes = propertyName.Substring(propertyName.IndexOf("["));
                isCollection = true;
            }

            if (isCollection == false)
            {
                MemberInfo member = instance.GetType().GetMember(pureProperty, memberAccess)[0];

                if (member.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo)member).SetValue(instance, value, null);
                }
                else
                {
                    ((FieldInfo)member).SetValue(instance, value);
                }

                return null;
            }
            else
            {
                MemberInfo member = instance.GetType().GetMember(pureProperty, memberAccess)[0];

                if (member.MemberType == MemberTypes.Property)
                {
                    result = ((PropertyInfo)member).GetValue(instance, null);
                }
                else
                {
                    result = ((FieldInfo)member).GetValue(instance);
                }
            }

            if (isCollection)
            {
                indexes = indexes.Replace("[", "").Replace("]", "");

                if (result is Array)
                {
                    int Index = -1;
                    int.TryParse(indexes, out Index);
                    result = CallMethod(result, "SetValue", value, Index);
                }
                else if (result is ICollection)
                {
                    if (indexes.StartsWith("\""))
                    {
                        indexes = indexes.Trim('\"');
                        result = CallMethod(result, "set_Item", indexes, value);
                    }
                    else
                    {
                        int Index = -1;
                        int.TryParse(indexes, out Index);
                        result = CallMethod(result, "set_Item", Index, value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 인스턴스 타입의 Array, ICollection의 속성 또는 필드값을 조회합니다. 조회시 명칭을 '[', ']'으로 감싸야 합니다.
        /// </summary>
        /// <param name="instance">속성 또는 필드값을 조회 할 인스턴스 타입입니다.</param>
        /// <param name="propertyName">조회할 속성 또는 필드명입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object GetPropertyEx(object instance, string propertyName)
        {
            Type type = instance.GetType();

            int indexes = propertyName.IndexOf(".");
            if (indexes < 0)
            {
                return GetPropertyInternal(instance, propertyName);
            }

            string main = propertyName.Substring(0, indexes);
            string subs = propertyName.Substring(indexes + 1);

            object sub = GetPropertyInternal(instance, main);

            return GetPropertyEx(sub, subs);
        }

        /// <summary>
        /// 인스턴스 타입의 필드값을 설정합니다.
        /// </summary>
        /// <param name="instance">속성값을 조회 할 인스턴스 타입입니다.</param>
        /// <param name="propertyName">설정할 필드명입니다.</param>
        /// <param name="value">설정값입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object SetPropertyEx(object instance, string propertyName, object value)
        {
            Type type = instance.GetType();
            int indexes = propertyName.IndexOf(".");

            if (indexes < 0)
            {
                SetPropertyInternal(instance, propertyName, value);
                return null;
            }

            string main = propertyName.Substring(0, indexes);
            string subs = propertyName.Substring(indexes + 1);

            object sub = GetPropertyInternal(instance, main);

            SetPropertyEx(sub, subs, value);

            return null;
        }

        /// <summary>
        /// 인스턴스 타입의 메서드를 접근자(public, private...)에 상관 없이 강제로 호출합니다.
        /// </summary>
        /// <param name="instance">메서드를 호출 할 인스턴스 타입입니다.</param>
        /// <param name="methodName">호출할 메서드 명입니다.</param>
        /// <param name="parameterTypes">매개 변수 타입입니다.</param>
        /// <param name="methodParameters">매개 변수 배열값입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CallMethod(object instance, string methodName, Type[] parameterTypes, params object[] methodParameters)
        {
            if (parameterTypes == null && methodParameters.Length > 0)
            {
                // ParameterTypes이 null 이면 매개 변수 타입에 대한 검증을 하지 않습니다. 반드시 모든 매개 변수에 값이 존재해야 합니다.
                return instance.GetType().GetMethod(methodName, memberAccess | BindingFlags.InvokeMethod).Invoke(instance, methodParameters);
            }
            else
            {
                // 호출할 메서드의 매개 변수 타입을 지정합니다.
                return instance.GetType().GetMethod(methodName, memberAccess | BindingFlags.InvokeMethod, null, parameterTypes, null).Invoke(instance, methodParameters);
            }
        }

        /// <summary>
        /// 인스턴스 타입의 메서드를 접근자(public, private...)에 상관 없이 강제로 호출합니다. 매개 변수 타입에 대한 검증을 내부에서 진행합니다.
        /// </summary>
        /// <param name="instance">메서드를 호출 할 인스턴스 타입입니다.</param>
        /// <param name="methodName">호출할 메서드 명입니다.</param>
        /// <param name="methodParameters">매개 변수 배열값입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CallMethod(object instance, string methodName, params object[] methodParameters)
        {
            Type[] parameterTypes = null;

            if (methodParameters != null)
            {
                parameterTypes = new Type[methodParameters.Length];
                for (int i = 0; i < methodParameters.Length; i++)
                {
                    if (methodParameters[i] == null)
                    {
                        parameterTypes = null;
                        break;
                    }

                    parameterTypes[i] = methodParameters[i].GetType();
                }
            }

            return CallMethod(instance, methodName, parameterTypes, methodParameters);
        }

        /// <summary>
        /// 인스턴스 타입의 메서드를 접근자(public, private...)에 상관 없이 강제로 호출합니다. 매개 변수 타입에 대한 검증을 내부에서 진행합니다.
        /// </summary>
        /// <param name="instance">메서드를 호출 할 인스턴스 타입입니다.</param>
        /// <param name="methodName">호출할 메서드 명입니다.</param>
        /// <param name="methodParameters">매개 변수 배열값입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CallMethodEx(object instance, string methodName, params object[] methodParameters)
        {
            Type type = instance.GetType();

            int indexes = methodName.IndexOf(".");
            if (indexes < 0)
            {
                return CallMethod(instance, methodName, methodParameters);
            }

            string main = methodName.Substring(0, indexes);
            string subs = methodName.Substring(indexes + 1);

            object sub = GetPropertyInternal(instance, main);

            return CallMethodEx(sub, subs, methodParameters);
        }

        /// <summary>
        /// 타입 객체에서 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="typeToCreate">인스턴스를 만들 Type 객체입니다.</param>
        /// <param name="arguments">인스턴스 생성자 매개 변수 입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CreateInstanceFromType(Type typeToCreate, params object[] arguments)
        {
            if (arguments == null)
            {
                Type[] Params = Type.EmptyTypes;
                return typeToCreate.GetConstructor(Params).Invoke(null);
            }

            return Activator.CreateInstance(typeToCreate, arguments);
        }

        /// <summary>
        /// 현재 도메인에 로드된 모든 어셈블리에서 타입명으로 객체를 조회하여, 인스턴스를 생성합니다.
        /// </summary>
        /// <param name="typeName">인스턴스를 만들 Type 명입니다.</param>
        /// <param name="arguments">인스턴스 생성자 매개 변수 입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CreateInstanceFromString(string typeName, params object[] arguments)
        {
            object instance = null;
            Type type = null;

            try
            {
                type = GetTypeFromName(typeName);
                if (type == null)
                {
                    return null;
                }

                instance = Activator.CreateInstance(type, arguments);
            }
            catch
            {
                return null;
            }

            return instance;
        }

        /// <summary>
        /// 현재 도메인에 로드된 모든 어셈블리에서 타입명으로 객체를 조회합니다.
        /// </summary>
        /// <param name="typeName">인스턴스를 만들 Type 명입니다.</param>
        /// <returns>클래스 형식, 인터페이스 형식, 배열 형식, 값 형식, 열거형 형식, 형식 매개 변수, 제네릭 형식 정의 및 개방형 생성 제네릭 형식이나 폐쇄형 생성 제네릭 형식에 대한 형식 선언을 나타냅니다.</returns>
        public static Type GetTypeFromName(string typeName)
        {
            Type type = null;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName, false);

                if (type != null)
                {
                    break;
                }
            }

            return type;
        }


        /// <summary>
        /// 시스템에 설치된 COM객체의 ProgID명으로 타입을 로드합니다.
        /// </summary>
        /// <param name="progID">COM ProgID입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object CreateComInstance(string progID)
        {
            Type SystemType = Type.GetTypeFromProgID(progID);

            if (SystemType == null)
            {
                return null;
            }

            return Activator.CreateInstance(SystemType);
        }

        /// <summary>
        /// 닷넷에서 지원하는 모든 데이터 타입의 값을 문자열로 변환하여 반환합니다.
        /// </summary>
        /// <param name="rawValue">문자열로 변환할 데이터 타입입니다.</param>
        /// <param name="culture">숫자 또는 일자값 반환시 적용할 CultureInfo 타입입니다.</param>
        /// <returns>텍스트를 일련의 유니코드 문자로 나타냅니다.</returns>
        public static string TypedValueToString(object rawValue, CultureInfo culture)
        {
            Type type = rawValue.GetType();
            string result = null;

            if (type == typeof(string))
            {
                result = rawValue.ToString();
            }
            else if (type == typeof(int) || type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            {
                result = string.Format(culture.NumberFormat, "{0}", rawValue);
            }
            else if (type == typeof(DateTime))
            {
                result = string.Format(culture.DateTimeFormat, "{0}", rawValue);
            }
            else if (type == typeof(bool))
            {
                result = rawValue.ToString();
            }
            else if (type == typeof(byte))
            {
                result = rawValue.ToString();
            }
            else if (type.IsEnum)
            {
                result = rawValue.ToString();
            }
            else if (type == typeof(Guid?))
            {
                if (rawValue == null)
                {
                    result = "";
                }
                else
                {
                    return rawValue.ToString();
                }
            }
            else
            {
                // 기본 닷넷 데이터 타입이 아닐경우 변환 가능여부에 따라 값을 반환합니다.
                TypeConverter converter = TypeDescriptor.GetConverter(type);

                if (converter != null && converter.CanConvertTo(typeof(string)))
                {
                    result = converter.ConvertToString(null, culture, rawValue);
                }
                else
                {
                    result = rawValue.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// 닷넷에서 지원하는 모든 데이터 타입의 값을 문자열로 변환하여 반환합니다. 숫자 또는 일자값 반환시 현재 도메인의 CultureInfo를 사용합니다.
        /// </summary>
        /// <param name="rawValue">문자열로 변환할 데이터 타입입니다.</param>
        /// <returns>텍스트를 일련의 유니코드 문자로 나타냅니다.</returns>
        public static string TypedValueToString(object rawValue)
        {
            return TypedValueToString(rawValue, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 문자열의 값을 닷넷에서 지원하는 데이터 타입으로 변환하여 반환합니다.
        /// </summary>
        /// <param name="value">변환할 문자열 값입니다.</param>
        /// <param name="targetType">변환할 데이터 타입입니다.</param>
        /// <param name="culture">숫자 또는 일자값 반환시 적용할 CultureInfo 타입입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object StringToTypedValue(string value, Type targetType, CultureInfo culture)
        {
            object result = null;

            bool isEmpty = false;

            if (string.IsNullOrEmpty(value) == true)
            {
                isEmpty = true;
            }

            if (targetType == typeof(string))
            {
                result = value;
            }
            else if (targetType == typeof(Int32) || targetType == typeof(int))
            {
                if (isEmpty == true)
                {
                    result = 0;
                }
                else
                {
                    result = Int32.Parse(value, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(Int64))
            {
                if (isEmpty == true)
                {
                    result = (Int64)0;
                }
                else
                {
                    result = Int64.Parse(value, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(Int16))
            {
                if (isEmpty == true)
                {
                    result = (Int16)0;
                }
                else
                {
                    result = Int16.Parse(value, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(decimal))
            {
                if (isEmpty == true)
                {
                    result = 0M;
                }
                else
                {
                    result = decimal.Parse(value, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(DateTime))
            {
                if (isEmpty == true)
                {
                    result = DateTime.MinValue;
                }
                else
                {
                    result = Convert.ToDateTime(value, culture.DateTimeFormat);
                }
            }
            else if (targetType == typeof(byte))
            {
                if (isEmpty == true)
                {
                    result = 0;
                }
                else
                {
                    result = Convert.ToByte(value);
                }
            }
            else if (targetType == typeof(double))
            {
                if (isEmpty == true)
                {
                    result = 0F;
                }
                else
                {
                    result = Double.Parse(value, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(Single))
            {
                if (isEmpty == true)
                {
                    result = 0F;
                }
                else
                {
                    result = Single.Parse(value, NumberStyles.Any, culture.NumberFormat);
                }
            }
            else if (targetType == typeof(bool))
            {
                if (isEmpty == false && (value.ToLower() == "true" || value.ToLower() == "on" || value == "1" || value == "Y"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (targetType == typeof(Guid))
            {
                if (isEmpty == true)
                {
                    result = Guid.Empty;
                }
                else
                {
                    result = new Guid(value);
                }
            }
            else if (targetType.IsEnum)
            {
                result = Enum.Parse(targetType, value);
            }
            else if (targetType == typeof(byte[]))
            {
                // 바이트 배열 변환은 불가능합니다.
                result = null;
            }
            else if (targetType.Name.StartsWith("Nullable`")) // 타입의 멤버명에 Nullable이 포함되어 있으면 실행합니다.
            {
                if (value.ToLower() == "null" || value == "")
                {
                    result = null;
                }
                else
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                    result = StringToTypedValue(value, targetType);
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);

                if (converter != null && converter.CanConvertFrom(typeof(string)))
                {
                    result = converter.ConvertFromString(null, culture, value);
                }
                else
                {
                    throw (new InvalidCastException(string.Format("다음과 같은 조건은 현재 도메인에서 변환이 지원되지 않습니다. {0} {1}", targetType.Name, value)));
                }
            }

            return result;
        }

        /// <summary>
        /// 문자열의 값을 닷넷에서 지원하는 데이터 타입으로 변환하여 반환합니다. 숫자 또는 일자값 반환시 현재 도메인의 CultureInfo를 사용합니다.
        /// </summary>
        /// <param name="value">변환할 문자열 값입니다.</param>
        /// <param name="targetType">변환할 데이터 타입입니다.</param>
        /// <returns>모든 클래스 중에서 기본 클래스이며 형식 계층 구조의 루트입니다.</returns>
        public static object StringToTypedValue(string value, Type targetType)
        {
            return StringToTypedValue(value, targetType, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// StringToTypedValue 메서드의 제네릭 버전입니다.
        /// </summary>
        /// <typeparam name="T">변환할 데이터 제네릭 타입입니다.</typeparam>
        /// <param name="value">변환할 문자열 값입니다.</param>
        /// <param name="culture">숫자 또는 일자값 반환시 적용할 CultureInfo 타입입니다.</param>
        /// <returns>변환된 데이터 제네릭 타입입니다.</returns>
        public static T StringToTypedValue<T>(string value, CultureInfo culture)
        {
            return (T)StringToTypedValue(value, typeof(T), culture);
        }

        /// <summary>
        /// StringToTypedValue 메서드의 제네릭 버전입니다. 숫자 또는 일자값 반환시 현재 도메인의 CultureInfo를 사용합니다.
        /// </summary>
        /// <typeparam name="T">변환할 데이터 제네릭 타입입니다.</typeparam>
        /// <param name="value">변환할 문자열 값</param>
        /// <returns>변환된 데이터 제네릭 타입입니다.</returns>
        public static T StringToTypedValue<T>(string value)
        {
            return (T)StringToTypedValue(value, typeof(T), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 타입에서 상수 이름의 배열을 List배열로 반환합니다.
        /// </summary>
        /// <param name="instance">상수 이름의 배열을 추출할 인스턴스 타입입니다.</param>
        /// <returns>인덱싱을 지원하는 List 타입의 확장 클래스입니다.</returns>
        public static KeyedList<string, string> GetEnumList(Type instance)
        {
            string[] enumStrings = Enum.GetNames(instance);

            KeyedList<string, string> enumList = new KeyedList<string, string>();

            foreach (string enumString in enumStrings)
            {
                enumList.Add(enumString, enumString);
            }

            return enumList;
        }

        /// <summary>
        /// 지정된 제너릭 타입의 정보를 XmlSerializer를 통해 XML 파일로 저장합니다. 파일이 이미 있으면 해당 파일을 덮어씁니다. 다른 프로세스에서 해당 파일을 사용 중에도 파일을 덮어씁니다. 
        /// </summary>
        /// <code>
        /// Reflector.Serialize&lt;targetType&gt;(new NewType(), "c:\\NewType.xml");
        /// </code>
        /// <typeparam name="T">XML 파일로 저장할 제너릭 타입입니다.</typeparam>
        /// <param name="sourceType">XML 파일로 저장할 제너릭 타입입니다.</param>
        /// <param name="fileName">지정된 형식의 개체를 XML 문서를 저장할 XML 파일명입니다.</param>
        public static void Serialize<T>(T sourceType, string fileName)
        {
            Stream writeStream = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                writeStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                serializer.Serialize(writeStream, sourceType);
            }
            catch (Exception exception)
            {
            }
            finally
            {
                writeStream.Close();
            }
        }

        /// <summary>
        /// 지정된 제너릭 타입의 정보를 XmlSerializer를 통해 XML 문서로 반환합니다.
        /// </summary>
        /// <code>
        /// string TypeString = Reflector.Serialize&lt;targetType&gt;(new NewType());
        /// </code>
        /// <typeparam name="T">XML 문서로 반환할 제너릭 타입입니다.</typeparam>
        /// <param name="sourceType">XML 문서로 반환할 제너릭 타입입니다.</param>
        /// <returns>지정된 형식의 개체의 XML 문서입니다.</returns>
        public static string Serialize<T>(T sourceType)
        {
            string result = null;
            StringWriter writer = new StringWriter();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, sourceType);

                result = writer.ToString();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                writer.Close();
            }

            return result;
        }

        /// <summary>
        /// 지정된 형식의 개체의 XML 문서로 타입의 정보를 복원합니다.
        /// </summary>
        /// <typeparam name="T">지정된 형식으로 복원할 제너릭 타입입니다.</typeparam>
        /// <param name="xml">지정된 형식으로 복원할 Deserialize XML 문서입니다.</param>
        /// <returns>지정된 형식으로 복원한 제너릭 타입입니다.</returns>
        public static T DeSerializeXml<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml) == true)
            {
                return default(T);
            }

            StringReader reader = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                reader = new StringReader(xml);

                return (T)serializer.Deserialize(reader);
            }
            catch (Exception exception)
            {
            }
            finally
            {
                reader.Close();
            }

            return default(T);
        }

        /// <summary>
        /// 지정된 제너릭 타입의 XML 정보를 XmlSerializer를 통해 복원합니다.
        /// </summary>
        /// <code>
        /// dynamic targetType = Reflector.Deserialize&lt;targetType&gt;("c:\\NewType.xml");
        /// </code>
        /// <typeparam name="T">XML 파일로 복원할 제너릭 타입입니다.</typeparam>
        /// <param name="fileName">지정된 형식의 개체를 XML 문서를 저장한 XML 파일명입니다.</param>
        public static T Deserialize<T>(string fileName) where T : class, new()
        {
            T result;
            FileInfo settingFile = new FileInfo(fileName);
            if (settingFile.Exists == false)
            {
                return null; // return new T();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (Stream readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    result = serializer.Deserialize(readStream) as T;
                }

                return result;
            }
            catch (Exception exception)
            {
            }

            return null;
        }

        /*
            [DataContract]
            public class Person
            {
                public Person() { }
                public Person(string firstname, string lastname)
                {
                    this.FirstName = firstname;
                    this.LastName = lastname;
                }

                [DataMember]
                public string FirstName { get; set; }

                [DataMember]
                public string LastName { get; set; }
            }

            Person myPerson = new Person("Chris", "Pietschmann");
            string JsonString = Reflector.JsonSerialize&lt;targetType&gt;(myPerson);
         */
        /// <summary>
        /// 지정된 제너릭 타입의 정보를 DataContractJsonSerializer를 통해 Json 문서로 반환합니다.
        /// </summary>
        /// <code>
        /// string JsonString = Reflector.JsonSerialize&lt;targetType&gt;(new NewType());
        /// </code>
        /// <typeparam name="T">Json 문서로 반환할 제너릭 타입입니다.</typeparam>
        /// <param name="sourceType">Json 문서로 반환할 제너릭 타입입니다.</param>
        /// <returns>지정된 형식의 개체의 Json 문서입니다.</returns>
        public static string JsonSerialize<T>(T sourceType)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(sourceType.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, sourceType);
            string retVal = Encoding.Default.GetString(ms.ToArray());
            ms.Dispose();
            return retVal;
        }

        /// <summary>
        /// 지정된 제너릭 타입의 Json 정보를 DataContractJsonSerializer를 통해 복원합니다.
        /// </summary>
        /// <code>
        /// dynamic targetType = Reflector.JsonDeserialize&lt;targetType&gt;("json 문자열");
        /// </code>
        /// <typeparam name="T">Json 파일로 복원할 제너릭 타입입니다.</typeparam>
        /// <param name="json">지정된 형식의 개체를 표현하는 Json 문자열입니다.</param>
        /// <returns>DataContractJsonSerializer를 통해 복원된 제너릭 타입입니다.</returns>
        public static T JsonDeserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }

        /// <summary>
        /// sourceType의 모든 요소를 지정된 대상으로 복사합니다.
        /// </summary>
        /// <typeparam name="T">sourceType의 모든 요소를 지정된 대상으로 복사 할 때, 지정하는 제네릭 타입입니다.</typeparam>
        /// <param name="sourceType">복사할 요소의 대상인 object입니다.</param>
        /// <param name="targetType">지정된 대상으로 복사할 object입니다.</param>
        public static void CopyTo<T>(T sourceType, T targetType)
        {
            CopyTo(sourceType, targetType, memberAccess);
        }

        /// <summary>
        /// sourceType의 모든 요소를 BindingFlags에 따라, 지정된 대상으로 복사합니다.
        /// </summary>
        /// <typeparam name="T">sourceType의 모든 요소를 지정된 대상으로 복사 할 때, 지정하는 제네릭 타입입니다.</typeparam>
        /// <param name="sourceType">복사할 요소의 대상인 object입니다.</param>
        /// <param name="targetType">지정된 대상으로 복사할 object입니다.</param>
        /// <param name="memberAccessFlags">리플렉션에서 멤버 및 형식 검색이 수행되는 방식과 바인딩을 제어하는 플래그입니다.</param>
        public static void CopyTo<T>(T sourceType, T targetType, BindingFlags memberAccessFlags)
        {
            CopyTo(sourceType, targetType, null, memberAccessFlags);
        }

        /// <summary>
        /// sourceType의 모든 요소를 지정된 대상으로 복사합니다. 확장 속성값의 구분자를 지정해야 합니다.
        /// </summary>
        /// <typeparam name="T">sourceType의 모든 요소를 지정된 대상으로 복사 할 때, 지정하는 제네릭 타입입니다.</typeparam>
        /// <param name="sourceType">복사할 요소의 대상인 object입니다.</param>
        /// <param name="targetType">지정된 대상으로 복사할 object입니다.</param>
        /// <param name="excludedProperties">확장 속성값을 가지고 있는 콤마로 구분자를 가지고 있는 속성 목록입니다.</param>
        public static void CopyTo<T>(T sourceType, T targetType, string excludedProperties)
        {
            CopyTo(sourceType, targetType, excludedProperties, memberAccess);
        }

        /// <summary>
        /// sourceType의 모든 요소를 지정된 대상으로 복사합니다. BindingFlags와 확장 속성값의 구분자를 지정해야 합니다.
        /// </summary>
        /// <typeparam name="T">sourceType의 모든 요소를 지정된 대상으로 복사 할 때, 지정하는 제네릭 타입입니다.</typeparam>
        /// <param name="sourceType">복사할 요소의 대상인 object입니다.</param>
        /// <param name="targetType">지정된 대상으로 복사할 object입니다.</param>
        /// <param name="excludedProperties">확장 속성값을 가지고 있는 콤마로 구분자를 가지고 있는 속성 목록입니다.</param>
        /// <param name="memberAccessFlags">리플렉션에서 멤버 및 형식 검색이 수행되는 방식과 바인딩을 제어하는 플래그입니다.</param>
        public static void CopyTo<T>(T sourceType, T targetType, string excludedProperties, BindingFlags memberAccessFlags)
        {
            string[] excluded = null;

            if (excludedProperties.Length > 0)
            {
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            MemberInfo[] memberInfoType = targetType.GetType().GetMembers(memberAccessFlags);

            foreach (MemberInfo memberInfo in memberInfoType)
            {
                string FieldName = memberInfo.Name;

                if (excludedProperties.Length > 0 && excluded.Contains(FieldName))
                {
                    continue;
                }

                if (memberInfo.MemberType == MemberTypes.Field)
                {
                    FieldInfo sourceField = sourceType.GetType().GetField(FieldName);

                    if (sourceField == null)
                    {
                        continue;
                    }

                    object SourceValue = sourceField.GetValue(sourceType);

                    ((FieldInfo)memberInfo).SetValue(targetType, SourceValue);
                }
                else if (memberInfo.MemberType == MemberTypes.Property)
                {
                    PropertyInfo targetProperty = memberInfo as PropertyInfo;
                    PropertyInfo sourceProperty = sourceType.GetType().GetProperty(FieldName, memberAccessFlags);

                    if (sourceProperty == null)
                    {
                        continue;
                    }

                    if (targetProperty.CanWrite && sourceProperty.CanRead)
                    {
                        object SourceValue = sourceProperty.GetValue(sourceType, null);
                        targetProperty.SetValue(targetType, SourceValue, null);
                    }
                }
            }
        }

        /// <summary>
        /// 지정된 인스턴스의 Property를 대상 인스턴스의 Property에 바인딩합니다.
        /// </summary>
        /// <typeparam name="S">sourceType의 모든 요소를 지정된 대상으로 복사 할 때, 지정하는 제네릭 타입입니다.</typeparam>
        /// <typeparam name="T">targetType의 모든 요소를 지정된 대상으로 복사 할 때, 지정하는 제네릭 타입입니다.</typeparam>
        /// <param name="sourceType">복사할 요소의 대상인 object입니다.</param>
        /// <param name="targetType">지정된 대상으로 복사할 object입니다.</param>
        public static void CopyTo<S, T>(S sourceType, T targetType)
        {
            MemberInfo[] members = sourceType.GetType().GetMembers(memberAccess);

            var queryResults = from m in members.AsParallel() where m.MemberType == MemberTypes.Field select m;

            foreach (MemberInfo member in queryResults)
            {
                try
                {
                    Reflector.SetField(targetType, member.Name, Reflector.GetField(sourceType, member.Name));
                }
                catch
                {
                    continue;
                }
            }
        }

        public static T Clone<T>(T instance)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable", "instance");
            }

            if (Object.ReferenceEquals(instance, null))
            {
                return default(T);
            }

            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, instance);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static void FileSerialize<T>(T instance, string filePath)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable", "instance");
            }

            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, instance);
            }
        }

        public static byte[] ByteSerialize<T>(T instance)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable", "instance");
            }

            byte[] results = null;
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, instance);
                stream.Seek(0, SeekOrigin.Begin);

                results = new byte[stream.Length];
                stream.Write(results, 0, results.Length);
            }

            return results;
        }

        public static T FileDeserialize<T>(string filePath)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable", "instance");
            }
            
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T ByteDeserialize<T>(byte[] instanceBytes)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable", "instance");
            }

            byte[] results = null;
            using (Stream stream = new MemoryStream())
            {
                stream.Read(instanceBytes, 0, results.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
