using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace System.ServiceModel.Composition.Hosting
{
    /// <summary>
    /// Providers instance creation through a composition container.
    /// </summary>
    public class ExportInstanceProvider<T> : IInstanceProvider where T : IHostedService
    {
        #region Fields

        private readonly string serviceName;
        private readonly CompositionContainer container;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="ExportInstanceProvider"/>.
        /// </summary>
        /// <param name="container">The current composition container.</param>
        /// <param name="name">The name of the service.</param>
        public ExportInstanceProvider(CompositionContainer container, string name)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
            serviceName = name;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a service object given the specified <see cref="InstanceContext"/> object.
        /// </summary>
        /// <param name="context">The current <see cref="InstanceContext"/> object.</param>
        /// <returns>The service object.</returns>
        public object GetInstance(InstanceContext context)
        {
            return GetInstance(context, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="InstanceContext"/> object.
        /// </summary>
        /// <param name="context">The current <see cref="InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        public object GetInstance(InstanceContext context, Message message)
        {
            return container.GetExports<T, IHostedServiceMetadata>()
                .Where(l => l.Metadata.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase))
                .Select(l => l.Value)
                .FirstOrDefault();
        }

        /// <summary>
        /// Called when an <see cref="InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="context">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext context, object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        #endregion
    }
}
