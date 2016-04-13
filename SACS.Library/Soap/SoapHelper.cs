using System.ComponentModel;
using System.Threading.Tasks;

namespace SACS.Library.Soap
{
    /// <summary>
    /// The helper methods for SOAP calls.
    /// </summary>
    public static class SoapHelper
    {
        /// <summary>
        /// Handles the errors in asynchronous calls. 
        /// If an error has occurred, sets a result in the task completion source containing information about the problem:
        /// either the Exception that has occurred or the cancellation flag.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="args">The <see cref="AsyncCompletedEventArgs"/> instance containing the event data.</param>
        /// <param name="source">The <see cref="TaskCompletionSource"/> instance related to the SOAP result.</param>
        /// <returns><c>true</c> if no errors have occurred; otherwise <c>false</c></returns>
        public static bool HandleErrors<TResult>(AsyncCompletedEventArgs args, TaskCompletionSource<SoapResult<TResult>> source)
        {
            if (args.Error != null)
            {
                source.TrySetResult(SoapResult<TResult>.Error(args.Error));
                return false;
            }
            else if (args.Cancelled)
            {
                source.TrySetResult(SoapResult<TResult>.Cancelled());
                return false;
            }

            return true;
        }
    }
}