using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Microsoft.Win32
{
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool LookupAccountSid(string machineName, IntPtr pSid, StringBuilder szName, ref int nameSize, StringBuilder szDomain, ref int domainSize, out SidNameUse accountType);
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool LookupAccountName(string machineName, string accountName, IntPtr pSid, ref int sidLen, StringBuilder szDomain, ref int domainSize, out SidNameUse accountType);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CreateWellKnownSid(int sidType, IntPtr pDomainSid, out IntPtr pSid, out int sidLen);
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ConvertStringSidToSid(string stringSid, out IntPtr pSid);
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool ConvertSidToStringSid(IntPtr pSid, out IntPtr stringSid);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool CheckTokenMembership(IntPtr hToken, IntPtr pSid, out bool isMember);
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool LogonUser(string lpszUserName, string lpszDomain, string lpszPassword, LogonSessionType logonType, LogonProvider logonProvider, out IntPtr phToken);
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool DuplicateToken(IntPtr hToken, SecurityLevel impersonationLevel, out IntPtr hNewToken);
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool RevertToSelf();

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool CloseHandle(IntPtr handle);
	}
}
