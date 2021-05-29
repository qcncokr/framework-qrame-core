using System;
using System.IO;

namespace Qrame.CoreFX.Configuration.Settings
{
	/// <summary>
	/// LogFactory 기본 동작에 필요한 구성 항목을 정의합니다.
	/// </summary>
	public class DiagnosticsSetting : ApplicationConfig, IDisposable
	{
		/// <summary>
		/// 구성 항목과 설정 원본을 지정합니다.
		/// </summary>
		public DiagnosticsSetting()
		{
			this.Provider = new ConfigurationFile<DiagnosticsSetting>()
			{
				ConfigurationSection = "Diagnostics",
				ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
			};

			this.Provider.Read(this);
		}
		
		public DiagnosticsSetting(string environmentName)
		{
			string dataSourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Qrame.{environmentName}.config");
			if (File.Exists(dataSourceFilePath) == true)
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Diagnostics",
					ConfigurationFileName = dataSourceFilePath
				};
			}
			else
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Diagnostics",
					ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
				};
			}

			this.Provider.Read(this);
		}

		/// <summary>
		/// LogFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 Adapter입니다.
		/// </summary>
		private string defaultAdapter = "";

		/// <summary>
		/// LogFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 Adapter를 가져오거나, 설정합니다.
		/// </summary>
		public string DefaultAdapter
		{
			get { return defaultAdapter; }
			set { defaultAdapter = value; }
		}

		public void Dispose()
		{
		}
	}
}
