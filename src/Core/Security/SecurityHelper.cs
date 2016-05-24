using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Hasseware.Security
{
	/// <summary>
	/// A utility class with several static methods that are used to access Windows authentication functions.
	/// </summary>
	public static class SecurityHelper
	{
		#region Constants

		private const int ERROR_INSUFFICIENT_BUFFER = 0x7A;

		#endregion

		/// <summary>
		/// This method attempts to log a user on to the local computer. If the method succeeds,
		/// you receive an identity that represents the logged-on user.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">Thrown if a null reference is passed a parameter.</exception>
		/// <exception cref="System.ComponentModel.Win32Native">Thrown if a Win32 error occurred.</exception>
		public static WindowsIdentity LogonUser(System.Net.NetworkCredential credential)
		{
			if (credential == null) throw new ArgumentNullException("credential");
			return LogonUser(credential.UserName, credential.Password, credential.Domain);
		}

		/// <summary>
		/// This method attempts to log a user on to the local computer. If the method succeeds,
		/// you receive an identity that represents the logged-on user.
		/// </summary>
		/// <param name="accountName">Name of the user account to log on to.</param>
		/// <param name="password">Plaintext password for the user account specified by <paramref name="accountName"/>.</param>
		/// <param name="domainName">Name of the domain or server whose account database contains the <paramref name="accountName"/>.</param>
		/// <returns>An identity object that represents the specified user.</returns>
		/// <exception cref="System.ComponentModel.Win32Native">Thrown if a Win32 error occurred.</exception>
		public static WindowsIdentity LogonUser(string accountName, string password, string domainName)
		{
			if (UnsafeNativeMethods.RevertToSelf())
			{
				IntPtr token = IntPtr.Zero;
				IntPtr tokenDuplicate = IntPtr.Zero;

				try
				{
					if (UnsafeNativeMethods.LogonUser(accountName, domainName, password,
						LogonSessionType.Network, LogonProvider.Default, out token))
					{
						if (UnsafeNativeMethods.DuplicateToken(token, SecurityLevel.Impersonation, out tokenDuplicate))
							return new WindowsIdentity(tokenDuplicate);
					}
					// if this point is reached, and error ocurred.
					throw new Win32Exception();
				}
				finally // close all open object handles before leaving
				{
					if (tokenDuplicate != IntPtr.Zero) UnsafeNativeMethods.CloseHandle(tokenDuplicate);
					if (token != IntPtr.Zero) UnsafeNativeMethods.CloseHandle(token);
				}
			}
			else throw new Win32Exception();
		}

		/// <summary>
		/// This method accepts a security identifier (SID) as input and retrieves
		/// the corresponding account and domain names.
		/// </summary>
		/// <param name="sid">The SID to lookup.</param>
		/// <param name="accountName">Account name that corresponds to the <paramref name="sid"/> parameter.</param>
		/// <param name="domainName">Name of the domain where the account name was found.</param>
		/// <returns>Indication of the type of the account. Valid values range from 1 to 10. A value of 0 indicates an error occurred.</returns>
		/// <exception cref="System.ComponentModel.Win32Native">Thrown if a Win32 error occurred.</exception>
		public static SidNameUse LookupAccountSID(string sid, out string accountName, out string domainName)
		{
			IntPtr pSid = IntPtr.Zero;
			if ((UnsafeNativeMethods.ConvertStringSidToSid(sid, out pSid)) && (pSid != IntPtr.Zero))
			{
				SidNameUse accountType;
				int nameSize = 0x100, domainSize = 0x100;

				StringBuilder accountNameBuilder = new StringBuilder(nameSize);
				StringBuilder domainNameBuilder = new StringBuilder(domainSize);

				if (UnsafeNativeMethods.LookupAccountSid(null, pSid, accountNameBuilder, ref nameSize,
					domainNameBuilder, ref domainSize, out accountType))
				{
					accountName = accountNameBuilder.ToString();
					domainName = domainNameBuilder.ToString();
					return accountType;
				}
				// if this point is reached, and error ocurred.
				throw new Win32Exception();
			}
			else domainName = accountName = null;
			return 0;
		}

		/// <summary>
		/// This method accepts a security identifier (SID) as input and retrieves
		/// the corresponding account and domain names.
		/// </summary>
		/// <param name="sidType">A well known SID to lookup.</param>
		/// <param name="accountName">Account name that corresponds to the <paramref name="sidType"/> parameter.</param>
		/// <param name="domainName">Name of the domain where the account name was found.</param>
		/// <returns>Indication of the type of the account. Valid values range from 1 to 10. A value of 0 indicates an error occurred.</returns>
		/// <exception cref="System.ComponentModel.Win32Native">Thrown if a Win32 error occurred.</exception>
		public static SidNameUse LookupAccountSID(WellKnownSidType sidType, out string accountName, out string domainName)
		{
			SecurityIdentifier si = new SecurityIdentifier(sidType, null);
			return LookupAccountSID(si.Value, out accountName, out domainName);
		}

		/// <summary>
		/// This method accepts the name of an account as input and retrieves 
		/// the corresponding security identifier (SID) and domain name.
		/// </summary>
		/// <param name="accountName">Name of the account.</param>
		/// <param name="accountSid">The account sid.</param>
		/// <param name="domainName">Name of the domain.</param>
		/// <returns>Indication of the type of the account. Valid values range from 1 to 10. A value of 0 indicates an error occurred.</returns>
		/// <exception cref="System.ComponentModel.Win32Native">Thrown if a Win32 error occurred.</exception>
		public static SidNameUse LookupAccountName(string accountName, out string accountSid, out string domainName)
		{
			string serverName = null;
			int sidLength = 0, domainNameSize = 0;
			SidNameUse accountType;
			IntPtr pSid = IntPtr.Zero;

			// get the required buffer sizes for SID and domain name.
			if (!UnsafeNativeMethods.LookupAccountName(serverName, accountName, pSid, ref sidLength, null, ref domainNameSize, out accountType))
			{
				if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
				{
					// allocate the buffers with actual sizes that are required for SID and domain name.
					StringBuilder domainNameBuilder = new StringBuilder(domainNameSize);
					pSid = Marshal.AllocHGlobal(sidLength);

					if (UnsafeNativeMethods.LookupAccountName(serverName, accountName, pSid,
						ref sidLength, domainNameBuilder, ref domainNameSize, out accountType))
					{
						IntPtr stringSid;
						if (!UnsafeNativeMethods.ConvertSidToStringSid(pSid, out stringSid))
							throw new Win32Exception(); // last error

						domainName = domainNameBuilder.ToString();
						accountSid = Marshal.PtrToStringAuto(stringSid);

						Marshal.FreeBSTR(stringSid);
						Marshal.FreeHGlobal(pSid);
						return accountType;
					}
				}
				// if this point is reached, and error ocurred.
				throw new Win32Exception();
			}
			else accountSid = domainName = null;
			return 0;
		}
	}
}
