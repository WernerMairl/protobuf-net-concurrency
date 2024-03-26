using System;
using ProtoBuf.Meta;

namespace PerfDemo
{
    public class MeasurementInputs
    {
        /// <summary>
        /// precompiled TypeModel!
        /// </summary>
        /// <param name="model"></param>
        public MeasurementInputs(TypeModel model)
        {
            this.ProtoBufTypeModel = model;
        }
        public TypeModel ProtoBufTypeModel { get; set; }
        public int DeSerializationRequests { get; set; } = 1000 * 1000;
        public int Concurrency { get; set; } = 1;
        public ReadOnlyMemory<byte> InputData { get; set; } = ReadOnlyMemory<byte>.Empty;

        /// <summary>
        /// DemoData with 500 and 4000 available
        /// </summary>
        public int ExpectedSamples { get; set; } = 4000;
    }
}
