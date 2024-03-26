using System;

namespace PerfDemo
{
    public class MeasurementInputs
    {
        public int DeSerializationRequests { get; set; } = 1000 * 1000;
        public int Concurrency { get; set; } = 1;
        public ReadOnlyMemory<byte> InputData { get; set; } = ReadOnlyMemory<byte>.Empty;

        /// <summary>
        /// DemoData with 500 and 4000 available
        /// </summary>
        public int ExpectedSamples { get; set; } = 4000;
    }
}
