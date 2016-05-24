using Microsoft.Win32;
using System;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Composition;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Sample
{
    internal class SampleServiceHost : ServiceBase
	{
		#region Fields

		// Singleton object references...
		private ComposedServiceManager manager;

		#endregion

		#region Constructors

		public SampleServiceHost()
		{
			AppDomain.CurrentDomain.UnhandledException += (_, e) =>
				this.OnUnhandledException((Exception)e.ExceptionObject, e.IsTerminating);

			System.Net.ServicePointManager.ServerCertificateValidationCallback += (_, certificate, chain, sslPolicyErrors) =>
				this.OnValidadeCertificate(certificate, chain, sslPolicyErrors);

			this.ServiceName = SampleServiceInstaller.ServiceName;
		}

		#endregion

		#region Startup

		static void Main(string[] args)
		{
			var service = new SampleServiceHost();
			service.EnsureEventLogListener();

			if (Environment.UserInteractive)
			{
				Trace.Listeners.Add(new ConsoleTraceListener());

				UnsafeNativeMethods.SetThreadExecutionState(
					ThreadExecutionState.SYSTEM_REQUIRED |
					ThreadExecutionState.CONTINUOUS);

				try { service.RunInteractive(); }
				finally { service.Dispose(); }
			}
			else ServiceBase.Run(service);
		}

		#endregion

		private void RunInteractive()
		{
			Console.WriteLine("{0} running in interactive mode.\n", this.ServiceName);

			try
			{
				this.OnStart(Environment.GetCommandLineArgs());

				Console.WriteLine("\nPress any key to stop and terminate all listeners...");
				Console.ReadKey();
			}
			catch (Exception ex)
			{
                var msgb = new System.Text.StringBuilder(ex.Message);
                while ((ex = ex.InnerException) != null)
                    msgb.Append(" ").Append(ex.Message.TrimEnd('.')).Append(".");
                Trace.TraceError(msgb.ToString());
			}
			finally
			{
				this.OnStop();
				Console.WriteLine("Service stopped.");
			}
			// Keep the console alive for a second to allow the user to see the message.
			System.Threading.Thread.Sleep(1000);
		}

		private void EnsureEventLogListener()
		{
			foreach (TraceListener listener in Trace.Listeners)
			{
				if (listener is EventLogTraceListener &&
					((EventLogTraceListener)listener).EventLog == this.EventLog)
					return; // already hooked, so just leave.
			}
			Trace.Listeners.Add(new EventLogTraceListener(this.EventLog));
		}

		protected override void OnStart(string[] args)
		{
			SampleServiceInstaller.EnsureServicesDependedOn();
			SampleServiceInstaller.EnsureFirewallAuthorization();

			this.manager = new ComposedServiceManager(ep =>
				new CompositionContainer(new DirectoryCatalog(".", "*.dll"), ep));

			Parallel.ForEach(this.manager, async (serviceHost, state) =>
			{
				foreach (var endpoint in serviceHost.Description.Endpoints)
				{
					Trace.WriteLine(String.Format(CultureInfo.InvariantCulture,
						"Hosting {0} at {1}", serviceHost.Meta.Name, endpoint.Address.Uri));
				}
				try
				{
					EnsureMetadataPublishing(serviceHost);
					serviceHost.Description.Behaviors.Add(new ServiceModel.HostServiceBehavior());

					await serviceHost.OpenAsync();
				}
				catch (InvalidOperationException ex)
				{
					Trace.TraceError(ex.Message);
				}
			});
		}

		protected override void OnStop()
		{
			if (this.manager != null)
			{
				Parallel.ForEach(this.manager, async serviceHost =>
				{
					if (serviceHost.State < CommunicationState.Closing)
						await serviceHost.CloseAsync();
				});
			}
		}

		private bool OnValidadeCertificate(X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		private void OnUnhandledException(Exception exception, bool terminating)
		{
			if (Environment.UserInteractive && exception is System.Security.SecurityException)
			{
				Console.WriteLine(exception.Message);
				System.Threading.Thread.Sleep(1000);
				Environment.Exit(-1);
			}
			for (Exception ex = exception; ex != null; ex = ex.InnerException)
			{
				if (ex is ReflectionTypeLoadException)
				{
					foreach (var typeLoadException in ((ReflectionTypeLoadException)ex).LoaderExceptions)
						Trace.TraceError(typeLoadException.Message);
				}
				else Trace.TraceError(ex.Message);
			}
			if (!Environment.UserInteractive)
				this.Stop();
		}

		private static void EnsureMetadataPublishing(ServiceHostBase serviceHost)
		{
			// Ensure the service host has a ServiceMetadataBehavior
			var behaviour = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
			if (behaviour == null) serviceHost.Description.Behaviors.Add(behaviour = new ServiceMetadataBehavior());

			var hostSchemes = serviceHost.Description.Endpoints
				.Where(s => !s.IsSystemEndpoint && s.Contract.ContractType != typeof(IMetadataExchange))
				.Select(s => s.Binding.Scheme);

			behaviour.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
			behaviour.HttpGetEnabled = hostSchemes.Contains(Uri.UriSchemeHttp);
			behaviour.HttpsGetEnabled = hostSchemes.Contains(Uri.UriSchemeHttps);

			var mexBindings = hostSchemes.Select(s =>
			{
				switch (s)
				{
					case "http": return MetadataExchangeBindings.CreateMexHttpBinding();
					case "net.tcp": return MetadataExchangeBindings.CreateMexTcpBinding();
					case "net.pipe": return MetadataExchangeBindings.CreateMexNamedPipeBinding();
					default: return null;
				}
			});
			foreach (var binding in mexBindings.ToList())
			{
				serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, binding, "mex");
			}
		}

		private static bool RequiresPortSharing(ServiceHostBase serviceHost)
		{
			return serviceHost.Description.Endpoints
				.Any(s => (s.Binding is NetTcpBinding) && ((NetTcpBinding)s.Binding).PortSharingEnabled);
		}
	}
}
