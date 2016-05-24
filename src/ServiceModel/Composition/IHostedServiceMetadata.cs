namespace System.ServiceModel.Composition
{
    /// <summary>
    /// Defines the required contract for implementing hosted service metadata.
    /// </summary>
    public interface IHostedServiceMetadata
    {
        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the service type.
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        /// Specifies if a session is required
        /// </summary>
        bool SessionRequired { get; }
    }
}