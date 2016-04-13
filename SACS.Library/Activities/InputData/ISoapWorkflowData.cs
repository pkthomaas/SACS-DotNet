using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SACS.Library.Activities.InputData
{
    /// <summary>
    /// The data used in calls in the SOAP workflow.
    /// </summary>
    public interface ISoapWorkflowData : IPassengerDetailsActivityData, IBargainFinderMaxData
    {
    }
}
