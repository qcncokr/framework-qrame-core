using System.Text;
using System.IO;

namespace Qrame.CoreFX.ExtensionMethod
{
    /// <summary>
    /// MemoryStream Extension Methods that provide conversions to and from strings
    /// </summary>
    public static class MemoryStreamExtensions
    {
        /// <summary>
        /// Returns the content of the stream as a string
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetAsString(this MemoryStream @this, Encoding encoding)
        {
            return encoding.GetString(@this.ToArray());
        }


        /// <summary>
        /// Returns the content of the stream as a string
        /// </summary>
        /// <param name="@this"></param>
        /// <returns></returns>
        public static string GetAsString(this MemoryStream @this)
        {
            return GetAsString(@this, Encoding.Default);
        }

        /// <summary>
        /// Writes the specified string into the memory stream
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="inputString"></param>
        /// <param name="encoding"></param>
        public static void WriteString(this MemoryStream @this, string inputString, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(inputString);
            @this.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes the specified string into the memory stream
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="inputString"></param>
        public static void WriteString(this MemoryStream @this, string inputString)
        {
            WriteString(@this, inputString, Encoding.Default);
        }
    }
}
