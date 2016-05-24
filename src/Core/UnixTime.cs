using System.ComponentModel;
using System.Runtime.Serialization;

namespace System
{
    /// <summary>
    /// Unix time is simply a count of seconds that have elapsed since 
    /// January 1 1970, 00:00:00 UTC (the Unix Epoch), 
    /// </summary>
	[Serializable]
	[TypeConverter(typeof(UnixTimeTypeConverter))]
	public struct UnixTime : IComparable, IFormattable, IConvertible, ISerializable, IComparable<UnixTime>, IEquatable<UnixTime>
	{
		private double _timestamp;

		private static class MagicValues
		{
			public const string PropertyName = "Timestamp";
		}

		#region Constructors

		/// <summary>
		/// Creates an instance of System.UnixTime from the Unix
		/// timestamp value.
		/// </summary>
		/// <param name="timestamp">A 64-bit integer representing a Unix Time.</param>
		public UnixTime(long timestamp)
		{
			_timestamp = timestamp;
		}

		/// <summary>
		/// Creates an instance of System.UnixTime from the Unix
		/// timestamp value.
		/// </summary>
		/// <param name="timestamp">A 32-bit integer representing a Unix Time.</param>
		public UnixTime(int timestamp)
		{
			_timestamp = timestamp;
		}

		/// <summary>
		/// Creates an instance of System.UnixTime from the Unix timestamp value.
		/// </summary>
		/// <param name="timestamp">A double-precision floating-point number representing a Unix Time.</param>
		public UnixTime(double timestamp)
		{
			_timestamp = timestamp;
		}

		/// <summary>
		/// Creates an instance of System.UnixTime from the System.DateTime
		/// instance.
		/// </summary>
		/// <param name="datetime">An instance of System.DateTime.</param>
		public UnixTime(DateTime datetime)
		{
			_timestamp = UnixTime.FromDateTime(datetime);
		}

		/// <summary>
		/// Constructs an instance of System.UnixTime from serialized information.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public UnixTime(SerializationInfo info, StreamingContext context)
		{
			_timestamp = (double)info.GetValue(MagicValues.PropertyName, typeof(string));
		}

		#endregion

		#region Public Members
		/// <summary>
		/// Gets/sets the Unix Time value.
		/// </summary>
		public double Timestamp
		{
			get
			{
				return _timestamp;
			}
			set
			{
				_timestamp = value;
			}
		}

		/// <summary>
		/// Gets a System.DateTime in local time represented by this
		/// Unix Time value.
		/// </summary>
		public DateTime DateTime
		{
			get
			{
				return UnixTime.ToLocalDateTime(this.Timestamp);
			}
		}

		/// <summary>
		/// Gets a UTC System.DateTime time represented by this
		/// Unix Time value.
		/// </summary>
		public DateTime DateTimeUtc
		{
			get
			{
				return UnixTime.ToUniversalDateTime(this.Timestamp);
			}
		}
		#endregion

		#region Implicit Operators
		/// <summary>
		/// Converts a System.UnixTime to a System.Int64 value.
		/// </summary>
		/// <param name="value">An instance of System.UnixTime.</param>
		/// <returns>A System.Double value.</returns>
		public static implicit operator double(UnixTime value)
		{
			return value.Timestamp;
		}

		/// <summary>
		/// Converts a System.Int64 to a System.UnixTime value.
		/// </summary>
		/// <param name="value">A System.Double value.</param>
		/// <returns>An instance of System.UnixTime.</returns>
		public static implicit operator UnixTime(double value)
		{
			return new UnixTime(value);
		}

		/// <summary>
		/// Converts a System.UnixTime to a System.Int64 value.
		/// </summary>
		/// <param name="value">An instance of System.UnixTime.</param>
		/// <returns>A System.Int64 value.</returns>
		public static implicit operator long(UnixTime value)
		{
			return (long)value.Timestamp;
		}

		/// <summary>
		/// Converts a System.Int64 to a System.UnixTime value.
		/// </summary>
		/// <param name="value">A System.Int64 value.</param>
		/// <returns>An instance of System.UnixTime.</returns>
		public static implicit operator UnixTime(long value)
		{
			return new UnixTime(value);
		}

