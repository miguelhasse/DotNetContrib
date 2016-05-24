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
