using Microsoft.Win32;
using Sample.Messages;
using Sample.Model;
using System;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Composition;
using System.ServiceModel.Composition.Description;

// http://www.antoniob.com/windows-psapi-getperformanceinfo-csharp-wrapper.html

namespace Sample.Services
{
    [TcpEndpoint]
    [ExportService("PerformanceService", typeof(PerformanceService))]
    public sealed class PerformanceService : IPerformanceService, IHostedService
    {
        public Perfomance GetData()
        {
            System.Diagnostics.Trace.TraceInformation("PerformanceService::SessionID: {0}", OperationContext.Current.SessionId);
            return new Perfomance { Data = GetPerformanceInfo() };
        }

        public static PerfomanceData GetPerformanceInfo()
        {
            PsApiPerformanceInformation perfInfo = new PsApiPerformanceInformation();
            if (UnsafeNativeMethods.GetPerformanceInfo(out perfInfo, Marshal.SizeOf(perfInfo)))
            {
                Int64 pageSize = perfInfo.PageSize.ToInt64();

                return new PerfomanceData
                {
                    /// data in pages
                    CommitTotalPages = perfInfo.CommitTotal.ToInt64(),
                    CommitLimitPages = perfInfo.CommitLimit.ToInt64(),
                    CommitPeakPages = perfInfo.CommitPeak.ToInt64(),
                    /// data in bytes
                    PhysicalTotalBytes = perfInfo.PhysicalTotal.ToInt64() * pageSize,
                    PhysicalAvailableBytes = perfInfo.PhysicalAvailable.ToInt64() * pageSize,
                    SystemCacheBytes = perfInfo.SystemCache.ToInt64() * pageSize,
                    KernelTotalBytes = perfInfo.KernelTotal.ToInt64() * pageSize,
                    KernelPagedBytes = perfInfo.KernelPaged.ToInt64() * pageSize,
                    KernelNonPagedBytes = perfInfo.KernelNonPaged.ToInt64() * pageSize,
                    PageSizeBytes = pageSize,
                    /// counters
                    HandlesCount = perfInfo.HandlesCount,
                    ProcessCount = perfInfo.ProcessCount,
                    ThreadCount = perfInfo.ThreadCount
                };
            }
            return null;
        }
    }
}
