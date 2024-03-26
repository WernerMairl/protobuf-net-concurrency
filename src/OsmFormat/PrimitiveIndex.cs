//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Diagnostics;
//using System.Linq;

//namespace MSS.Tools.Pbf.IO.OsmFormat
//{
//    /// <summary>
//    /// Implements the PrimitiveIndex feature:
//    /// a list of Tile ID's and a list of Entity-ID's and the type of Entity (only one entity-type is allowed)
//    /// means: this PrimitiveIndex Item contains (=entityType) Nodes with the following ID's, and the nodes are located on the following Tiles (typically a unique or small selection)
//    /// Nodes are located on one tile, ways and relations may be located on multiple tiles.
//    /// PrimitiveIndex-Objects from type node typically have only ONE TileID
//    /// </summary>
//    [DebuggerDisplay("{DebuggerDisplay,nq}")]
//    [global::ProtoBuf.ProtoContract(Name = "PrimitiveIndex")]
//    public class PrimitiveIndex : global::ProtoBuf.IExtensible
//    {
//        private IReadOnlySet<long>? EntitySet;

//        private readonly object SetLockObject = new object();

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
//                tls = "Tiles=" + tls;
//            }
//            return $"{(OsmEntityType)this.EntityType} Index, {this.Entities?.Length} entities, {tls}";
//        }

//        internal string DebuggerDisplay
//        {
//            get
//            {
//                return this.GetDisplay(detailed: false);
//            }
//        }

//        internal bool HasAnyEntity(IEnumerable<long> subjects)
//        {
//            if (this.Entities == null)
//            {
//                throw new InvalidOperationException("Entities!!!");
//            }
//            var subjectsSet = subjects as IReadOnlySet<long>;
//            if (subjectsSet != null)
//            {
//                return subjectsSet.Overlaps(this.Entities!);
//            }
//            IReadOnlySet<long> entitySet;
//            lock (SetLockObject)
//            {
//                this.EntitySet = this.EntitySet ?? this.Entities!.ToImmutableHashSet<long>();
//                entitySet = this.EntitySet;
//            }
//            return entitySet.Overlaps(subjects);
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

//        public bool IsEntityType(OsmEntityType type)
//        {
//            return this.EntityType == (byte)type;
//        }

//        [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name = "entitytype", DataFormat = global::ProtoBuf.DataFormat.Default)]
//        public byte EntityType { get; set; }

//        private global::ProtoBuf.IExtension? extensionObject;

//        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
//        {
//            return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
//        }

//        [global::ProtoBuf.ProtoMember(
//            3,
//            Name = "entities",
//            IsRequired = false,
//            DataFormat = global::ProtoBuf.DataFormat.Default
//            //Options = global::ProtoBuf.MemberSerializationOptions.Packed
//            )]
//        [global::System.ComponentModel.DefaultValue(null)]
//        public long[]? Entities { get; set; }
//    }
//}
