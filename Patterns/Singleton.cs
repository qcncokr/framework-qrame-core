namespace Qrame.CoreFX.Patterns
{
	/// <summary>
	/// public으로 선언된 클래스의 기본 생성자를 이용하여 언제 어디서든 싱글턴 클래스의 인스턴스를 반환합니다.
	/// </summary>
	/// <typeparam name="T">단일 인스턴스를 반환할 제너릭 타입입니다.</typeparam>
	public class Singleton<T> : SingletonBase<T> where T : new()
	{
		/// <summary>
		/// 기본 생성자입니다.
		/// </summary>
		static Singleton()
		{
			createInstanceDelegate = DefaultCreateInstance;
		}

		/// <summary>
		/// 기본 생성자를 이용하여 인스턴스를 반환합니다.
		/// </summary>
		/// <returns></returns>
		private static T DefaultCreateInstance()
		{
			return new T();
		}

		/// <summary>
		/// 싱글턴 클래스의 인스턴스를 반환합니다.
		/// </summary>
		public static T Instance
		{
			get
			{
				return SingletonInstance;
			}
			set
			{
				SingletonInstance = value;
			}
		}
	}
}
