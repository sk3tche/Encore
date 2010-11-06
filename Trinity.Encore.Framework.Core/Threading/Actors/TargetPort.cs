using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    public sealed class TargetPort<TInput>
    {
        internal ITargetBlock<TInput> Block { get; private set; }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Block != null);
        }

        public TargetPort(ITargetBlock<TInput> block)
        {
            Contract.Requires(block != null);

            Block = block;
        }

        public bool Post(TInput item)
        {
            return Block.Post(item);
        }

        public Task<bool> PostAsync(TInput item)
        {
            return Block.PostAsync(item);
        }
    }
}
