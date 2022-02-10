using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CS.Sdk.Converters
{
    /// <summary>
    /// Class representing the result of conversion from one data format to Json
    /// </summary>
    [DebuggerDisplay("{Profile} : {Errors} errors, {Warnings} : {Condition} msg from {NationalReportingJurisdiction} : {Elapsed} ms")]
    public sealed class ConversionResult
    {
        private const string UNKNOWN = "unknown";

        /// <summary>
        /// The transaction Id for the data processing job for which this conversion outcome was a part
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// List of conversion errors and warnings
        /// </summary>
        public List<ProcessResultMessage> ConversionMessages { get; set; } = new List<ProcessResultMessage>();

        /// <summary>
        /// Time the conversion took in milliseconds
        /// </summary>
        public double Elapsed { get; set; } = 0.0;

        /// <summary>
        /// Timestamp for when this conversion result was created
        /// </summary>
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Timestamp for the message's date/time
        /// </summary>
        public DateTimeOffset? MessageDateTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The converted representation of the case notification message
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Base profile for the case notification
        /// </summary>
        public string BaseProfile { get; set; } = UNKNOWN;

        /// <summary>
        /// Main, higher-level profile for the case notification
        /// </summary>
        public string Profile { get; set; } = UNKNOWN;

        /// <summary>
        /// The condition the message is for
        /// </summary>
        public string Condition { get; set; } = UNKNOWN;

        /// <summary>
        /// The condition code the message is for
        /// </summary>
        public string ConditionCode { get; set; } = UNKNOWN;

        /// <summary>
        /// The national reporting jurisdiction within the U.S. that this message was sent from
        /// </summary>
        public string NationalReportingJurisdiction { get; set; } = string.Empty;

        /// <summary>
        /// The local record ID of the message
        /// </summary>
        public string LocalRecordId { get; set; } = string.Empty;

        /// <summary>
        /// The unique case ID of this message
        /// </summary>
        public string UniqueCaseId { get; set; } = string.Empty;

        /// <summary>
        /// Whether the conversion was successful
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return !(ConversionMessages.Any(m => m.Severity == Severity.Error));
            }
        }

        /// <summary>
        /// The number of errors encountered during the conversion
        /// </summary>
        public int Errors
        {
            get
            {
                return ConversionMessages.Count(m => m.Severity == Severity.Error);
            }
        }

        /// <summary>
        /// The number of warnings encountered during the conversion
        /// </summary>
        public int Warnings
        {
            get
            {
                return ConversionMessages.Count(m => m.Severity == Severity.Warning);
            }
        }

        /// <summary>
        /// The number of notable things encountered during the conversion that are not errors or warnings
        /// </summary>
        public int Others
        {
            get
            {
                return ConversionMessages.Count(m => m.Severity == Severity.Information);
            }
        }
    }
}
