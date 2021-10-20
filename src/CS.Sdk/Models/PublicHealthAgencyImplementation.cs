using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Public Health Agency Implementation
    /// </summary>
    public sealed class PublicHealthAgencyImplementation
    {
        /// <summary>
        /// Data Element Collected
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public ImplementationCollectionTypes Collected { get; set; }

        /// <summary>
        /// PHA Notes on Collection (e.g., If Only Collected For Certain Condition(s), Which Conditions, Discrepancy with Multiple Values)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Conditions { get; set; }

        /// <summary>
        /// PHA Mapped Data Element Description (e.g., Label in Surveillance System)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// PHA Local Value Set or Collection of Values
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string LocalValueSet { get; set; }

        /// <summary>
        /// PHA Value (Database Column Header and Table Name)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Value { get; set; }

        /// <summary>
        /// PHA Translation Needed (Yes / No)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public dynamic IsTranslationNeeded { get; set; }

        /// <summary>
        /// PHA Notes on Translations Needed (e.g., Details on What Translations Needed, Calculations)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string TranslationNotes { get; set; }

        /// <summary>
        /// PHA Extract Value (Data Extract Column Header and Table Name)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string ExtractValue { get; set; }

        /// <summary>
        /// PHA Notes on Data Alterations (If Any, What Alterations Were Done to the Data to Get it into the Extract)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string DataAlterationNotes { get; set; }

        /// <summary>
        /// PHA Follow-up Questions (To Be Resolved Prior to Onboarding)
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string FollowUpQuestions { get; set; }
    }
}