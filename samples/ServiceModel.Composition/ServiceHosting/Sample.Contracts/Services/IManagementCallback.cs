using System.ServiceModel;

namespace Sample.Services
{
    /// <summary>
    /// The callback contract to be implemented by the client application.
    /// </summary>
    [ServiceContract(Namespace = Constants.ManagementNamespace)]
	public interface IManagementCallback
	{
		[OperationContract(IsOneWay = true)]
		void HandleEvent(string message);
	}
}
