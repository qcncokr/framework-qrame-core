using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class ByteExtensions
    {
        public static byte[] Combine(params byte[][] @this)
        {
            byte[] result = new byte[@this.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in @this)
            {
                Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }
            return result;
        }

        public static byte[] Combine(this byte[] @this, byte[] bind)
        {
            byte[] result = new byte[@this.Length + bind.Length];
            Buffer.BlockCopy(@this, 0, result, 0, @this.Length);
            Buffer.BlockCopy(bind, 0, result, @this.Length, bind.Length);
            return result;
        }

        public static int Find(this byte[] @this, byte[] search, int startIndex = 0)
        {
            int result = -1;
            int matchIndex = 0;

            for (int i = startIndex; i < @this.Length; i++)
            {
                if (@this[i] == search[matchIndex])
                {
                    if (matchIndex == (search.Length - 1))
                    {
                        result = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (@this[i] == search[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }
            }

            return result;
        }

        public static byte[] Replace(this byte[] @this, byte[] search, byte[] replace)
        {
            byte[] result = null;
            int index = Find(@this, search);

            if (index >= 0)
            {
                result = new byte[@this.Length - search.Length + replace.Length];

                Buffer.BlockCopy(@this, 0, result, 0, index);
                Buffer.BlockCopy(replace, 0, result, index, replace.Length);
                Buffer.BlockCopy(@this, index + search.Length, result, index + replace.Length, @this.Length - (index + search.Length));
            }

            return result;
        }

        /// <summary>
        /// 바이트 배열을 String으로 변환 합니다.
        /// </summary>      
        public static string ToString(this byte[] @this, Encoding encoding)
        {
            return encoding.GetString(@this);
        }

        /// <summary>
        /// 바이트 배열을 Hex 문자열로 반환 합니다.
        /// </summary>
        public static string ToHex(this byte[] @this)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < @this.Length; ++i)
            {
                sb.Append(@this[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 바이트를 Hex 문자열로 반환 합니다.
        /// </summary>      
        public static string ToHex(this byte @this)
        {
            return @this.ToString("x2");
        }

        /// <summary>
        /// 바이트 데이터가 동일한지 비교합니다.
        /// </summary>    
        public static bool AreEqual(this byte[] @this, byte[] target)
        {
            if (@this == null || target == null)
            {
                return false;
            }

            if (ReferenceEquals(@this, target))
            {
                return true;
            }

            if (@this.Length != target.Length)
            {
                return false;
            }

            for (int i = 0; i < @this.Length; i++)
            {
                if (@this[i] != target[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static string ToBase64String(this byte[] @this)
        {
            return Convert.ToBase64String(@this);
        }

        public static String ToBase64String(this byte[] @this, Base64FormattingOptions options)
        {
            return Convert.ToBase64String(@this, options);
        }

        public static String ToBase64String(this byte[] @this, Int32 offset, Int32 length)
        {
            return Convert.ToBase64String(@this, offset, length);
        }

        public static String ToBase64String(this byte[] @this, Int32 offset, Int32 length, Base64FormattingOptions options)
        {
            return Convert.ToBase64String(@this, offset, length, options);
        }

        public static Image ToImage(this byte[] @this)
        {
            using (var ms = new MemoryStream(@this))
            {
                return Image.FromStream(ms);
            }
        }

        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            return new MemoryStream(@this);
        }
    }
}
