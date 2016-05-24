using System.ServiceModel.Description;

namespace Sample.ServiceModel
{
    internal sealed class HostEndpointBehaviour : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new HostMessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
