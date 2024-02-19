using System.IO;
using CS.Sdk.Batchers;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace CS.Sdk.Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class DebatcherBenchmarks100
    {
        private static DebatchOptions options = new DebatchOptions(
                doPreProcessSanityChecks: false,
                checkFileSegments: false,
                checkBatchSegments: false,
                checkBatchTrailerCountAgainstActualCount: false,
                checkBatchForEmptiness: false
            );

        [Benchmark]
        public void BenchmarkSpanDebatcher100()
        {
            StringDebatchHandler handler = new StringDebatchHandler(Callback_Debatcher_Test);
            string batch100 = File.ReadAllText("data/hl7v2-batch-100.hl7");
            HL7v2Debatcher debatcher = new HL7v2Debatcher();
            DebatchResult result = debatcher.Debatch(
                hl7v2batch: batch100,
                options: options,
                debatchHandler: handler,
                transactionId: "1");
        }

        [Benchmark]
        public void BenchmarkStreamingDebatcher100()
        {
            StringDebatchHandler handler = new StringDebatchHandler(Callback_Debatcher_Test);

            Stream fs = File.OpenRead("data/hl7v2-batch-100.hl7");

            HL7v2StreamingDebatcher debatcher = new HL7v2StreamingDebatcher();
            DebatchResult result = debatcher.DebatchAsync(fs, /*options, */handler, "1").Result;

        }

        private static void Callback_Debatcher_Test(ReadOnlySpan<char> message, MessageDebatchMetadata metadata)
        {
            string payload = message.ToString();
        }
    }
}