		/// <summary>
		/// Converts a System.UnixTime to a System.DateTime value.
		/// </summary>
		/// <param name="value">An instance of System.UnixTime.</param>
		/// <returns>A System.Int64 value.</returns>
		public static implicit operator DateTime(UnixTime value)
		{
			return value.DateTime;
		}

		/// <summary>
		/// Converts a System.DateTime to a System.UnixTime value.
		/// </summary>
		/// <param name="value">A System.Int64 value.</param>
		/// <returns>An instance of System.UnixTime.</returns>
		public static implicit operator UnixTime(DateTime value)
		{
			return new UnixTime(value);
		}

		/// <summary>
		/// Converts a System.UnixTime to a System.TimeSpan setting the 
		/// total seconds of the TimeSpan to the total seconds since the
		/// Unix Epoch.
		/// </summary>
		/// <param name="value">An instance of System.UnixTime.</param>
		/// <returns>An instance of System.TimeSpan.</returns>
		public static implicit operator TimeSpan(UnixTime value)
		{
			return TimeSpan.FromSeconds(value);
		}

		/// <summary>
		/// Converts a System.TimeSpan to a System.UnixTime value setting
		/// the total seconds since Unix Epoch to the total seconds in the
		/// System.TimeSpan.
		/// </summary>
		/// <param name="value">An instance of System.TimeSpan.</param>
		/// <returns>An instance of System.UnixTime.</returns>
		public static implicit operator UnixTime(TimeSpan value)
		{
			return new UnixTime(value.TotalSeconds);
		}
		#endregion

		#region Public Overrides
		/// <summary>
		/// Returns the value as a fully formatted string.
		/// </summary>
		/// <returns>A System.String value.</returns>
		public override string ToString()
		{
			return this.Timestamp.ToString();
		}

		/// <summary>
		/// Indicates whether this instance and a specified object are equal.
		/// </summary>
		/// <param name="obj">Another object to compare to.</param>
		/// <returns>True if obj and this instance are the same type and represent the same 
		/// value; False otherwise.</returns>
		public override bool Equals(object obj)
		{
			bool returnValue = false;

			if (obj is UnixTime)
			{
				returnValue = (((UnixTime)obj).Timestamp == this.Timestamp);
			}

			return returnValue;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return this.Timestamp.GetHashCode();
		}
		#endregion

		#region Static Members
		/// <summary>
		/// Represents the date January 1 1970, 00:00:00 UTC.
		/// </summary>
		public static DateTime Epoch
		{
			get { return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc); }
		}

		/// <summary>
		/// Convert a System.DateTime value to a Unix Time.
		/// </summary>
		/// <param name="value">System.DateTime object.</param>
		/// <returns>A 64-bit integer representing the Unix Time.</returns>
		public static long FromDateTime(DateTime value)
		{
			long returnValue = 0;

			DateTime internalValue = DateTime.MinValue;

			if (value.Kind == DateTimeKind.Utc ||
				value.Kind == DateTimeKind.Unspecified)
			{
				internalValue = value;
			}
			else if (value.Kind == DateTimeKind.Local)
			{
				internalValue = value.ToUniversalTime();
			}

			// ***
			// *** Formula:
			// ***
			// *** UNIX.TS = DateTime - Epoch
			// ***

			// ***
			// *** Create TimeSpan by subtracting the value provided from
			// *** the Unix Epoch. Return the total seconds (which is a 
			// *** UNIX timestamp)
			// ***
			returnValue = Convert.ToInt64(internalValue.Subtract(UnixTime.Epoch).TotalSeconds);

			return returnValue;
		}

		/// <summary>
		/// Converts a Unix Time to a System.DateTime object in
		/// universal time.
		/// </summary>
		/// <param name="value">A 64-bit integer representing the Unix Time.</param>
		/// <returns>System.DateTime object.</returns>
		public static DateTime ToUniversalDateTime(long value)
		{
            // *** DateTime = Epoch + UNIX.TS
            return UnixTime.Epoch.Add(TimeSpan.FromSeconds(value));
		}

