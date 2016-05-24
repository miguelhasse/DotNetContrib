using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    /// <summary>
    /// Generic helper class for a WCF service proxy.
    /// </summary>
    internal class ServiceChannelProxyHelper<TChannel> : IDisposable
		where TChannel : class
	{
		private static readonly object SyncRoot = new object();
		private ChannelFactory<TChannel> channelFactory;
		private TChannel channel;

		public ServiceChannelProxyHelper(string endpointConfigurationName)
		{
			this.channelFactory = new ChannelFactory<TChannel>(endpointConfigurationName);
		}

		public ServiceChannelProxyHelper(Binding binding, EndpointAddress remoteAddress)
		{
			this.channelFactory = new ChannelFactory<TChannel>(binding, remoteAddress);
		}

		public ServiceChannelProxyHelper(InstanceContext instanceContext, string endpointConfigurationName)
		{
			this.channelFactory = new DuplexChannelFactory<TChannel>(instanceContext, endpointConfigurationName);
		}

		public ServiceChannelProxyHelper(InstanceContext instanceContext, Binding binding, EndpointAddress remoteAddress)
		{
			this.channelFactory = new DuplexChannelFactory<TChannel>(instanceContext, binding, remoteAddress);
		}

		public event EventHandler ChannelFaulted;

		public TChannel Proxy
		{
			get
			{
				lock (SyncRoot)
				{
					EnsureChannel();
					return this.channel;
				}
			}
		}

		public ClientCredentials Credentials
		{
			get { return ((ChannelFactory)this.channelFactory).Credentials; }
		}

		public ServiceEndpoint Endpoint
		{
			get { return ((ChannelFactory)this.channelFactory).Endpoint; }
		}

		public void Dispose()
		{
			try
			{
				Dispose(this.channelFactory);
			}
			finally
			{
				this.channelFactory = null;
			}
		}

		public void Execute(Action<TChannel> func, RetryPolicy retryPolicy)
		{
			if (func == null) throw new ArgumentNullException("func");
			if (retryPolicy == null) throw new ArgumentNullException("retryPolicy");
			retryPolicy.ExecuteAction(() => func(this.Proxy));
		}

		public void Execute(Action<TChannel> func, Func<Exception, bool> transientCheckFunc, int retryCount, TimeSpan retryInterval)
		{
			Execute(func, CreateRetryPolicy(transientCheckFunc, retryCount, retryInterval));
		}

		public TResult Execute<TResult>(Func<TChannel, TResult> func, RetryPolicy retryPolicy)
		{
			if (func == null) throw new ArgumentNullException("func");
			if (retryPolicy == null) throw new ArgumentNullException("retryPolicy");

			return retryPolicy.ExecuteAction(() => func(this.Proxy));
		}

		public TResult Execute<TResult>(Func<TChannel, TResult> func, Func<Exception, bool> transientCheckFunc, int retryCount, TimeSpan retryInterval)
		{
			return Execute<TResult>(func, CreateRetryPolicy(transientCheckFunc, retryCount, retryInterval));
		}

		public Task ExecuteAsync(Func<TChannel, Task> taskFunc, RetryPolicy retryPolicy, CancellationToken cancellationToken)
		{
			if (taskFunc == null) throw new ArgumentNullException("taskFunc");
			if (retryPolicy == null) throw new ArgumentNullException("retryPolicy");
			return retryPolicy.ExecuteAsync(() => taskFunc(this.Proxy), cancellationToken);
		}

		public Task ExecuteAsync(Func<TChannel, Task> taskFunc, Func<Exception, bool> transientCheckFunc, int retryCount, TimeSpan retryInterval)
		{
			return ExecuteAsync(taskFunc, CreateRetryPolicy(transientCheckFunc, retryCount, retryInterval), CancellationToken.None);
		}

		public Task<TResult> ExecuteAsync<TResult>(Func<TChannel, Task<TResult>> taskFunc, RetryPolicy retryPolicy, CancellationToken cancellationToken)
		{
			if (taskFunc == null) throw new ArgumentNullException("taskFunc");
			if (retryPolicy == null) throw new ArgumentNullException("retryPolicy");
			return retryPolicy.ExecuteAsync(() => taskFunc(this.Proxy), cancellationToken);
		}

		public Task<TResult> ExecuteAsync<TResult>(Func<TChannel, Task<TResult>> taskFunc, Func<Exception, bool> transientCheckFunc, int retryCount, TimeSpan retryInterval)
		{
			return ExecuteAsync<TResult>(taskFunc, CreateRetryPolicy(transientCheckFunc, retryCount, retryInterval), CancellationToken.None);
		}

		private void EnsureChannel()
		{
			lock (SyncRoot)
			{
				if (this.channel != null && ((ICommunicationObject)this.channel).State == CommunicationState.Faulted)
				{
					Dispose((ICommunicationObject)this.channel);
					((ICommunicationObject)this.channel).Faulted -= OnChannelFaulted;
					this.channel = null;
				}
				if (this.channel == null)
				{
					if (this.channelFactory == null)
						throw new ObjectDisposedException(GetType().Name);

					this.channel = this.channelFactory.CreateChannel(); 
					((ICommunicationObject)this.channel).Faulted += OnChannelFaulted;
				}
			}
		}

		private static void Dispose(ICommunicationObject communicationObject)
		{
			try
			{
				if (communicationObject != null)
				{
					if (communicationObject.State != CommunicationState.Faulted)
						communicationObject.Close();
					else communicationObject.Abort();
				}
			}
			catch (CommunicationException)
			{
				if (communicationObject != null)
					communicationObject.Abort();
			}
			catch (TimeoutException)
			{
				if (communicationObject != null)
					communicationObject.Abort();
			}
			catch (Exception)
			{
				if (communicationObject != null)
					communicationObject.Abort();
				throw;
			}
		}

		private void OnChannelFaulted(object sender, EventArgs e)
		{
			var handler = ChannelFaulted;
			if (handler != null) handler(sender, e);
		}

		private RetryPolicy CreateRetryPolicy(Func<Exception, bool> transientCheckFunc, int retryCount, TimeSpan retryInterval)
		{
			var transientStrategy = new TransientErrorCallbackStrategy(this.Proxy, transientCheckFunc);
			return new RetryPolicy(transientStrategy, retryCount, retryInterval);
		}

		private sealed class TransientErrorCallbackStrategy : ITransientErrorDetectionStrategy
		{
			private Func<Exception, bool> transientCheckFunc;
			private TChannel channel;

			public TransientErrorCallbackStrategy(TChannel channel, Func<Exception, bool> transientCheckFunc)
			{
				this.channel = channel;
				this.transientCheckFunc = transientCheckFunc;
			}

			public Boolean IsTransient(Exception ex)
			{
				if (ex is CommunicationException)
				{
					Dispose((ICommunicationObject)this.channel);
					return false;
				}
				return (transientCheckFunc != null) ? transientCheckFunc(ex) : false;
			}
		}
	}
}
