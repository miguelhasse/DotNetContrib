using System.Configuration;

namespace Sample.Configuration
{
    public sealed class HostingSection : ConfigurationSection
	{
		#region Constants

		const string ApplyFirewallRulesKey = "applyFirewallRules";

		#endregion

		[ConfigurationProperty(ApplyFirewallRulesKey, DefaultValue = true)]
		public bool ApplyFirewallRules
		{
			get { return (bool)base[ApplyFirewallRulesKey]; }
			set { base[ApplyFirewallRulesKey] = value; }
		}

		internal static HostingSection GetSection()
		{
			return (HostingSection)ConfigurationManager.GetSection(Constants.HostSectionName) ?? new HostingSection();
		}
	}
}
