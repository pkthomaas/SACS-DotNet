using System;
using System.Net;

namespace SACS.Library.Rest
{
    /// <summary>
    /// The REST authentication token holder.
    /// </summary>
    public class TokenHolder
    {
        /// <summary>
        /// Gets the authentication token string.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; private set; }

        /// <summary>
        /// Gets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime ExpirationDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the token is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the token is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the error HTTP status code obtained during authentication call.
        /// </summary>
        /// <value>
        /// The error status code.
        /// </value>
        public HttpStatusCode ErrorStatusCode { get; private set; }

        /// <summary>
        /// Gets the error message obtained during authentication call.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Creates a new empty token holder.
        /// </summary>
        /// <returns>The token holder.</returns>
        public static TokenHolder Empty()
        {
            return new TokenHolder
            {
                ExpirationDate = DateTime.MinValue,
                IsValid = false,
                Token = null
            };
        }

        /// <summary>
        /// Creates a new token holder.
        /// </summary>
        /// <param name="token">The token string.</param>
        /// <param name="expirationSeconds">The expiration time in seconds.</param>
        /// <returns>The token holder.</returns>
        public static TokenHolder Valid(string token, int expirationSeconds)
        {
            return new TokenHolder
            {
                ExpirationDate = DateTime.Now.AddSeconds(expirationSeconds),
                IsValid = true,
                Token = token
            };
        }

        /// <summary>
        /// Creates a new empty token holder when the authentication call has failed.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code obtained during authentication call.</param>
        /// <param name="message">The response message.</param>
        /// <returns>The token holder.</returns>
        public static TokenHolder Invalid(HttpStatusCode httpStatusCode, string message)
        {
            var tokenHolder = Empty();
            tokenHolder.ErrorStatusCode = httpStatusCode;
            tokenHolder.ErrorMessage = message;
            return tokenHolder;
        }
    }
}