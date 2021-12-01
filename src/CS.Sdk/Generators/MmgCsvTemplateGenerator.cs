using System.Linq;
using System.Text;
using CS.Mmg;
using CS.Sdk.Services;

namespace CS.Sdk.Generators
{
    /// <summary>
    /// Class used to generate a CSV header based on the content of a message mapping guide
    /// </summary>
    public sealed class MmgCsvTemplateGenerator : IGenerator
    {
        private readonly IMmgService _mmgService = new InMemoryMmgService();

        public MmgCsvTemplateGenerator()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mmgService">Service for retrieving message mapping guides (MMGs)</param>
        public MmgCsvTemplateGenerator(IMmgService mmgService)
        {
            _mmgService = mmgService;
        }

        public string Generate(string mmgIdentifier, string conditionCode)
        {
            MessageMappingGuide mmg = _mmgService.Get(mmgIdentifier, conditionCode);

            StringBuilder sb = new StringBuilder("");

            sb.Append("source_format,");
            sb.Append("datetime_of_message,");
            sb.Append("unique_case_id,");
            sb.Append("message_profile_identifier,");

            foreach (var block in mmg.Blocks
                .Where(b => b.Type != BlockType.Info)
                .Where(b => b.Elements.Count > 0))
            {
                string columnNameBlockPrefix = string.Empty;
                int blockInstances = 1;

                if (block.Type != BlockType.Single)
                {
                    blockInstances = 5;
                    columnNameBlockPrefix = Common.FormatDataElementName(block.Name);
                }

                for (int i = 0; i < blockInstances; i++)
                {
                    foreach (var element in block.Elements)
                    {
                        string columnName = Common.FormatDataElementName(element.Name);

                        if (columnName.Equals("message_profile_identifier"))
                        {
                            // special case for MSH-21 stuff
                            columnName = "message_profile_identifiers";
                        }

                        if (block.Type != BlockType.Single)
                        {
                            columnName = columnNameBlockPrefix + $"[{i}]." + columnName;
                        }

                        if (columnName.Equals("race_category") || (element.Repetitions.HasValue && element.Repetitions.Value > 1))
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                string repeatString = $"[{j}]";
                                sb.Append(columnName + repeatString);
                                sb.Append(",");

                                if (element.DataType == DataType.Coded)
                                {
                                    sb.Append(columnName + "__code" + repeatString);
                                    sb.Append(",");

                                    sb.Append(columnName + "__code_system" + repeatString);
                                    sb.Append(",");
                                }
                            }
                        }
                        else
                        {
                            sb.Append(columnName);
                            sb.Append(",");

                            if (element.DataType == DataType.Coded)
                            {
                                sb.Append(columnName + "__code");
                                sb.Append(",");

                                sb.Append(columnName + "__code_system");
                                sb.Append(",");
                            }
                        }
                    }
                }
            }

            string headerRow = sb.ToString().TrimEnd(',');
            return headerRow;
        }
    }
}
