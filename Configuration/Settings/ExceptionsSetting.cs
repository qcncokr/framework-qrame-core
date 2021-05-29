using System;
using System.IO;

namespace Qrame.CoreFX.Configuration.Settings
{
	/// <summary>
	/// ExceptionFactory 기본 동작에 필요한 구성 항목을 정의합니다.
	/// </summary>
	public class ExceptionsSetting : ApplicationConfig, IDisposable
	{
		/// <summary>
		/// 구성 항목과 설정 원본을 지정합니다.
		/// </summary>
		public ExceptionsSetting()
		{
			this.Provider = new ConfigurationFile<ExceptionsSetting>()
			{
				ConfigurationSection = "Exceptions",
				ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
			};

			this.Provider.Read(this);
		}

		public ExceptionsSetting(string environmentName)
		{
			string dataSourceFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Qrame.{environmentName}.config");
			if (File.Exists(dataSourceFilePath) == true)
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Exceptions",
					ConfigurationFileName = dataSourceFilePath
				};
			}
			else
			{
				this.Provider = new ConfigurationFile<CoreSetting>()
				{
					ConfigurationSection = "Exceptions",
					ConfigurationFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Qrame.config")
				};
			}

			this.Provider.Read(this);
		}

		/// <summary>
		/// ExceptionFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 Handler입니다.
		/// </summary>
		private string defaultHandler = "DefaultException";

		/// <summary>
		/// ExceptionFactory 기본 인스턴스 생성시, 응용 프로그램 구성에 설정할, 기본 Handler를 가져오거나, 설정합니다.
		/// </summary>
		public string DefaultHandler
		{
			get { return defaultHandler; }
			set { defaultHandler = value; }
		}

		/// <summary>
		/// WarningException Handler에서 예외 처리시 응용 프로그램에 예외를 전달 할지 여부입니다.
		/// </summary>
		private bool warningThrowException = false;

		/// <summary>
		/// WarningException Handler에서 예외 처리시 응용 프로그램에 예외를 전달 할지 여부를 가져오거나, 설정합니다.
		/// </summary>
		public bool WarningThrowException
		{
			get { return warningThrowException; }
			set { warningThrowException = value; }
		}

		public void Dispose()
		{
		}
	}
}
