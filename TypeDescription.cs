using System;

namespace Qrame.CoreFX
{
    /// <summary>
    /// 클래스의 타입과 정보를 관리하는 타입 정보 클래스입니다.
    /// </summary>
    public class TypeDescription
    {
        /// <summary>
        /// 클래스 형식, 인터페이스 형식, 배열 형식, 값 형식, 열거형 형식, 형식 매개 변수, 제네릭 형식 정의 및 개방형 생성 제네릭 형식이나 폐쇄형 생성 제네릭 형식에 대한 형식 선언을 나타냅니다.
        /// </summary>
        public Type TypeObject;

        /// <summary>
        /// 지정된 형식의 인스턴스입니다.
        /// </summary>
        public object ClassObject;

        /// <summary>
        /// Type 클래스와 Object 클래스를 포함합니다.
        /// </summary>
        /// <param name="typeObject">클래스 형식, 인터페이스 형식, 배열 형식, 값 형식, 열거형 형식, 형식 매개 변수, 제네릭 형식 정의 및 개방형 생성 제네릭 형식이나 폐쇄형 생성 제네릭 형식에 대한 형식 선언을 나타냅니다.</param>
        /// <param name="classObject">지정된 형식의 인스턴스입니다.</param>
        public TypeDescription(Type typeObject, object classObject)
        {
            TypeObject = typeObject;
            ClassObject = classObject;
        }
    }
}
