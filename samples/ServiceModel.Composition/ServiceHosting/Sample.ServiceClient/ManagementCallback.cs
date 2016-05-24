using Sample.Services;
using System;
using System.ComponentModel;

namespace Sample
{
    internal sealed class ManagementCallback : IManagementCallback
	{
		private System.Threading.SynchronizationContext syncContext;
		private EventHandler<CallbackEventArgs> eventHandler;

		public ManagementCallback(EventHandler<CallbackEventArgs> handler)
		{
			this.syncContext = AsyncOperationManager.SynchronizationContext;
			this.eventHandler = handler;
		}

		void IManagementCallback.HandleEvent(string eventData)
		{
			syncContext.Post(new System.Threading.SendOrPostCallback(OnCallback), eventData);
		}

		private void OnCallback(object eventData)
		{
            this.eventHandler.Invoke(this, new CallbackEventArgs(eventData));
		}
	}

    public class CallbackEventArgs : EventArgs
    {
        internal CallbackEventArgs(object eventData)
        {
            Data = eventData;
        }

        public object Data { get; private set; }
    }

}
