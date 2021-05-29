using System.IO;
using System.Net.Http;

namespace Qrame.CoreFX.HttpClient
{
    public class FileContent : MultipartFormDataContent
    {
        public FileContent(string filePath, string paramName)
        {
            var filestream = File.Open(filePath, FileMode.Open);
            var filename = Path.GetFileName(filePath);

            Add(new StreamContent(filestream), paramName, filename);
        }
    }
}
