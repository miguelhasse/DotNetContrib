using Sample.Model;
using Sample.Services;

namespace Sample
{
    public sealed class PerformanceGateway : ServiceGatewayBase<IPerformanceService>
    {
        #region Constructors

        public PerformanceGateway(string endpointConfigurationName, string username, string password)
            : this(endpointConfigurationName, 3, 3)
        {
            Credentials.UserName.UserName = username;
            Credentials.UserName.Password = password;
        }

        public PerformanceGateway(string endpointConfigurationName, int retryCount = 3, int retryInterval = 3)
            : base(endpointConfigurationName, retryCount, retryInterval)
        {
            Endpoint.EndpointBehaviors.Add(new ServiceModel.ClientEndpointBehaviour());
        }

        #endregion

        #region Synchronous interface implementation

        public PerfomanceData GetData()
        {
            return InternalExecute(p => p.GetData()).Data;
        }

        #endregion

        #region Asynchronous interface implementation

        #endregion
    }
}
