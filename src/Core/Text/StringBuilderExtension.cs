using System.Globalization;
using System.Reflection;

namespace System.Text
{
    public static class StringBuilderExtension
    {
        public static StringBuilder AppendFormat(this StringBuilder @this, IFormatProvider provider, String format, Func<String, Object> func)
        {
            if (format == null || func == null)
            {
                throw new ArgumentNullException((format == null) ? "format" : "func");
            }
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.EndContractBlock();

            if (@this.Capacity < @this.Length + format.Length)
                @this.EnsureCapacity(@this.Capacity + format.Length);

            StringBuilder name = new StringBuilder();
            int pos = 0, len = format.Length;
            char ch = '\x0';

            while (true)
            {
                int p = pos;
                int i = pos;

                while (pos < len)
                {
                    ch = format[pos];
                    pos++;

                    if (ch == '}')
                    {
                        // Treat as escape character for }}
                        if (pos < len && format[pos] == '}') pos++;
                        else FormatError(null);
                    }
                    if (ch == '{')
                    {
                        // Treat as escape character for {{
                        if (pos < len && format[pos] == '{') pos++;
                        else
                        {
                            pos--;
                            break;
                        }
                    }
                    @this.Append(ch);
                }

                if (pos == len)
                    break;

                pos++;
                if (pos == len || !IsValidName(ch = format[pos]))
                    FormatError(null);

                name.Length = 0;

                do
                {
                    name.Append(ch);
                    pos++;

                    if (pos < len) ch = format[pos];
                    else FormatError(null);
                }
                while (IsValidName(ch));

                while (pos < len && (ch = format[pos]) == ' ') pos++;

                bool leftJustify = false;
                int width = 0;

                if (ch == ',')
                {
                    pos++;
                    while (pos < len && format[pos] == ' ') pos++;

                    if (pos < len) ch = format[pos];
                    else FormatError(null);

                    if (ch == '-')
                    {
                        leftJustify = true;
                        pos++;

                        if (pos < len) ch = format[pos];
                        else FormatError(null);
                    }
                    if (ch < '0' || ch > '9')
                    {
                        FormatError(null);
                    }
                    do
                    {
                        width = width * 10 + ch - '0';
                        pos++;

                        if (pos < len) ch = format[pos];
                        else FormatError(null);
                    }
                    while (ch >= '0' && ch <= '9' && width < 1000000);
                }

                while (pos < len && (ch = format[pos]) == ' ') pos++;

                Object arg = null;
                StringBuilder fmt = null;

                try
                {
                    arg = func(name.ToString());
                }
                catch (Exception ex)
                {
                    FormatError(ex);
                }
                if (ch == ':')
                {
                    pos++;
                    p = pos;
                    i = pos;

                    while (true)
                    {
                        if (pos < len) ch = format[pos];
                        else FormatError(null);

                        pos++;

                        if (ch == '{')
                        {
                            // Treat as escape character for {{
                            if (pos < len && format[pos] == '{') pos++;
                            else FormatError(null);
                        }
                        else if (ch == '}')
                        {
                            // Treat as escape character for }}
                            if (pos < len && format[pos] == '}') pos++;
                            else
                            {
                                pos--;
                                break;
                            }
                        }

                        if (fmt == null)
                            fmt = new StringBuilder();

                        fmt.Append(ch);
                    }
                }
                if (ch != '}')
                    FormatError(null);

                pos++;
                String sFmt = null, s = null;
                ICustomFormatter cf = null;

                if (provider != null)
                {
                    cf = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));
                }
                if (cf != null)
                {
                    if (fmt != null)
                        sFmt = fmt.ToString();

                    s = cf.Format(sFmt, arg, provider);
                }
                if (s == null)
                {
                    IFormattable formattableArg = arg as IFormattable;

                    if (formattableArg != null)
                    {
                        if (sFmt == null && fmt != null)
                            sFmt = fmt.ToString();

                        s = formattableArg.ToString(sFmt, provider);
                    }
                    else if (arg != null)
                    {
                        s = arg.ToString();
                    }
                }
                if (s != null)
                {
                    int pad = width - s.Length;

                    if (pad > 0)
                    {
                        if (leftJustify)
                        {
                            @this.Append(s);
                            @this.Append(' ', pad);
                        }
                        else
                        {
                            @this.Append(' ', pad);
                            @this.Append(s);
                        }
                    }
                    else @this.Append(s);
                }
            }
            return @this;
        }

        public static string ToSlug(this string @this)
        {
            var sb = new StringBuilder(@this.Length);
            bool prevdash = false;

            foreach (char c in @this)
            {
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    sb.Append(Char.ToLower(c));
                    prevdash = false;
                }
                else if ((int)c >= 128)
                {
                    string remapping;
                    if (TryRemapInternationalCharToAscii(c, out remapping))
                    {
                        sb.Append(remapping);
                        prevdash = false;
                    }
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
            }
            if (prevdash) sb.Length--;
            return sb.ToString();
        }

        public static int TrimmedCompareTo(this string @this, string str, bool ignoreCase)
        {
            return TrimmedCompare(@this, str, ignoreCase);
        }

        public static int TrimmedCompareTo(this string @this, string str, CultureInfo culture, CompareOptions options)
        {
            return TrimmedCompare(@this, str, culture, options);
        }

        public static int TrimmedCompare(string strA, string strB, bool ignoreCase)
        {
            return TrimmedCompare(strA, strB, CultureInfo.CurrentCulture,
                ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public static int TrimmedCompare(string strA, string strB, CultureInfo culture, CompareOptions options)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            if (strA == null || strB == null)
            {
                if ((object)strA == (object)strB)
                    return 0;

                return (strA != null) ? 1 : -1;
            }
            int offsetA, lengthA, offsetB, lengthB;
            FindTrimmedSubstring(strA, out offsetA, out lengthA);
            FindTrimmedSubstring(strB, out offsetB, out lengthB);

            return culture.CompareInfo.Compare(strA, offsetA, lengthA, strB, offsetB, lengthB);
        }

        /// <summary>
        /// Find the trimmed substring offset and length in the current instance
        /// </summary>
        private static void FindTrimmedSubstring(string str, out int offset, out int length)
        {
            offset = 0;
            while (Char.IsWhiteSpace(str[offset]) && offset < str.Length)
                offset++;

            length = str.Length - offset;
            while (Char.IsWhiteSpace(str[offset + length - 1]) && length > 0)
                length--;
        }

        private static bool TryRemapInternationalCharToAscii(char c, out string result)
        {
            switch (Char.ToLower(c))
            {
                case 'à':
                case 'å':
                case 'á':
                case 'â':
                case 'ä':
                case 'ã':
                case 'ą':
                    result = "a";
                    return true;
                case 'è':
                case 'é':
                case 'ê':
                case 'ë':
                case 'ę':
                    result = "e";
                    return true;
                case 'ì':
                case 'í':
                case 'î':
                case 'ï':
                case 'ı':
                    result = "i";
                    return true;
                case 'ò':
                case 'ó':
                case 'ô':
                case 'õ':
                case 'ö':
                case 'ø':
                case 'ő':
                case 'ð':
                    result = "o";
                    return true;
                case 'ù':
                case 'ú':
                case 'û':
                case 'ü':
                case 'ŭ':
                case 'ů':
                    result = "u";
                    return true;
                case 'ç':
                case 'ć':
                case 'č':
                case 'ĉ':
                    result = "c";
                    return true;
                case 'ż':
                case 'ź':
                case 'ž':
                    result = "z";
                    return true;
                case 'ś':
                case 'ş':
                case 'š':
                case 'ŝ':
                    result = "s";
                    return true;
                case 'ñ':
                case 'ń':
                    result = "n";
                    return true;
                case 'ý':
                case 'ÿ':
                    result = "y";
                    return true;
                case 'ğ':
                case 'ĝ':
                    result = "g";
                    return true;
                case 'ř':
                    result = "r";
                    return true;
                case 'ł':
                    result = "l";
                    return true;
                case 'đ':
                    result = "d";
                    return true;
                case 'ß':
                    result = "ss";
                    return true;
                case 'Þ':
                    result = "th";
                    return true;
                case 'ĥ':
                    result = "h";
                    return true;
                case 'ĵ':
                    result = "j";
                    return true;
                default:
                    result = String.Empty;
                    return false;
            }
        }

        private static bool IsValidName(char c)
        {
            return Char.IsLetterOrDigit(c) || (c == '_') || (c == '@') || (c == '.');
        }

        private static void FormatError(Exception ex)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(StringBuilder));
            var rm = new System.Resources.ResourceManager(assembly.GetName().Name, assembly);
            throw new FormatException(rm.GetString("Format_InvalidString"), ex);
        }
    }

}
