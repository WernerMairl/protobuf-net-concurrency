using System.Globalization;

namespace PerfDemo.OsmFormat
{
    /// <summary>
    /// HeaderBBox
    /// </summary>
    public partial class HeaderBBox
    {
        internal const double Nano = 1000000000d;
        private const string FormatString = "##0.########";

        public string ToTraceString()
        {
            HeaderBBox bbox = this;
            var ci = CultureInfo.InvariantCulture;
            return $"({(bbox.left / Nano).ToString(FormatString, ci)}, {(bbox.top / Nano).ToString(FormatString, ci)}, {(bbox.right / Nano).ToString(FormatString, ci)}, {(bbox.top / Nano).ToString(FormatString, ci)})";
        }
    }
}
