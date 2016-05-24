using System.ServiceModel.Description;

namespace System.ServiceModel.Composition.Description
{
    /// <summary>
    /// Describes an endpoint that supports the Named Pipe scheme.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class PipeEndpointAttribute : EndpointAttribute
	{
		#region Fields

		private const int DefaultPort = 60001;

		#endregion

		#region Constructor

		/// <summary>
		/// Initialises a new instance of <see cref="PipeEndpointAttribute"/>.
		/// </summary>
		public PipeEndpointAttribute() : base(DefaultPort) { }

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
			var uri = CreateUri(Uri.UriSchemeNetPipe, meta);
			var address = new EndpointAddress(uri);

			var binding = (BindingConfiguration == null) ?
				new NetNamedPipeBinding() : new NetNamedPipeBinding(BindingConfiguration);

			return new ServiceEndpoint(description, binding, address);
		}

		#endregion
	}
}
