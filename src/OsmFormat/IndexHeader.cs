//using System.Diagnostics;
//using System.Linq;

//namespace MSS.Tools.Pbf.IO.OsmFormat
//{
//    [DebuggerDisplay("{DebuggerDisplay,nq}")]
//    [global::ProtoBuf.ProtoContract(Name = "IndexHeader")]
//    public class IndexHeader : global::ProtoBuf.IExtensible
//    {
//        internal string GetDisplay(bool detailed)
//        {
//            string tls = string.Empty;
//            if (this.Tiles != null)
//            {
//                if (detailed)
//                {
//                    tls = string.Join("|", this.Tiles.Select(t1 => new Tile(t1).ToTraceString()));
//                }
//                else
//                {
//                    tls = string.Join("|", this.Tiles.Select(t1 => t1.ToString()));
//                }
//            }
//            return $"Type={(OsmEntityType)this.EntityType}, TileIds={tls}";
//        }

//        internal string DebuggerDisplay
//        {
//            get
//            {
//                return this.GetDisplay(detailed: false);
//            }
//        }

//        [global::ProtoBuf.ProtoMember(
//            1,
//            Name = "tiles", IsRequired = false,
//            DataFormat = global::ProtoBuf.DataFormat.Default
//            //Options = global::ProtoBuf.MemberSerializationOptions.Packed
//            )]
//        [global::System.ComponentModel.DefaultValue(null)]
//        public ulong[]? Tiles { get; set; }

//        private Tile[]? TileObjects = null;

//        private readonly object TileLockObject = new object();

//        internal Tile[]? GetTiles()
//        {
//            if (this.Tiles == null)
//            {
//                return null;
//            }
//            if (this.TileObjects != null)
//            {
//                return this.TileObjects;
//            }
//            lock (TileLockObject)
//            {
//                this.TileObjects = this.Tiles.Select(t => new Tile(t)).ToArray();
//            }
//            return this.TileObjects;
//        }

//        /// <summary>
//        /// OsmEntityType
//        /// </summary>
//        [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name = "entitytype", DataFormat = global::ProtoBuf.DataFormat.Default)]
//        public byte EntityType { get; set; }

//        private global::ProtoBuf.IExtension? extensionObject;

//        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
//        {
//            return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
//        }
//    }
//}
