using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Compression
{
    internal sealed class HuffmanLinkedNode
    {
        public int DecompressedValue { get; private set; }

        public int Weight { get; set; }

        public HuffmanLinkedNode Parent { get; set; }

        public HuffmanLinkedNode Child0 { get; set; }

        public HuffmanLinkedNode Child1
        {
            get { return Child0.Prev; }
        }

        public HuffmanLinkedNode Next { get; set; }

        public HuffmanLinkedNode Prev { get; set; }
        
        public HuffmanLinkedNode(int decompVal, int weight)
        {
            DecompressedValue = decompVal;
            Weight = weight;
        }

        public HuffmanLinkedNode Insert(HuffmanLinkedNode other)
        {
            Contract.Requires(other != null);
            Contract.Ensures(Contract.Result<HuffmanLinkedNode>() != null);

            if (other.Weight <= Weight)
            {
                if (Next != null)
                {
                    Next.Prev = other;
                    other.Next = Next;
                }

                Next = other;
                other.Prev = this;
                return other;
            }

            if (Prev == null)
            {
                other.Prev = null;
                Prev = other;
                other.Next = this;
            }
            else
                Prev.Insert(other);

            return this;
        }
    }
}
