using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32
{
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern ThreadExecutionState SetThreadExecutionState(ThreadExecutionState esFlags);
    }

    [FlagsAttribute]
    internal enum ThreadExecutionState : uint
    {
        CONTINUOUS = 0x80000000,
        DISPLAY_REQUIRED = 0x00000002,
        SYSTEM_REQUIRED = 0x00000001,
        USER_PRESENT = 0x00000004,
        AWAYMODE_REQUIRED = 0x00000040
    }
}
