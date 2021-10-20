using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace CS.Mmg
{
    /// <summary>
    /// Represents the status of a message mapping guide
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GuideStatus
    {
        /// <summary>
        /// For internal use only at this time
        /// </summary>
        [EnumMember(Value = "Development")]
        Development,

        /// <summary>
        /// Peer review
        /// </summary>
        [EnumMember(Value = "PeerReview")]
        PeerReview,

        /// <summary>
        /// Program and NMI approvals
        /// </summary>
        [EnumMember(Value = "Approvals")]
        Approvals,

        /// <summary>
        /// External review
        /// </summary>
        [EnumMember(Value = "ExternalReview")]
        ExternalReview,

        /// <summary>
        /// Pilot testing
        /// </summary>
        [EnumMember(Value = "PilotTesting")]
        PilotTesting,

        /// <summary>
        /// UAT
        /// </summary>
        [EnumMember(Value = "UserAcceptanceTesting")]
        UserAcceptanceTesting,

        /// <summary>
        /// Final
        /// </summary>
        [EnumMember(Value = "Final")]
        Final,
    }
}