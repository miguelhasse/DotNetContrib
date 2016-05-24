using System;
using System.Runtime.Serialization;

namespace Sample.Model
{
    [DataContract(Namespace = Constants.PerformanceServiceNamespace)]
	public class PerfomanceData
	{
        [DataMember]
        public Int64 CommitTotalPages;

        [DataMember]
        public Int64 CommitLimitPages;

        [DataMember]
        public Int64 CommitPeakPages;

        [DataMember]
        public Int64 PhysicalTotalBytes;

        [DataMember]
        public Int64 PhysicalAvailableBytes;

        [DataMember]
        public Int64 SystemCacheBytes;

        [DataMember]
        public Int64 KernelTotalBytes;

        [DataMember]
        public Int64 KernelPagedBytes;

        [DataMember]
        public Int64 KernelNonPagedBytes;

        [DataMember]
        public Int64 PageSizeBytes;

        [DataMember]
        public int HandlesCount;

        [DataMember]
        public int ProcessCount;

        [DataMember]
        public int ThreadCount;
	}
}
