using System;
using System.Configuration;
using System.Globalization;

namespace Hasseware.Configuration
{
	internal sealed class ConnectionStringNameValidator : ConfigurationValidatorBase
	{
		private readonly string providerName;

		internal ConnectionStringNameValidator(string providerName)
		{
			this.providerName = providerName;
		}

		public override bool CanValidate(Type type)
		{
			return (type == typeof(string));
		}

		public override void Validate(object value)
		{
			string connectionStringName = (string)value;
			if (!String.IsNullOrWhiteSpace(connectionStringName))
			{
				var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

				if (connectionStringSettings == null)
				{
					throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                        Properties.Resources.InvalidConnectionStringName, connectionStringName));
				}
				if (this.providerName != null && (connectionStringSettings.ProviderName == null ||
					!this.providerName.Equals(connectionStringSettings.ProviderName, StringComparison.OrdinalIgnoreCase)))
				{
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                        Properties.Resources.InvalidConnectionProviderName, connectionStringName));
				}
			}
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class ConnectionStringNameValidatorAttribute : ConfigurationValidatorAttribute
	{
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get { return new ConnectionStringNameValidator(this.ProviderName); }
		}

		public string ProviderName { get; set; }
	}
}
