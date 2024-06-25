using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        /// <summary>
        /// Baseline means: we measure the pure creation of all the nodes, but without using the protobf framework
        /// goial: demonstarte that the pure .net bcl don't has this problem!
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="processid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task MeasureBaseLineAsync(MeasurementInputs inputs, int processid, CancellationToken cancellationToken)
        {
            Debug.Assert(inputs.ThreadConcurrency > 0);
            Task[] tasks = new Task[inputs.ThreadConcurrency];
            Debug.Assert(inputs.ThreadConcurrency >= 0);
            Debug.Assert(inputs.DeSerializationsPerThread > 0);
            for (int i = 0; i < inputs.ThreadConcurrency; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var watch = Stopwatch.StartNew();
                    Debug.Assert(!inputs.InputData.IsEmpty);
                    Debug.Assert(inputs.InputData.Length > 1);
                    int counter = 0;
                    for (int l1 = 0; l1 < inputs.DeSerializationsPerThread; l1++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        // FEATURE UNDER TEST => Deserialize!!
                        var target = new PrimitiveBlock();
                        var source = inputs.Block!;
                        foreach (var sourceGroup in source.primitivegroup)
                        {
                            var targetGroup = new PrimitiveGroup();
                            target.primitivegroup.Add(targetGroup);
                            foreach (var sourceNode in sourceGroup.nodes)
                            {
                                var n = new Node()
                                {
                                    id = sourceNode.id,
                                    info = sourceNode.info,
                                    lat = sourceNode.lat,
                                    lon = sourceNode.lon,
                                    //keys = sourceNode.keys,
                                    //vals =  sourceNode.vals,
                                };
                                n.keys.AddRange(sourceNode.keys);
                                n.vals.AddRange(sourceNode.vals);
                                targetGroup.nodes.Add(n);
                            }
                            if (sourceGroup.dense != null)
                            {
                                var large = true;
                                targetGroup.dense = new DenseNodes();
                                targetGroup.dense.id.AddRange(sourceGroup.dense.id);
                                if (large)
                                {
                                    targetGroup.dense.keys_vals.AddRange(sourceGroup.dense.keys_vals);
                                    targetGroup.dense.lat.AddRange(sourceGroup.dense.lat);
                                    targetGroup.dense.lon.AddRange(sourceGroup.dense.lon);
                                }
                                if (sourceGroup.dense.denseinfo != null)
                                {
                                    targetGroup.dense.denseinfo = new DenseInfo();
                                    if (large)
                                    {
                                        targetGroup.dense.denseinfo.uid.AddRange(sourceGroup.dense.denseinfo.uid);
                                        targetGroup.dense.denseinfo.user_sid.AddRange(sourceGroup.dense.denseinfo.user_sid);
                                        targetGroup.dense.denseinfo.timestamp.AddRange(sourceGroup.dense.denseinfo.timestamp);
                                        targetGroup.dense.denseinfo.version.AddRange(sourceGroup.dense.denseinfo.version);
                                        targetGroup.dense.denseinfo.changeset.AddRange(sourceGroup.dense.denseinfo.changeset);
                                    }
                                }
                            }
                        }
                        //PrimitiveBlock pb = (PrimitiveBlock)inputs.ProtoBufTypeModel.Deserialize(inputs.InputData, value, typeof(PrimitiveBlock));
                        //Debug.Assert(pb != null);
                        //Debug.Assert(target.GetNodesCount() == inputs.ExpectedSamples);
                        counter += inputs.ExpectedSamples;
                    }
                    watch.Stop();
                    Debug.Assert(counter == inputs.ExpectedNodeCreationsPerThread);
                    var rate = inputs.DeSerializationsPerThread / watch.Elapsed.TotalSeconds;
                    Console.WriteLine($"PID {processid.ToString().PadLeft(5)} TID {Environment.CurrentManagedThreadId.ToString().PadLeft(3)}: {inputs.DeSerializationsPerThread} calls takes {watch.ElapsedMilliseconds} ms ({rate.ToString("#,##0.0", CultureInfo.InvariantCulture)} copy calls per second)");
                },
               cancellationToken,
               TaskCreationOptions.None,  //investigate impact of LongRunning 
               TaskScheduler.Default);
            }
            await Task.WhenAll(tasks);
        }


        public static async Task MeasureAsync(MeasurementInputs inputs, int processid, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException("STOP");
            Debug.Assert(inputs.ThreadConcurrency > 0);
            Task[] tasks = new Task[inputs.ThreadConcurrency];
            Debug.Assert(inputs.ThreadConcurrency >= 0);
            Debug.Assert(inputs.DeSerializationsPerThread > 0);
            for (int i = 0; i < inputs.ThreadConcurrency; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var watch = Stopwatch.StartNew();
                    Debug.Assert(!inputs.InputData.IsEmpty);
                    Debug.Assert(inputs.InputData.Length > 1);
                    int counter = 0;
                    for (int l1 = 0; l1 < inputs.DeSerializationsPerThread; l1++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        // FEATURE UNDER TEST => Deserialize!!
                        object value = new PrimitiveBlock();
                        //PrimitiveBlock pb2 = (PrimitiveBlock)inputs.ProtoBufTypeModel.Deserialize(inputs.InputData, value:value, typeof(PrimitiveBlock),);
                        PrimitiveBlock pb = (PrimitiveBlock)inputs.ProtoBufTypeModel.Deserialize(typeof(PrimitiveBlock), inputs.InputData, value: value, userState: "nnn");

                        Debug.Assert(pb != null);
                        Debug.Assert(pb.GetNodesCount() == inputs.ExpectedSamples);
                        counter += inputs.ExpectedSamples;
                    }
                    watch.Stop();
                    Debug.Assert(counter == inputs.ExpectedNodeCreationsPerThread);
                    var rate = inputs.DeSerializationsPerThread / watch.Elapsed.TotalSeconds;
                    Console.WriteLine($"PID {processid.ToString().PadLeft(5)} TID {Environment.CurrentManagedThreadId.ToString().PadLeft(3)}: {inputs.DeSerializationsPerThread} calls takes {watch.ElapsedMilliseconds} ms ({rate.ToString("#,##0.0", CultureInfo.InvariantCulture)} deserializer calls per second)");
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
        private static int ExpectedNodeCreations = 1000000; //=> (int.Max(SerializedSample, 1000000));

        //private static int 
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

        public static int SubProcesses { get; set; } = Environment.ProcessorCount;

        private static bool GetBooleanArgument(string[] args, string key, bool defaultValue = false)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key);
            Debug.Assert(!key.StartsWith("--"));
            Debug.Assert(!key.EndsWith("="));
            string fullKey = $"--{key}";
            if (args.Length == 0)
            {
                return defaultValue;
            }
            var sItems = args
                .Where(a => a.StartsWith(fullKey, StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
            if (sItems.Length == 0)
            {
                return defaultValue;
            }
            var sel = sItems.First();
            if (sel.Contains("="))
            {
                string val = sel.Replace(fullKey + "=", string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
                if (bool.TryParse(val, out var res))
                {
                    return res;
                }
                else
                {
                    throw new InvalidOperationException("oops");
                }
            }
            else
            {
                return true;
            }
        }
        private static int[] GetIntArgumentArray(string[] args, string key)
        {
            return GetIntArgumentArray(args, key, null, out var _);
        }
        private static int[] GetIntArgumentArray(string[] args, string key, int[]? defaultValue, out bool isDefined)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key);
            Debug.Assert(!key.StartsWith("--"));
            Debug.Assert(!key.EndsWith("="));
            string fullKey = $"--{key}=";
            if (args.Length == 0)
            {
                isDefined = false;
                return defaultValue ?? Array.Empty<int>();
            }
            var sItems = args
                .Where(a => a.StartsWith(fullKey, StringComparison.InvariantCultureIgnoreCase))
                .Select(a => int.Parse(a.Replace(fullKey, string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim()))
                .ToArray();
            if (sItems.Length == 0)
            {
                isDefined = false;
                return defaultValue ?? Array.Empty<int>();
            }
            isDefined = true;
            return sItems;
        }

        public static async Task<int> Main(string[] args)
        {
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
                bool doWorkInThreads;
                bool doWorkInProcesses;
                Concurrencies = GetIntArgumentArray(args, "c", Concurrencies, out doWorkInThreads);
                SubProcesses = GetIntArgumentArray(args, "p", null, out doWorkInProcesses).FirstOrDefault(SubProcesses);
                if (doWorkInThreads == doWorkInProcesses)
                {
                    throw new InvalidOperationException("Invalid Arguments for Threads and Processes");
                }

                SerializedSample = GetIntArgumentArray(args, "s").FirstOrDefault(SerializedSample);
                ExpectedNodeCreations = GetIntArgumentArray(args, "n").FirstOrDefault(SerializedSample * 10 * 8);

                bool quiet = GetBooleanArgument(args, "quiet");
                bool noLogo = GetBooleanArgument(args, "nologo");

                Debug.Assert(SubProcesses > 0);
                int DeserializationRequestsOverAllThreads = int.MinValue;
                if (doWorkInThreads)
                {
                    //howMany we need to get deserialized the requested number of nodes!
                    DeserializationRequestsOverAllThreads = (ExpectedNodeCreations / SerializedSample); // *8, *10
                }
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
                    if (doWorkInThreads)
                    {
                        Console.WriteLine($"  Threads: {string.Join(", ", Concurrencies.Select(c => c.ToString()))}");
                        Console.WriteLine($"  ChunkSize: {SerializedSample.ToString("#,###,##0", CultureInfo.InvariantCulture)}");
                        Console.WriteLine($"  Expected deserializer calls (over all threads): {DeserializationRequestsOverAllThreads.ToString("#,###,##0", CultureInfo.InvariantCulture)}");
                        if (Concurrencies.Length == 1)
                        {
                            int deSerializationsPerThread = DeserializationRequestsOverAllThreads / Concurrencies.First();
                            Console.WriteLine($"  Expected deserializer calls (per single thread): {deSerializationsPerThread.ToString("#,###,##0", CultureInfo.InvariantCulture)}");
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine($"  Press Ctrl+C or Ctrl+Break for cancel!");
                    Console.WriteLine();
                }

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
                    var expectedNodes = ExpectedNodeCreations / SubProcesses;
                    var options = new List<string>();
                    options.Add("--c=1");
                    options.Add("--quiet");
                    options.Add("--nologo");
                    options.Add($"--s={SerializedSample.ToString(CultureInfo.InvariantCulture)}");
                    options.Add($"--n={expectedNodes.ToString(CultureInfo.InvariantCulture)}");
                    options.Add($"--quiet={quiet}");
                    var cmdArguments = string.Join(" ", options);
                    var pi = new ProcessStartInfo(exeName, cmdArguments);
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

                TypeModel protoBufModel = ProtoBufTypeInfo.CreateOsmFormatModel(compile: false);

                //protoBufModel = ProtoBufTypeInfo.CreateOsmFormatModel(compile: true);

                //Inputs are produced and calculated outside the measurements!
                //Core input data are provided as ReadOnlyMemory<byte>, so they are thread-safe and without any impact from Disk/Storage during the measurement!

                bool baseLine = false;

                var inputs = new MeasurementInputs(protoBufModel)
                {
                    DeSerializationRequests = DeserializationRequestsOverAllThreads,
                    InputData = DemoDataHelper.GenerateSerializedDemoData(SerializedSample),
                    ExpectedSamples = SerializedSample,
                    ExpectedNodeCreations = ExpectedNodeCreations
                };

                if (baseLine)
                {
                    object value = new PrimitiveBlock();
                    PrimitiveBlock pb = (PrimitiveBlock)inputs.ProtoBufTypeModel.Deserialize(inputs.InputData, value, typeof(PrimitiveBlock));
                    Debug.Assert(pb != null);
                    inputs.Block = pb;
                }


                foreach (var concurrency in Concurrencies)
                {
                    inputs.ThreadConcurrency = concurrency;
                    long lockContentionBefore = Monitor.LockContentionCount;
                    if (!quiet)
                    {

                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"PID {currentProcess.Id.ToString(CultureInfo.InvariantCulture).PadLeft(5)}: ConcurrentTasks={inputs.ThreadConcurrency}, BlockSize={inputs.ExpectedSamples.ToString("#,###,##0", CultureInfo.InvariantCulture)}, Overall={inputs.ExpectedNodeCreations.ToString("#,###,##0", CultureInfo.InvariantCulture)}");
                        Console.ResetColor();
                    }

                    var watch = Stopwatch.StartNew();
                    if (baseLine)
                    {
                        await MeasureBaseLineAsync(inputs, currentProcess.Id, cts.Token);
                    }
                    else
                    {
                        await MeasureAsync(inputs, currentProcess.Id, cts.Token);
                    }
                    watch.Stop();
                    long lockContentionAfter = Monitor.LockContentionCount;
                    var rate = (DeserializationRequestsOverAllThreads / watch.Elapsed.TotalSeconds).ToString("##0.0", CultureInfo.InvariantCulture);
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
