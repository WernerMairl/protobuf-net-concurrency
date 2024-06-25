using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf.Meta;

namespace ListDemo
{
    /// <summary>
    /// Disposable for cleanup
    /// </summary>
    public static class Program
    {

      

        public static async Task<int> Main(string[] args)
        {
            await Task.CompletedTask;
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
