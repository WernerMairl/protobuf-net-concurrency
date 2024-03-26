using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PerfDemo.OsmFormat
{
    /// <summary>
    /// PrimitiveGroup
    /// </summary>
    public partial class PrimitiveGroup
    {
        private readonly object primitiveLock = new object();
        private PrimitiveTypes? primitiveType = null;

        /// <summary>
        /// Osm spec means: a group can have only ONE primitivetype inside!
        /// PrimitiveGroup MUST NEVER contain different types of objects
        /// </summary>
        [global::ProtoBuf.ProtoIgnore]
        public PrimitiveTypes PrimitiveType
        {
            get
            {
                if (primitiveType.HasValue) return primitiveType.Value;
                lock (this.primitiveLock)
                {
                    if (this.HasNodes)
                    {
                        primitiveType = PrimitiveTypes.Node;
                        return primitiveType.Value;
                    }

                    if (this.HasRelations)
                    {
                        primitiveType = PrimitiveTypes.Relation;
                        return primitiveType.Value;
                    }

                    if (this.HasWays)
                    {
                        primitiveType = PrimitiveTypes.Way;
                        return primitiveType.Value;
                    }
                    primitiveType = PrimitiveTypes.None;
                    return primitiveType.Value;
                }
            }
        }

        [global::ProtoBuf.ProtoIgnore]
        public bool HasNodes
        {
            get
            {
                var primitiveGroup = this;
                return primitiveGroup.dense?.id.Count > 0 || this.nodes.Count > 0;
            }
        }

        [global::ProtoBuf.ProtoIgnore]
        public bool HasRelations => this.relations?.Count > 0;

        [global::ProtoBuf.ProtoIgnore]
        public bool HasWays => this.ways?.Count > 0;

        /// <summary>
        /// Protobuf Node (not Entity)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Node> GetNodes()
        {
            return this.GetNodes(true);
        }

        /// <summary>
        /// returns ProtoBuf (OsmFormat) Nodes
        /// </summary>
        /// <param name="checkDenseProperty"></param>
        /// <returns></returns>
        public List<Node> GetNodes(bool checkDenseProperty)
        {
            //Keys and values for all nodes are encoded as a single array of stringIDs.
            //Each node's tags are encoded in alternating <keyid> <valid>.
            //We use a single stringid of 0 to delimit when the tags of a node ends and the tags of the next node begin.
            //The storage pattern is: ((<keyid> <valid>)* '0' )* As an exception, if no node in the current block has any key/value pairs, this array does not contain any delimiters,
            //but is simply empty.
            List<Node> result = new List<Node>();
            PrimitiveGroup primitivegroup = this;

            if (checkDenseProperty)
            {
                // check/assert dense key/value
                // get the keys/vals.
                var groupKeyVals = primitivegroup.dense.keys_vals;
                int keyValsIdx = 0;
                long currentId = 0;
                long currentLat = 0;
                long currentLon = 0;
                long currentChangeset = 0;
                long currentTimestamp = 0;
                int currentUid = 0;
                int currentUserSid = 0;
                int currentVersion = 0;

                for (int idx = 0; idx < primitivegroup.dense.id.Count; idx++)
                {
                    // do the delta decoding stuff.
                    currentId = currentId + primitivegroup.dense.id[idx];
                    currentLat = currentLat + primitivegroup.dense.lat[idx];
                    currentLon = currentLon + primitivegroup.dense.lon[idx];
                    if (primitivegroup.dense.denseinfo != null)
                    {
                        // add all the metadata.
                        currentChangeset = currentChangeset + primitivegroup.dense.denseinfo.changeset[idx];
                        currentTimestamp = currentTimestamp + primitivegroup.dense.denseinfo.timestamp[idx];
                        currentUid = currentUid + primitivegroup.dense.denseinfo.uid[idx];
                        currentUserSid = currentUserSid + primitivegroup.dense.denseinfo.user_sid[idx];
                        currentVersion = primitivegroup.dense.denseinfo.version[idx];
                    }

                    var node = new Node
                    {
                        id = currentId,
                        info = new Info
                        {
                            changeset = currentChangeset,
                            timestamp = (int)currentTimestamp,
                            uid = currentUid,
                            user_sid = currentUserSid,
                            version = currentVersion
                        },
                        lat = currentLat,
                        lon = currentLon
                    };

                    while (groupKeyVals.Count > keyValsIdx)
                    {
                        uint currentKey = (uint)groupKeyVals[keyValsIdx];
                        if (currentKey == 0)
                        {
                            //next Node signal!
                            break;
                        }
                        node.keys.Add((uint)groupKeyVals[keyValsIdx]);
                        keyValsIdx++;
                        node.vals.Add((uint)groupKeyVals[keyValsIdx]);
                        keyValsIdx++;
                    }
                    keyValsIdx++;
                    result.Add(node);
                }
            }

            if (primitivegroup.nodes?.Count > 0)
            {
                Debug.Assert(!checkDenseProperty);
                result.AddRange(primitivegroup.nodes);
            }

            return result;
        }

        private List<Node>? nodesResolved;

        public int GetNodesCount()
        {
            nodesResolved = nodesResolved ?? this.GetNodes().ToList();
            return nodesResolved.Count;
        }

        public int GetWaysCount()
        {
            return this.ways.Count;
        }

        public int GetRelationsCount()
        {
            return this.relations.Count;
        }
    }
}
