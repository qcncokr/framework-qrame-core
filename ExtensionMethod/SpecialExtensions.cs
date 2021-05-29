using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Drawing;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class SpecialExtensions
    {
        public static string SpecialMethod<T>(this T @this)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            return @this.ToString();
        }
    }
}
