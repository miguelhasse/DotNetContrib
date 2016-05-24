using System.ComponentModel.Composition;

namespace System.ServiceModel.Composition
{
    /// <summary>
    /// Allows the export of a hosted service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true), MetadataAttribute]
    public class ExportServiceAttribute : ExportAttribute, IHostedServiceMetadata
	{
		#region Constructors

		/// <summary>
		/// Initialises a new instance of <see cref="ExportServiceAttribute" />.
		/// </summary>
		/// <param name="name">The name of the service.</param>
		/// <param name="serviceType">The service type.</param>
		public ExportServiceAttribute(string name, Type serviceType) 
			: this(name, serviceType, typeof(IHostedService))
		{
		}

		/// <summary>
		/// Initialises a new instance of <see cref="ExportServiceAttribute" />.
		/// </summary>
		/// <param name="name">The name of the service.</param>
		/// <param name="serviceType">The service type.</param>
		/// <param name="hostedServiceType">The hosted service interface type, derived from <see cref="IHostedService" />.</param>
		protected ExportServiceAttribute(string name, Type serviceType, Type hostedServiceType)
            : base(hostedServiceType.FullName, hostedServiceType)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is a required parameter.", "name");

            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

			if (!typeof(IHostedService).IsAssignableFrom(hostedServiceType))
				throw new ArgumentException("The hosted service type must inherit IHostedService.", "hostedServiceType");

			this.Name = name;
            this.ServiceType = serviceType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the service type.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Specifies if a session is required
        /// </summary>
        public bool SessionRequired { get; set; }

        #endregion
    }
}