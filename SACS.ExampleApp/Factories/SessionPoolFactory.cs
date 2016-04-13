using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SACS.Library.Configuration;
using SACS.Library.Soap;

namespace SACS.ExampleApp.Factories
{
    /// <summary>
    /// The session pool static factory.
    /// </summary>
    public static class SessionPoolFactory
    {
        /// <summary>
        /// The default session pool size
        /// </summary>
        private const int DefaultSessionPoolSize = 10;

        /// <summary>
        /// Creates a session pool that caches sessions and can refresh them.
        /// </summary>
        /// <returns>Session pool.</returns>
        public static ISessionPool Create()
        {
            IConfigProvider config = ConfigFactory.CreateForSoap();
            var sessionPool = new SessionPool(new SoapAuth(new SoapServiceFactory(config)), DefaultSessionPoolSize);
            Task.Run(() => sessionPool.PopulateAsync());
            return sessionPool;
        }

        /// <summary>
        /// Creates a simple session pool.
        /// </summary>
        /// <returns>Simple session pool</returns>
        public static ISessionPool CreateSimple()
        {
            IConfigProvider config = ConfigFactory.CreateForSoap();
            return new SessionPoolSimple(new SoapAuth(new SoapServiceFactory(config)));
        }
    }
}