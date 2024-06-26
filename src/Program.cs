using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        public static async Task MeasureAsync(MeasurementInputs inputs, int processid, CancellationToken cancellationToken)
        {
            Debug.Assert(inputs.Concurrency > 0);
            Task[] tasks = new Task[inputs.Concurrency];
            Debug.Assert(inputs.Concurrency >= 0);
            Debug.Assert(inputs.DeSerializationsPerThread > 0);
            for (int i = 0; i < inputs.Concurrency; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var watch = Stopwatch.StartNew();
                    Debug.Assert(!inputs.InputData.IsEmpty);
                    Debug.Assert(inputs.InputData.Length > 1);
                    for (int l1 = 0; l1 < inputs.DeSerializationsPerThread; l1++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // FEATURE UNDER TEST => Deserialize!!
                        object value = new PrimitiveBlock();
                        PrimitiveBlock pb = (PrimitiveBlock)inputs.ProtoBufTypeModel.Deserialize(inputs.InputData, value, typeof(PrimitiveBlock));
                        Debug.Assert(pb != null);
                        //Debug.Assert(pb.GetNodesCount() == inputs.ExpectedSamples);
                    }
                    watch.Stop();
                    var rate = inputs.DeSerializationsPerThread / watch.Elapsed.TotalSeconds;
                    //Console.WriteLine($"{processid.ToString().PadLeft(5)}: ThreadId {Environment.CurrentManagedThreadId} takes {watch.ElapsedMilliseconds} ms for {serializationsPerThread} deserialization calls ({Convert.ToInt32(rate)} per second)");
                    Console.WriteLine($"PID {processid.ToString().PadLeft(5)} TID {Environment.CurrentManagedThreadId.ToString().PadLeft(3)}: {inputs.DeSerializationRequests} calls takes {watch.ElapsedMilliseconds} ms ({Convert.ToInt32(rate)} deserializer calls per second)");

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
        private static int ExpectedNodeCreations => (int.Max(SerializedSample, 1000000 * 5));

        /// <summary>
        /// This is the parameter to play with!!
        /// uses OSM-SampleData with x Nodes per PrimitiveBlock. 
        /// Available sets: 10, 500, 1000, 4000, and 8000, 16000, 32000 
        /// OSM default is 8000! 
        /// </summary>
        private static int SerializedSample = 8000;

        /// <summary>
        /// execute measurements for all this number of tasks/threads
        /// </summary>
        private static int[] Concurrencies = new int[] { 1 };// new int[] { 1, 2, 3, 4, 8 };

        public static int SubProcesses { get; set; } = 1;

        /// <summary>
        /// how many deserializations should be done overall (splitted over n tasks/threads
        /// </summary>
        public static int DeserializationRequests => ((ExpectedNodeCreations * 8 * 10 / SerializedSample)) / SubProcesses;
        public static async Task<int> Main(string[] args)
        {
            Debug.Assert(args != null);
            var currentProcess = Process.GetCurrentProcess();
            var sItems = args.Where(a => a.StartsWith("--s", StringComparison.InvariantCultureIgnoreCase)).ToArray();
            if (sItems.Length == 1)
            {
                SerializedSample = sItems.Select(a => int.Parse(a.Replace("--s", string.Empty, StringComparison.InvariantCultureIgnoreCase))).Single();
            }

            var ccItems = args.Where(a => a.StartsWith("--cc", StringComparison.InvariantCultureIgnoreCase)).ToArray();
            if (ccItems.Length > 0)
            {
                Concurrencies = ccItems.Select(a => int.Parse(a.Replace("--cc", string.Empty, StringComparison.InvariantCultureIgnoreCase))).ToArray();
            }

            bool quiet = args.Where(a => string.Equals(a, "--quiet", StringComparison.InvariantCultureIgnoreCase)).Any();
            //bool doWorkInThreads = args.Where(a => string.Equals(a, "--NoProc", StringComparison.InvariantCultureIgnoreCase)).Any();
            bool doWorkInThreads = true;
            bool doWorkInProcesses = !doWorkInThreads;

            bool noLogo = args.Where(a => string.Equals(a, "--nologo", StringComparison.InvariantCultureIgnoreCase)).Any();
            Program.SubProcesses = 10;
            Debug.Assert(SubProcesses > 0);
            if (!noLogo)
            {
                Console.ResetColor();
                Console.WriteLine($"{Helper.ProductName} {Helper.GetProductVersionFromEntryAssembly()} ({Helper.Configuration})");
                Console.WriteLine();
            }

            if (!quiet)
            {
                Console.ResetColor();
                Console.WriteLine($"  Processors (available): {Environment.ProcessorCount}");
                if (doWorkInProcesses)
                {
                    Console.WriteLine($"  Processes (started): {SubProcesses}");
                }
                Console.WriteLine($"  Process: {currentProcess.Id} (BasePriority={currentProcess.BasePriority})");

                Console.WriteLine($"  Osm-Nodes to deserialize: {ExpectedNodeCreations.ToString("#,###,##0", CultureInfo.InvariantCulture)}");
                Console.WriteLine($"  Expected deserializer calls: {DeserializationRequests.ToString("#,###,##0", CultureInfo.InvariantCulture)}");

                Console.WriteLine();
                Console.WriteLine($"  Press Ctrl+C or Ctrl+Break for cancel!");
                Console.WriteLine();
            }

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                //Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Canceling ({currentProcess.Id})...");
                //Console.ResetColor();
                cts.Cancel();
                e.Cancel = true;
            };
            try
            {

                if (doWorkInProcesses)
                {
                    //execute via multiple processes
                    Task<Results>[] tasks = new Task<Results>[SubProcesses];
                    string exeName = Assembly.GetEntryAssembly()!.Location;
                    if (exeName.EndsWith(".dll"))
                    {
                        exeName = exeName.Replace(".dll", ".exe");
                    }
                    Debug.Assert(System.IO.File.Exists(exeName));
                    var pi = new ProcessStartInfo(exeName, "--NoProc --nologo --quiet");
                    var sw = Stopwatch.StartNew();
                    for (int i = 0; i < SubProcesses; i++)
                    {
                        tasks[i] = ProcessAyncHelper.RunProcessAsync(pi, cts.Token);
                    }
                    await Task.WhenAll(tasks);
                    sw.Stop();
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine($"MultiProcess-Duration: {sw.Elapsed}");
                    Console.ResetColor();
                    return 0;
                }

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
                    if (!quiet)
                    {
                        Console.ResetColor();
                        Console.WriteLine($"PID {currentProcess.Id.ToString().PadLeft(5)}: ConcurrentTasks={inputs.Concurrency}, BlockSize={inputs.ExpectedSamples} Nodes");
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    var watch = Stopwatch.StartNew();
                    await MeasureAsync(inputs, currentProcess.Id, cts.Token);
                    watch.Stop();
                    long lockContentionAfter = Monitor.LockContentionCount;
                    var rate = (DeserializationRequests / watch.Elapsed.TotalSeconds).ToString("##0.0", CultureInfo.InvariantCulture);
                    Console.ResetColor();
                    Console.WriteLine($"PID {currentProcess.Id.ToString().PadLeft(5)}: Duration={Convert.ToInt64(watch.Elapsed.TotalMilliseconds)} ms, Lock Contention: {lockContentionAfter - lockContentionBefore}, Rate={rate} deserializations/sec");
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
                Console.ResetColor();
                return 1;
            }
        }
    }
}
