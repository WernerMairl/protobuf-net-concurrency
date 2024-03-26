using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
//using MSS.Tools.Pbf.IO.FileFormat;

namespace PerfDemo.OsmFormat
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class PrimitiveBlockPosition
    {
        /// <summary>
        /// deserialized Item with PrimitiveGroups inside
        /// should not stay in memory for longer time because it must be scaling with data size!
        /// </summary>
        public PrimitiveBlock? Target { get; set; }

        /// <summary>
        /// the OSMHeader Block for this Position
        /// </summary>
        public HeaderBlock? TargetHeader { get; set; }

        //public PrimitiveIndex? TargetIndex { get; set; }

        private string DebuggerDisplay
        {
            get
            {
                string pt = this.Primitives.HasValue ? this.Primitives.Value.ToString() : string.Empty;
                return $"{pt}";// {this.Position.DebuggerDisplay}".Trim();
            }
        }

        public bool ShouldBeVisited(PrimitiveTypes selectionFilter)
        {
            if (!this.Primitives.HasValue)
            {
                throw new InvalidOperationException("Property EntityType not set!");
            }
            return ShouldBeVisited(this.Primitives.Value, selectionFilter);
        }

        internal static bool ShouldBeVisited(PrimitiveTypes value, PrimitiveTypes selectionFilter)
        {
            bool result = (value & selectionFilter) > PrimitiveTypes.None;
            return result;
        }

        ///// <summary>
        ///// main version should be UNFILTERED, because we also need metadata about headers and Index
        ///// </summary>
        ///// <param name="collection"></param>
        ///// <returns></returns>
        //public static Dictionary<long, PrimitiveBlockPosition> CreatePositionDictionary(BlockingCollection<PrimitiveBlockPosition[]> collection)
        //{
        //    var resultDictionary = new Dictionary<long, PrimitiveBlockPosition>();

        //    foreach (PrimitiveBlockPosition[] items in collection.GetConsumingEnumerable())
        //    {
        //        IEnumerable<KeyValuePair<long, PrimitiveBlockPosition>> resultItems = items.Select(p => new KeyValuePair<long, PrimitiveBlockPosition>(p.Position.Position, p));
        //        foreach (var si in resultItems)
        //        {
        //            resultDictionary.Add(si.Key, si.Value);
        //        }
        //    }
        //    return resultDictionary;
        //}

        //public PrimitiveBlockPosition(BlobPosition position, PrimitiveTypes? types)
        //{
        //    this.Position = position;
        //    this.IsBlockContentResolved = false;
        //    this.Primitives = types;
        //}

        //public BlobPosition Position { get; }

        /// <summary>
        /// One blob can contain multiple PrimitiveGroup.
        /// A PrimitiveGroup MUST NEVER contain different types of objects
        /// only available for BlobType==2
        /// </summary>
        public PrimitiveTypes? Primitives { get; private set; }

        public bool IsBlockContentResolved { get; }

        //public List<OsmEntity> GetOrCreateOsmEntities(Func<PrimitiveTypes, List<OsmEntity>> creator)
        //{
        //    Debug.Assert(this.Primitives.HasValue);
        //    PrimitiveTypes type = this.Primitives.Value;
        //    List<OsmEntity> result = new List<OsmEntity>();
        //    if ((type & PrimitiveTypes.Node) == PrimitiveTypes.Node)
        //    {
        //        var part = creator(PrimitiveTypes.Node);
        //        result.AddRange(part);
        //    }

        //    if ((type & PrimitiveTypes.Way) == PrimitiveTypes.Way)
        //    {
        //        var part = creator(PrimitiveTypes.Way);
        //        result.AddRange(part);
        //    }
        //    if ((type & PrimitiveTypes.Relation) == PrimitiveTypes.Relation)
        //    {
        //        var part = creator(PrimitiveTypes.Relation);
        //        result.AddRange(part);
        //    }
        //    return result;
        //}

    }
}
