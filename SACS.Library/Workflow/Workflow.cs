using System;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Repository.Hierarchy;
using SACS.Library.Activities;

namespace SACS.Library.Workflow
{
    /// <summary>
    /// The workflow (a sequence of activities running one after another with a shared context).
    /// </summary>
    public class Workflow
    {
        /// <summary>
        /// The alphabet
        /// </summary>
        private const string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(Workflow));

        /// <summary>
        /// The random number generator
        /// </summary>
        private static readonly Random Rand = new Random();

        /// <summary>
        /// The start activity
        /// </summary>
        private readonly IActivity startActivity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Workflow"/> class.
        /// </summary>
        /// <param name="startActivity">The start activity.</param>
        public Workflow(IActivity startActivity)
        {
            this.startActivity = startActivity;
        }

        /// <summary>
        /// Runs the whole workflow asynchronously.
        /// First creates a shared context and runs the first activity.
        /// Then runs the returned activity, etc., until the context is faulty.
        /// Last activity in the context should return null.
        /// </summary>
        /// <returns>The shared context.</returns>
        public async Task<SharedContext> RunAsync()
        {
            Log.Debug("Running workflow with the start activity: " + this.startActivity.GetType().ToString());
            IActivity activity = this.startActivity;
            string longRandom = new string(Enumerable.Range(0, 8).Select(i => Alphabet[Rand.Next(Alphabet.Length)]).ToArray());
            string conversationId = DateTime.Now.ToString("YYYYMMddhhmmss") + "-" + longRandom;
            SharedContext sharedContext = new SharedContext(conversationId);
            while (activity != null && !sharedContext.IsFaulty)
            {
                try
                {
                    activity = await activity.RunAsync(sharedContext);
                }
                catch (Exception ex)
                {
                    Log.Debug("Error in activity " + activity.GetType().ToString(), ex);
                    sharedContext.IsFaulty = true;
                }
            }

            return sharedContext;
        }
    }
}