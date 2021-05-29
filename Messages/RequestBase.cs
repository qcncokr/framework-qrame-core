using System;
using System.Data;
using System.Configuration;

using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Qrame.CoreFX.Messages
{
	/// <summary>
	/// WCF 요청에 대한 베이스 기능을 제공합니다.
	/// </summary>
	[DataContract(Namespace = "http://www.tempuri.com/types/")]
	public class RequestBase
	{
		/// <summary>
		/// 클라이언트에 대한 태그 식별자입니다.
		/// </summary>
		[DataMember]
		public string ClientTag;

		/// <summary>
		/// 보안 토큰 식별자입니다.
		/// </summary>
		[DataMember]
		public string AccessTokenID;

		/// <summary>
		/// 버전 정보입니다.
		/// </summary>
		[DataMember]
		public string Version;

		/// <summary>
		/// 요청 식별자입니다.
		/// </summary>
		[DataMember]
		public string RequestID;

		/// <summary>
		/// 요청에 대한 부가 정보입니다.
		/// </summary>
		[DataMember]
		public Dictionary<string, string> LoadOptions;

		/// <summary>
		/// 요청에 대한 기본 정보입니다.
		/// </summary>
		[DataMember]
		public string Action;

		/// <summary>
		/// 실행환경 정보입니다.
		/// </summary>
		[DataMember]
		public string Environment;

		/// <summary>
		/// WCF 요청의 유효성을 3가지 방식으로 검사합니다. ClientTag, AccessToken, User Credentials
		/// </summary>
		/// <param name="request">요청 메시지에 필요한 기본 구성 객체입니다.</param>
		/// <param name="response">응답 메시지에 필요한 기본 구성 객체입니다.</param>
		/// <returns>유효한 요청이면 true를 아니면 false를 반환합니다.</returns>
		public virtual bool ValidRequest(RequestBase request, ResponseBase response)
		{
			string validClientTag = ConfigurationManager.AppSettings["validateClientTag"];
			if (string.IsNullOrEmpty(validClientTag) == false)
			{
				if (request.ClientTag != validClientTag)
				{
					response.Acknowledge = AcknowledgeType.Failure;
					response.ExceptionText = "알수 없는 WCF 요청입니다.";
					return false;
				}
			}

			return true;
		}
	}
}