		/// <summary>
		/// Converts a Unix Time to a System.DateTime object in
		/// local time.
		/// </summary>
		/// <param name="value">A 64-bit integer representing the Unix Time.</param>
		/// <returns>System.DateTime object.</returns>
		public static DateTime ToLocalDateTime(long value)
		{
            // *** DateTime = Epoch + UNIX.TS
            return UnixTime.Epoch.Add(TimeSpan.FromSeconds(value)).ToLocalTime();
		}

		/// <summary>
		/// Converts a Unix Time to a System.DateTime object in
		/// universal time.
		/// </summary>
		/// <param name="value">A System.Double representing the Unix Time.</param>
		/// <returns>System.DateTime object.</returns>
		public static DateTime ToUniversalDateTime(double value)
		{
            // *** DateTime = Epoch + UNIX.TS
            return UnixTime.Epoch.Add(TimeSpan.FromSeconds(value));
		}

		/// <summary>
		/// Converts a Unix Time to a System.DateTime object in
		/// local time.
		/// </summary>
		/// <param name="value">A System.Double representing the Unix Time.</param>
		/// <returns>System.DateTime object.</returns>
		public static DateTime ToLocalDateTime(double value)
		{
            // *** DateTime = Epoch + UNIX.TS
            return UnixTime.Epoch.Add(TimeSpan.FromSeconds(value)).ToLocalTime();
		}

		#endregion

		#region UnixTime/UnixTime Operators
		/// <summary>
		/// Adds one instance of a Unix Time to another instance.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>A System.UnixTime instance representing the addition of the first and second instance.</returns>
		public static UnixTime operator +(UnixTime t1, UnixTime t2)
		{
			return t1.Timestamp + t2.Timestamp;
		}

		/// <summary>
		/// Subtracts one instance of a Unix Time from another instance.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>A System.UnixTime instance representing the difference between the the first and second instance.</returns>
		public static UnixTime operator -(UnixTime t1, UnixTime t2)
		{
			return t1.Timestamp - t2.Timestamp;
		}

		/// <summary>
		/// Compares to instance of a Unix Time.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>Returns True if the two instances are the same, False otherwise.</returns>
		public static bool operator ==(UnixTime t1, UnixTime t2)
		{
			return (t1.Timestamp == t2.Timestamp);
		}

		/// <summary>
		/// Compares to instance of a Unix Time.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>Returns False if the two instances are the same, True otherwise.</returns>
		public static bool operator !=(UnixTime t1, UnixTime t2)
		{
			return (t1.Timestamp != t2.Timestamp);
		}

		/// <summary>
		/// Compares to instance of a Unix Time.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>Returns True the first instance is greater than the second instance, False otherwise.</returns>
		public static bool operator <(UnixTime t1, UnixTime t2)
		{
			return (t1.Timestamp < t2.Timestamp);
		}

		/// <summary>
		/// Compares to instance of a Unix Time.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>Returns True the first instance is greater than or equal to the second instance, False otherwise.</returns>
		public static bool operator <=(UnixTime t1, UnixTime t2)
		{
			return (t1.Timestamp <= t2.Timestamp);
		}

		/// <summary>
		/// Compares to instance of a Unix Time.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>Returns True the first instance is less than the second instance, False otherwise.</returns>
		public static bool operator >(UnixTime t1, UnixTime t2)
		{
			return (t1.Timestamp > t2.Timestamp);
		}

		/// <summary>
		/// Compares to instance of a Unix Time.
		/// </summary>
		/// <param name="t1">The first System.UnixTime instance.</param>
		/// <param name="t2">The second System.UnixTime instance.</param>
		/// <returns>Returns True the first instance is less than or equal to the second instance, False otherwise.</returns>
		public static bool operator >=(UnixTime t1, UnixTime t2)
		{
			return (t1.Timestamp >= t2.Timestamp);
		}
		#endregion

		#region UnixTime/TimeSpan Operators
		/// <summary>
		/// Adds an instance of System.TimeSpan to an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>A System.UnixTime instance representing the addition of the System.UnixTime and the System.TimeSpan instances.</returns>
		public static UnixTime operator +(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp + (long)t2.TotalSeconds;
		}

