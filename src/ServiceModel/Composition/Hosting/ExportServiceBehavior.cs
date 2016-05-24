using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Hosting;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace System.ServiceModel.Composition.Hosting
{
    /// <summary>
    /// Defines a service behaviour that supports instantiating the instance via MEF.
    /// </summary>
    public class ExportServiceBehavior<T> : IServiceBehavior where T : IHostedService
    {
        #region Fields

        private readonly string serviceName;
        private readonly CompositionContainer container;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="ExportServiceBehavior"/>.
        /// </summary>
        /// <param name="container">The current composition container.</param>
        /// <param name="name">The name of the service.</param>
        public ExportServiceBehavior(CompositionContainer container, string name)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
            this.serviceName = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="description">The service description of the service.</param>
        /// <param name="service">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="parameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(ServiceDescription description, ServiceHostBase service,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="description">The service description.</param>
        /// <param name="host">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase host)
        {
            foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
            {
                foreach (var endpoint in dispatcher.Endpoints)
                {
                    endpoint.DispatchRuntime.InstanceProvider = new ExportInstanceProvider<T>(container, serviceName);
                }
            }
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="description">The service description.</param>
        /// <param name="host">The host that is currently being built.</param>
        public void Validate(ServiceDescription description, ServiceHostBase host)
        {
        }

        #endregion
    }
}