using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Composition.Hosting;

namespace System.ServiceModel.Composition
{
    public sealed class ComposedServiceHostFactory : ComposedServiceHostFactory<IHostedService>
    {

    }

    /// <summary>
    /// Defines a service host factory for dynamic web services.
    /// </summary>
    public class ComposedServiceHostFactory<T> : ServiceHostFactory where T : IHostedService
    {
        #region Fields

        private static CompositionContainer container;
        private static readonly object sync = new object();

        #endregion

        #region Properties
        /// <summary>
        /// Gets the composition container.
        /// </summary>
        public CompositionContainer Container
        {
            get
            {
                lock (sync)
                {
                    return container;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the service host that will handle web service requests.
        /// </summary>
        /// <param name="constructorString">The constructor string used to select the service.</param>
        /// <param name="baseAddresses">The set of base address for the service.</param>
        /// <returns>An instance of <see cref="ServiceHostBase"/> for the service.</returns>
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            var meta = Container.GetExports<T, IHostedServiceMetadata>()
                .Where(e => e.Metadata.Name.Equals(constructorString, StringComparison.OrdinalIgnoreCase))
                .Select(e => e.Metadata)
                .SingleOrDefault();

            if (meta == null)
                return null;

            var host = new ExportServiceHost<T>(meta, baseAddresses);
            host.Description.Behaviors.Add(new ExportServiceBehavior<T>(Container, meta.Name));

            var contracts = meta.ServiceType.GetInterfaces()
                .Where(t => t.IsDefined(typeof(ServiceContractAttribute), true));

            EnsureHttpBinding(host, contracts);

            return host;
        }

        /// <summary>
        /// Ensures that the Http binding has been created.
        /// </summary>
        /// <param name="host">The Http binding.</param>
        /// <param name="contracts">The set of contracts</param>
        private static void EnsureHttpBinding(ExportServiceHost<T> host, IEnumerable<Type> contracts)
        {
            var binding = new BasicHttpBinding();

            host.Description.Endpoints.Clear();

            foreach (var contract in contracts)
                host.AddServiceEndpoint(contract.FullName, binding, string.Empty);
        }

        /// <summary>
        /// Sets the composition container factory.
        /// </summary>
        /// <param name="factory">The container factory.</param>
        public static void SetCompositionContainerFactory(ICompositionContainerFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            lock (sync)
            {
                var provider = new ExportServiceHostProvider<T>();
                container = factory.CreateCompositionContainer(provider);
                provider.SourceContainer = container;
            }
        }

        /// <summary>
        /// Sets the composition container factory.
        /// </summary>
        /// <param name="factory">The container factory.</param>
        public static void SetCompositionContainerFactory(Func<ExportProvider[], CompositionContainer> factory)
        {
            SetCompositionContainerFactory(new DelegateCompositionContainerFactory(factory));
        }
        #endregion
    }
}