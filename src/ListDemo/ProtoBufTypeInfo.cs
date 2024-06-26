using System;
using MSS.Tools.Pbf.IO.FileFormat;
using ProtoBuf;
using ProtoBuf.Meta;
using ProtoBuf.Serializers;
namespace ListDemo
{
    public class BufferSerializer : ISerializer<ArraySegment<byte>>//, IMemoryConverter<byte>
    {
        SerializerFeatures ISerializer<ArraySegment<byte>>.Features => throw new NotImplementedException();

        ArraySegment<byte> ISerializer<ArraySegment<byte>>.Read(ref ProtoReader.State state, ArraySegment<byte> value)
        {
            throw new NotImplementedException();
        }

        void ISerializer<ArraySegment<byte>>.Write(ref ProtoWriter.State state, ArraySegment<byte> value)
        {
            throw new NotImplementedException();
        }
    }

    public class Blob2Serializer : ISerializer<Blob2>//, IMemoryConverter<byte>
    {
        SerializerFeatures ISerializer<Blob2>.Features => throw new NotImplementedException();

        Blob2 ISerializer<Blob2>.Read(ref ProtoReader.State state, Blob2 value)
        {
            throw new NotImplementedException();
        }

        void ISerializer<Blob2>.Write(ref ProtoWriter.State state, Blob2 value)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class ProtoBufTypeInfo
    {
        public static TypeModel CreateFileFormatModel(bool compile)
        {
            var rt = RuntimeTypeModel.Create(nameof(CreateFileFormatModel));
            //rt.Add(_blobHeaderType, true).SetFactory(typeof(OsmProtoBufFactory).GetMethod(nameof(OsmProtoBufFactory.CreateFileFormatItems)));
            rt.Add(typeof(global::MSS.Tools.Pbf.IO.FileFormat.Blob), true); //.SetFactory(typeof(OsmProtoBufFactory).GetMethod(nameof(OsmProtoBufFactory.CreateFileFormatItems)));
            if (compile)
            {
                return rt.Compile();
            }
            return rt;
        }

        public static TypeModel CreateFileFormatModel2(bool compile)
        {
            var rt = RuntimeTypeModel.Create(nameof(CreateFileFormatModel2));
            //rt.Add(_blobHeaderType, true).SetFactory(typeof(OsmProtoBufFactory).GetMethod(nameof(OsmProtoBufFactory.CreateFileFormatItems)));
            rt.Add(typeof(global::MSS.Tools.Pbf.IO.FileFormat.Blob2), true); //.SetFactory(typeof(OsmProtoBufFactory).GetMethod(nameof(OsmProtoBufFactory.CreateFileFormatItems)));
            //rt.Add(typeof(ArraySegment<byte>), false);
            //rt.AddSerializer(typeof(Blob2), typeof(BufferSerializer));


            //rt.Add(typeof(ByteList), true);
            //if (compile)
            {
                return rt.Compile();
            }
            return rt;
        }

    }
}
