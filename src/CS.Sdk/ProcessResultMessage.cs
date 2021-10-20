using System;
using System.Diagnostics;

namespace CS.Sdk
{
    /// <summary>
    /// An error or warning that occurred during a processing operation
    /// </summary>
    [DebuggerDisplay("{Severity} : {Content}")]
    public sealed class ProcessResultMessage
    {
        /// <summary>
        /// Unique ID of this processing result message
        /// </summary>
        public Guid Id { get; private set; } = System.Guid.NewGuid();

        /// <summary>
        /// Error code associated with this message, if any
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// The content of this message. Typically this will be a human-readable explanation for what went wrong.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The severity associated with this message
        /// </summary>
        public Severity Severity { get; set; } = Severity.Information;
    }
}