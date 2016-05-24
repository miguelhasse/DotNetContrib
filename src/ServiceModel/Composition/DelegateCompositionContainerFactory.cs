using System.ComponentModel.Composition.Hosting;

namespace System.ServiceModel.Composition
{
    /// <summary>
    /// Defines a container factory that supporst delegated container creation.
    /// </summary>
    public sealed class DelegateCompositionContainerFactory : ICompositionContainerFactory
    {
        #region Fields

        private Func<ExportProvider[], CompositionContainer> factory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="DelegateCompositionContainerFactory"/>.
        /// </summary>
        /// <param name="factory">The container factory.</param>
        public DelegateCompositionContainerFactory(Func<ExportProvider[], CompositionContainer> factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            this.factory = factory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a container used for composition.
        /// </summary>
        /// <param name="providers">The set of export providers.</param>
        /// <returns>An instance of <see cref="CompositionContainer"/>.</returns>
        public CompositionContainer CreateCompositionContainer(params ExportProvider[] providers)
        {
            return this.factory(providers);
        }

        #endregion
    }
}