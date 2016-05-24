using System.ComponentModel.Composition.Hosting;

namespace System.ServiceModel.Composition
{
    /// <summary>
    /// Defines the required contract for implementing a composition container factory.
    /// </summary>
    public interface ICompositionContainerFactory
    {
        #region Methods
        /// <summary>
        /// Creates a container used for composition.
        /// </summary>
        /// <param name="providers">The set of export providers.</param>
        /// <returns>An instance of <see cref="CompositionContainer"/>.</returns>
        CompositionContainer CreateCompositionContainer(params ExportProvider[] providers);
        #endregion
    }
}