using ARMeilleure.Memory;
using Ryujinx.Memory;

namespace Ryujinx.Cpu.Jit
{
    public class JitMemoryAllocator : IJitMemoryAllocator
    {
        private static readonly MemoryAllocationFlags JitMemoryFlags = MemoryAllocationFlags.Reserve | MemoryAllocationFlags.Jit;

        public IJitMemoryBlock Allocate(ulong size) => new JitMemoryBlock(size, MemoryAllocationFlags.None);
        public IJitMemoryBlock Reserve(ulong size) => new JitMemoryBlock(size, JitMemoryFlags);

        public ulong GetPageSize() => MemoryBlock.GetPageSize();
    }
}
