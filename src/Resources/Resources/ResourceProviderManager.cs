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
        private static readonly Func<ResourceManager, CultureInfo> NeutralCultureAccessor;
        private static readonly Func<ResourceManager, IDictionary<string, ResourceSet>> ResourceSetsAccessor;

        #region Constructors

        public ResourceProviderManager(string baseName) : 
            base(baseName, Assembly.GetCallingAssembly(), typeof(ResourceProviderSet))
        {
        }

        static ResourceProviderManager()
        {
            ParameterExpression param = Expression.Parameter(typeof(ResourceManager), "arg");
            var fields = typeof(ResourceManager).GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);

            ResourceProviderManager.NeutralCultureAccessor = (Func<ResourceManager, CultureInfo>)
                Expression.Lambda(typeof(Func<ResourceManager, CultureInfo>), 
                    Expression.Field(param, fields.Where(f => typeof(CultureInfo)
                    .IsAssignableFrom(f.FieldType)).Select(f => f.Name).First()), param).Compile();

            ResourceProviderManager.ResourceSetsAccessor = (Func<ResourceManager, IDictionary<string, ResourceSet>>)
               Expression.Lambda(typeof(Func<ResourceManager, IDictionary<string, ResourceSet>>),
                   Expression.Field(param, fields.Where(f => typeof(IDictionary<string, ResourceSet>)
                    .IsAssignableFrom(f.FieldType)).Select(f => f.Name).First()), param).Compile();
        }

        #endregion

        public override object GetObject(string name, CultureInfo culture)
        {
            return base.GetObject(name, culture);
        }

        protected override ResourceSet InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
        {
            ResourceSet rs = base.InternalGetResourceSet(culture, false, false);

            if (rs == null && createIfNotExists)
            {
                var resourceSets = ResourceSetsAccessor(this);

                lock (resourceSets)
                {
                    if (!resourceSets.TryGetValue(culture.Name, out rs))
                    {
                        var provider = CreateResourceDataProvider();
                        var neutralCulture = NeutralCultureAccessor(this);

                        if (provider != null)
                        {
                            rs = new ResourceProviderSet(provider, base.BaseName, (culture == CultureInfo.InvariantCulture) ? neutralCulture : culture);                            
                            resourceSets.Add((neutralCulture == culture) ? CultureInfo.InvariantCulture.Name : culture.Name, rs);
                        }
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

