using ARMeilleure.Memory;
using ARMeilleure.Translation;
using System;

namespace Ryujinx.Cpu.Jit
{
    class JitCpuContext : ICpuContext
    {
        private readonly ITickSource _tickSource;
        private readonly Translator _translator;
        private readonly JitMemoryAllocator _memoryAllocator;

        public JitCpuContext(ITickSource tickSource, IMemoryManager memory, bool for64Bit)
        {
            _tickSource = tickSource;
            _memoryAllocator = new JitMemoryAllocator();
            _translator = new Translator(_memoryAllocator, memory, for64Bit);
            memory.UnmapEvent += UnmapHandler;
        }
        public IDiskCacheLoadState LoadDiskCache(string titleIdText, string displayVersion, bool enabled)
{
    return new JitDiskCacheLoadState(_translator.LoadDiskCache(titleIdText, displayVersion, enabled));
}

        private void UnmapHandler(ulong address, ulong size)
        {
            _translator.InvalidateJitCacheRegion(address, size);
        }

        /// <inheritdoc/>
        public IExecutionContext CreateExecutionContext(ExceptionCallbacks exceptionCallbacks)
        {
            return new JitExecutionContext(_memoryAllocator, _tickSource, exceptionCallbacks);
        }

        /// <inheritdoc/>
        public void Execute(IExecutionContext context, ulong address)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _translator.Execute(((JitExecutionContext)context).Impl, address);
        }

        /// <inheritdoc/>
        public void InvalidateCacheRegion(ulong address, ulong size)
        {
            _translator.InvalidateJitCacheRegion(address, size);
        }

        /// <inheritdoc/>
        public IDiskCacheLoadState LoadDiskCache(string titleIdText, string displayVersion, bool? enabled)
        {
            return new JitDiskCacheLoadState(_translator.LoadDiskCache(titleIdText, displayVersion, enabled ?? false));
        }

        /// <inheritdoc/>
        public void PrepareCodeRange(ulong address, ulong size)
        {
            _translator.PrepareCodeRange(address, size);
        }
    }
}
