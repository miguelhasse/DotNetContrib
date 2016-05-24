using System;

namespace Sample.Configuration
{
    internal static class Constants
	{
		internal const string SectionGroupName = "hasseware.services";
		internal const string HostSectionName = "hosting";

		internal static string HostSectionPath
		{
			get { return GetSectionPath(HostSectionName); }
		}

		private static string GetSectionPath(string sectionName)
		{
            return String.Join("/", SectionGroupName, sectionName);
		}
	}
}
