using System;

namespace CS.Sdk.Batchers
{
    /// <summary>
    /// Interface for handling what happens when an individual message is debatched from a batch payload
    /// </summary>
    public interface IDebatchHandler
    {
        /// <summary>
        /// Method that will be called every time an HL7v2 message is debatched
        /// </summary>
        /// <param name="hl7v2message">The debatched HL7v2 message</param>
        /// <param name="metadata">Metadata about both the HL7v2 batch and how this specific HL7v2 message was debatched</param>
        /// <returns>Metadata about how the handling operation went</returns>
        MessageHandleMetadata HandleDebatch(ReadOnlySpan<char> hl7v2message, MessageDebatchMetadata metadata);
    }
}
