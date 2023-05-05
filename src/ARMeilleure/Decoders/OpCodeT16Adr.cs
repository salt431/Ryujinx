namespace ARMeilleure.Decoders
{
    class OpCodeT16Adr : OpCodeT16, IOpCode32Adr
    {
        public int Rd { get; }

        public int Immediate { get; }

        public new static OpCode Create(InstDescriptor inst, ulong address, int opCode) => new OpCodeT16Adr(inst, address, opCode);

        public OpCodeT16Adr(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            Rd = opCode & 7;

            int imm = (opCode >> 3) & 0x1f;
            if (imm == 0)
            {
                Immediate = (int)(GetPc() & 0xfffffffc);
            }
            else if (imm == 0x1f)
            {
                Immediate = (int)(GetPc() & 0xfffffffc) - 0x1000;
            }
            else
            {
                int sign = (opCode >> 9) & 1;
                imm |= (opCode >> 4) & 0x18;

                if (sign == 0)
                {
                    Immediate = (int)(GetPc() & 0xfffffffc) + imm;
                }
                else
                {
                    Immediate = (int)(GetPc() & 0xfffffffc) - imm;
                }
            }
        }
    }
}
