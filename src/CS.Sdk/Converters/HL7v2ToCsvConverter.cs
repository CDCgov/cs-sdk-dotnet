using CS.Sdk.Generators;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Sdk.Converters
{
    public sealed class HL7v2ToCsvConverter : IConverter
    {
        private readonly IConverter _converter = new HL7v2ToJsonConverter();
        private readonly IGenerator _generator = new MmgCsvTemplateGenerator();
        private readonly bool _shouldWriteHeaderRow = true;

        public HL7v2ToCsvConverter(IGenerator generator, IConverter hl7v2toJsonConverter, bool writeHeaders = true)
        {
            _shouldWriteHeaderRow = writeHeaders;
            _generator = generator;
            _converter = hl7v2toJsonConverter;
        }

        public ConversionResult Convert(string message, string transactionId = "")
        {
            if (message.StartsWith("BHS") || message.StartsWith("FHS"))
            {
                throw new InvalidOperationException("HL7v2 to CSV converter needs single messages, not batches. Use a debatcher class to send individual HL7v2 messages to this converter.");
            }
            else
            {
                return ConvertSingleMessage(message, transactionId);
            }
        }

        private ConversionResult ConvertSingleMessage(string hl7v2message, string transactionId)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            // Step 1 - Convert HL7v2 to Json
            ConversionResult jsonConversionResult = _converter.Convert(hl7v2message);
            string json = jsonConversionResult.Content;
            string profileIdentifier = jsonConversionResult.Profile;

            // Step 2 - Flatten the Json
            Dictionary<string, string> flatJsonDictionary = Common.FlattenJson(json);

            // Step 3 - Generate CSV headers
            string csvHeaders = _generator.Generate(profileIdentifier, string.Empty);

            // Step 4 - Set up CSV config
            Dictionary<string, int> columnIndexes = new Dictionary<string, int>();
            string[] columnHeaders = csvHeaders.Split(",");

            int startIndex = 0;
            foreach (string columnHeader in columnHeaders)
            {
                columnIndexes.Add(columnHeader, startIndex);
                startIndex++;
            }

            string[] csvRow = new string[columnHeaders.Length];

            foreach (KeyValuePair<string, string> kvp in flatJsonDictionary)
            {
                string key = kvp.Key;

                // Get the column index
                if (columnIndexes.ContainsKey(key))
                {
                    int index = columnIndexes[kvp.Key];

                    string cellContent = kvp.Value;

                    if (cellContent.Contains(","))
                    {
                        cellContent = "\"" + cellContent + "\"";
                    }

                    csvRow[index] = cellContent;
                }
            }

            string line = string.Join(",", csvRow);

            StringBuilder sb = new StringBuilder();

            if (_shouldWriteHeaderRow)
            {
                sb.AppendLine(csvHeaders);
            }
            sb.AppendLine(line);

            sw.Stop();

            ConversionResult conversionResult = new ConversionResult()
            {
                Elapsed = sw.Elapsed.TotalMilliseconds,
                Created = DateTime.Now,
                TransactionId = transactionId,
                Content = sb.ToString(),
                Profile = jsonConversionResult.Profile,
                BaseProfile = jsonConversionResult.BaseProfile,
                UniqueCaseId = jsonConversionResult.UniqueCaseId,
                Condition = jsonConversionResult.Condition,
                ConditionCode = jsonConversionResult.ConditionCode,
                NationalReportingJurisdiction = jsonConversionResult.NationalReportingJurisdiction,
                LocalRecordId = jsonConversionResult.LocalRecordId,
                MessageDateTime = jsonConversionResult.MessageDateTime,
            };

            return conversionResult;
        }
    }
}
