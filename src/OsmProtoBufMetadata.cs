using System;
using PerfDemo.OsmFormat;
using ProtoBuf.Meta;

namespace PerfDemo
{
    public sealed class ProtoBufTypeInfo
    {
        public readonly TypeModel OsmFormatModel;
        //private readonly TypeModel _fileFormatTypeModel;
        //private static readonly Type _headerBlockType = typeof(PerfDemo.OsmFormat.HeaderBlock);

        private static RuntimeTypeModel InternalCreate(string name)
        {
            return RuntimeTypeModel.Create(name);
        }

        private static TypeModel CreateFileFormatModel(bool compile)
        {
            var rt = InternalCreate(nameof(CreateFileFormatModel));
            if (compile)
            {
                return rt.Compile();
            }
            return rt;
        }
        private static TypeModel CreateOsmFormatModel(bool compile)
        {
            var rt = InternalCreate(nameof(CreateOsmFormatModel));
            rt.Add(typeof(PrimitiveBlock), true);
            rt.Add(typeof(OsmFormat.Relation.MemberType), true);
            //rt.Add(_headerBlockType, applyDefaultBehaviour: true);
            if (compile)
            {
                return rt.Compile();
            }
            return rt;
        }

        internal ProtoBufTypeInfo(bool compile)
        {
            OsmFormatModel = CreateOsmFormatModel(compile);
            //          _fileFormatTypeModel = CreateFileFormatModel(compile);
        }
    }
}
