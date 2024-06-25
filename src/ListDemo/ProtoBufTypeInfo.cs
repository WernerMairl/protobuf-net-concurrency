using ProtoBuf.Meta;
namespace ListDemo
{
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
            var rt = RuntimeTypeModel.Create(nameof(CreateFileFormatModel));
            //rt.Add(_blobHeaderType, true).SetFactory(typeof(OsmProtoBufFactory).GetMethod(nameof(OsmProtoBufFactory.CreateFileFormatItems)));
            rt.Add(typeof(global::MSS.Tools.Pbf.IO.FileFormat.Blob2), true); //.SetFactory(typeof(OsmProtoBufFactory).GetMethod(nameof(OsmProtoBufFactory.CreateFileFormatItems)));
            if (compile)
            {
                return rt.Compile();
            }
            return rt;
        }

    }
}
