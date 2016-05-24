using Sample.Services;
using System;

namespace Sample
{
    public sealed class ManagementGateway : ServiceGatewayBase<IManagementService>
	{
		private event EventHandler<CallbackEventArgs> onCallbackEvent;
		private static readonly object SyncRoot = new object();
		
		#region Constructors

		public ManagementGateway(string endpointConfigurationName, string username, string password)
			: this(endpointConfigurationName)
		{
			Credentials.UserName.UserName = username;
			Credentials.UserName.Password = password;
		}

		public ManagementGateway(string endpointConfigurationName, int retryCount = 3, int retryInterval = 3)
			: base(endpointConfigurationName, retryCount, retryInterval)
		{
			Endpoint.EndpointBehaviors.Add(new ServiceModel.ClientEndpointBehaviour());
		}

		#endregion

		public event EventHandler<CallbackEventArgs> ServerEvent
		{
			add
			{
				lock (SyncRoot)
				{
					onCallbackEvent += value;
					CheckSubscriptions();
				}
			}
			remove
			{
				lock (SyncRoot)
				{
					onCallbackEvent -= value;
					CheckSubscriptions();
				}
			}
		}

		protected override object InitializeServiceInstance()
		{
			return new ManagementCallback((sender, args) =>
            {
                var handler = onCallbackEvent;
                if (handler != null) handler(this, args);
            });
		}

		#region Synchronous interface implementation
		
		private void CheckSubscriptions()
		{
			if (onCallbackEvent == null)
			{
				InternalExecute(p => p.UnsubscribeNotifications());
			}
			if (onCallbackEvent != null && onCallbackEvent.GetInvocationList().Length == 1)
			{
				InternalExecute(p => p.SubscribeNotifications());
			}
		}

		public void CreateSession(Guid installationId)
		{
			InternalExecute(p => p.CreateSession(installationId));
		}
		
		#endregion
		
		#region Asynchronous interface implementation
		
		#endregion
	}
}
