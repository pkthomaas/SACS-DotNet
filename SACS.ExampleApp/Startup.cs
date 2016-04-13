using System.Net;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SACS.ExampleApp.Startup))]

namespace SACS.ExampleApp
{
    /// <summary>
    /// The startup class.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configures the application.
        /// Sets the TLS 1.2 as default security protocol.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}