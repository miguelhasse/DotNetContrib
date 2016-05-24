using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Composition.Description;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace System.ServiceModel.Composition.Hosting
{
    /// <summary>
    /// Defines a service host created from exported parts.
    /// </summary>
    public class ExportServiceHost<T> : ServiceHostBase where T : IHostedService
    {
        #region Fields

        private static readonly Type HostedServiceType = typeof(T);
		private readonly Uri[] baseAddresses;

		#endregion

		#region Constructors

		/// <summary>
		/// Initialises a new instance of <see cref="ExportServiceHost"/>.
		/// </summary>
		/// <param name="meta">The service host metadata.</param>
		/// <param name="baseAddresses">The collection of base addresses</param>
		public ExportServiceHost(IHostedServiceMetadata meta, Uri[] baseAddresses)
		{
			if (meta == null)
				throw new ArgumentNullException("meta");

			this.Meta = meta;

			this.baseAddresses = (baseAddresses == null || baseAddresses.Length == 0) ? null : baseAddresses;
			InitializeDescription(new UriSchemeKeyedCollection(baseAddresses));
		}

		#endregion

		#region Properties

		/// <summary>
		/// Get the service host metadata.
		/// </summary>
		public IHostedServiceMetadata Meta { get; private set; }

		#endregion

		#region Methods

		public Task OpenAsync(TimeSpan? timeout = null)
		{
			return Task.Factory.FromAsync(
				base.BeginOpen(timeout.HasValue ? timeout.Value : base.DefaultOpenTimeout, null, null),
				base.EndOpen);
		}

		public Task CloseAsync(TimeSpan? timeout = null)
		{
			return Task.Factory.FromAsync(
				base.BeginClose(timeout.HasValue ? timeout.Value : base.DefaultCloseTimeout, null, null),
				base.EndClose);
		}

		/// <summary>
		/// Adds the base addresses to the service.
		/// </summary>
		/// <param name="endpoints">The endpoints.</param>
		private void AddBaseAddresses(IEnumerable<ServiceEndpoint> endpoints)
		{
			if (baseAddresses == null)
			{
				var addresses = endpoints.Select(s => s.Address.Uri).Distinct();

				foreach (Uri address in addresses)
					AddBaseAddress(address);
			}
		}

		/// <summary>
		/// Creates a service description
		/// </summary>
		/// <param name="implementedContracts">[Out] The set of contracts.</param>
		protected override ServiceDescription CreateDescription(out IDictionary<string, ContractDescription> implementedContracts)
		{
			var sd = new ServiceDescription { ServiceType = Meta.ServiceType };

			implementedContracts = GetContracts(Meta.ServiceType)
				.ToDictionary(cd => cd.ConfigurationName, cd => cd);

			var endpointAttributes = GetEndpoints(Meta.ServiceType);

			foreach (var cd in implementedContracts.Values)
			{
                if (Meta.SessionRequired)
                    cd.SessionMode = SessionMode.Required;

                foreach (var endpoint in GetServiceEndpoints(endpointAttributes, Meta, cd))
					sd.Endpoints.Add(endpoint);
			}

			var serviceBehaviour = EnsureServiceBehavior(sd);
			serviceBehaviour.InstanceContextMode = InstanceContextMode.PerSession;

			foreach (var endpointAttribute in endpointAttributes)
				endpointAttribute.UpdateServiceDescription(sd);

			AddBaseAddresses(sd.Endpoints);
			return sd;
		}

		/// <summary>
		/// Ensures the <see cref="ServiceDescription"/> has a service behaviour attribute specified.
		/// </summary>
		/// <param name="description">The service description.</param>
		/// <returns>An instance of <see cref="ServiceBehaviorAttribute"/>.</returns>
		private static ServiceBehaviorAttribute EnsureServiceBehavior(ServiceDescription description)
		{
			var attr = description.Behaviors.Find<ServiceBehaviorAttribute>();
			if (attr == null) 
			{
				attr = new ServiceBehaviorAttribute();
				description.Behaviors.Insert(0, attr);
			}
			return attr;
		}

        /// <summary>
        /// Gets the set of <see cref="ContractDescription"/> that describe the available contracts of the service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>An enumerable of <see cref="ContractDescription"/>.</returns>
        private static IEnumerable<ContractDescription> GetContracts(Type serviceType)
		{
			var collection = new ReflectedContractCollection();
			foreach (var contract in serviceType.GetInterfaces().Where(t => t != HostedServiceType))
			{
				if (!collection.Contains(contract))
				{
					var cd = ContractDescription.GetContract(contract, serviceType);
					collection.Add(cd);

					foreach (var icd in cd.GetInheritedContracts())
					{
						if (!collection.Contains(icd.ContractType))
							collection.Add(icd);
					}
				}
			}
			return collection;
		}

		/// <summary>
		/// Gets all <see cref="EndpointAttribute"/> which describe serviceable Uris.
		/// </summary>
		/// <param name="serviceType">The service type.</param>
		/// <returns>The set of <see cref="EndpointAttribute"/> instances.</returns>
		private static IEnumerable<EndpointAttribute> GetEndpoints(Type serviceType)
		{
			var attrs = (EndpointAttribute[])serviceType.GetCustomAttributes(typeof(EndpointAttribute), true);
			return attrs.Any() ? attrs : new[] { new HttpEndpointAttribute() };
		}

		/// <summary>
		/// Gets the service endpoints for the service.
		/// </summary>
		/// <param name="attributes">The set of endpoint attributes.</param>
		/// <param name="meta">The service metadata.</param>
		/// <param name="description">The contract description</param>
		/// <returns>The set of endpoints.</returns>
		private static IEnumerable<ServiceEndpoint> GetServiceEndpoints(IEnumerable<EndpointAttribute> attributes,
            IHostedServiceMetadata meta, ContractDescription description)
		{
			return attributes.Select(a => a.CreateEndpoint(description, meta));
		}

		#endregion

		#region Types

		/// <summary>
		/// Represents a collection of type / contract description mappings.
		/// </summary>
		private class ReflectedContractCollection : KeyedCollection<Type, ContractDescription>
		{
			#region Constructors

			/// <summary>
			/// Initialises a new instance of <see cref="ReflectedContractCollection"/>
			/// </summary>
			public ReflectedContractCollection() : base(null, 4)
			{
			}

			#endregion

			#region Methods

			/// <summary>
			/// Gets the key for the specified contract description.
			/// </summary>
			/// <param name="item">The contract description.</param>
			/// <returns>The key type.</returns>
			protected override Type GetKeyForItem(ContractDescription item)
			{
				if (item == null)
					throw new ArgumentNullException("item");

				return item.ContractType;
			}

			#endregion
		}

		#endregion
	}
}