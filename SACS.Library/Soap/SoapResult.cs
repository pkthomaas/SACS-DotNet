using System;
using SACS.Library.SabreSoapApi;

namespace SACS.Library.Soap
{
    /// <summary>
    /// The SOAP result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class SoapResult<TResult>
    {
        /// <summary>
        /// Gets the security token.
        /// </summary>
        /// <value>
        /// The security token.
        /// </value>
        public Security Security { get; private set; }

        /// <summary>
        /// Gets the result (response model).
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public TResult Result { get; private set; }

        /// <summary>
        /// Gets the exception that has occurred during request.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the request was cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the request was cancelled; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelled { get; private set; }

        /// <summary>
        /// Gets the type of the error.
        /// </summary>
        /// <value>
        /// The type of the error.
        /// </value>
        public ErrorType1 ErrorType { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the request has succeeded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the request has succeeded; otherwise, <c>false</c>.
        /// </value>
        public bool IsOk
        {
            get { return !this.IsCancelled && !this.ErrorOccurred; }
        }

        /// <summary>
        /// Gets a value indicating whether an error has occurred. 
        /// If <c>true</c>, then either Exception or ErrorType is set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if an error has occurred; otherwise, <c>false</c>.
        /// </value>
        public bool ErrorOccurred
        {
            get { return this.Exception != null || this.ErrorType != null; }
        }

        /// <summary>
        /// Creates a new success SOAP result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="security">The security token.</param>
        /// <returns>The SOAP result.</returns>
        public static SoapResult<TResult> Success(TResult result, Security security)
        {
            return new SoapResult<TResult>
            {
                Result = result,
                Security = security
            };
        }

        /// <summary>
        /// Creates a new success SOAP result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The SOAP result.</returns>
        public static SoapResult<TResult> Success(TResult result)
        {
            return new SoapResult<TResult>
            {
                Result = result
            };
        }

        /// <summary>
        /// Creates a new error SOAP result.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The SOAP result.</returns>
        public static SoapResult<TResult> Error(Exception exception)
        {
            return new SoapResult<TResult>
            {
                Exception = exception
            };
        }

        /// <summary>
        /// Creates a new cancelled SOAP result.
        /// </summary>
        /// <returns>The SOAP result.</returns>
        public static SoapResult<TResult> Cancelled()
        {
            return new SoapResult<TResult>
            {
                IsCancelled = true
            };
        }

        /// <summary>
        /// Creates a new SOAP result with an error type.
        /// </summary>
        /// <param name="errorType">Type of the error.</param>
        /// <returns>The SOAP result.</returns>
        public static SoapResult<TResult> Error(ErrorType1 errorType)
        {
            return new SoapResult<TResult>
            {
                ErrorType = errorType
            };
        }
    }
}