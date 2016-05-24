using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Globalization;

namespace Hasseware.Configuration
{
    public sealed class ResourceProviderSection : ConfigurationSection
    {
        private const string DefaultProviderKey = "defaultProvider";

        private const string ProvidersElementName = "providers";

        private static ResourceProviderSection instance;
        private Resources.ResourceProviderCollection resourceProviders;

        #region Constructors

        public ResourceProviderSection()
        {
            base[ProvidersElementName] = new ProviderSettingsCollection();
        }

        #endregion

        [ConfigurationProperty(DefaultProviderKey)]
        public string DefaultProvider
        {
            get { return (string)this[DefaultProviderKey]; }
        }

        [ConfigurationProperty(ProvidersElementName, IsRequired = true)]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)this[ProvidersElementName]; }
        }

        internal Resources.ResourceProviderCollection ProvidersInternal
        {
            get
            {
                if (this.resourceProviders == null)
                {
                    lock (this)
                    {
                        if (this.resourceProviders == null)
                        {
                            var resourceProviderCollection = new Resources.ResourceProviderCollection();
                            InstantiateProviders(this.Providers, resourceProviderCollection, typeof(Resources.ResourceProvider));
                            this.resourceProviders = resourceProviderCollection;
                        }
                    }
                }
                return this.resourceProviders;
            }
        }

        protected override void PostDeserialize()
        {
            // validate values of dependent section properties
            if (!String.IsNullOrEmpty(this.DefaultProvider) && this.Providers[this.DefaultProvider] == null)
            {
                var defaultProviderInfo = base.ElementInformation.Properties[DefaultProviderKey];
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture,
                    Hasseware.Properties.Resources.Config_provider_must_exist, this.DefaultProvider),
                    defaultProviderInfo.Source, defaultProviderInfo.LineNumber);
            }
            if (this.Providers.Count > 1 && String.IsNullOrEmpty(this.DefaultProvider))
            {
                var defaultProviderInfo = base.ElementInformation.Properties[DefaultProviderKey];
                throw new ConfigurationErrorsException(String.Format(CultureInfo.CurrentCulture,
                    Hasseware.Properties.Resources.Config_provider_default_required),
                    defaultProviderInfo.Source, defaultProviderInfo.LineNumber);
            }
            base.PostDeserialize();
        }

        private static void InstantiateProviders(ProviderSettingsCollection configProviders, ProviderCollection providers, Type providerType)
        {
            foreach (ProviderSettings configProvider in configProviders)
            {
                Type type = Type.GetType(configProvider.Type, true, true);
                if (!typeof(Resources.ResourceProvider).IsAssignableFrom(type))
                {
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture,
                         Hasseware.Properties.Resources.Provider_must_implement_type, typeof(Resources.ResourceProvider)));
                }

                var resourceProvider = (Resources.ResourceProvider)Activator.CreateInstance(type);

                NameValueCollection parameters = configProvider.Parameters;
                NameValueCollection nameValueCollection = new NameValueCollection(parameters.Count, StringComparer.Ordinal);

                foreach (string parameter in parameters)
                {
                    nameValueCollection[parameter] = parameters[parameter];
                }
                resourceProvider.Initialize(configProvider.Name, nameValueCollection);
                providers.Add(resourceProvider);
            }
        }

        public static ResourceProviderSection GetConfiguration()
        {
            lock (typeof(ResourceProviderSection))
            {
                if (instance == null)
                {
                    lock (typeof(ResourceProviderSection))
                    {
                        if ((instance = (ConfigurationManager.GetSection("resourceManager") as ResourceProviderSection)) == null)
                        {
                            throw new ApplicationException(String.Format(CultureInfo.CurrentCulture,
                                Hasseware.Properties.Resources.MissingConfigurationSection, typeof(ResourceProviderSection).Name));
                        }
                    }
                }
                return instance;
            }
        }
    }
}
