using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using SACS.Library.Configuration;

namespace SACS.ExampleApp.Factories
{
    /// <summary>
    /// The configuration provider static factory.
    /// </summary>
    public static class ConfigFactory
    {
        /// <summary>
        /// The configuration file name
        /// </summary>
        private const string ConfigFileName = "SACSConfig.properties";

        /// <summary>
        /// The SOAP configuration provider
        /// </summary>
        private static readonly IConfigProvider SoapConfig = CreateForSoap();

        /// <summary>
        /// The rest configuration provider
        /// </summary>
        private static readonly IConfigProvider RestConfig = CreateForRest();

        /// <summary>
        /// Creates a configuration provider for SOAP.
        /// If a provider was already created, the method will not create new but return existing.
        /// </summary>
        /// <returns>Configuration provider.</returns>
        public static IConfigProvider CreateForSoap()
        {
            if (SoapConfig != null)
            {
                return SoapConfig;
            }

            string virtualPath = Path.Combine("~", "Soap", ConfigFileName);
            return new SACSConfigProvider(HostingEnvironment.MapPath(virtualPath));
        }

        /// <summary>
        /// Creates a configuration provider for REST.
        /// If a provider was already created, the method will not create new but return existing.
        /// </summary>
        /// <returns>Configuration provider.</returns>
        public static IConfigProvider CreateForRest()
        {
            if (RestConfig != null)
            {
                return RestConfig;
            }

            string virtualPath = Path.Combine("~", "Rest", ConfigFileName);
            return new SACSConfigProvider(HostingEnvironment.MapPath(virtualPath));
        }
    }
}