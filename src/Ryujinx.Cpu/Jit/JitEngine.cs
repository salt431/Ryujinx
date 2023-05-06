using ARMeilleure.Memory;

namespace Ryujinx.Cpu.Jit
{
    public class JitEngine : ICpuEngine
    {
        private readonly ITickSource _tickSource;

        public JitEngine(ITickSource tickSource)
        {
            _tickSource = tickSource;
        }

        public ICpuContext CreateCpuContext(IMemoryManager memoryManager, bool for64Bit)
            => new JitCpuContext(_tickSource, memoryManager, for64Bit);
    }
}
