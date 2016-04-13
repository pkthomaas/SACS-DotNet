using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Library.Activities.InputData;
using SACS.Library.Soap;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The initial activity for SOAP workflow. Sets up the data to be used by next activities.
    /// </summary>
    public class InitialSoapActivity : IActivity
    {
        /// <summary>
        /// The SOAP service factory
        /// </summary>
        private readonly SoapServiceFactory soapServiceFactory;

        /// <summary>
        /// The session pool
        /// </summary>
        private readonly ISessionPool sessionPool;

        /// <summary>
        /// The input data
        /// </summary>
        private readonly ISoapWorkflowData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialSoapActivity" /> class.
        /// </summary>
        /// <param name="soapServiceFactory">The SOAP service factory.</param>
        /// <param name="sessionPool">The session pool.</param>
        /// <param name="data">The data.</param>
        public InitialSoapActivity(SoapServiceFactory soapServiceFactory, ISessionPool sessionPool, ISoapWorkflowData data)
        {
            this.soapServiceFactory = soapServiceFactory;
            this.sessionPool = sessionPool;
            this.data = data;
        }

        /// <summary>
        /// Runs the activity asynchronously.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>
        /// Next activity to be run or <c>null</c> if last in flow.
        /// </returns>
        public Task<IActivity> RunAsync(SharedContext sharedContext)
        {
            sharedContext.AddResult(BargainFinderMaxSoapActivity.InputDataSharedContextKey, this.data);
            sharedContext.AddResult(PassengerDetailsContactActivity.InputDataSharedContextKey, this.data);
            sharedContext.AddResult(PassengerDetailsAgencyActivity.InputDataSharedContextKey, this.data);
            IActivity nextActivity = new BargainFinderMaxSoapActivity(this.soapServiceFactory, this.sessionPool);
            return Task.FromResult(nextActivity);
        }
    }
}
