using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PerfDemo
{

    public class MeasurementInputs
    {
        public int Concurrency { get; set; } = 1;
    }
    /// <summary>
    /// Disposable for cleanup
    /// </summary>
    public static class Program
    {
        public static async Task MeasureAsync(MeasurementInputs inputs, CancellationToken cancellationToken)
        {
            Debug.Assert(inputs.Concurrency > 0);
            Task[] tasks = new Task[inputs.Concurrency];
            for (int i = 0; i < inputs.Concurrency; i++)
            {
                var counterClosure = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(250);
                    int z = counterClosure;
                    //var runtimeModel = OsmProtoBufMetadataFactory.DefaultInstance;
                    //TypeModel rtModel = runtimeModel.OsmFormatModel;
                    //var sw = Stopwatch.StartNew();
                    //for (int l = 0; l < serializationsPerThread; l++)
                    //{
                    //    if (method == "net-stream")
                    //    {
                    //        ms.Position = 0;
                    //        object value = new PrimitiveBlock();
                    //        PrimitiveBlock pb = (PrimitiveBlock)rtModel.Deserialize(ms, value, runtimeModel.PrimitiveBlockType);
                    //        Assert.Equal(NodesCount, pb.GetNodesCount());
                    //    }
                    //}
                    //sw.Stop();
                    ////var avg = sw.Elapsed / serializationsPerThread;
                    ////this.Output.WriteLine($"Task {z.ToString().PadLeft(2, '0')}: threadDuration={sw.ToFormattedDuration()}, avg/serialization={avg.ToFormattedDuration()}, serializationsPerThread={serializationsPerThread.ToFormattedCount()} (ThreadId={Environment.CurrentManagedThreadId})");
                },
               CancellationToken.None,
               TaskCreationOptions.None,  //investigate impact of LongRunning 
               TaskScheduler.Default);
            }
            await Task.WhenAll(tasks);
        }

        public static async Task<int> Main(string[] args)
        {
            Console.WriteLine("PerfDemo v0.1.0");
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
                var inputs = new MeasurementInputs()
                {
                    Concurrency = Environment.ProcessorCount
                };


                long lockContentionBefore = Monitor.LockContentionCount;
                var watch = Stopwatch.StartNew();
                await MeasureAsync(inputs, cts.Token);
                watch.Stop();
                long lockContentionAfter = Monitor.LockContentionCount;
                Console.WriteLine("Details:");
                Console.WriteLine("--------------------------");
                Console.WriteLine($"Processors (available): {Environment.ProcessorCount}");
                Console.WriteLine($"Duration (milliseconds): {Convert.ToInt64(watch.Elapsed.TotalMilliseconds)}");
                Console.WriteLine($"Tasks: {inputs.Concurrency}");
                Console.WriteLine($"Lock Contention: {lockContentionAfter - lockContentionBefore}");
                Console.WriteLine();
                return 0;
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{ex.GetType().FullName}: " + ex.Message);
                    Console.ResetColor();
                    Console.WriteLine();
                }
                return 1;
            }
        }
    }
}
