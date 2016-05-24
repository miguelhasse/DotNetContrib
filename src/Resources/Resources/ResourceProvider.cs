using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;

namespace Hasseware.Resources
{
    public abstract class ResourceProvider : ProviderBase, IResourceDataProvider
    {
        public virtual IResourceConverter Converter { get; protected set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            Converter = new ResourceConverter();
            base.Initialize(name, config);
        }

        public abstract void Delete(string key, string resourceSet, CultureInfo culture);
        public abstract IDictionary<string, object> Get(string resourceSet, CultureInfo culture);
        public abstract object Get(string key, string resourceSet, CultureInfo culture);
        public abstract void Save(IDictionary<string, object> resources, string resourceSet, CultureInfo culture);
        public abstract void Save(string key, object value, string resourceSet, CultureInfo culture);
    }
}
