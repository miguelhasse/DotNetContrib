using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Composition.Hosting;
using System.Threading.Tasks;

namespace System.ServiceModel.Composition
{
    public sealed class ComposedServiceManager : ComposedServiceManager<IHostedService>
    {
        public ComposedServiceManager(ICompositionContainerFactory factory) : base(factory) { }

        public ComposedServiceManager(Func<ExportProvider[], CompositionContainer> factory) : base(factory) { }
    }

    /// <summary>
    /// Defines a service manager.
    /// </summary>
    public class ComposedServiceManager<T> : IEnumerable<ExportServiceHost<T>> where T : IHostedService
    {
        #region Fields

        private readonly CompositionContainer container;
        public IEnumerable<ExportServiceHost<T>> services;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="ComposedServiceManager"/>.
        /// </summary>
        /// <param name="factory">The container factory.</param>
        public ComposedServiceManager(ICompositionContainerFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            var provider = new ExportServiceHostProvider<T>();
            container = factory.CreateCompositionContainer(provider);
            container.ComposeExportedValue(this);

            provider.SourceContainer = container;
            this.services = container.GetExportedValues<ExportServiceHost<T>>();
        }

        /// <summary>
        /// Initialises a new instance of <see cref="ComposedServiceManager"/>.
        /// </summary>
        /// <param name="factory">The delegate used to create a container.</param>
        public ComposedServiceManager(Func<ExportProvider[], CompositionContainer> factory)
            : this(new DelegateCompositionContainerFactory(factory))
        {
        }

        #endregion

        #region Methods

        public IEnumerator<ExportServiceHost<T>> GetEnumerator()
        {
            return (services != null) ? services.GetEnumerator() :
                Enumerable.Empty<ExportServiceHost<T>>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Open(TimeSpan? timeout = null)
        {
            foreach (var service in this)
            {
                if (service.State != CommunicationState.Created)
                    continue;

                foreach (var address in service.Description.Endpoints)
                {
                    Trace.TraceInformation("Hosting Service \"{0}\" at {1}",
                        service.Meta.Name, address.Address.Uri);
                }
                if (timeout.HasValue)
                {
                    service.Open(timeout.Value);
                }
                else service.Open();
            }
        }

        public async Task OpenAsync(TimeSpan? timeout = null)
        {
            foreach (var service in this)
            {
                if (service.State != CommunicationState.Created)
                    continue;

                foreach (var address in service.Description.Endpoints)
                {
                    Trace.TraceInformation("Hosting Service \"{0}\" at {1}",
                        service.Meta.Name, address.Address.Uri);
                }
                await service.OpenAsync(timeout);
            }
        }

        public void Close(TimeSpan? timeout = null)
        {
            foreach (var service in this)
            {
                if (service.State <= CommunicationState.Opened)
                {
                    if (timeout.HasValue)
                        service.Close(timeout.Value);
                    else service.Close();
                }
            }
        }

        public async Task CloseAsync(TimeSpan? timeout = null)
        {
            foreach (var service in this)
            {
                if (service.State <= CommunicationState.Opened)
                    await service.CloseAsync(timeout);
            }
        }

        #endregion
    }
}