using System;
using System.Diagnostics;
using MSS.Tools.Pbf.IO.FileFormat;
using ProtoBuf;

namespace ListDemo
{
    public static class OsmProtoBufFactory
    {
        //public static object CreatePrimitiveBlock(Type type, SerializationContext context)
        //{
        //    Debug.Assert(type == typeof(PrimitiveBlock));
        //    var cntx = context.Context as ProtoReader;
        //    var pb = cntx?.UserState as ReaderContext;
        //    int listSizeDefault = pb?.InitialListSize ?? 0;
        //    return new PrimitiveBlock(listSizeDefault);
        //}

        //public static object CreatePrimitiveGroup(Type type, SerializationContext context)
        //{
        //    Debug.Assert(type == typeof(PrimitiveGroup));
        //    Debug.Assert(context != null);
        //    return new PrimitiveGroup();
        //}

        public static object CreateFileFormatItems(Type type, SerializationContext context)
        {
            var cntx = context.Context as ProtoReader;
            //var pb = cntx?.UserState as ReaderContext;
            //Pooling for Blob.Raw (byte[]) not implemented yet because of missing veatures in ProtoBuf-Net
            if (type == typeof(BlobHeader))
            {
                //if (pb != null)
                //{
                //    return pb.FileFormatPools.Headers.Get();
                //}
                return new BlobHeader();
            }
            if (type == typeof(Blob))
            {
                //if (pb != null)
                //{
                //    return pb.FileFormatPools.Blobs.Get();
                //}
                return new Blob();
            }
            throw new NotImplementedException($"Factory not ready for {type.Name}");
        }

        /// <summary>
        /// special part of OsmFileFormat
        /// </summary>
        /// <param name="type"></param>
        /// <param name="context"></param>
        /// <returns></returns>
    }
}
