using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace CS.Mmg
{
    /// <summary>
    /// Message Mapping Guide V3
    /// </summary>
    [DebuggerDisplay("{ShortName} : {Name} : v{InternalVersion}")]
    public class MessageMappingGuide
    {
        /// <summary>
        /// MMG ID field (GUID)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Guide type currently guide or template
        /// </summary>
        public GuideType Type { get; set; }

        /// <summary>
        /// Gets/sets the state of this message mapping guide
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public GuideStatus? GuideStatus { get; set; }

        /// <summary>
        /// Gets/sets the guide's template status.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public TemplateStatus? TemplateStatus { get; set; }

        /// <summary>
        /// Jurisdiction field, optional as only appears in jurisdiction configs
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public Jurisdiction? Jurisdiction { get; set; }

        /// <summary>
        /// MMG Name field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets/sets the guide's short name, Example: CONSYPH
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets/sets the guide's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets/sets whether this MMG is active or inactive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets/sets the user ID of the person who created this entity (3 alpha characters and 1 number)
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets/sets the user ID of the person who currently owns entity (3 alpha characters and 1 number)
        /// </summary>
        public Guid OwnedBy { get; set; }

        /// <summary>
        /// Gets/sets the internal version of the entity.
        /// </summary>
        public int InternalVersion { get; set; }

        /// <summary>
        /// Gets/sets the date this entity was created
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets/sets the date this entity was last updated
        /// </summary>
        public DateTime LastUpdatedDate { get; set; }

        /// <summary>
        /// Leave this string empty for entities that are not published.
        /// </summary>
        public string PublishVersion { get; set; }

        /// <summary>
        /// Leave this null for entities that are not published.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public DateTime? PublishDate { get; set; }

        /// <summary>
        /// A list of published versions.
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<PublishInfo> Published { get; set; }

        /// <summary>
        /// Gets/sets the MSH-21 profile identifier string for this MMG
        /// </summary>
        public string ProfileIdentifier { get; set; }

        /// <summary>
        /// Gets/sets the collection of blocks that are used in this guide
        /// </summary>
        public List<Block> Blocks { get; set; } = new List<Block>();

        /// <summary>
        /// Gets the collection of data elements that are used in this guide
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DataElement> Elements
        {
            get
            {
                

                return _elements;
            }
        }

        [JsonIgnore]
        private Dictionary<Guid, List<DataElement>> RelatedElements
        {
            get
            {
                return _relatedElements;
            }
        }

        public MessageMappingGuide() { }

        [JsonConstructor]
        public MessageMappingGuide(List<Block> blocks)
        {
            Blocks = blocks;

            if (_elements.Count == 0)
            {
                List<DataElement> elements = new List<DataElement>(300);

                foreach (var block in Blocks)
                {
                    foreach (var element in block.Elements)
                    {
                        elements.Add(element);

                        if (!_relatedElements.ContainsKey(element.Id))
                        {
                            _relatedElements.Add(element.Id, new List<DataElement>());
                        }
                    }
                }
                _elements = elements;
            }
            
            foreach (var element in _elements)
            {
                if (element.RelatedElementId.HasValue && _relatedElements.ContainsKey(element.RelatedElementId.Value))
                {
                    _relatedElements[element.RelatedElementId.Value].Add(element);
                }
            }

            if (_elementIdentifierDictionary.Count == 0)
            {
                var elementIdentifierDictionary = new Dictionary<string, DataElement>(Elements.Count());

                foreach (var element in Elements)
                {
                    var mapping = element.Mappings.Hl7v251;

                    var id = mapping.Identifier;
                    var legacyId = mapping.LegacyIdentifier;

                    if (legacyId == "NOT115") continue;

                    if (!id.StartsWith("N/A"))
                    {
                        if (elementIdentifierDictionary.ContainsKey(mapping.Identifier))
                        {
                            Console.WriteLine("DUPLICATE: " + mapping.Identifier);
                            continue;
                        }
                        elementIdentifierDictionary.Add(mapping.Identifier, element);
                    }

                    if (legacyId != id && !legacyId.StartsWith("N/A") && !elementIdentifierDictionary.ContainsKey(legacyId))
                    {
                        elementIdentifierDictionary.Add(legacyId, element);
                    }
                }
                _elementIdentifierDictionary = elementIdentifierDictionary;
            }
        }

        public List<DataElement> GetChildRelationships(DataElement element)
        {
            var children = RelatedElements[element.Id];
            return children;
        }

        [JsonIgnore]
        public Dictionary<string, DataElement> ElementIdentifierDictionary
        {
            get
            {
                

                return _elementIdentifierDictionary;
            }
        }

        /// <summary>
        /// Gets/sets the collection of test scenarios that are used in this guide.
        /// </summary>
        public List<TestScenario> TestScenarios { get; set; } = new List<TestScenario>();

        /// <summary>
        /// Gets/sets the collection of analyzed messages
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<AnalysisReport> AnalysisReports { get; set; }

        /// <summary>
        /// A List of Columns for viewing the test case scenario worksheet
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<DataColumn> TestCaseScenarioWorksheetColumns { get; set; }

        /// <summary>
        /// A List of Columns for viewing the MMG
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<DataColumn> Columns { get; set; }

        /// <summary>
        /// List of templates by their ID as aggregated from the blocks
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<TemplateInfo> Templates { get; set; }

        /// <summary>
        /// Aggregate of all VADS value sets pulled from the elements
        /// </summary>
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public List<VADS.ValueSetWithConcept> ValueSets { get; set; }

        /// <summary>
        /// GenV2 Guid
        /// </summary>
        static public Guid GetGenV2Id()
        {
            return new Guid("88e35837-86b3-4166-ade9-ee4bc2b9bee3");
        }

        private readonly List<DataElement> _elements = new List<DataElement>();
        private readonly Dictionary<string, DataElement> _elementIdentifierDictionary = new Dictionary<string, DataElement>();
        private readonly Dictionary<Guid, List<DataElement>> _relatedElements = new Dictionary<Guid, List<DataElement>>();

        public bool TryGetElementByHl7Identifier(string identifier, out DataElement element)
        {
            if (ElementIdentifierDictionary.TryGetValue(identifier, out element))
            {
                return true;
            }
            return false;
        }
    }
}