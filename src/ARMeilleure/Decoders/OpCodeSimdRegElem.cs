namespace ARMeilleure.Decoders
{
    class OpCodeSimdRegElem : OpCodeSimdReg
    {
        private static readonly int[] IndexTableSize1 =
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
        };

        private static readonly int[] IndexTableSize2 =
        {
            0, 2, 1, 3
        };

        public int Index { get; }

        public new static OpCode Create(InstDescriptor inst, ulong address, int opCode) => new OpCodeSimdRegElem(inst, address, opCode);

        public OpCodeSimdRegElem(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            if (Size == 1)
            {
                Index = IndexTableSize1[(opCode >> 9) & 0x0f] |
                        ((opCode >> 20) & 0x03) << 4;
                Rm &= 0xf;
            }
            else if (Size == 2)
            {
                Index = IndexTableSize2[(opCode >> 10) & 0x03] |
                        ((opCode >> 21) & 0x01) << 2;
            }
            else
            {
                Instruction = InstDescriptor.Undefined;
            }
        }
    }
}
