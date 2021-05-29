using System.IO;
using System.Reflection;
using System.Drawing;
using System;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class AssemblyExtensions
    {
        public static string GetStringEmbeddedResource(this Assembly @this, string resourceName)
        {
            using (Stream stream = @this.GetStreamEmbeddedResource(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader Reader = new StreamReader(stream))
                    {
                        return Reader.ReadToEnd();
                    }
                }
            }
            return null;
        }

        public static Stream GetStreamEmbeddedResource(this Assembly @this, string resourceName)
        {
            try
            {
                return @this.GetManifestResourceStream(resourceName);
            }
            catch
            {
                return null;
            }
        }

        public static Image GetImageEmbeddedResource(this Assembly @this, string resourceName)
        {
            using (Stream stream = @this.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    return new Bitmap(stream);
                }

                return null;
            }
        }

        public static DateTime GetLinkerTimestamp(this Assembly @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("@this", "@this는 null일 수 없습니다");
            }

            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;

            byte[] buffer = new byte[2048];
            string path = @this.Location;
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, buffer.Length);
            }

            int i = BitConverter.ToInt32(buffer, peHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(buffer, i + linkerTimestampOffset);

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
            dateTime = dateTime.AddSeconds(secondsSince1970);
            dateTime = dateTime.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dateTime).Hours);
            return dateTime;
        }
    }
}
