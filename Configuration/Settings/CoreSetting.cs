using Qrame.CoreFX.Configuration;
using System;
using System.IO;

namespace Qrame.CoreFX.Configuration.Settings
{
	/// <summary>
	/// Qrame의 정보를 표현하는 구성 항목을 정의합니다.
	/// </summary>
	public class CoreSetting : ApplicationConfig, IDisposable
	{
		/// <summary>
		/// 구성 항목과 설정 원본을 지정합니다.
		/// </summary>
		public CoreSetting()
		{
			this.Provider = new ConfigurationFile<CoreSetting>()
			{
				ConfigurationSection = "Core",
				ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
			};

			this.Provider.Read(this);
		}

		public CoreSetting(string environmentName)
		{
			string dataSourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Qrame.{environmentName}.config");
			if (File.Exists(dataSourceFilePath) == true)
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Core",
					ConfigurationFileName = dataSourceFilePath
				};
			}
			else
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Core",
					ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
				};
			}

			this.Provider.Read(this);
		}

		/// <summary>
		/// 응용 프로그램의 프로젝트 명을 정의합니다.
		/// </summary>
		private string applicationName = "Qrame";

		/// <summary>
		/// 응용 프로그램의 프로젝트 명을 가져오거나, 설정합니다.
		/// </summary>
		public string ApplicationName
		{
			get { return applicationName; }
			set { applicationName = value; }
		}

		/// <summary>
		/// 응용 프로그램의 제품 버전을 정의합니다.
		/// </summary>
		private string productVersion = "1.0";

		/// <summary>
		/// 응용 프로그램의 제품 버전을 가져오거나, 설정합니다.
		/// </summary>
		public string ProductVersion
		{
			get { return productVersion; }
			set { productVersion = value; }
		}

		public void Dispose()
		{
		}
	}
}
