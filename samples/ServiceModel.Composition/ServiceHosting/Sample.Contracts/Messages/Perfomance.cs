using Sample.Model;
using System.ServiceModel;

namespace Sample.Messages
{
    [MessageContract]
	public class Perfomance
	{
		[MessageBodyMember(Namespace = Constants.ServiceNamespace)]
		public PerfomanceData Data { get; set; }
	}
}
