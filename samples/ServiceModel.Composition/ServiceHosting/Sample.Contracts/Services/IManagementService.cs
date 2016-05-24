using System;
using System.ServiceModel;

namespace Sample.Services
{
    [ServiceContract(Namespace = Constants.ManagementNamespace, CallbackContract = typeof(IManagementCallback))]
    public interface IManagementService
    {
        [OperationContract]
        void CreateSession(Guid installationId);

		//Configuration GetConfiguration(Guid installationId);

		/// <summary>
		/// Subcribes a client for event broadcasts.
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void SubscribeNotifications();

		/// <summary>
		/// Unsubscribes a client from any event broadcast.
		/// </summary>
		[OperationContract(IsOneWay = true)]
		void UnsubscribeNotifications();
	}
}
