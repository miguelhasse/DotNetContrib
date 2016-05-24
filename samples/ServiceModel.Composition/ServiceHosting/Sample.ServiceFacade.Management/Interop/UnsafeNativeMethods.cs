using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32
{
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPerformanceInfo([Out] out PsApiPerformanceInformation PerformanceInformation, [In] int Size);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PsApiPerformanceInformation
    {
        public int Size;
        public IntPtr CommitTotal;
        public IntPtr CommitLimit;
        public IntPtr CommitPeak;
        public IntPtr PhysicalTotal;
        public IntPtr PhysicalAvailable;
        public IntPtr SystemCache;
        public IntPtr KernelTotal;
        public IntPtr KernelPaged;
        public IntPtr KernelNonPaged;
        public IntPtr PageSize;
        public int HandlesCount;
        public int ProcessCount;
        public int ThreadCount;
    }
}
