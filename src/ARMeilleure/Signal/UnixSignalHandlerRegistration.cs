using System;
using System.Runtime.InteropServices;

namespace ARMeilleure.Signal
{
    static partial class UnixSignalHandlerRegistration
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public unsafe struct SigSet
        {
            fixed ulong sa_mask[8];
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SigAction
        {
            public IntPtr sa_handler;
            public SigSet sa_mask;
            public int sa_flags;
            public IntPtr sa_restorer;
        }

        private const int SIGSEGV = 11;
        private const int SIGBUS = 10;
        private const int SA_SIGINFO = 0x00000004;

        [LibraryImport("libc", SetLastError = true)]
        private static partial int sigaction(int signum, ref SigAction sigAction, out SigAction oldAction);

        [LibraryImport("libc", SetLastError = true)]
        private static partial int sigaction(int signum, IntPtr sigAction, out SigAction oldAction);

        [LibraryImport("libc", SetLastError = true)]
        private static partial int sigemptyset(SigSet set);

        public static SigAction GetSegfaultExceptionHandler()
        {
            int result = sigaction(SIGSEGV, IntPtr.Zero, out SigAction old);

            if (result != 0)
            {
                throw new InvalidOperationException($"Could not get SIGSEGV sigaction. Error: {result}");
            }

            return old;
        }

        public static SigAction RegisterExceptionHandler(IntPtr action)
        {
            SigAction sig = new SigAction
            {
                sa_handler = action,
                sa_flags = SA_SIGINFO
            };

            sigemptyset(sig.sa_mask);

            if (OperatingSystem.IsMacOS() ? sigaction(SIGBUS, ref sig, out _) != 0 : true)
            {
                if (sigaction(SIGSEGV, ref sig, out SigAction old) != 0)
                {
                    throw new InvalidOperationException($"Could not register SIGSEGV sigaction. Error: {Marshal.GetLastWin32Error()}");
                }

                return old;
            }

            throw new InvalidOperationException($"Could not register SIGBUS sigaction. Error: {Marshal.GetLastWin32Error()}");
        }

        public static bool RestoreExceptionHandler(SigAction oldAction)
        {
            return sigaction(SIGSEGV, ref oldAction, out SigAction _) == 0 && (OperatingSystem.IsMacOS() ? sigaction(SIGBUS, ref oldAction, out SigAction _) == 0 : true);
        }
    }
}
