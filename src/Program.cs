using System;
using System.Diagnostics;
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
                    Debug.Assert(inputs.InputData.Length > 1000);
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

        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("PerfDemo v0.2.0");
            Console.WriteLine();
            Console.WriteLine($"Processors (available): {Environment.ProcessorCount}");
            Console.WriteLine();
            Debug.Assert(args != null);
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Canceling...");
                cts.Cancel();
                e.Cancel = true;
            };
            try
            {
                int[] concurrencies = new int[] { 1, 2, 4, 6, 8 };
                int samples = 4000;
                TypeModel protoBufModel = ProtoBufTypeInfo.CreateOsmFormatModel(compile: true);
                var inputs = new MeasurementInputs(protoBufModel)
                {
                    DeSerializationRequests = 1200,
                    InputData = DemoDataHelper.GenerateSerializedDemoData(samples),
                    ExpectedSamples = samples
                };

                foreach (var concurrency in concurrencies)
                {
                    inputs.Concurrency = concurrency;
                    long lockContentionBefore = Monitor.LockContentionCount;
                    Console.ForegroundColor = ConsoleColor.Green;
                    var watch = Stopwatch.StartNew();
                    await MeasureAsync(inputs, cts.Token);
                    watch.Stop();
                    long lockContentionAfter = Monitor.LockContentionCount;
                    Console.ResetColor();
                    Console.WriteLine("Measurement:");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine($"Tasks: {inputs.Concurrency}");
                    Console.WriteLine($"Duration (milliseconds): {Convert.ToInt64(watch.Elapsed.TotalMilliseconds)}");
                    Console.WriteLine($"Lock Contention: {lockContentionAfter - lockContentionBefore}");
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
