using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks.Dataflow;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    public sealed class SourcePort<TOutput>
    {
        private readonly ISourceBlock<TOutput> _block;

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(_block != null);
        }

        public SourcePort(ISourceBlock<TOutput> block)
        {
            Contract.Requires(block != null);

            _block = block;
        }

        public IDisposable Link(TargetPort<TOutput> target, bool unlinkAfterOne = false)
        {
            Contract.Ensures(Contract.Result<IDisposable>() != null);

            var link = _block.LinkTo(target.Block, unlinkAfterOne);
            Contract.Assume(link != null);
            return link;
        }
    }
}
