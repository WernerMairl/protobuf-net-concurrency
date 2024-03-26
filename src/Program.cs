using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
                var counterClosure = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    Debug.Assert(!inputs.InputData.IsEmpty);
                    Debug.Assert(inputs.InputData.Length > 1000);
                    int z = counterClosure;
                    var runtimeModel = OsmProtoBufMetadataFactory.DefaultInstance;
                    TypeModel rtModel = runtimeModel.OsmFormatModel;
                    for (int l = 0; l < serializationsPerThread; l++)
                    {
                        object value = new PrimitiveBlock();
                        PrimitiveBlock pb = (PrimitiveBlock)rtModel.Deserialize(inputs.InputData, value, typeof(PrimitiveBlock));
                        Debug.Assert(pb != null);
                        Debug.Assert(pb.GetNodesCount() == inputs.ExpectedSamples);
                    }
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
                var inputs = new MeasurementInputs()
                {
                    DeSerializationRequests = 1200,
                    InputData = DemoDataHelper.GenerateSerializedDemoData(samples),
                    ExpectedSamples = samples
                };

                foreach (var concurrency in concurrencies)
                {
                    inputs.Concurrency = concurrency;
                    long lockContentionBefore = Monitor.LockContentionCount;
                    var watch = Stopwatch.StartNew();
                    await MeasureAsync(inputs, cts.Token);
                    watch.Stop();
                    long lockContentionAfter = Monitor.LockContentionCount;
                    Console.WriteLine("Details:");
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
