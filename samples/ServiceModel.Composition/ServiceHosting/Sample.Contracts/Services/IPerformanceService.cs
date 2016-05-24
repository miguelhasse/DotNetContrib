using Sample.Messages;
using System.ServiceModel;

namespace Sample.Services
{
    [ServiceContract(Namespace = Constants.PerformanceServiceNamespace)]
    public interface IPerformanceService
    {
        [OperationContract]
        Perfomance GetData();
    }
}
