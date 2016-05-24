using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace System.ServiceModel.Composition.Hosting
{
    /// <summary>
    /// Defines a virtual path provider for dynamic service urls.
    /// </summary>
    public class ComposedServicePathProvider<T> : VirtualPathProvider where T : IHostedService

    {
        #region Methods

        /// <summary>
        /// Determines if the file exists at the virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>True if the file exists, otherwise false.</returns>
        public override bool FileExists(string virtualPath)
        {
            if (IsServiceCall(virtualPath))
                return true;

            return Previous.FileExists(virtualPath);
        }

        /// <summary>
        /// Creates a cache dependency based on the specified virtual paths.
        /// </summary>
        /// <param name="virtualPath">The path to the primary virtual resource.</param>
        /// <param name="virtualPathDependencies">An array of paths to other resources required by the primary virtual resource.</param>
        /// <param name="utcStart">The UTC time at which the virtual resources were read.</param>
        /// <returns>A <see cref="CacheDependency"/> object for the specified virtual resources</returns>
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return (IsServiceCall(virtualPath)) ? null :
				Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        /// <summary>
        /// Gets the virtual file that is represented by the virtual path.
        /// </summary>
        /// <param name="virtualPath">The virtual file path.</param>
        /// <returns>The <see cref="VirtualFile"/> instance.</returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            return IsServiceCall(virtualPath) ? new ComposedServiceVirtualFile<T>(virtualPath) : Previous.GetFile(virtualPath);
        }

        /// <summary>
        /// Determines if the current virtual path is a service call.
        /// </summary>
        /// <param name="virtualPath">The virtual file path.</param>
        /// <returns>True if the current virtual path is a service call, otherwise false.</returns>
        private static bool IsServiceCall(string virtualPath)
        {
            virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return (virtualPath.ToLower().StartsWith("~/services/"));
        }

        #endregion
    }
}