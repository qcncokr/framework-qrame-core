using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Qrame.CoreFX.HttpClient
{
	public class JsonContent : StringContent
	{
		public JsonContent(object value) : base(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
		{
		}

		public JsonContent(object value, string mimeType) : base(JsonConvert.SerializeObject(value), Encoding.UTF8, mimeType)
		{
		}
	}
}
