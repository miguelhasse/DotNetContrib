using System;
using System.Configuration.Provider;

namespace Hasseware.Resources
{
    public sealed class ResourceProviderCollection : ProviderCollection
    {
        public new ResourceProvider this[string name]
        {
            get { return (ResourceProvider)base[name]; }
        }

        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider", "The provider parameter cannot be null.");
            }
            if (!(provider is ResourceProvider))
            {
                throw new ArgumentException("The provider parameter must be of type ResourceProvider.", "provider");
            }
            base.Add(provider);
        }

        public void CopyTo(ResourceProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
