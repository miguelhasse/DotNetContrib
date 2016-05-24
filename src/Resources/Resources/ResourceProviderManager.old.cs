using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;

namespace Hasseware.Resources
{
    public class ResourceProviderManager : ResourceManager
    {
        private static readonly Func<ResourceManager, CultureInfo> delegateNeutralResourcesCulture;

        public ResourceProviderManager(string baseName) : 
            base(baseName, Assembly.GetCallingAssembly(), typeof(ResourceProviderSet))
        {
        }

        static ResourceProviderManager()
        {
            ParameterExpression param = Expression.Parameter(typeof(ResourceManager), "arg");

            delegateNeutralResourcesCulture = (Func<ResourceManager, CultureInfo>)
                Expression.Lambda(typeof(Func<ResourceManager, CultureInfo>), 
                Expression.Field(param, "_neutralResourcesCulture"), param).Compile();
        }

        public override object GetObject(string name, CultureInfo culture)
        {
            return base.GetObject(name, culture);
        }

        protected override ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            ResourceSet rs = base.InternalGetResourceSet(culture, false, false);

            if (rs == null)
            {
                if (culture == CultureInfo.InvariantCulture)
                    culture = delegateNeutralResourcesCulture(this);

#pragma warning disable CS0618 // Type or member is obsolete
                rs = ResourceSets[culture.Name] as ResourceSet;
#pragma warning restore CS0618 // Type or member is obsolete

                if (rs == null && createIfNotExists)
                {
                    var provider = CreateResourceDataProvider();

                    if (provider != null)
                    {
                        rs = new ResourceProviderSet(provider, base.BaseName, culture);
#pragma warning disable CS0618 // Type or member is obsolete
                        ResourceSets.Add(culture.Name, rs);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                }
            }
            return rs;
        }

        public static IResourceDataProvider CreateResourceDataProvider()
        {
            var config = Configuration.ResourceProviderSection.GetConfiguration();

            if (config.ProvidersInternal.Count == 1)
            {
                for (var enumerator = config.ProvidersInternal.GetEnumerator(); enumerator.MoveNext();)
                    return (IResourceDataProvider)enumerator.Current;
            }
            return config.ProvidersInternal[config.DefaultProvider];
        }
    }
}

