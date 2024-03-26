using System;
using System.IO;

namespace PerfDemo
{
    public static class DemoDataHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="samples">500 and 4000 implemented</param>
        /// <returns></returns>
        public static ReadOnlyMemory<byte> GenerateSerializedDemoData(int samples)
        {
            string current = Directory.GetCurrentDirectory();
            var samplesFolder = Path.Combine(current, "DemoData");
            string fileName = Path.Combine(samplesFolder, $"Sample.{samples}.pbf");
            var buffer = File.ReadAllBytes(fileName);
            return buffer;
        }

    }
}
