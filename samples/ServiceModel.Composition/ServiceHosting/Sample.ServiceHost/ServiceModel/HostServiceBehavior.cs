using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Sample.ServiceModel
{
    internal sealed class HostServiceBehavior : IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var endpoint in serviceDescription.Endpoints)
            {
                if (endpoint.IsSystemEndpoint || 
					endpoint.Contract.ContractType == typeof(IMetadataExchange))
                    continue;

                endpoint.EndpointBehaviors.Add(new ServiceModel.HostEndpointBehaviour());
            }
			//serviceHostBase.Authentication.AuthenticationSchemes = System.Net.AuthenticationSchemes.Basic;
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
	}
}