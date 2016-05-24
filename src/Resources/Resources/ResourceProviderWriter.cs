using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;

namespace Hasseware.Resources
{
    public class ResourceProviderWriter : IResourceWriter
    {
        private IDictionary<string, object> resources;
        private bool hasBeenSaved;
        private Action saveFunc;

        #region Constructors

        public ResourceProviderWriter(IResourceDataProvider provider, string baseName, CultureInfo cultureInfo)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            hasBeenSaved = false;
            resources = new Dictionary<string, object>();

            saveFunc = () => provider.Save(resources, baseName, cultureInfo);
        }

        #endregion

        public void AddResource(string name, byte[] value)
        {
            AddResource(name, (object)value);
        }

        public void AddResource(string name, string value)
        {
            AddResource(name, (object)value);
        }

        public void AddResource(string name, object value)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (resources == null)
                throw new ObjectDisposedException(GetType().Name);

            resources[name] = value;
            hasBeenSaved = false;
        }

        public void CopyFrom(IResourceReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            var enumerator = reader.GetEnumerator();
            hasBeenSaved = false;

            while (enumerator.MoveNext())
                resources[(string)enumerator.Key] = enumerator.Value;
        }

        public void CopyFrom(System.Collections.IDictionary dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");

            var enumerator = dictionary.GetEnumerator();
            hasBeenSaved = false;

            while (enumerator.MoveNext())
                resources[(string)enumerator.Key] = enumerator.Value;
        }

        public void Generate()
        {
            if (!hasBeenSaved)
            {
                saveFunc();
                hasBeenSaved = true;
            }
        }

        void IResourceWriter.Close()
        {
            Generate();
        }

        void IDisposable.Dispose()
        {
            ((IResourceWriter)this).Close();
            resources = null;

            GC.SuppressFinalize(this);
        }
    }
}
