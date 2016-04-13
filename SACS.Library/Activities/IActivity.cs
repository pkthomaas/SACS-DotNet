using System.Threading.Tasks;
using SACS.Library.Workflow;

namespace SACS.Library.Activities
{
    /// <summary>
    /// The workflow activity
    /// </summary>
    public interface IActivity
    {
        /// <summary>
        /// Runs the activity asynchronously.
        /// </summary>
        /// <param name="sharedContext">The shared context.</param>
        /// <returns>Next activity to be run or <c>null</c> if last in flow.</returns>
        Task<IActivity> RunAsync(SharedContext sharedContext);
    }
}