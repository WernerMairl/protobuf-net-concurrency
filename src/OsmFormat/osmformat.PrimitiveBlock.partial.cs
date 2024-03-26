using System;
using System.Linq;

namespace PerfDemo.OsmFormat
{
    /// <summary>
    /// sss
    /// </summary>
    public partial class PrimitiveBlock
    {

        /// <summary>
        /// should not be part of ProtoBuf!
        /// </summary>
        internal bool IsEmpty { get; private set; } = false;

        public static readonly PrimitiveBlock Empty = new PrimitiveBlock() { IsEmpty = true };

        public int GetNodesCount()
        {
            if (this.primitivegroup == null) return 0;
            return this.primitivegroup.Where(g => g.HasNodes).Sum(g => g.GetNodesCount());
        }

        public int GetWaysCount()
        {
            if (this.primitivegroup == null) return 0;
            return this.primitivegroup.Where(g => g.HasWays).Sum(g => g.GetWaysCount());
        }

        public int GetRelationsCount()
        {
            if (this.primitivegroup == null) return 0;
            return this.primitivegroup.Where(g => g.HasRelations).Sum(g => g.GetRelationsCount());
        }
    }
}
