using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PerfDemo
{
    public static class Helper
    {
#if DEBUG
        public static readonly string Configuration = "Debug";
#else
        public static readonly string Configuration = "Release";
#endif
        public static readonly string ProductName = "PerfDemo";
        private static readonly char[] separator = new char[] { '.', '-' };

        /// <summary>
        /// reading from InformationalVersionAttribute, the only Version that is integrated as Attribute directly on the assembly!
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetProductVersion(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute));
            if (attributes.SingleOrDefault() is not AssemblyInformationalVersionAttribute attr)
            {
                return "0.0.0.0-undefined";
            }
            return attr.InformationalVersion;
        }

        public static string GetStartTime()
        {
            var t = Process.GetCurrentProcess().StartTime.ToUniversalTime();
            return t.ToString("o", CultureInfo.InvariantCulture);
        }

        public static string GetCurrentDateTime()
        {
            var t = DateTime.UtcNow;
            return t.ToString("o", CultureInfo.InvariantCulture);
        }

        public static string GetAssemblyTitle(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute));
            if (attributes.SingleOrDefault() is not AssemblyTitleAttribute attr)
            {
                return string.Empty;
            }
            return attr.Title;
        }

        public static string GetAssemblyDescription(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute));
            if (attributes.SingleOrDefault() is not AssemblyDescriptionAttribute attr)
            {
                return string.Empty;
            }
            return attr.Description;
        }

        public static string GetProcessInfo()
        {
            string platform;
            if (System.Environment.Is64BitProcess)
            {
                platform = "64 Bit";
            }
            else
            {
                platform = "32 Bit";
            }

            string osPF = string.Empty;
            OSPlatform[] pfList = new OSPlatform[] { OSPlatform.Linux, OSPlatform.OSX, OSPlatform.Windows };
            foreach (OSPlatform pf in pfList)
            {
                if (RuntimeInformation.IsOSPlatform(pf))
                {
                    osPF = string.Format("on {0}", pf);
                }
            }
            return string.Format("{0} Cores {1} {2}", System.Environment.ProcessorCount, osPF, platform).Trim();
        }

        public static string GetMinorProductVersionFromEntryAssembly()
        {
            Assembly ass = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Missing Entry Assembly");
            string productVersion = GetProductVersion(ass);
            var splits = productVersion.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length < 2) throw new InvalidOperationException("Version cannot be splited");
            return $"{splits[0]}.{splits[1]}";
        }

        public static string GetMajorProductVersionFromEntryAssembly()
        {
            Assembly ass = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Missing Entry Assembly");
            string productVersion = GetProductVersion(ass);
            var splits = productVersion.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length < 2) throw new InvalidOperationException("Version cannot be splited");
            return $"{splits[0]}";
        }

        public static string GetProductVersionFromEntryAssembly()
        {
            Assembly ass = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Missing Entry Assembly");
            return GetProductVersion(ass);
        }       
    }
}
