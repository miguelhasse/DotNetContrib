using System;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;

namespace Sample
{
    [RunInstaller(true)]
	public partial class SampleServiceInstaller : System.Configuration.Install.Installer
	{
		private static readonly string[] ServicesDependedOn = { "NetTcpPortSharing" };
		internal const string ServiceDisplayName = "Hasseware Sample Service";
		internal const string ServiceName = "HassewareSampleService";

		public SampleServiceInstaller()
		{
			this.Installers.Add(new ServiceProcessInstaller
			{
				Account = ServiceAccount.LocalService
			});
			this.Installers.Add(new ServiceInstaller
			{
				DisplayName = ServiceDisplayName,
				ServiceName = ServiceName,
				StartType = ServiceStartMode.Automatic,
				ServicesDependedOn = ServicesDependedOn
			});
		}

		internal static void EnsureServicesDependedOn()
		{
			var controllers = ServiceController.GetServices()
				.Where(s => ServicesDependedOn.Contains(s.ServiceName, StringComparer.OrdinalIgnoreCase));

            foreach (var controller in controllers)
			{
				switch (controller.Status)
				{
					case ServiceControllerStatus.Stopped:
						controller.Start();
						break;
					case ServiceControllerStatus.Paused:
						controller.Continue();
						break;
				}
			}
            foreach (var controller in controllers) // wait for services to start
            {
                if (controller.Status == ServiceControllerStatus.StartPending)
                    controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1));
            }
        }

		internal static void EnsureFirewallAuthorization()
		{
			Type type = Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication");
			dynamic auth = Activator.CreateInstance(type);
			auth.Name = ServiceDisplayName;
			auth.ProcessImageFileName = AppDomain.CurrentDomain.FriendlyName;
			auth.Enabled = true;

			type = Type.GetTypeFromCLSID(Guid.Parse("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
			dynamic manager  = Activator.CreateInstance(type);
			manager.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(auth);
		}
	}
}
