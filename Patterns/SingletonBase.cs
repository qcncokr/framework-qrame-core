using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.Patterns
{
	/// <summary>
	/// 클래스의 기본 생성자를 이용하여 스레드 접근에 안전한 싱글턴 인스턴스를 생성하여 반환합니다.
	/// </summary>
	/// <typeparam name="T">단일 인스턴스를 반환할 제너릭 타입입니다.</typeparam>
	public abstract class SingletonBase<T>
	{
		/// <summary>
		/// 스레드 동기화를 위해 락킹용 객체를 선언합니다.ㅕ
		/// </summary>
		private static readonly object lockObject = new object();

		/// <summary>
		/// 기본 생성자를 이용하여 스레드 접근에 안전한 싱글턴 인스턴스를 생성할 제네릭 타입입니다.
		/// </summary>
		private static T singletonInstance;

		/// <summary>
		/// 기본 생성자에 대한 델리게이트 선언입니다.
		/// </summary>
		/// <returns></returns>
		public delegate T CreateInstanceDelegate();

		/// <summary>
		/// 기본 생성자에 대한 델리게이트를 반환합니다.
		/// </summary>
		protected static CreateInstanceDelegate createInstanceDelegate;

		/// <summary>
		/// 싱글턴 인스턴스를 생성하여 반환합니다.
		/// </summary>
		protected static T SingletonInstance
		{
			get
			{
				// 이전에 만들어진 싱글턴 인스턴스가 있는지 확인합니다.
				if (singletonInstance == null)
				{
					// 스레드 접근에 안정성을 제공하기 위해 락을 겁니다.
					lock (lockObject)
					{
						// 락을 거는 도중 싱글턴 인스턴스가 삭제될 수 있어 한번더 체크합니다.
						if (singletonInstance == null)
						{
							if (createInstanceDelegate == null)
							{
								throw new Exception("기본 생성자가 없는 클래스는 싱글턴 인스턴스를 만들수 없습니다.");
							}

							singletonInstance = createInstanceDelegate();
						}
					}
				}

				return singletonInstance;
			}
			set
			{
				singletonInstance = value;
			}
		}
	}
}
