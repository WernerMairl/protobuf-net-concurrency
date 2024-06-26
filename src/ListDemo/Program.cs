using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MSS.Tools.Pbf.IO.FileFormat;
using ProtoBuf.Meta;
using ProtoBuf.Serializers;

namespace ListDemo
{
    /// <summary>
    /// Disposable for cleanup
    /// </summary>
    public static class Program
    {
        //public static readonly TypeModel FileFormatModel = ProtoBufTypeInfo.CreateFileFormatModel(false);
        public static long SerializeBlob(Stream target, Blob blob)
        {
            //Blobs with length => NO, Only BlobHeader is written WITH length. The Internal reader loop reads only ONE Length Prefix per BlobHeader/Blob duo!
            var model = ProtoBufTypeInfo.CreateFileFormatModel(false);
            return model.Serialize(target, blob);
        }

        public static Blob DeSerializeBlob(Stream stream, Blob? state = null)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var model = ProtoBufTypeInfo.CreateFileFormatModel(false);
            return (Blob)model.Deserialize(typeof(Blob), stream, value: state, userState: null);

        }
        public static Blob2 DeSerializeBlob2(Stream stream, Blob2? state = null)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var model = ProtoBufTypeInfo.CreateFileFormatModel2(false);
            return (Blob2)model.Deserialize(typeof(Blob2), stream, value: state, userState: null);

        }

        //protected internal static long WriteFileFormatBlob(Stream? targetStream, Blob blob,  string osmFormatType, bool writeHeader, bool flush, CancellationToken cancellationToken)
        //{
        //    int requiredMinimumSize = blob.raw_size;
        //    if (blob.zlib_data != null)
        //    {
        //        requiredMinimumSize = blob.zlib_data.Length + 100;
        //    }
        //    using (var blobDataStream = ProtobufStreamManager.Instance.GetStream("BlobTag", requiredSize: requiredMinimumSize))
        //    {
        //        //1. we need body length for the header => write boy to memoryStream!
        //        Debug.Assert(blobDataStream.Position == 0);
        //        long written01 = this.Encoder.SerializeBlob(blobDataStream, blob);
        //        Debug.Assert(written01 == blobDataStream.Position);
        //        byte[]? proposedIndexData = null;
        //        if (indexHeader != null)
        //        {
        //            proposedIndexData = this.Encoder.SerializeIndexHeader(indexHeader);
        //        }
        //        var pureDataSize = blobDataStream.Length;
        //        long written2 = 0;
        //        long written0 = 0;
        //        long blobPosition = -1;
        //        //SeekOrigin.current => means don't move but assume append!
        //        this.AquireTargetStream(targetStream, FileMode.OpenOrCreate, FileAccess.Write, SeekOrigin.Current, tgs =>
        //        {
        //            if (writeHeader)
        //            {
        //                var fileFormatBlobHeader = new BlobHeader()
        //                {
        //                    datasize = (int)blobDataStream.Length,
        //                    indexdata = proposedIndexData,
        //                    type = osmFormatType
        //                };
        //                //1. write header directly into the target stream!
        //                cancellationToken.ThrowIfCancellationRequested();
        //                written0 = this.Encoder.SerializeBlobHeader(tgs, fileFormatBlobHeader);
        //            }
        //            //now write the data (just serialized to protobuf with length prefix)
        //            blobPosition = tgs.Position;
        //            written2 = tgs.Position - blobPosition;
        //            blobDataStream.Seek(0, SeekOrigin.Begin);
        //            blobDataStream.CopyTo(tgs);
        //            if (flush)
        //            {
        //                tgs.Flush();
        //            }
        //        });
        //        if (this.TraceEnabled)
        //        {
        //            string indexSize = "<null>";
        //            if (proposedIndexData != null)
        //            {
        //                indexSize = proposedIndexData.Length.ToFormattedFileSize();
        //            }
        //            this.Logger.LogTrace("{nm}: BlobPosition={p}, Data={d}, IndexHeader={ih}", nameof(WriteFileFormatBlob), blobPosition, pureDataSize.ToFormattedFileSize(), indexSize);
        //        }
        //        return written01 + written2 + written0;
        //    }
        //}

        public static async Task<int> Main(string[] args)
        {
            await Task.CompletedTask;
            Debug.Assert(args != null);
            var currentProcess = Process.GetCurrentProcess();

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Canceling ({currentProcess.Id})...");
                Console.ResetColor();
                cts.Cancel();
                e.Cancel = true;
            };

            try
            {
                bool forceCreation = false;
                string testDataFile = Path.Combine(Path.GetTempPath(), "ListDemo.pbf");
                if (forceCreation || !File.Exists(testDataFile))
                {
                    var blob = new Blob();
                    int size = 7000;
                    var buffer = new byte[size];
                    for (int i = 0; i < size; i++)
                    {
                        byte v = (byte)(i % 256);
                        buffer[i] = v;
                    }
                    blob.zlib_data = buffer;
                    using (var storage = File.OpenWrite(testDataFile))
                    {
                        var written = SerializeBlob(storage, blob);
                    }
                }
                //SerializerCache.Get
                //RepeatedSerializer.CreateImmutableIList
                //    //step1: serialize
                //    var blob = new Blob();
                //int size = 7023;
                //var buffer = new byte[size];
                //Random.Shared.NextBytes(buffer);
                //blob.zlib_data = buffer;
                using (var storage = File.OpenRead(testDataFile))
                {
                    //    var written = SerializeBlob(storage, blob);
                        //var blobOriginal = DeSerializeBlob(storage);
                       // Debug.Assert(blobOriginal.zlib_data.Length == blob.zlib_data.Length);
                        var blob2 = DeSerializeBlob2(storage);
                    ArrayPool<byte>.Shared.Return(blob2.zlib_data.Array);
                    //    //Debug.Assert(blob2.zlib_data.Length == blob.zlib_data.Length);
                    //    //Debug.Assert(blob2.zlib_data.Count() == blob.zlib_data.Count());
                }
                return 0;
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                {
                    Console.WriteLine();
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{ex.GetType().FullName}: " + ex.Message);
                    Console.ResetColor();
                    Console.WriteLine();
                }
                Console.ResetColor();
                return 1;
            }
        }
    }
}
