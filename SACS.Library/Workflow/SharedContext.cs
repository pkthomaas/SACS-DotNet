using System.Collections.Generic;

namespace SACS.Library.Workflow
{
    /// <summary>
    /// The shared context using to pass information between activities and returning results from last activity in a flow.
    /// </summary>
    public class SharedContext
    {
        /// <summary>
        /// The results dictionary (mapping between string keys and results).
        /// </summary>
        private readonly Dictionary<string, object> results = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SharedContext"/> class.
        /// </summary>
        /// <param name="conversationId">The conversation identifier.</param>
        public SharedContext(string conversationId)
        {
            this.ConversationId = conversationId;
        }

        /// <summary>
        /// Gets the conversation identifier.
        /// </summary>
        /// <value>
        /// The conversation identifier.
        /// </value>
        public string ConversationId { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the context is faulty (e.g. a request has failed).
        /// </summary>
        /// <value>
        ///   <c>true</c> if the context is faulty; otherwise, <c>false</c>.
        /// </value>
        public bool IsFaulty { get; set; }

        /// <summary>
        /// Adds the result to the context.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        public void AddResult(string key, object result)
        {
            this.results.Add(key, result);
        }

        /// <summary>
        /// Gets the result from the context.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The result.</returns>
        public object GetResult(string key)
        {
            object value;
            if (this.results.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the result from the content and casts it to the specified type.
        /// Returns null if the key doesn't exist or type is wrong.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The result.</returns>
        public TResult GetResult<TResult>(string key)
            where TResult : class
        {
            object value;
            if (this.results.TryGetValue(key, out value))
            {
                return value as TResult;
            }
            else
            {
                return null;
            }
        }
    }
}