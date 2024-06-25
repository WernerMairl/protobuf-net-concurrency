using System.Diagnostics;

namespace MSS.Tools.Pbf.IO.FileFormat
{
    /// <summary>
    /// Blob positions (lowest level) inside osm-pbf
    /// </summary>
    //[DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class BlobPosition
    {
        //internal string GetDebuggerDisplay(bool includeBlobType, bool detailedIndex, bool includeSize)
        //{
        //    var i = this.PrimitiveIndex as PrimitiveIndex;
        //    string sizeSuffix = string.Empty;
        //    if (includeSize)
        //    {
        //        sizeSuffix = " (" + this.Size.ToFormattedFileSize() + ")";
        //    }
        //    if (includeBlobType)
        //    {
        //        if (this.BlobType == BlobType.OSMIndex)
        //        {
        //            return $"{this.BlobType} at position {this.Position}".Trim() + sizeSuffix;
        //        }
        //        else
        //        {
        //            if (i == null)
        //            {
        //                return $"{this.BlobType} at position {this.Position} (no PrimitiveIndex)".Trim() + sizeSuffix;
        //            }
        //            else
        //            {
        //                return $"{this.BlobType} at position {this.Position} with PrimitiveIndex {i?.GetDisplay(detailed: detailedIndex)}".Trim() + sizeSuffix;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (i == null)
        //        {
        //            return $"{this.Position} (no PrimitiveIndex)".Trim() + sizeSuffix;
        //        }
        //        else
        //        {
        //            return $"{this.Position} with PrimitiveIndex {i?.GetDisplay(detailed: detailedIndex)}".Trim() + sizeSuffix;
        //        }
        //    }
        //}

        //internal string DebuggerDisplay
        //{
        //    get
        //    {
        //        return this.GetDebuggerDisplay(true, false, true);
        //    }
        //}

        public BlobHeader GetBlobHeaderOrDefault(BlobHeader defaultValue)
        {
            BlobHeader blobHeader = defaultValue;
            if (this.Header.Header != null)
            {
                blobHeader = this.Header.Header as BlobHeader ?? defaultValue;
            }
            return blobHeader;
        }

        /// <summary>
        /// can be casted into PrimitiveIndex
        /// </summary>
        public ProtoBuf.IExtensible? PrimitiveIndex { get; set; }

        /// <summary>
        /// can be casted into IndexHeader
        /// </summary>
        public ProtoBuf.IExtensible? IndexHeader { get; set; }

        public BlobHeaderPosition Header { get; set; }

        public BlobPosition? IndexPosition { get; set; }

        public BlobPosition(long position, long size, BlobType blobType, BlobHeaderPosition headerPosition)
        {
            this.Position = position;
            this.Header = headerPosition;
            this.Size = size;
            this.BlobType = blobType;
            this.IndexPosition = null;
        }

        public long Position { get; }

        public long Size { get; }

        /// <summary>
        /// 0...Unknown
        /// 1...OSMHeader
        /// 2...OSMData
        /// 3...index 3 inside HeaderType list...
        /// </summary>
        public BlobType BlobType { get; set; }
    }
}
