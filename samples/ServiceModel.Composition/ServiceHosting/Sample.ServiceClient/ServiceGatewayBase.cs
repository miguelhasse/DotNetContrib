using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace Sample
{
    public abstract class ServiceGatewayBase<TChannel> : IDisposable
        where TChannel : class
    {
        private ServiceChannelProxyHelper<TChannel> service;

        private event EventHandler onCallbackEvent;

        private TimeSpan retryInterval;
        private int retryCount;

        #region Constructors

        protected ServiceGatewayBase(string endpointConfigurationName, int retryCount, int retryInterval)
            : this(retryCount, retryInterval)
        {
            object implementation = InitializeServiceInstance();
            this.service = (implementation == null) ? new ServiceChannelProxyHelper<TChannel>(endpointConfigurationName)
                : new ServiceChannelProxyHelper<TChannel>(new InstanceContext(implementation), endpointConfigurationName);
        }

        private ServiceGatewayBase(int retryCount, int retryInterval)
        {
            this.retryInterval = TimeSpan.FromSeconds(retryInterval);
            this.retryCount = retryCount;
        }

        #endregion

        protected ClientCredentials Credentials
        {
            get { return service.Credentials; }
        }

        protected ServiceEndpoint Endpoint
        {
            get { return service.Endpoint; }
        }

        public void Dispose()
        {
            ((IDisposable)service).Dispose();
        }

        protected virtual object InitializeServiceInstance()
        {
            return null;
        }

        protected virtual bool IsTransient(Exception exception)
        {
            return true; // check for service related transient exception here
        }

        protected void InternalExecute(Action<TChannel> func)
        {
            try { this.service.Execute(func, e => IsTransient(e), retryCount, retryInterval); }
            catch (Exception ex) { throw new ServiceClientException(ex); }
        }

        protected TResult InternalExecute<TResult>(Func<TChannel, TResult> taskFunc)
        {
            try { return this.service.Execute(taskFunc, e => IsTransient(e), retryCount, retryInterval); }
            catch (Exception ex) { throw new ServiceClientException(ex); }
        }

        protected async Task InternalExecuteAsync(Func<TChannel, Task> taskFunc)
        {
            try { await this.service.ExecuteAsync(taskFunc, e => IsTransient(e), retryCount, retryInterval); }
            catch (Exception ex) { throw new ServiceClientException(ex); }
        }

        protected async Task<TResult> InternalExecuteAsync<TResult>(Func<TChannel, Task<TResult>> taskFunc)
        {
            try { return await this.service.ExecuteAsync(taskFunc, e => IsTransient(e), retryCount, retryInterval); }
            catch (Exception ex) { throw new ServiceClientException(ex); }
        }
    }
}
