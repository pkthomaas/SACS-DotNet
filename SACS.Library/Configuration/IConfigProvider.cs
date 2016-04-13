namespace SACS.Library.Configuration
{
    /// <summary>
    /// The configuration provider.
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Gets the REST group setting or SOAP organization setting.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
        string Group { get; }

        /// <summary>
        /// Gets the REST user identifier or SOAP username.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        string UserId { get; }

        /// <summary>
        /// Gets the REST client secret or SOAP password.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        string ClientSecret { get; }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        string Domain { get; }

        /// <summary>
        /// Gets the environment URL address.
        /// </summary>
        /// <value>
        /// The environment URL address.
        /// </value>
        string Environment { get; }
    }
}