using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace MeerkatUpdater.Core.Config.Model.CustomExceptions
{
    /// <summary>
    /// Exception when fails to deserialize the configuration
    /// </summary>
    [Serializable]
    [ExcludeFromCodeCoverage]
    public class DeserealizationException : Exception
    {
        private const string BaseMassage = "Could not deserealize the configurations file";

        /// <summary>
        /// Constructor for add a custom message
        /// </summary>
        /// <param name="message"></param>
        public DeserealizationException(string message) : base($"{BaseMassage} {message}")
        {
        }

        /// <summary>
        /// Constructor for add a custom message and inner exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DeserealizationException(string message, Exception innerException) : base($"{BaseMassage} {message}", innerException)
        {
        }

        /// <summary>
        /// base constructor
        /// </summary>
        public DeserealizationException()
        {
        }

        /// <summary>
        /// Stream serealization constructo
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="streamingContext"></param>
        protected DeserealizationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}