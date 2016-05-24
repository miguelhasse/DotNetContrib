using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Web.Compilation;

namespace Hasseware.Web.Compilation
{
    public class ResourceProvider : IResourceProvider, IImplicitResourceProvider, IResourceEnumerable
    {
        private Lazy<ResourceManager> resourceManager;
        private Lazy<IResourceReader> readerReader;

        #region Constructors

        public ResourceProvider(string baseName)
        {
            resourceManager = new Lazy<ResourceManager>(() =>
            {
                var rm = new Resources.ResourceProviderManager(baseName);
                rm.IgnoreCase = true;
                return rm;
            });
            readerReader = new Lazy<IResourceReader>(() => new Resources.ResourceProviderReader(
                Resources.ResourceProviderManager.CreateResourceDataProvider(),
                baseName, CultureInfo.InvariantCulture));
        }

        #endregion

        #region IResourceProvider

        public IResourceReader ResourceReader
        {
            get
            {
                try { return readerReader.Value; }
                catch { return null; }
            }
        }

        public object GetObject(string resourceKey, CultureInfo culture)
        {
            var obj = resourceManager.Value.GetObject(resourceKey, culture);
            return (obj == null && culture.Parent != CultureInfo.InvariantCulture) ?
                GetObject(resourceKey, culture.Parent) : obj;
        }

        #endregion

        #region IImplicitResourceProvider

        public ICollection GetImplicitResourceKeys(string keyPrefix)
        {
            List<ImplicitResourceKey> keys = new List<ImplicitResourceKey>();
            string extendedKeyPrefix = String.Concat(keyPrefix, ".");

            foreach (DictionaryEntry dictentry in ResourceReader)
            {
                string key = (string)dictentry.Key;
                if (key.StartsWith(extendedKeyPrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    string keyproperty = String.Empty;
                    if (key.Length > (keyPrefix.Length + 1))
                    {
                        int pos = key.IndexOf('.');
                        if ((pos > 0) && (pos == keyPrefix.Length))
                            keys.Add(new ImplicitResourceKey(String.Empty, keyPrefix, key.Substring(pos + 1)));
                    }
                }
            }
            return keys;
        }

        public object GetObject(ImplicitResourceKey key, CultureInfo culture)
        {
            string format = String.IsNullOrWhiteSpace(key.Filter) ? "{0}.{1}" : "{2}:{0}.{1}";
            string resourceKey = String.Format(format, key.KeyPrefix, key.Property, key.Filter);
            return resourceManager.Value.GetObject(resourceKey, culture);
        }

        #endregion

        #region IResourceEnumerable

        public IDictionaryEnumerator GetEnumerator(CultureInfo culture)
        {
            var rs = resourceManager.Value.GetResourceSet(culture, true, true);
            return (rs != null) ? rs.GetEnumerator() : new Dictionary<string, object>().GetEnumerator();
        }

        #endregion
    }
}
