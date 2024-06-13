using System;
using ProtoBuf.Meta;

namespace PerfDemo
{
    //Overall Input Parameters
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

        /// <summary>
        /// overall deserializations requested, they must be splitted over Threads (Concurrency)
        /// calculated outside
        /// </summary>
        public int DeSerializationRequests { get; set; } = 1000;

        /// <summary>
        /// How many threads should be used to do the work.
        /// Time measurements is done OVER ALL this threads
        /// </summary>
        public int ThreadConcurrency { get; set; } = 1;


        /// <summary>
        /// how often the protobuf deserializer is called inside the thread.
        /// calculated by DeserializationRequests / threadConcurrency
        /// </summary>
        public int DeSerializationsPerThread => DeSerializationRequests / ThreadConcurrency;

        public ReadOnlyMemory<byte> InputData { get; set; } = ReadOnlyMemory<byte>.Empty;

        /// <summary>
        /// DemoData with 500 and 4000 available
        /// </summary>
        public int ExpectedSamples { get; set; } = 4000;
        //public int OverallNodes => ExpectedSamples * DeSerializationsPerThread;

        public int ExpectedNodeCreations = 1000000;

        public int ExpectedNodeCreationsPerThread => ExpectedNodeCreations / ThreadConcurrency;
    }
}
