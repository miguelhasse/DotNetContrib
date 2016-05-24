using System;
using System.Configuration;

namespace Sample.Configuration
{
    public sealed class SectionGroup : ConfigurationSectionGroup
	{
		public HostingSection Host
		{
			get { return (HostingSection)base.Sections[Constants.HostSectionName]; }
		}

		public static SectionGroup GetSectionGroup(System.Configuration.Configuration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
            return (SectionGroup)config.SectionGroups[Constants.SectionGroupName];
		}
	}
}
