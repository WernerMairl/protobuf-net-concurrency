using System.Threading;

namespace PerfDemo
{
    public static class OsmProtoBufMetadataFactory
    {
        /// <summary>
        /// compiled default Instance!
        /// </summary>
        public static ProtoBufTypeInfo DefaultInstance { get; } = Create(compiled: true);

        private static ProtoBufTypeInfo ThreadLocalFactory()
        {
            return Create(compiled: true);
        }

        public static ThreadLocal<ProtoBufTypeInfo> ThreadLocalInstance { get; } = new ThreadLocal<ProtoBufTypeInfo>(ThreadLocalFactory);

        public static ProtoBufTypeInfo Create(bool compiled)
        {
            var m = new ProtoBufTypeInfo(compiled);
            return m;
        }
    }
}
