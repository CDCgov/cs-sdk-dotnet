using System.Collections.Generic;

namespace CS.Mmg
{
    /// <summary>
    /// Represents a test scenario block value
    /// </summary>
    public sealed class TestBlock
    {
        /// <summary>
        /// This is an array of all the instances in this repeating group
        /// </summary>
        public List<IDictionary<string, TestDataElement>> Instances { get; set; }
    }
}