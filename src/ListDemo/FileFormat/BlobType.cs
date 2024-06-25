namespace MSS.Tools.Pbf.IO.FileFormat
{
    public enum BlobType
    {
        Unknown = 0,
        OSMHeader = 1,
        OSMData = 2,
        /// <summary>
        /// number should not matter, it is not used inside the file
        /// headertype is encoded as string there
        /// </summary>
        OSMIndex = 3,
    }
}
