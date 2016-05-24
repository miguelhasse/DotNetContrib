using System.ServiceModel.Description;

namespace System.ServiceModel.Composition.Description
{
    /// <summary>
    /// Describes an endpoint that supports the Tcp scheme.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class TcpEndpointAttribute : EndpointAttribute
    {
        #region Fields

        private const int DefaultPort = 40001;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialises a new instance of <see cref="TcpEndpointAttribute"/>.
        /// </summary>
        public TcpEndpointAttribute() : base(DefaultPort) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets whether the reliable session is enabled.
        /// </summary>
        public bool ReliableSession { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of a <see cref="ServiceEndpoint"/> that represents the endpoint.
        /// </summary>
        /// <param name="description">The contract description.</param>
        /// <param name="meta">The hosted service metadata.</param>
        /// <returns>An instance of <see cref="ServiceEndpoint"/></returns>
        internal override ServiceEndpoint CreateEndpoint(ContractDescription description, IHostedServiceMetadata meta)
        {
            var uri = CreateUri(Uri.UriSchemeNetTcp, meta);
            var address = new EndpointAddress(uri);

			var binding = (BindingConfiguration == null) ?
                new NetTcpBinding() : new NetTcpBinding(BindingConfiguration);
            binding.ReliableSession.Enabled = this.ReliableSession;

            return new ServiceEndpoint(description, binding, address);
        }

        #endregion
    }
}
