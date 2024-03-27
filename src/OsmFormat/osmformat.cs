#nullable disable
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable IDE0028
#pragma warning disable IDE0090
#pragma warning disable IDE0034

// Generated (and modified) from: osmformat.proto
namespace PerfDemo.OsmFormat
{
    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"HeaderBlock")]
    public partial class HeaderBlock : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaderBlock() { }


        private HeaderBBox _bbox = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"bbox", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public HeaderBBox bbox
        {
            get { return _bbox; }
            set { _bbox = value; }
        }
        private readonly global::System.Collections.Generic.List<string> _required_features = new global::System.Collections.Generic.List<string>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, Name = @"required_features", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<string> required_features
        {
            get { return _required_features; }
        }

        private readonly global::System.Collections.Generic.List<string> _optional_features = new global::System.Collections.Generic.List<string>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(5, Name = @"optional_features", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<string> optional_features
        {
            get { return _optional_features; }
        }


        private string _writingprogram = "";
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(16, IsRequired = false, Name = @"writingprogram", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string writingprogram
        {
            get { return _writingprogram; }
            set { _writingprogram = value; }
        }

        private string _source = "";
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(17, IsRequired = false, Name = @"source", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue("")]
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"HeaderBBox")]
    public partial class HeaderBBox : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public HeaderBBox() { }

        private long _left;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"left", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public long left
        {
            get { return _left; }
            set { _left = value; }
        }
        private long _right;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name = @"right", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public long right
        {
            get { return _right; }
            set { _right = value; }
        }
        private long _top;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name = @"top", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public long top
        {
            get { return _top; }
            set { _top = value; }
        }
        private long _bottom;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name = @"bottom", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public long bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"PrimitiveBlock")]
    public partial class PrimitiveBlock : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public PrimitiveBlock() { }

        private StringTable _stringtable;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"stringtable", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public StringTable stringtable
        {
            get { return _stringtable; }
            set { _stringtable = value; }
        }
        private readonly global::System.Collections.Generic.List<PrimitiveGroup> _primitivegroup = new global::System.Collections.Generic.List<PrimitiveGroup>();

        /// <summary>
        /// A PrimitiveGroup MUST NEVER contain different types of objects
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, Name = @"primitivegroup", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<PrimitiveGroup> primitivegroup
        {
            get { return _primitivegroup; }
        }


        private int _granularity = (int)100;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(17, IsRequired = false, Name = @"granularity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((int)100)]
        public int granularity
        {
            get { return _granularity; }
            set { _granularity = value; }
        }

        private long _lat_offset = (long)0;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(19, IsRequired = false, Name = @"lat_offset", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((long)0)]
        public long lat_offset
        {
            get { return _lat_offset; }
            set { _lat_offset = value; }
        }

        private long _lon_offset = (long)0;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(20, IsRequired = false, Name = @"lon_offset", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((long)0)]
        public long lon_offset
        {
            get { return _lon_offset; }
            set { _lon_offset = value; }
        }

        private int _date_granularity = (int)1000;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(18, IsRequired = false, Name = @"date_granularity", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((int)1000)]
        public int date_granularity
        {
            get { return _date_granularity; }
            set { _date_granularity = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// A PrimitiveGroup MUST NEVER contain different types of objects
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"PrimitiveGroup")]
    public partial class PrimitiveGroup : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public PrimitiveGroup() { }

        private readonly global::System.Collections.Generic.List<Node> _nodes = new global::System.Collections.Generic.List<Node>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, Name = @"nodes", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<Node> nodes
        {
            get { return _nodes; }
        }

        private DenseNodes _dense = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"dense", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public DenseNodes dense
        {
            get { return _dense; }
            set { _dense = value; }
        }
        private readonly global::System.Collections.Generic.List<Way> _ways = new global::System.Collections.Generic.List<Way>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"ways", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<Way> ways
        {
            get { return _ways; }
        }

        private readonly global::System.Collections.Generic.List<Relation> _relations = new global::System.Collections.Generic.List<Relation>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, Name = @"relations", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<Relation> relations
        {
            get { return _relations; }
        }

        private readonly global::System.Collections.Generic.List<ChangeSet> _changesets = new global::System.Collections.Generic.List<ChangeSet>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(5, Name = @"changesets", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<ChangeSet> changesets
        {
            get { return _changesets; }
        }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"StringTable")]
    public partial class StringTable : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public StringTable() { }

        private readonly global::System.Collections.Generic.List<byte[]> _s = new global::System.Collections.Generic.List<byte[]>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, Name = @"s", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public global::System.Collections.Generic.List<byte[]> s
        {
            get { return _s; }
        }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"Info")]
    public partial class Info : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public Info() { }


        private int _version = (int)-1;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name = @"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue((int)-1)]
        public int version
        {
            get { return _version; }
            set { _version = value; }
        }

        private int _timestamp = default(int);
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name = @"timestamp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(int))]
        public int timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private long _changeset = default(long);
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name = @"changeset", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long changeset
        {
            get { return _changeset; }
            set { _changeset = value; }
        }

        private int _uid = default(int);
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"uid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(int))]
        public int uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private int _user_sid = default(int);
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name = @"user_sid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(int))]
        public int user_sid
        {
            get { return _user_sid; }
            set { _user_sid = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"DenseInfo")]
    public partial class DenseInfo : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public DenseInfo() { }

        private readonly global::System.Collections.Generic.List<int> _version = new global::System.Collections.Generic.List<int>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, Name = @"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<int> version
        {
            get { return _version; }
        }

        private readonly global::System.Collections.Generic.List<long> _timestamp = new global::System.Collections.Generic.List<long>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, Name = @"timestamp", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<long> timestamp
        {
            get { return _timestamp; }
        }

        private readonly global::System.Collections.Generic.List<long> _changeset = new global::System.Collections.Generic.List<long>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"changeset", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<long> changeset
        {
            get { return _changeset; }
        }

        private readonly global::System.Collections.Generic.List<int> _uid = new global::System.Collections.Generic.List<int>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, Name = @"uid", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<int> uid
        {
            get { return _uid; }
        }

        private readonly global::System.Collections.Generic.List<int> _user_sid = new global::System.Collections.Generic.List<int>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(5, Name = @"user_sid", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<int> user_sid
        {
            get { return _user_sid; }
        }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"ChangeSet")]
    public partial class ChangeSet : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public ChangeSet() { }

        private long _id;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private readonly global::System.Collections.Generic.List<uint> _keys = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, Name = @"keys", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> keys
        {
            get { return _keys; }
        }

        private readonly global::System.Collections.Generic.List<uint> _vals = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"vals", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> vals
        {
            get { return _vals; }
        }


        private Info _info = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public Info info
        {
            get { return _info; }
            set { _info = value; }
        }
        private long _created_at;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name = @"created_at", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public long created_at
        {
            get { return _created_at; }
            set { _created_at = value; }
        }

        private long _closetime_delta = default(long);
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name = @"closetime_delta", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        [global::System.ComponentModel.DefaultValue(default(long))]
        public long closetime_delta
        {
            get { return _closetime_delta; }
            set { _closetime_delta = value; }
        }
        private bool _open;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(10, IsRequired = true, Name = @"open", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public bool open
        {
            get { return _open; }
            set { _open = value; }
        }

        private HeaderBBox _bbox = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(11, IsRequired = false, Name = @"bbox", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public HeaderBBox bbox
        {
            get { return _bbox; }
            set { _bbox = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// ProtoBuf Node
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"Node")]
    public partial class Node : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public Node() { }

        private long _id;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"id", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private readonly global::System.Collections.Generic.List<uint> _keys = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, Name = @"keys", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> keys
        {
            get { return _keys; }
        }

        private readonly global::System.Collections.Generic.List<uint> _vals = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"vals", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> vals
        {
            get { return _vals; }
        }


        private Info _info = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public Info info
        {
            get { return _info; }
            set { _info = value; }
        }
        private long _lat;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name = @"lat", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public long lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        private long _lon;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(9, IsRequired = true, Name = @"lon", DataFormat = global::ProtoBuf.DataFormat.ZigZag)]
        public long lon
        {
            get { return _lon; }
            set { _lon = value; }
        }
        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"DenseNodes")]
    public partial class DenseNodes : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public DenseNodes() { }

        private readonly global::System.Collections.Generic.List<long> _id = new global::System.Collections.Generic.List<long>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, Name = @"id", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<long> id
        {
            get { return _id; }
        }


        private DenseInfo _denseinfo = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name = @"denseinfo", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public DenseInfo denseinfo
        {
            get { return _denseinfo; }
            set { _denseinfo = value; }
        }
        private readonly global::System.Collections.Generic.List<long> _lat = new global::System.Collections.Generic.List<long>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(8, Name = @"lat", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<long> lat
        {
            get { return _lat; }
        }

        private readonly global::System.Collections.Generic.List<long> _lon = new global::System.Collections.Generic.List<long>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(9, Name = @"lon", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<long> lon
        {
            get { return _lon; }
        }

        private readonly global::System.Collections.Generic.List<int> _keys_vals = new global::System.Collections.Generic.List<int>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(10, Name = @"keys_vals", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<int> keys_vals
        {
            get { return _keys_vals; }
        }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"Way")]
    public partial class Way : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public Way() { }

        private long _id;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private readonly global::System.Collections.Generic.List<uint> _keys = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, Name = @"keys", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> keys
        {
            get { return _keys; }
        }

        private readonly global::System.Collections.Generic.List<uint> _vals = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"vals", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> vals
        {
            get { return _vals; }
        }


        private Info _info = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public Info info
        {
            get { return _info; }
            set { _info = value; }
        }
        private readonly global::System.Collections.Generic.List<long> _refs = new global::System.Collections.Generic.List<long>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(8, Name = @"refs", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<long> refs
        {
            get { return _refs; }
        }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

    /// <summary>
    /// 
    /// </summary>
    [global::ProtoBuf.ProtoContract(Name = @"Relation")]
    public partial class Relation : global::ProtoBuf.IExtensible
    {
        /// <summary>
        /// 
        /// </summary>
        public Relation() { }

        private long _id;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name = @"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }
        private readonly global::System.Collections.Generic.List<uint> _keys = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, Name = @"keys", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> keys
        {
            get { return _keys; }
        }

        private readonly global::System.Collections.Generic.List<uint> _vals = new global::System.Collections.Generic.List<uint>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"vals", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<uint> vals
        {
            get { return _vals; }
        }


        private Info _info = null;
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name = @"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
        [global::System.ComponentModel.DefaultValue(null)]
        public Info info
        {
            get { return _info; }
            set { _info = value; }
        }
        private readonly global::System.Collections.Generic.List<int> _roles_sid = new global::System.Collections.Generic.List<int>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(8, Name = @"roles_sid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<int> roles_sid
        {
            get { return _roles_sid; }
        }

        private readonly global::System.Collections.Generic.List<long> _memids = new global::System.Collections.Generic.List<long>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(9, Name = @"memids", DataFormat = global::ProtoBuf.DataFormat.ZigZag, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<long> memids
        {
            get { return _memids; }
        }

        private readonly global::System.Collections.Generic.List<Relation.MemberType> _types = new global::System.Collections.Generic.List<Relation.MemberType>();
        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoMember(10, Name = @"types", DataFormat = global::ProtoBuf.DataFormat.TwosComplement, Options = global::ProtoBuf.MemberSerializationOptions.Packed)]
        public global::System.Collections.Generic.List<Relation.MemberType> types
        {
            get { return _types; }
        }

        /// <summary>
        /// 
        /// </summary>
        [global::ProtoBuf.ProtoContract(Name = @"MemberType")]
        public enum MemberType
        {

            /// <summary>
            /// 
            /// </summary>
            [global::ProtoBuf.ProtoEnum(Name = @"NODE")]
            NODE = 0,

            /// <summary>
            /// 
            /// </summary>
            [global::ProtoBuf.ProtoEnum(Name = @"WAY")]
            WAY = 1,

            /// <summary>
            /// 
            /// </summary>
            [global::ProtoBuf.ProtoEnum(Name = @"RELATION")]
            RELATION = 2
        }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }

}
