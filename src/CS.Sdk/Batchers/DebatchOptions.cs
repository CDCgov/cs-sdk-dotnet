using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CS.Sdk.Batchers
{
    /// <summary>
    /// Options for configuring debatchers
    /// </summary>
    [DebuggerDisplay("HL7v2 Debatch Options")]
    public sealed class DebatchOptions
    {
        public bool DoPreProcessSanityChecks { get; private set; }

        public bool CheckFileSegments { get; private set; }

        public bool CheckBatchSegments { get; private set; }

        public bool CheckBatchTrailerCountAgainstActualCount { get; private set; }

        public bool CheckBatchForEmptiness { get; private set; }

        public DebatchOptions(
            bool doPreProcessSanityChecks = true,
            bool checkFileSegments = true,
            bool checkBatchSegments = true,
            bool checkBatchTrailerCountAgainstActualCount = true,
            bool checkBatchForEmptiness = true)
        {
            DoPreProcessSanityChecks = doPreProcessSanityChecks;
            CheckFileSegments = checkFileSegments;
            CheckBatchSegments = checkBatchSegments;
            CheckBatchTrailerCountAgainstActualCount = checkBatchTrailerCountAgainstActualCount;
            CheckBatchForEmptiness = checkBatchForEmptiness;
        }
    }
}