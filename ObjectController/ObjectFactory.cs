using System;
using System.IO;
using System.Reflection;

using Qrame.CoreFX.Collections;

namespace Qrame.CoreFX.ObjectController
{
    /// <summary>
    /// 비즈니스 로직을 구현하는 인스턴스를 생성하는 기능을 제공합니다.
    /// </summary>
    /// <typeparam name="T">인스턴스를 생성할 제네릭 타입입니다.</typeparam>
    public class ObjectFactory<T> where T : class
    {
        /// <summary>
        /// 클래스 정보를 관리하는 컬렉션입니다.
        /// </summary>
        private static KeyedList<string, dynamic> typeList = new KeyedList<string, dynamic>();

        private dynamic instance;

        /// <summary>
        /// 인스턴스를 표현하는 런타임 식입니다.
        /// </summary>
        public dynamic Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 인스턴스를 null로 초기화하는 기본 생성자입니다.
        /// </summary>
        public ObjectFactory()
        {
            instance = null;
        }

        /// <summary>
        /// 현재 실행 중인 메서드를 호출하는 메서드의 Assembly에서 인스턴스를 추가합니다.
        /// </summary>
        /// <param name="typeName">인스턴스 타입에 대한 이름입니다.</param>
        public ObjectFactory(string typeName)
        {
            AddController(typeName, null, Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// 현재 실행 중인 메서드를 호출하는 메서드의 Assembly에서 인스턴스를 추가합니다.
        /// </summary>
        /// <param name="typeName">인스턴스 타입에 대한 이름입니다.</param>
        /// <param name="assemblyName">긴 형식의 이름으로 지정된 어셈블리 이름입니다.</param>
        public ObjectFactory(string typeName, string assemblyName)
            : this(typeName, null, assemblyName)
        {
        }

        /// <summary>
        /// 현재 실행 중인 메서드를 호출하는 메서드의 Assembly에서 인스턴스를 추가합니다.
        /// </summary>
        /// <param name="typeName">인스턴스 타입에 대한 이름입니다.</param>
        /// <param name="namespaceName">네임스페이스 이름입니다.</param>
        /// <param name="assemblyName">긴 형식의 이름으로 지정된 어셈블리 이름입니다.</param>
        public ObjectFactory(string typeName, string namespaceName, string assemblyName)
        {
            AddController(typeName, namespaceName, Assembly.Load(assemblyName));
        }

        /// <summary>
        /// 인스턴스를 추가합니다.
        /// </summary>
        /// <param name="typeName">인스턴스 타입에 대한 이름입니다.</param>
        /// <param name="namespaceName">네임스페이스 이름입니다.</param>
        /// <param name="assemblyFilePath">어셈블리 경로입니다.</param>
        public void AddController(string typeName, string namespaceName, string assemblyFilePath)
        {
            AddController(typeName, namespaceName, Assembly.LoadFrom(assemblyFilePath));
        }

        /// <summary>
        /// 인스턴스를 추가합니다.
        /// </summary>
        /// <param name="typeName">인스턴스 타입에 대한 이름입니다.</param>
        /// <param name="namespaceName">네임스페이스 이름입니다.</param>
        /// <param name="loadAssembly">다시 사용 및 버전 지정이 가능한, 공용 언어 런타임 응용 프로그램의 자체 설명 빌딩 블록인 어셈블리를 나타냅니다.</param>
        private void AddController(string typeName, string namespaceName, Assembly loadAssembly)
        {
            if (string.IsNullOrEmpty(namespaceName) == true)
            {
                namespaceName = "emptyNamespace";
            }

            if (typeList.ContainsKey(namespaceName + "." + typeName) == true)
            {
                instance = typeList[namespaceName + "." + typeName] as T;
            }
            else
            {
                foreach (Type FindType in loadAssembly.GetTypes())
                {
                    if (string.IsNullOrEmpty(namespaceName) == false)
                    {
                        if (FindType.Namespace == namespaceName && FindType.IsClass == true && FindType.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                        {
                            instance = Activator.CreateInstance(FindType, false) as T;
                            typeList.Add(namespaceName + "." + typeName, instance);
                            return;
                        }
                    }
                }

                foreach (Type FindType in loadAssembly.GetTypes())
                {
                    if (FindType.IsClass == true && FindType.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                    {
                        instance = Activator.CreateInstance(FindType, false) as T;
                        typeList.Add(namespaceName + "." + typeName, instance);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 지정한 타입의 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="typeName">인스턴스 타입에 대한 이름입니다.</param>
        /// <param name="namespaceName">네임스페이스 이름입니다.</param>
        /// <returns>지정한 제네릭 타입에 대한 인스턴스 객체입니다.</returns>
        public T GetController(string typeName, string namespaceName)
        {
            T Result = null;
            if (typeList.ContainsKey(namespaceName + "." + typeName) == true)
            {
                Result = typeList[namespaceName + "." + typeName] as T;
            }

            return Result;
        }

        /// <summary>
        /// 지정한 타입의 인스턴스를 반환합니다.
        /// </summary>
        /// <param name="typeName">인스턴스 타입에 대한 이름입니다.</param>
        /// <returns>지정한 제네릭 타입에 대한 인스턴스 객체입니다.</returns>
        public T GetController(string typeName)
        {
            T Result = null;
            if (typeList.ContainsKey(typeName) == true)
            {
                Result = typeList[typeName] as T;
            }

            return Result;
        }
    }
}
