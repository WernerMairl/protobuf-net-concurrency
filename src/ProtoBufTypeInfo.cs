using PerfDemo.OsmFormat;
using ProtoBuf.Meta;

namespace PerfDemo
{
    public sealed class ProtoBufTypeInfo
    {
        public static TypeModel CreateOsmFormatModel(bool compile)
        {
            var rt = RuntimeTypeModel.Create();
            rt.Add(typeof(PrimitiveBlock), true);
            //rt.Add(typeof(OsmFormat.Relation.MemberType), true); //TODO expected or missing ??
            //rt.Add(typeof(OsmFormat.Node), true); //TODO expected or missing ??
            //rt.Add(typeof(OsmFormat.DenseNodes), true); //TODO expected or missing ??
            if (compile)
            {
                return rt.Compile();
            }
            return rt;
        }
    }
}