		/// <summary>
		/// Subtracts an instance of System.TimeSpan from an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>A System.UnixTime instance representing the subtraction of the System.TimeSpan from the System.UnixTime instances.</returns>
		public static UnixTime operator -(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp - (long)t2.TotalSeconds;
		}

		/// <summary>
		/// Compares an instance of System.TimeSpan to an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.UnixTime and the System.TimeSpan instances are the same, False otherwise.</returns>
		public static bool operator ==(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp == (long)t2.TotalSeconds;
		}

		/// <summary>
		/// Compares an instance of System.TimeSpan to an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>Returns False if the number of seconds represented by the System.UnixTime and the System.TimeSpan instances are the same, True otherwise.</returns>
		public static bool operator !=(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp != (long)t2.TotalSeconds;
		}

		/// <summary>
		/// Compares an instance of System.TimeSpan to an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.UnixTime is less than the total number of seconds represented by the System.TimeSpan instance, False otherwise.</returns>
		public static bool operator <(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp < (long)t2.TotalSeconds;
		}

		/// <summary>
		/// Compares an instance of System.TimeSpan to an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.UnixTime is less than or equal to the total number of seconds represented by the System.TimeSpan instance, False otherwise.</returns>
		public static bool operator <=(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp <= (long)t2.TotalSeconds;
		}

		/// <summary>
		/// Compares an instance of System.TimeSpan to an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.UnixTime is greater than the total number of seconds represented by the System.TimeSpan instance, False otherwise.</returns>
		public static bool operator >(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp > (long)t2.TotalSeconds;
		}

		/// <summary>
		/// Compares an instance of System.TimeSpan to an instance of System.UnixTime.
		/// </summary>
		/// <param name="t1">The System.UnixTime instance.</param>
		/// <param name="t2">The System.TimeSpan instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.UnixTime is greater than or equal to the total number of seconds represented by the System.TimeSpan instance, False otherwise.</returns>
		public static bool operator >=(UnixTime t1, TimeSpan t2)
		{
			return t1.Timestamp >= (long)t2.TotalSeconds;
		}
		#endregion

		#region TimeSpan/UnixTime Operators
		/// <summary>
		/// Adds an instance of System.UnixTime to an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>A System.UnixTime instance representing the addition of the System.TimeSpan and the System.UnixTime instances.</returns>
		public static UnixTime operator +(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds + t2.Timestamp;
		}

		/// <summary>
		/// Subtracts an instance of System.UnixTime from an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>A System.UnixTime instance representing the subtraction of the System.UnixTime from the System.TimeSpan instances.</returns>
		public static UnixTime operator -(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds - t2.Timestamp;
		}

		/// <summary>
		/// Compares an instance of System.UnixTime to an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.TimeSpan and the System.UnixTime instances are the same, False otherwise.</returns>
		public static bool operator ==(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds == t2.Timestamp;
		}

		/// <summary>
		/// Compares an instance of System.UnixTime to an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>Returns False if the number of seconds represented by the System.TimeSpan and the System.UnixTime instances are the same, True otherwise.</returns>
		public static bool operator !=(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds != t2.Timestamp;
		}

		/// <summary>
		/// Compares an instance of System.UnixTime to an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.TimeSpan is less than the total number of seconds represented by the System.UnixTime instance, False otherwise.</returns>
		public static bool operator <(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds < t2.Timestamp;
		}

		/// <summary>
		/// Compares an instance of System.UnixTime to an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.TimeSpan is less than or equal to the total number of seconds represented by the System.UnixTime instance, False otherwise.</returns>
		public static bool operator <=(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds <= t2.Timestamp;
		}

		/// <summary>
		/// Compares an instance of System.UnixTime to an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.TimeSpan is greater than the total number of seconds represented by the System.UnixTime instance, False otherwise.</returns>
		public static bool operator >(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds > t2.Timestamp;
		}

		/// <summary>
		/// Compares an instance of System.UnixTime to an instance of System.TimeSpan.
		/// </summary>
		/// <param name="t1">The System.TimeSpan instance.</param>
		/// <param name="t2">The System.UnixTime instance.</param>
		/// <returns>Returns True if the number of seconds represented by the System.TimeSpan is greater than or equal to the total number of seconds represented by the System.UnixTime instance, False otherwise.</returns>
		public static bool operator >=(TimeSpan t1, UnixTime t2)
		{
			return (long)t1.TotalSeconds >= t2.Timestamp;
		}
		#endregion

