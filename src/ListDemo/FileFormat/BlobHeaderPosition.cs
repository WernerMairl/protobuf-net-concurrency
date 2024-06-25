namespace MSS.Tools.Pbf.IO.FileFormat
{
    public sealed class BlobHeaderPosition
    {
        public BlobHeaderPosition(long position, long size)
        {
            this.Position = position;
            this.Size = size;
        }

        public long Position { get; }
        public long Size { get; }

        /// <summary>
        /// BlobHeader/OSMHeader as Protobuf object
        /// can be casted into BlobHeader
        /// </summary>
        public ProtoBuf.IExtensible? Header { get; set; }
    }
}
