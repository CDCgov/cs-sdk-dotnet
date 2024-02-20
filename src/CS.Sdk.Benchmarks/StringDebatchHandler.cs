
using System.Buffers;
using System.IO;
using System.Text;
using CS.Sdk.Batchers;

namespace CS.Sdk.Benchmarks
{
    public class StringDebatchHandler : IDebatchHandler
    {
        private readonly ReadOnlySpanAction<char, MessageDebatchMetadata> _callback;


        public StringDebatchHandler(ReadOnlySpanAction<char, MessageDebatchMetadata> callback)
        {
            _callback = callback;
        }

        public MessageHandleMetadata HandleDebatch(ReadOnlySpan<char> hl7v2message, MessageDebatchMetadata metadata)
        {
            _callback(hl7v2message, metadata);
            return default(MessageHandleMetadata);
        }
    }
}