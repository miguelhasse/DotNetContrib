using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Composition;
using System.ServiceModel.Composition.Description;

namespace Sample.Services
{
    [TcpEndpoint]
	[ExportService("ManagementService", typeof(ManagementService))]
	public sealed class ManagementService : IManagementService, IHostedService
	{
		private readonly ConcurrentDictionary<string, IManagementCallback> clients = new ConcurrentDictionary<string, IManagementCallback>();
		System.Threading.Timer timer;

		public void CreateSession(Guid installationId)
		{
			System.Diagnostics.Trace.TraceInformation("ManagementService::SessionID: {0}", OperationContext.Current.SessionId);
		}

		public void SubscribeNotifications()
		{
			IManagementCallback callback = OperationContext.Current.GetCallbackChannel<IManagementCallback>();
			string sessionId = OperationContext.Current.SessionId;

			if (callback != null && clients.TryAdd(sessionId, callback))
			{
				if (clients.Count == 1)
				{
					timer = new System.Threading.Timer(state => BroadcastEvent(DateTime.UtcNow.ToString()));
					timer.Change(500, 1000);
                }
			}
			else throw new FaultException("Failed to create client subscription.");
		}

		public void UnsubscribeNotifications()
		{
			IManagementCallback callback;
			if (!clients.TryRemove(OperationContext.Current.SessionId, out callback))
				throw new FaultException("No client subscription found.");

			if (clients.Count == 0)
				timer.Dispose();
		}

		private void BroadcastEvent(string message)
		{
			var disconnectedSessions = new List<string>();
			foreach (KeyValuePair<string, IManagementCallback> client in clients)
			{
				try
				{
					client.Value.HandleEvent(message);
				}
				catch
				{
					// TODO: Better to catch specific exception types.                     

					// If a timeout exception occurred, it means that the server
					// can't connect to the client. It might be because of a network
					// error, or the client was closed  prematurely due to an exception or
					// and was unable to unregister from the server. In any case, we 
					// must remove the client from the list of clients.

					// Another type of exception that might occur is that the communication
					// object is aborted, or is closed.

					// Mark the key for deletion. We will delete the client after the 
					// for-loop because using foreach construct makes the clients collection
					// non-modifiable while in the loop.
					disconnectedSessions.Add(client.Key);
				}
			}
			foreach (var sessionId in disconnectedSessions)
			{
				IManagementCallback callback;
				clients.TryRemove(sessionId, out callback);
			}
		}
	}
}
