using System;
using System.IO;
using Qrame.CoreFX.Configuration;

namespace Qrame.CoreFX.Configuration.Settings
{
	/// <summary>
	/// DatabaseFactory 기본 동작에 필요한 구성 항목을 정의합니다.
	/// </summary>
	public class DatabaseSetting : ApplicationConfig, IDisposable
	{
		/// <summary>
		/// 구성 항목과 설정 원본을 지정합니다.
		/// </summary>
		public DatabaseSetting()
		{
			this.Provider = new ConfigurationFile<DatabaseSetting>()
			{
				ConfigurationSection = "Database",
				ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
			};

			this.Provider.Read(this);
		}

		public DatabaseSetting(string environmentName)
		{
			string dataSourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Qrame.{environmentName}.config");
			if (File.Exists(dataSourceFilePath) == true)
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Database",
					ConfigurationFileName = dataSourceFilePath
				};
			}
			else
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Database",
					ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
				};
			}

			this.Provider.Read(this);
		}

		/// <summary>
		/// DatabaseFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 데이터베이스 제공자입니다.
		/// </summary>
		private string dataProvider = "SqlServer";

		/// <summary>
		/// DatabaseFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 데이터베이스 제공자를 가져오거나, 설정합니다.
		/// </summary>
		public string DataProvider
		{
			get { return dataProvider; }
			set { dataProvider = value; }
		}

		/// <summary>
		/// DatabaseFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 데이터베이스 연결 문자열입니다.
		/// </summary>
		private string connectionString = "";

		/// <summary>
		/// DatabaseFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 데이터베이스 연결 문자열을 가져오거나, 설정합니다.
		/// </summary>
		public string ConnectionString
		{
			get { return connectionString; }
			set { connectionString = value; }
		}

		/// <summary>
		/// 연결중인 데이터베이스에서 Procedure의 매개 변수 항목을 DbParameter[]로 반환할 때 캐시에 관리할 지 여부를 정의합니다. 
		/// </summary>
		private bool isParameterCache = false;

		/// <summary>
		/// 연결중인 데이터베이스에서 Procedure의 매개 변수 항목을 DbParameter[]로 반환할 때 캐시에 관리할 지 여부를 정의를 가져오거나, 설정합니다.
		/// </summary>
		public bool IsParameterCache
		{
			get { return isParameterCache; }
			set { isParameterCache = value; }
		}

		/// <summary>
		/// DatabaseFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 데이터베이스 연결 문자열이 있는 섹션입니다.
		/// </summary>
		private string customDefinedSection = "defaultConnectSection";

		/// <summary>
		/// DatabaseFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 데이터베이스 연결 문자열이 있는 섹션을 가져오거나, 설정합니다.
		/// </summary>
		public string CustomDefinedSection
		{
			get { return customDefinedSection; }
			set { customDefinedSection = value; }
		}

		/// <summary>
		/// 연결문자열 암호화 여부를 정의합니다. 
		/// </summary>
		private bool isConnectionStringEncryption = false;

		/// <summary>
		/// 연결문자열 암호화 여부를 가져오거나, 설정합니다.
		/// </summary>
		public bool IsConnectionStringEncryption
		{
			get { return isConnectionStringEncryption; }
			set { isConnectionStringEncryption = value; }
		}

		/// <summary>
		/// 연결문자열 암호화때 사용 할 기본 비밀키입니다.
		/// </summary>
		private string decryptionKey = "12345678901234567890123456789012";

		/// <summary>
		/// 연결문자열 암호화때 사용 할 기본 비밀키를 가져오거나, 설정합니다.
		/// </summary>
		public string DecryptionKey
		{
			get { return decryptionKey; }
			set { decryptionKey = value; }
		}

		public void Dispose()
		{
		}
	}
}
