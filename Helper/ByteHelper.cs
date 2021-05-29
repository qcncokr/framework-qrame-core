using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qrame.CoreFX.Helper
{
    public struct ByteSize : IComparable<ByteSize>, IEquatable<ByteSize>
    {
        public static readonly ByteSize MinValue = ByteSize.FromBits(0);
        public static readonly ByteSize MaxValue = ByteSize.FromBits(long.MaxValue);

        public const long BitsInByte = 8;
        public const long BytesInKiloByte = 1024;
        public const long BytesInMegaByte = 1048576;
        public const long BytesInGigaByte = 1073741824;
        public const long BytesInTeraByte = 1099511627776;
        public const long BytesInPetaByte = 1125899906842624;

        public const string BitSymbol = "b";
        public const string ByteSymbol = "B";
        public const string KiloByteSymbol = "KB";
        public const string MegaByteSymbol = "MB";
        public const string GigaByteSymbol = "GB";
        public const string TeraByteSymbol = "TB";
        public const string PetaByteSymbol = "PB";

        public long Bits { get; private set; }
        public double Bytes { get; private set; }
        public double KiloBytes => Bytes / BytesInKiloByte;
        public double MegaBytes => Bytes / BytesInMegaByte;
        public double GigaBytes => Bytes / BytesInGigaByte;
        public double TeraBytes => Bytes / BytesInTeraByte;
        public double PetaBytes => Bytes / BytesInPetaByte;

        public string LargestWholeNumberSymbol
        {
            get
            {
                if (Math.Abs(this.PetaBytes) >= 1)
                    return ByteSize.PetaByteSymbol;

                if (Math.Abs(this.TeraBytes) >= 1)
                    return ByteSize.TeraByteSymbol;

                if (Math.Abs(this.GigaBytes) >= 1)
                    return ByteSize.GigaByteSymbol;

                if (Math.Abs(this.MegaBytes) >= 1)
                    return ByteSize.MegaByteSymbol;

                if (Math.Abs(this.KiloBytes) >= 1)
                    return ByteSize.KiloByteSymbol;

                if (Math.Abs(this.Bytes) >= 1)
                    return ByteSize.ByteSymbol;

                return ByteSize.BitSymbol;
            }
        }

        public double LargestWholeNumberValue
        {
            get
            {
                if (Math.Abs(this.PetaBytes) >= 1)
                    return this.PetaBytes;

                if (Math.Abs(this.TeraBytes) >= 1)
                    return this.TeraBytes;

                if (Math.Abs(this.GigaBytes) >= 1)
                    return this.GigaBytes;

                if (Math.Abs(this.MegaBytes) >= 1)
                    return this.MegaBytes;

                if (Math.Abs(this.KiloBytes) >= 1)
                    return this.KiloBytes;

                if (Math.Abs(this.Bytes) >= 1)
                    return this.Bytes;

                return this.Bits;
            }
        }

        public ByteSize(double byteSize)
            : this()
        {
            Bits = (long)Math.Ceiling(byteSize * BitsInByte);

            Bytes = byteSize;
        }

        public static ByteSize FromBits(long value)
        {
            return new ByteSize(value / (double)BitsInByte);
        }

        public static ByteSize FromBytes(double value)
        {
            return new ByteSize(value);
        }

        public static ByteSize FromKiloBytes(double value)
        {
            return new ByteSize(value * BytesInKiloByte);
        }

        public static ByteSize FromMegaBytes(double value)
        {
            return new ByteSize(value * BytesInMegaByte);
        }

        public static ByteSize FromGigaBytes(double value)
        {
            return new ByteSize(value * BytesInGigaByte);
        }

        public static ByteSize FromTeraBytes(double value)
        {
            return new ByteSize(value * BytesInTeraByte);
        }

        public static ByteSize FromPetaBytes(double value)
        {
            return new ByteSize(value * BytesInPetaByte);
        }
        
        public override string ToString()
        {
            return this.ToString("0.##", CultureInfo.CurrentCulture);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            if (!format.Contains("#") && !format.Contains("0"))
                format = "0.## " + format;

            if (provider == null) provider = CultureInfo.CurrentCulture;

            Func<string, bool> has = s => format.IndexOf(s, StringComparison.CurrentCultureIgnoreCase) != -1;
            Func<double, string> output = n => n.ToString(format, provider);

            if (has("PB"))
                return output(this.PetaBytes);
            if (has("TB"))
                return output(this.TeraBytes);
            if (has("GB"))
                return output(this.GigaBytes);
            if (has("MB"))
                return output(this.MegaBytes);
            if (has("KB"))
                return output(this.KiloBytes);

            if (format.IndexOf(ByteSize.ByteSymbol) != -1)
                return output(this.Bytes);

            if (format.IndexOf(ByteSize.BitSymbol) != -1)
                return output(this.Bits);

            return string.Format("{0} {1}", this.LargestWholeNumberValue.ToString(format, provider), this.LargestWholeNumberSymbol);
        }

        public override bool Equals(object value)
        {
            if (value == null)
                return false;

            ByteSize other;
            if (value is ByteSize)
                other = (ByteSize)value;
            else
                return false;

            return Equals(other);
        }

        public bool Equals(ByteSize value)
        {
            return this.Bits == value.Bits;
        }

        public override int GetHashCode()
        {
            return this.Bits.GetHashCode();
        }

        public int CompareTo(ByteSize other)
        {
            return this.Bits.CompareTo(other.Bits);
        }

        public ByteSize Add(ByteSize bs)
        {
            return new ByteSize(this.Bytes + bs.Bytes);
        }

        public ByteSize AddBits(long value)
        {
            return this + FromBits(value);
        }

        public ByteSize AddBytes(double value)
        {
            return this + ByteSize.FromBytes(value);
        }

        public ByteSize AddKiloBytes(double value)
        {
            return this + ByteSize.FromKiloBytes(value);
        }

        public ByteSize AddMegaBytes(double value)
        {
            return this + ByteSize.FromMegaBytes(value);
        }

        public ByteSize AddGigaBytes(double value)
        {
            return this + ByteSize.FromGigaBytes(value);
        }

        public ByteSize AddTeraBytes(double value)
        {
            return this + ByteSize.FromTeraBytes(value);
        }

        public ByteSize AddPetaBytes(double value)
        {
            return this + ByteSize.FromPetaBytes(value);
        }

        public ByteSize Subtract(ByteSize bs)
        {
            return new ByteSize(this.Bytes - bs.Bytes);
        }

        public static ByteSize operator +(ByteSize b1, ByteSize b2)
        {
            return new ByteSize(b1.Bytes + b2.Bytes);
        }

        public static ByteSize operator ++(ByteSize b)
        {
            return new ByteSize(b.Bytes + 1);
        }

        public static ByteSize operator -(ByteSize b)
        {
            return new ByteSize(-b.Bytes);
        }

        public static ByteSize operator -(ByteSize b1, ByteSize b2)
        {
            return new ByteSize(b1.Bytes - b2.Bytes);
        }

        public static ByteSize operator --(ByteSize b)
        {
            return new ByteSize(b.Bytes - 1);
        }

        public static bool operator ==(ByteSize b1, ByteSize b2)
        {
            return b1.Bits == b2.Bits;
        }

        public static bool operator !=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits != b2.Bits;
        }

        public static bool operator <(ByteSize b1, ByteSize b2)
        {
            return b1.Bits < b2.Bits;
        }

        public static bool operator <=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits <= b2.Bits;
        }

        public static bool operator >(ByteSize b1, ByteSize b2)
        {
            return b1.Bits > b2.Bits;
        }

        public static bool operator >=(ByteSize b1, ByteSize b2)
        {
            return b1.Bits >= b2.Bits;
        }

        public static ByteSize Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException("s", "String is null or whitespace");

            s = s.TrimStart();

            var num = 0;
            var found = false;

            var decimalSeparator = Convert.ToChar(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            var groupSeparator = Convert.ToChar(NumberFormatInfo.CurrentInfo.NumberGroupSeparator);

            for (num = 0; num < s.Length; num++)
                if (!(char.IsDigit(s[num]) || s[num] == decimalSeparator || s[num] == groupSeparator))
                {
                    found = true;
                    break;
                }

            if (found == false)
                throw new FormatException($"No byte indicator found in value '{ s }'.");

            int lastNumber = num;

            string numberPart = s.Substring(0, lastNumber).Trim();
            string sizePart = s.Substring(lastNumber, s.Length - lastNumber).Trim();

            double number;
            if (!double.TryParse(numberPart, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out number))
                throw new FormatException($"No number found in value '{ s }'.");

            switch (sizePart)
            {
                case "b":
                    if (number % 1 != 0)
                        throw new FormatException($"Can't have partial bits for value '{ s }'.");

                    return FromBits((long)number);

                case "B":
                    return FromBytes(number);

                case "KB":
                case "kB":
                case "kb":
                    return FromKiloBytes(number);

                case "MB":
                case "mB":
                case "mb":
                    return FromMegaBytes(number);

                case "GB":
                case "gB":
                case "gb":
                    return FromGigaBytes(number);

                case "TB":
                case "tB":
                case "tb":
                    return FromTeraBytes(number);

                case "PB":
                case "pB":
                case "pb":
                    return FromPetaBytes(number);

                default:
                    throw new FormatException($"Bytes of magnitude '{ sizePart }' is not supported.");
            }
        }

        public static bool TryParse(string s, out ByteSize result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch
            {
                result = new ByteSize();
                return false;
            }
        }
    }
}
