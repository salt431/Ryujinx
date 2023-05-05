using System;
using System.Threading;

namespace ARMeilleure.Memory
{
    class ReservedRegion
    {
        public const int DefaultGranularity = 65536; // Mapping granularity in Windows.

        public IJitMemoryBlock Block { get; }

        private IntPtr _pointer;
        public IntPtr Pointer
        {
            get
            {
                if (_pointer == IntPtr.Zero)
                {
                    _pointer = Block.Pointer;
                }
                return _pointer;
            }
        }

        private readonly ulong _maxSize;
        private readonly ulong _sizeGranularity;
        private ulong _currentSize;

        public ReservedRegion(IJitMemoryAllocator allocator, ulong maxSize, ulong granularity = 0)
        {
            if (granularity == 0)
            {
                granularity = DefaultGranularity;
            }

            Block = allocator.Reserve(maxSize);
            _maxSize = maxSize;
            _sizeGranularity = granularity;
            _currentSize = 0;
        }

        public void ExpandIfNeeded(ulong desiredSize)
        {
            if (desiredSize > _maxSize)
            {
                throw new OutOfMemoryException();
            }

            ulong currentSize = _currentSize;
            while (desiredSize > currentSize)
            {
                ulong overflowBytes = desiredSize - currentSize;
                ulong moreToCommit = (((_sizeGranularity - 1) + overflowBytes) >> 16 << 16); // Round up.
                ulong oldSize = currentSize;
                currentSize = Interlocked.CompareExchange(ref _currentSize, currentSize + moreToCommit, oldSize);
                if (currentSize == oldSize)
                {
                    Block.Commit(oldSize, moreToCommit);
                }
            }
        }

        public void Dispose()
        {
            Block.Dispose();
        }
    }
}