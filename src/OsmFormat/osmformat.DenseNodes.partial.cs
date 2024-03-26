using System.Collections.Generic;

namespace PerfDemo.OsmFormat
{
    /// <summary>
    /// DenseNodes
    /// </summary>
    public partial class DenseNodes
    {
        public bool HasValidKeys()
        {
            int denseItems = this.id.Count;
            if (denseItems < 1) return true;
            List<int> keyVals = this.keys_vals ?? new System.Collections.Generic.List<int>();
            int keyValIndex = 0;
            int zeroCounter = 0;
            for (int idx = 0; idx < denseItems; idx++)
            {
                while (keyValIndex < keyVals.Count)
                {
                    int keyVal = keyVals[keyValIndex];
                    keyValIndex++;
                    if (keyVal == 0)
                    {
                        zeroCounter++;
                        break;
                    }
                }
            }
            return zeroCounter == denseItems;
        }
    }
}
