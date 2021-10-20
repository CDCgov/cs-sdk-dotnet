using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CS.Sdk.Validators
{
    /// <summary>
    /// Class representing the outcome of validating a case notification
    /// </summary>
    [DebuggerDisplay("{Profile} : {Errors} errors, {Warnings} : {Condition} msg from {NationalReportingJurisdiction} : {Elapsed} ms")]
    public sealed class ValidationResult
    {
        /// <summary>
        /// The transaction Id for the data processing job for which this validation outcome was a part
        /// </summary>
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// List of validation errors and warnings
        /// </summary>
        public List<ValidationMessage> ValidationMessages { get; set; } = new List<ValidationMessage>();

        /// <summary>
        /// Time the validation took in milliseconds
        /// </summary>
        public double Elapsed { get; set; } = 0.0;

        /// <summary>
        /// Timestamp when this validation result object was created
        /// </summary>
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// Timestamp of when the message was created
        /// </summary>
        public DateTimeOffset MessageCreated { get; set; } = DateTimeOffset.MinValue;

        /// <summary>
        /// Main, higher-level profile for the case notification
        /// </summary>
        public string Profile { get; set; } = string.Empty;

        /// <summary>
        /// The local record ID of the message
        /// </summary>
        public string LocalRecordId { get; set; } = string.Empty;

        /// <summary>
        /// The condition the message is for
        /// </summary>
        public string Condition { get; set; } = string.Empty;

        /// <summary>
        /// The condition code the message is for
        /// </summary>
        public string ConditionCode { get; set; } = string.Empty;

        /// <summary>
        /// The national reporting jurisdiction within the U.S. that this message was sent from
        /// </summary>
        public string NationalReportingJurisdiction { get; set; } = string.Empty;

        /// <summary>
        /// The unique case ID of this message
        /// </summary>
        public string UniqueCaseId { get; set; } = string.Empty;

        /// <summary>
        /// Whether the validation was a success
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return !(ValidationMessages.Any(m => m.Severity == Severity.Error));
            }
        }

        /// <summary>
        /// Number of errors generated during the validation
        /// </summary>
        public int Errors
        {
            get
            {
                return ValidationMessages.Count(m => m.Severity == Severity.Error);
            }
        }

        /// <summary>
        /// Number of warnings generated during the validation
        /// </summary>
        public int Warnings
        {
            get
            {
                return ValidationMessages.Count(m => m.Severity == Severity.Warning);
            }
        }

        /// <summary>
        /// Number of informational (non-error, non-warning) events that were generated during the validation
        /// </summary>
        public int Others
        {
            get
            {
                return ValidationMessages.Count(m => m.Severity == Severity.Information);
            }
        }
    }
}
