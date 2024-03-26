using System;

namespace PerfDemo.OsmFormat
{
    [Flags]
    public enum PrimitiveTypes
    {
        /// <summary>
        /// None for PrimitiveBlocks means, that the entire Block has No Data!
        /// </summary>
        None = 0,

        /// <summary>
        /// Node
        /// </summary>
        Node = 1,

        /// <summary>
        /// Way
        /// </summary>
        Way = 2,

        /// <summary>
        /// Relation
        /// </summary>
        Relation = 4,

        /// <summary>
        /// Bit Flag  Node+Way+Relation
        /// </summary>
        All = Node + Way + Relation
    }
}
