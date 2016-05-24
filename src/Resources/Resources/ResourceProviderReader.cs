using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;

namespace Hasseware.Resources
{
    public class ResourceProviderReader : IResourceReader
    {
        private Func<IDictionary<string, object>> providerFunc;

        #region Constructors

        public ResourceProviderReader(IResourceDataProvider provider, string baseName, CultureInfo cultureInfo)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            providerFunc = () => provider.Get(baseName, cultureInfo);
        }

        #endregion

        public IDictionaryEnumerator GetEnumerator()
        {
            return ((IDictionary)new ReadOnlyDictionary<string, object>(this.providerFunc())).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IResourceReader.Close()
        {
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
