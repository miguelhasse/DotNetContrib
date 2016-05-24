using System.ComponentModel.Composition.Hosting;

namespace System.ServiceModel.Composition.Hosting
{
    /// <summary>
    /// Creates instances of <see cref="ExportServiceHost"/>.
    /// </summary>
    internal static class ExportServiceHostFactory<T> where T : IHostedService
    {
        #region Methods

        /// <summary>
        /// Creates an instance of <see cref="ExportServiceHost"/> using service metadata.
        /// </summary>
        /// <param name="container">The composition container.</param>
        /// <param name="meta">The metadata.</param>
        public static ExportServiceHost<T> CreateExportServiceHost(CompositionContainer container, IHostedServiceMetadata meta)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (meta == null) throw new ArgumentNullException("meta");

            var host = new ExportServiceHost<T>(meta, new Uri[0]);
            host.Description.Behaviors.Add(new ExportServiceBehavior<T>(container, meta.Name));
            return host;
        }

        #endregion
    }
}