		#region IComparable
		/// <summary>
		/// Compares the current instance with another object of the same type and returns
		/// an integer that indicates whether the current instance precedes, follows,
		/// or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>A value that indicates the relative order of the objects being compared.
		/// The return value has these meanings: Value Meaning Less than zero This instance
		/// precedes obj in the sort order. Zero This instance occurs in the same position
		/// in the sort order as obj. Greater than zero This instance follows obj in
		/// the sort order.</returns>
		public int CompareTo(object obj)
		{
			int returnValue = -1;

			if (obj is UnixTime)
			{
				returnValue = this.Timestamp.CompareTo(((UnixTime)obj).Timestamp);
			}

			return returnValue;
		}
		#endregion

		#region IFormattable
		/// <summary>
		/// Formats the value of the current instance using the specified format.
		/// </summary>
		/// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use
		/// the default format defined for the type of the System.IFormattable implementation.</param>
		/// <returns> The value of the current instance in the specified format.</returns>
		public string ToString(string format)
		{
			return this.DateTime.ToString(format);
		}

		/// <summary>
		/// Formats the value of the current instance using the specified format.
		/// </summary>
		/// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use
		/// the default format defined for the type of the System.IFormattable implementation.</param>
		/// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in
		/// Visual Basic) to obtain the numeric format information from the current locale
		/// setting of the operating system.</param>
		/// <returns> The value of the current instance in the specified format.</returns>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.DateTime.ToString(format, formatProvider);
		}
		#endregion

		#region IComparable<UnixTime>
		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>A value that indicates the relative order of the objects being compared.
		/// The return value has the following meanings: Value Meaning
		/// Less than zero: This object is less than the other parameter. 
		/// Zero: This object is equal to other.
		/// Greater than zero: This object is greater than other.</returns>
		public int CompareTo(UnixTime other)
		{
			int returnValue = -1;

			if (other != null)
			{
				returnValue = this.Timestamp.CompareTo(other.Timestamp);
			}

			return returnValue;
		}
		#endregion

		#region IEquatable<UnixTime>
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same
		/// type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>True if the current object is equal to the other parameter; False otherwise.</returns>
		public bool Equals(UnixTime other)
		{
			return (other != null) ? this.Timestamp.Equals(other.Timestamp) : false;
		}
		#endregion

		#region IConvertible
		/// <summary>
		/// Returns the System.TypeCode for this instance.
		/// </summary>
		/// <returns>The enumerated constant that is the System.TypeCode of the class or value
		/// type that implements this interface.</returns>
		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent Boolean value using
		/// the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>A Boolean value equivalent to the value of this instance.</returns>
		public bool ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 8-bit unsigned integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 8-bit unsigned integer equivalent to the value of this instance.</returns>
		public byte ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent Unicode character using
		/// the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>A Unicode character equivalent to the value of this instance.</returns>
		public char ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent System.DateTime using
		/// the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>A System.DateTime instance equivalent to the value of this instance.</returns>
		public DateTime ToDateTime(IFormatProvider provider)
		{
			return this.DateTime;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent System.Decimal number
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>A System.Decimal number equivalent to the value of this instance.</returns>
		public decimal ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent double-precision floating-point
		/// number using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>A double-precision floating-point number equivalent to the value of this
		/// instance.</returns>
		public double ToDouble(IFormatProvider provider)
		{
			return this.Timestamp;
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 16-bit signed integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 16-bit signed integer equivalent to the value of this instance.</returns>
		public short ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 32-bit signed integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 32-bit signed integer equivalent to the value of this instance.</returns>
		public int ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 64-bit signed integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 64-bit signed integer equivalent to the value of this instance.</returns>
		public long ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 8-bit signed integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 8-bit signed integer equivalent to the value of this instance.</returns>
		public sbyte ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent single-precision floating-point
		/// number using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>A single-precision floating-point number equivalent to the value of this
		/// instance.</returns>
		public float ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent System.String using
		/// the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>A System.String instance equivalent to the value of this instance.</returns>
		public string ToString(IFormatProvider provider)
		{
			return Convert.ToString(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an System.Object of the specified
		/// System.Type that has an equivalent value, using the specified culture-specific
		/// formatting information.
		/// </summary>
		/// <param name="conversionType">The System.Type to which the value of this instance is converted.</param>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An System.Object instance of type conversionType whose value is equivalent
		/// to the value of this instance.</returns>
		public object ToType(Type conversionType, IFormatProvider provider)
		{
			return Convert.ChangeType(this.Timestamp, conversionType, provider);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 16-bit unsigned integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 16-bit unsigned integer equivalent to the value of this instance.</returns>
		public ushort ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this.Timestamp);
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 32-bit unsigned integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 32-bit unsigned integer equivalent to the value of this instance.</returns>
		public uint ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this.Timestamp); throw new NotImplementedException();
		}

		/// <summary>
		/// Converts the value of this instance to an equivalent 64-bit unsigned integer
		/// using the specified culture-specific formatting information.
		/// </summary>
		/// <param name="provider">An System.IFormatProvider interface implementation that supplies culture-specific
		/// formatting information.</param>
		/// <returns>An 64-bit unsigned integer equivalent to the value of this instance.</returns>
		public ulong ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this.Timestamp);
		}
		#endregion

		#region ISerializable
		/// <summary>
		/// Populates a System.Runtime.Serialization.SerializationInfo with the data
		/// needed to serialize the target object.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo to populate with data.</param>
		/// <param name="context">The destination (see System.Runtime.Serialization.StreamingContext) for this
		/// serialization.</param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(MagicValues.PropertyName, this.Timestamp, typeof(double));
		}

		#endregion

		#region Parse

		/// <summary>
		/// Converts the specified string representation of a Unix time to its System.UnixTime
		/// equivalent and returns a value that indicates whether the conversion succeeded.
		/// </summary>
		/// <param name="s">A string containing a Unix time to convert.</param>
		/// <param name="result">When this method returns, contains the System.UnixTime value equivalent to
		/// the Unix time contained in s, if the conversion succeeded, or System.UnixTime.Epoch
		/// if the conversion failed. The conversion fails if the s parameter is null,
		/// is an empty string (""), or does not contain a valid string representation
		/// of a Unix time. This parameter is passed uninitialized.</param>
		/// <returns>True if the s parameter was converted successfully, False otherwise.</returns>
		public static bool TryParse(string s, out UnixTime result)
		{
			bool returnValue = false;

			result = UnixTime.Epoch;
			DateTime value1 = UnixTime.Epoch;
			TimeSpan value2 = TimeSpan.Zero;
			double value3 = 0D;

			if (DateTime.TryParse(s, out value1))
			{
				result = new UnixTime(value1);
				returnValue = true;
			}
			else if (double.TryParse(s, out value3))
			{
				result = new UnixTime(value3);
				returnValue = true;
			}
			else if (TimeSpan.TryParse(s, out value2))
			{
				result = new UnixTime(value2.TotalSeconds);
				returnValue = true;
			}
			return returnValue;
		}

		/// <summary>
		/// Converts the string representation of a Unix time to its System.UnixTime
		/// equivalent.
		/// </summary>
		/// <param name="s">A string that contains a Unix time to convert.</param>
		/// <returns>An object that is equivalent to the Unix time contained in s.</returns>
		public static UnixTime Parse(string s)
		{
			UnixTime returnValue = UnixTime.Epoch;
			if (!UnixTime.TryParse(s, out returnValue))
			{
				throw new FormatException();
			}
			return returnValue;
		}

		/// <summary>
		/// Determines if the specified string s can be converted to a System.UnixTime equivalent.
		/// </summary>
		/// <param name="s">A string containing a Unix time to convert.</param>
		/// <returns>True if the s parameter can be converted successfully, False otherwise.</returns>
		public static bool CanParse(string s)
		{
			UnixTime result = UnixTime.Epoch;
			return UnixTime.TryParse(s, out result);
		}

		#endregion
	}
}
