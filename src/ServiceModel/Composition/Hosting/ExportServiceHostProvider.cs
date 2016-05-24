using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace System.ServiceModel.Composition.Hosting
{
    /// <summary>
    /// Defines an export provider that creates <see cref="ServiceHost"/> instances.
    /// </summary>
    public class ExportServiceHostProvider<T> : ExportProvider where T : IHostedService
    {
        #region Fields

        private static readonly string MatchContractName;

        #endregion

        #region Constructors

        static ExportServiceHostProvider()
        {
            MatchContractName = AttributedModelServices.GetContractName(typeof(ExportServiceHost<T>));
        }

        #endregion

        #region Properties

        /// <summary>
        ///// Gets or sets the source container.
        /// </summary>
        public CompositionContainer SourceContainer { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets available exports for the specified import definition.
		/// </summary>
		/// <param name="importDefinition">The import definition.</param>
		/// <param name="composition">The composition.</param>
		/// <returns>A set of <see cref="Export"/> instances.</returns>
		protected override IEnumerable<Export> GetExportsCore(ImportDefinition importDefinition, AtomicComposition composition)
		{
			if (SourceContainer == null)
				throw new InvalidOperationException("SourceProvider property has not been set.");

			if (importDefinition.ContractName.Equals(MatchContractName))
			{
                var exports = SourceContainer.GetExports<T, IHostedServiceMetadata>(typeof(T).FullName);

                Func<IHostedServiceMetadata, Export> factory = m => new Export(MatchContractName, () =>
                    ExportServiceHostFactory<T>.CreateExportServiceHost(SourceContainer, m));

                switch (importDefinition.Cardinality)
				{
					case ImportCardinality.ExactlyOne:
					{
						var export = exports.Single();
						return new[] { factory(export.Metadata) };
					}
					case ImportCardinality.ZeroOrOne:
					{
						var export = exports.SingleOrDefault();
						return (export == null) ? Enumerable.Empty<Export>() : new[] { factory(export.Metadata) };
					}
					case ImportCardinality.ZeroOrMore:
					{
						return exports.Select(e => factory(e.Metadata));
					}
				}
			}
			return Enumerable.Empty<Export>();
		}
		
		#endregion
	}
}