namespace ARMeilleure.Decoders
{
    class OpCodeSystem : OpCode
    {
        public int Rt  { get; }
        public int Op2 { get; }
        public int CRm { get; }
        public int CRn { get; }
        public int Op1 { get; }
        public int Op0 { get; }

        public new static OpCode Create(InstDescriptor inst, ulong address, int opCode) => new OpCodeSystem(inst, address, opCode);

        public OpCodeSystem(InstDescriptor inst, ulong address, int opCode) : base(inst, address, opCode)
        {
            Rt = (opCode & 0x1F);
            Op2 = (opCode & 0x7E0) >> 5;
            CRm = (opCode & 0xF00) >> 8;
            CRn = (opCode & 0xF000) >> 12;
            Op1 = (opCode & 0x70000) >> 16;
            Op0 = ((opCode & 0x80000) >> 19) | 2;
        }
    }
}