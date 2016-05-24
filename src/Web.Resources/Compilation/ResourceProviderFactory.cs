using System;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Compilation;

namespace Hasseware.Web.Compilation
{
    public sealed class ResourceProviderFactory : System.Web.Compilation.ResourceProviderFactory
    {
        private static readonly Func<string, IResourceProvider> GetGlobalResourceProviderAccessor;

        #region Constructors

        static ResourceProviderFactory()
        {
            var method = typeof(ResourceExpressionBuilder).GetMethod("GetGlobalResourceProvider",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic);

            if (method != null)
            {
                ParameterExpression argumentParameter = Expression.Parameter(typeof(string), "arg");
                GetGlobalResourceProviderAccessor = Expression.Lambda<Func<string, IResourceProvider>>(
                    Expression.Convert(Expression.Call(method, new ParameterExpression[] { argumentParameter }), typeof(IResourceProvider)),
                    new ParameterExpression[] { argumentParameter }).Compile();
            }
        }

        #endregion

        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new ResourceProvider(classKey);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            return CreateGlobalResourceProvider(TranslatePath(virtualPath));
        }

        public static IDictionaryEnumerator GetGlobalResourceEnumerator(string classKey, CultureInfo culture)
        {
            if (GetGlobalResourceProviderAccessor == null)
                throw new OperationCanceledException("Failed to locate the global resource provider method on the base class.");

            var enumerableResourceProvider = GetGlobalResourceProviderAccessor(classKey) as IResourceEnumerable;
            return (enumerableResourceProvider != null) ? enumerableResourceProvider.GetEnumerator(culture) : null;
        }

        private static string TranslatePath(string virtualPath)
        {
            var serverUtility = HttpContext.Current.Server;
            return serverUtility.MapPath(virtualPath).Remove(0, serverUtility.MapPath("~").Length);
        }
    }
}
