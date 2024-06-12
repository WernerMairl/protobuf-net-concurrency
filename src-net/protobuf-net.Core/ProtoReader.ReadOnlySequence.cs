using ProtoBuf.Internal;
using ProtoBuf.Meta;
using System;
using System.Buffers;
using System.Runtime.InteropServices;

namespace ProtoBuf
{
    public partial class ProtoReader
    {
       
        partial struct State
        {
            /// <summary>
            /// Creates a new reader against a multi-segment buffer
            /// </summary>
            /// <param name="source">The source buffer</param>
            /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to deserialize sub-objects</param>
            /// <param name="userState">Additional context about this serialization operation</param>
            public static State Create(ReadOnlySequence<byte> source, TypeModel model, object userState = null)
            {
                var reader = Pool<ReadOnlySequenceProtoReader>.TryGet() ?? new ReadOnlySequenceProtoReader();
                reader.Init(source, model, userState);
                return new State(reader);
            }

            /// <summary>
            /// Creates a new reader against a multi-segment buffer
            /// </summary>
            /// <param name="source">The source buffer</param>
            /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to deserialize sub-objects</param>
            /// <param name="userState">Additional context about this serialization operation</param>
            public static State Create(ReadOnlyMemory<byte> source, TypeModel model, object userState = null)
                => Create(new ReadOnlySequence<byte>(source), model, userState);

#if FEAT_DYNAMIC_REF
            internal void SetRootObject(object value) => _reader.SetRootObject(value);
#endif
        }
    }
}