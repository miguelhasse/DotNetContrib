using System;
using System.Globalization;
using System.Resources;

namespace Hasseware.Resources
{
    public class ResourceProviderSet : ResourceSet
    {
        private Lazy<IResourceWriter> resourceWriter;

        #region Constructors

        public ResourceProviderSet(IResourceDataProvider provider, string baseName, CultureInfo cultureInfo) 
            : base(new ResourceProviderReader(provider, baseName, cultureInfo))
        {
            resourceWriter = new Lazy<IResourceWriter>(() => new ResourceProviderWriter(provider, baseName, cultureInfo));
        }

        #endregion

        public IResourceWriter Writer
        {
            get { return resourceWriter.Value; }
        }

        public override Type GetDefaultReader()
        {
            return typeof(ResourceProviderReader);
        }

        public override Type GetDefaultWriter()
        {
            return typeof(ResourceProviderWriter);
        }

        public void Save()
        {
            var writer = (ResourceProviderWriter)Writer;
            writer.CopyFrom(base.Table);
            writer.Generate();
        }
    }
}
