namespace System
{
	/// <summary>
	/// Provides extension methods for Sysem.DateTiem to work with System.UnixTime
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Convert a System.DateTime value to a Unix Time.
		/// </summary>
		/// <param name="value">System.DateTime object.</param>
		/// <returns>A 64-bit integer representing the Unix Time.</returns>
		public static long ToUnixTime(this DateTime value)
		{
			return UnixTime.FromDateTime(value);
		}
	}
}
