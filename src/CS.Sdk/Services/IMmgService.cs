using CS.Mmg;

namespace CS.Sdk.Services
{
    /// <summary>
    /// Interface for interacting with an MMG service that can be
    /// used to aid in message content validation.
    /// </summary>
    public interface IMmgService
    {
        /// <summary>
        /// Gets a message mapping guide
        /// </summary>
        /// <param name="profileIdentifier">The profile identifier for the MMG</param>
        /// <param name="conditionCode">The condition code</param>
        /// <returns>Message Mapping Guide</returns>
        MessageMappingGuide Get(string profileIdentifier, string conditionCode);
    }
}
