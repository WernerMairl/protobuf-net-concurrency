using System.Threading;
using System;
using PerfDemo.OsmFormat;
using ProtoBuf.Meta;
using System.Reflection;
using ProtoBuf;

namespace PerfDemo
{
    public sealed class ProtoBufTypeInfo
    {
        public static object ObjectMaker(Type type, SerializationContext context)
        {
            var cntx = context.Context as ProtoReader;
            var pb = cntx.UserState as PerfDemo.OsmFormat.PrimitiveBlock;
            int ListSizeDefault = 0;
            if (type == typeof(DenseNodes))
            {
                return new DenseNodes();
            }

            if (type == typeof(DenseInfo))
            {
                return new DenseInfo();
            }


            throw new NotImplementedException("NI");
            object obj = "";
            //if (type == typeof(Foo))
            //{
            //    obj = Foo.Create();
            //}
            //else
            //{
            //    obj = Activator.CreateInstance(type);
            //}
            //Interlocked.Increment(ref count);
            return obj;

        }
        public static TypeModel CreateOsmFormatModel(bool compile)
        {
            //compile = true;
            var rt = RuntimeTypeModel.Create();
            var d3 = rt.Add(typeof(byte[]), false);
            var dns2 = rt.Add(typeof(OsmFormat.DenseInfo), true); //TODO expected or missing ??
            dns2.SetFactory(typeof(ProtoBufTypeInfo).GetMethod("ObjectMaker"));

            var dns1 = rt.Add(typeof(OsmFormat.DenseNodes), true); //TODO expected or missing ??
            dns1.SetFactory(typeof(ProtoBufTypeInfo).GetMethod("ObjectMaker"));
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
