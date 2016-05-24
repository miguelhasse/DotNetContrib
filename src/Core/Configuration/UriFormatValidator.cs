using System;
using System.Configuration;
using System.Globalization;

namespace Hasseware.Configuration
{
	internal sealed class UriFormatValidator : ConfigurationValidatorBase
	{
		private readonly UriKind uriKind;

		public UriFormatValidator(UriKind uriKind)
		{
			this.uriKind = uriKind;
		}

		public override bool CanValidate(Type type)
		{
			return (type == typeof(string));
		}

		public override void Validate(object value)
		{
			string uri = (string)value;

            if (!String.IsNullOrEmpty(uri) && !Uri.IsWellFormedUriString(uri, this.uriKind))
			{
				throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                    Properties.Resources.InvalidUriFormat, uri));
			}
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class UriFormatValidatorAttribute : ConfigurationValidatorAttribute
	{
		public UriFormatValidatorAttribute(UriKind uriKind)
		{
			this.UriKind = uriKind;
		}

		public UriKind UriKind { get; set; }

		public override ConfigurationValidatorBase ValidatorInstance
		{
			get { return new UriFormatValidator(this.UriKind); }
		}
	}
}
