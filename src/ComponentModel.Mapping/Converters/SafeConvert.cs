﻿using System;
using System.Globalization;
using System.Linq;

namespace Hasseware.ComponentModel.Mapping.Converters
{
    internal sealed class SafeConvert
    {
        private const double MaxDecimalAsDouble = (double)decimal.MaxValue;

        private const double MinDecimalAsDouble = (double)decimal.MinValue;

        private const float MaxDecimalAsSingle = (float)decimal.MaxValue;

        private const float MinDecimalAsSingle = (float)decimal.MinValue;

        #region ToByte

        public static byte ToByte(string value)
        {
            ulong result = ToUInt64(value);
            return (result > byte.MaxValue) ? (byte)0 : (byte)result;
        }

        public static byte ToByte(byte value)
        {
            return value;
        }

        public static byte ToByte(sbyte value)
        {
            return (value < 0) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(short value)
        {
            return (value < 0 || value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(ushort value)
        {
            return (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(int value)
        {
            return (value < 0) || (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(uint value)
        {
            return (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(long value)
        {
            return (value < 0) || (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(ulong value)
        {
            return (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(decimal value)
        {
            return (value < 0) || (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(float value)
        {
            return (value < 0) || (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(double value)
        {
            return (value < 0) || (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        public static byte ToByte(bool value)
        {
            return value ? (byte)1 : (byte)0;
        }

        public static byte ToByte(char value)
        {
            return (value > byte.MaxValue) ? (byte)0 : (byte)value;
        }

        #endregion

        #region ToSByte

        public static sbyte ToSByte(string value)
        {
            long result = ToInt64(value);
            return (result < sbyte.MinValue || result > sbyte.MaxValue) ? (sbyte)0 : (sbyte)result;
        }

        public static sbyte ToSByte(byte value)
        {
            return (value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(sbyte value)
        {
            return value;
        }

        public static sbyte ToSByte(short value)
        {
            return (value < sbyte.MinValue || value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(ushort value)
        {
            return (value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(int value)
        {
            return (value < sbyte.MinValue || value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(uint value)
        {
            return (value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(long value)
        {
            return (value < sbyte.MinValue || value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(ulong value)
        {
            return (value > (ulong)sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(decimal value)
        {
            return (value < sbyte.MinValue || value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(float value)
        {
            return (value < sbyte.MinValue || value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(double value)
        {
            return (value < sbyte.MinValue || value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        public static sbyte ToSByte(bool value)
        {
            return value ? (sbyte)1 : (sbyte)0;
        }

        public static sbyte ToSByte(char value)
        {
            return (value > sbyte.MaxValue) ? (sbyte)0 : (sbyte)value;
        }

        #endregion

        #region ToInt16

        public static short ToInt16(string value)
        {
            long result = ToInt64(value);
            return (result < short.MinValue || result > short.MaxValue) ? (short)0 : (short)result;
        }

        public static short ToInt16(byte value)
        {
            return value;
        }

        public static short ToInt16(sbyte value)
        {
            return value;
        }

        public static short ToInt16(short value)
        {
            return value;
        }

        public static short ToInt16(ushort value)
        {
            return (value > short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(int value)
        {
            return (value < short.MinValue || value > short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(uint value)
        {
            return (value > short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(long value)
        {
            return (value < short.MinValue || value > short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(ulong value)
        {
            return (value > (ulong)short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(decimal value)
        {
            return (value < short.MinValue || value > short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(float value)
        {
            return (value < short.MinValue || value > short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(double value)
        {
            return (value < short.MinValue || value > short.MaxValue) ? (short)0 : (short)value;
        }

        public static short ToInt16(bool value)
        {
            return value ? (short)1 : (short)0;
        }

        public static short ToInt16(char value)
        {
            return (value > short.MaxValue) ? (short)0 : (short)value;
        }

        #endregion

        #region ToUInt16

        public static ushort ToUInt16(string value)
        {
            ulong result = ToUInt64(value);
            return (result > ushort.MaxValue) ? (ushort)0 : (ushort)result;
        }

        public static ushort ToUInt16(byte value)
        {
            return value;
        }

        public static ushort ToUInt16(sbyte value)
        {
            return (value < 0) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(short value)
        {
            return (value < 0) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(ushort value)
        {
            return value;
        }

        public static ushort ToUInt16(int value)
        {
            return (value < 0 || value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(uint value)
        {
            return (value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(long value)
        {
            return (value < 0 || value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(ulong value)
        {
            return (value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(decimal value)
        {
            return (value < 0 || value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(float value)
        {
            return (value < 0 || value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(double value)
        {
            return (value < 0 || value > ushort.MaxValue) ? (ushort)0 : (ushort)value;
        }

        public static ushort ToUInt16(bool value)
        {
            return value ? (ushort)1 : (ushort)0;
        }

        public static ushort ToUInt16(char value)
        {
            return value;
        }

        #endregion

        #region ToInt32

        public static int ToInt32(string value)
        {
            long result = ToInt64(value);
            return (result <= int.MaxValue && result >= int.MinValue) ? (int)result : 0;
        }

        public static int ToInt32(byte value)
        {
            return value;
        }

        public static int ToInt32(sbyte value)
        {
            return value;
        }

        public static int ToInt32(short value)
        {
            return value;
        }

        public static int ToInt32(ushort value)
        {
            return value;
        }

        public static int ToInt32(int value)
        {
            return value;
        }

        public static int ToInt32(uint value)
        {
            return (value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(long value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(ulong value)
        {
            return (value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(decimal value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(float value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(double value)
        {
            return (value < int.MinValue || value > int.MaxValue) ? 0 : (int)value;
        }

        public static int ToInt32(bool value)
        {
            return value ? 1 : 0;
        }

        public static int ToInt32(char value)
        {
            return value;
        }

        #endregion

        #region ToUInt32

        public static uint ToUInt32(string value)
        {
            ulong result = ToUInt64(value);
            return (result > uint.MaxValue) ? 0 : (uint)result;
        }

        public static uint ToUInt32(byte value)
        {
            return value;
        }

        public static uint ToUInt32(sbyte value)
        {
            return (value < 0) ? 0 : (uint)value;
        }

        public static uint ToUInt32(short value)
        {
            return (value < 0) ? 0 : (uint)value;
        }

        public static uint ToUInt32(ushort value)
        {
            return value;
        }

        public static uint ToUInt32(int value)
        {
            return (value < 0) ? 0 : (uint)value;
        }

        public static uint ToUInt32(uint value)
        {
            return value;
        }

        public static uint ToUInt32(long value)
        {
            return (value < 0 || value > uint.MaxValue) ? 0 : (uint)value;
        }

        public static uint ToUInt32(ulong value)
        {
            return (value > uint.MaxValue) ? 0 : (uint)value;
        }

        public static uint ToUInt32(decimal value)
        {
            return (value < 0 || value > uint.MaxValue) ? 0 : (uint)value;
        }

        public static uint ToUInt32(float value)
        {
            return (value < 0 || value > uint.MaxValue) ? 0 : (uint)value;
        }

        public static uint ToUInt32(double value)
        {
            return (value < 0 || value > uint.MaxValue) ? 0 : (uint)value;
        }

        public static uint ToUInt32(bool value)
        {
            return value ? (uint)1 : 0;
        }

        public static uint ToUInt32(char value)
        {
            return value;
        }

        #endregion

        #region ToInt64

        public static long ToInt64(string value)
        {
            long result = 0;

            // return long.TryParse(value, out result) ? result : 0;            
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            bool neg = value[0] == '-';
            for (int i = neg ? 1 : 0; i < value.Length; i++)
            {
                if ((value[i] >= 48) && (value[i] <= 57))
                {
                    result = (10 * result) + (value[i] - 48);
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            return neg ? result * -1 : result;
        }

        public static long ToInt64(byte value)
        {
            return value;
        }

        public static long ToInt64(sbyte value)
        {
            return value;
        }

        public static long ToInt64(short value)
        {
            return value;
        }

        public static long ToInt64(ushort value)
        {
            return value;
        }

        public static long ToInt64(int value)
        {
            return value;
        }

        public static long ToInt64(uint value)
        {
            return value;
        }

        public static long ToInt64(long value)
        {
            return value;
        }

        public static long ToInt64(ulong value)
        {
            return (value > long.MaxValue) ? 0 : (long)value;
        }

        public static long ToInt64(decimal value)
        {
            return (value < long.MinValue || value > long.MaxValue) ? 0 : (long)value;
        }

        public static long ToInt64(float value)
        {
            return (value < long.MinValue || value > long.MaxValue) ? 0 : (long)value;
        }

        public static long ToInt64(double value)
        {
            return (value < long.MinValue || value > long.MaxValue) ? 0 : (long)value;
        }

        public static long ToInt64(bool value)
        {
            return value ? 1 : 0;
        }

        public static long ToInt64(char value)
        {
            return value;
        }

        #endregion

        #region ToUInt64

        public static ulong ToUInt64(string value)
        {
            ulong result = 0;
            if (value[0] == '-')
            {
                return 0;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if ((value[i] >= 48) && (value[i] <= 57))
                {
                    result = (10 * result) + (value[i] - 48uL);
                }
                else
                {
                    result = 0;
                    break;
                }
            }

            return result;
        }

        public static ulong ToUInt64(byte value)
        {
            return value;
        }

        public static ulong ToUInt64(sbyte value)
        {
            return (value < 0) ? 0 : (ulong)value;
        }

        public static ulong ToUInt64(short value)
        {
            return (value < 0) ? 0 : (ulong)value;
        }

        public static ulong ToUInt64(ushort value)
        {
            return value;
        }

        public static ulong ToUInt64(int value)
        {
            return (value < 0) ? 0 : (ulong)value;
        }

        public static ulong ToUInt64(uint value)
        {
            return value;
        }

        public static ulong ToUInt64(long value)
        {
            return (value < 0) ? 0 : (ulong)value;
        }

        public static ulong ToUInt64(ulong value)
        {
            return value;
        }

        public static ulong ToUInt64(decimal value)
        {
            return (value < 0 || value > ulong.MaxValue) ? 0 : (ulong)value;
        }

        public static ulong ToUInt64(bool value)
        {
            return value ? (ulong)1 : 0;
        }

        public static ulong ToUInt64(char value)
        {
            return value;
        }

        public static ulong ToUInt64(float value)
        {
            return (value < 0 || value > ulong.MaxValue) ? 0 : (ulong)value;
        }

        public static ulong ToUInt64(double value)
        {
            return (value < 0 || value > (double)ulong.MaxValue) ? 0 : (ulong)value;
        }

        #endregion

        #region ToSingle

        public static float ToSingle(string s)
        {
            return ToSingle(s, CultureInfo.CurrentCulture);
        }

        public static float ToSingle(string s, IFormatProvider provider)
        {
            float f;
            return float.TryParse(s, NumberStyles.Number, provider, out f) ? f : 0f;
        }

        public static float ToSingle(byte value)
        {
            return value;
        }

        public static float ToSingle(sbyte value)
        {
            return value;
        }

        public static float ToSingle(short value)
        {
            return value;
        }

        public static float ToSingle(ushort value)
        {
            return value;
        }

        public static float ToSingle(int value)
        {
            return value;
        }

        public static float ToSingle(uint value)
        {
            return value;
        }

        public static float ToSingle(long value)
        {
            return value;
        }

        public static float ToSingle(ulong value)
        {
            return value;
        }

        public static float ToSingle(float value)
        {
            return value;
        }

        public static float ToSingle(double value)
        {
            return (value < (double)float.MinValue || value > (double)float.MaxValue) ? 0f : (float)value;
        }

        public static float ToSingle(decimal value)
        {
            return (float)value;
        }

        public static float ToSingle(bool value)
        {
            return value ? 1 : 0;
        }

        public static float ToSingle(char value)
        {
            return value;
        }

        #endregion

        #region ToDouble

        public static double ToDouble(string s)
        {
            return ToDouble(s, CultureInfo.CurrentCulture);
        }

        public static double ToDouble(string s, IFormatProvider provider)
        {
            double d;
            return double.TryParse(s, NumberStyles.Number, provider, out d) ? d : 0d;
        }

        public static double ToDouble(byte value)
        {
            return value;
        }

        public static double ToDouble(sbyte value)
        {
            return value;
        }

        public static double ToDouble(short value)
        {
            return value;
        }

        public static double ToDouble(ushort value)
        {
            return value;
        }

        public static double ToDouble(int value)
        {
            return value;
        }

        public static double ToDouble(uint value)
        {
            return value;
        }

        public static double ToDouble(long value)
        {
            return value;
        }

        public static double ToDouble(ulong value)
        {
            return value;
        }

        public static double ToDouble(float value)
        {
            return value;
        }

        public static double ToDouble(double value)
        {
            return value;
        }

        public static double ToDouble(decimal value)
        {
            return (double)value;
        }

        public static double ToDouble(bool value)
        {
            return value ? 1 : 0;
        }

        public static double ToDouble(char value)
        {
            return value;
        }

        #endregion

        #region ToDecimal

        public static decimal ToDecimal(string s)
        {
            return ToDecimal(s, CultureInfo.CurrentCulture);
        }

        public static decimal ToDecimal(string s, IFormatProvider provider)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Number, provider, out d) ? d : 0m;
        }

        public static decimal ToDecimal(byte value)
        {
            return value;
        }

        public static decimal ToDecimal(sbyte value)
        {
            return value;
        }

        public static decimal ToDecimal(short value)
        {
            return value;
        }

        public static decimal ToDecimal(ushort value)
        {
            return value;
        }

        public static decimal ToDecimal(int value)
        {
            return value;
        }

        public static decimal ToDecimal(uint value)
        {
            return value;
        }

        public static decimal ToDecimal(long value)
        {
            return value;
        }

        public static decimal ToDecimal(ulong value)
        {
            return value;
        }

        public static decimal ToDecimal(float value)
        {
            return value == MaxDecimalAsSingle
                       ? decimal.MaxValue
                       : value == MinDecimalAsSingle
                             ? decimal.MinValue
                             : value > MaxDecimalAsSingle ? 0m : value < MinDecimalAsSingle ? 0m : (decimal)value;
            /*
            return Math.Abs(value - MaxDecimalAsSingle) < float.Epsilon
                       ? decimal.MaxValue
                       : Math.Abs(value - MinDecimalAsSingle) < float.Epsilon
                             ? decimal.MinValue
                             : value > MaxDecimalAsSingle ? 0m : value < MinDecimalAsSingle ? 0m : (decimal)value;*/
        }

        public static decimal ToDecimal(double value)
        {
            return value == MaxDecimalAsDouble
                       ? decimal.MaxValue
                       : value == MinDecimalAsDouble
                             ? decimal.MinValue
                             : value > MaxDecimalAsDouble ? 0m : value < MinDecimalAsDouble ? 0m : (decimal)value;
            /*
            return Math.Abs(value - MaxDecimalAsDouble) < double.Epsilon
                       ? decimal.MaxValue
                       : Math.Abs(value - MinDecimalAsDouble) < double.Epsilon
                             ? decimal.MinValue
                             : value > MaxDecimalAsDouble ? 0m : value < MinDecimalAsDouble ? 0m : (decimal)value;*/
        }

        public static decimal ToDecimal(decimal value)
        {
            return value;
        }

        public static decimal ToDecimal(bool value)
        {
            return value ? 1 : 0;
        }

        public static decimal ToDecimal(char value)
        {
            return value;
        }

        #endregion

        #region ToBoolean

        public static bool ToBoolean(string s)
        {
            bool b;
            return bool.TryParse(s, out b) ? b : false;
        }

        public static bool ToBoolean(byte value)
        {
            return value != 0;
        }

        public static bool ToBoolean(sbyte value)
        {
            return value != 0;
        }

        public static bool ToBoolean(short value)
        {
            return value != 0;
        }

        public static bool ToBoolean(ushort value)
        {
            return value != 0;
        }

        public static bool ToBoolean(int value)
        {
            return value != 0;
        }

        public static bool ToBoolean(uint value)
        {
            return value != 0;
        }

        public static bool ToBoolean(long value)
        {
            return value != 0;
        }

        public static bool ToBoolean(ulong value)
        {
            return value != 0;
        }

        public static bool ToBoolean(float value)
        {
            return value != 0;
        }

        public static bool ToBoolean(double value)
        {
            return value != 0;
        }

        public static bool ToBoolean(decimal value)
        {
            return value != 0m;
        }

        public static bool ToBoolean(bool value)
        {
            return value;
        }

        public static bool ToBoolean(char value)
        {
            return (value != '0' && value != 0) || value == '1';
        }

        #endregion

        #region ToChar

        public static char ToChar(string value)
        {
            if (value == null || value.Length != 1)
            {
                return (char)0;
            }

            return value[0];
        }

        public static char ToChar(byte value)
        {
            return (char)value;
        }

        public static char ToChar(sbyte value)
        {
            return (value < 0) ? (char)0 : (char)value;
        }

        public static char ToChar(short value)
        {
            return (value < 0) ? (char)0 : (char)value;
        }

        public static char ToChar(ushort value)
        {
            return (char)value;
        }

        public static char ToChar(int value)
        {
            return (value < 0 || value > char.MaxValue) ? (char)0 : (char)value;
        }

        public static char ToChar(uint value)
        {
            return (value > char.MaxValue) ? (char)0 : (char)value;
        }

        public static char ToChar(long value)
        {
            return (value < 0 || value > char.MaxValue) ? (char)0 : (char)value;
        }

        public static char ToChar(ulong value)
        {
            return (value > char.MaxValue) ? (char)0 : (char)value;
        }

        public static char ToChar(decimal value)
        {
            return (value < 0 || value > char.MaxValue) ? (char)0 : (char)value;
        }

        public static char ToChar(float value)
        {
            return (value < 0 || value > char.MaxValue) ? (char)0 : (char)value;
        }

        public static char ToChar(double value)
        {
            return (value < 0 || value > char.MaxValue) ? (char)0 : (char)value;
        }

        public static char ToChar(bool value)
        {
            return value ? '1' : '0';
        }

        #endregion

        #region ToGuid

        public static Guid ToGuid(string s)
        {
            Guid g;
            return Guid.TryParse(s, out g) ? g : Guid.Empty;
        }

        #endregion

        #region ToDateTime

        public static DateTime ToDateTime(string s)
        {
            return ToDateTime(s, CultureInfo.CurrentCulture);
        }

        public static DateTime ToDateTime(string s, IFormatProvider provider)
        {
            DateTime d;
            return DateTime.TryParse(s, provider, DateTimeStyles.None, out d) ? d : DateTime.MinValue;
        }

        #endregion

        #region ToString

        public static string ToString(char[] value)
        {
            if (value == null)
            {
                return null;
            }

            return new string(value);
        }

        #endregion

        #region Other

        public static char[] ToCharArray(string value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToArray();
        }

        #endregion
    }
}
