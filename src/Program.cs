using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using PerfDemo.OsmFormat;
using ProtoBuf.Meta;

namespace PerfDemo
{
    /// <summary>
    /// Disposable for cleanup
    /// </summary>
    public static class Program
    {
        public static async Task MeasureAsync(MeasurementInputs inputs, CancellationToken cancellationToken)
        {
            Debug.Assert(inputs.Concurrency > 0);
            Task[] tasks = new Task[inputs.Concurrency];
            int serializationsPerThread = inputs.DeSerializationRequests / inputs.Concurrency;
            for (int i = 0; i < inputs.Concurrency; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var watch = Stopwatch.StartNew();
                    Debug.Assert(!inputs.InputData.IsEmpty);
                    Debug.Assert(inputs.InputData.Length > 1);
                    for (int l = 0; l < serializationsPerThread; l++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // FEATURE UNDER TEST => Deserialize!!
                        object value = new PrimitiveBlock();
                        PrimitiveBlock pb = (PrimitiveBlock)inputs.ProtoBufTypeModel.Deserialize(inputs.InputData, value, typeof(PrimitiveBlock));
                        Debug.Assert(pb != null);
                        Debug.Assert(pb.GetNodesCount() == inputs.ExpectedSamples);
                    }
                    watch.Stop();
                    var rate = serializationsPerThread / watch.Elapsed.TotalSeconds;
                    Console.WriteLine($"ThreadId {Environment.CurrentManagedThreadId} takes {watch.ElapsedMilliseconds} ms for {serializationsPerThread} deserialization calls ({Convert.ToInt32(rate)} per second)");
                },
               cancellationToken,
               TaskCreationOptions.None,  //investigate impact of LongRunning 
               TaskScheduler.Default);
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// constant number over all tests, so we can see the impact of the "packaging"
        /// Assumption: smaller Node packages inside the deserialized objects are better in term of deserialization perf, but worser in terms of file-storage-compression
        /// </summary>
        private const int ExpectedNodeCreations = 1000 * 1000;

        /// <summary>
        /// This is the parameter to play with!!
        /// uses OSM-SampleData with x Nodes per PrimitiveBlock. 
        /// Available sets: 10, 500, 1000, 4000, and 8000 
        /// OSM default is 8000! 
        /// </summary>
        private const int SerializedSample = 8000;

        /// <summary>
        /// execute measurements for all this number of tasks/threads
        /// </summary>
        private static readonly int[] Concurrencies = new int[] { 1, 2, 3, 4, 8 };

        /// <summary>
        /// how many deserializations should be done overall (splitted over n tasks/threads
        /// </summary>
        private const int DeserializationRequests = (ExpectedNodeCreations / SerializedSample) * 4;

        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("PerfDemo v1.2.0");
            Console.WriteLine();
            Console.WriteLine($"  Processors (available): {Environment.ProcessorCount}");
            Console.WriteLine($"  Osm-Nodes to deserialize: {ExpectedNodeCreations.ToString("#,###,##0", CultureInfo.InvariantCulture)}");
            Console.WriteLine();
            Console.WriteLine($"  Press Ctrl+C or Ctrl+Break for cancel!");
            Console.WriteLine();
            Debug.Assert(args != null);
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Canceling...");
                Console.ResetColor();
                cts.Cancel();
                e.Cancel = true;
            };
            try
            {

                TypeModel protoBufModel = ProtoBufTypeInfo.CreateOsmFormatModel(compile: true);

                //Inputs are produced and calculated outside the measurements!
                //Core input data are provided as ReadOnlyMemory<byte>, so they are thread-safe and without any impact from Disk/Storage during the measurement!

                var inputs = new MeasurementInputs(protoBufModel)
                {
                    DeSerializationRequests = DeserializationRequests,
                    InputData = DemoDataHelper.GenerateSerializedDemoData(SerializedSample),
                    ExpectedSamples = SerializedSample
                };

                foreach (var concurrency in Concurrencies)
                {
                    inputs.Concurrency = concurrency;
                    long lockContentionBefore = Monitor.LockContentionCount;
                    Console.ResetColor();
                    Console.WriteLine($"ConcurrentTasks={inputs.Concurrency}, Nodes={inputs.ExpectedSamples}");
                    Console.ForegroundColor = ConsoleColor.Green;

                    var watch = Stopwatch.StartNew();
                    await MeasureAsync(inputs, cts.Token);
                    watch.Stop();

                    long lockContentionAfter = Monitor.LockContentionCount;
                    Console.ResetColor();
                    Console.WriteLine($"Duration={Convert.ToInt64(watch.Elapsed.TotalMilliseconds)} ms, Lock Contention: {lockContentionAfter - lockContentionBefore}");
                    Console.WriteLine();
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
                return 1;
            }
        }
    }
}
