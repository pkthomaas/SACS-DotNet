using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SACS.Library.Configuration;
using SACS.Library.Rest;

namespace SACS.ExampleApp.Factories
{
    /// <summary>
    /// The REST client static factory.
    /// </summary>
    public static class RestClientFactory
    {
        /// <summary>
        /// Creates a REST client.
        /// </summary>
        /// <returns>REST client</returns>
        public static RestClient Create()
        {
            IConfigProvider config = ConfigFactory.CreateForRest();
            RestAuthorizationManager restAuthorizationManager = new RestAuthorizationManager(config);
            return new RestClient(restAuthorizationManager, config);
        }
    }
}