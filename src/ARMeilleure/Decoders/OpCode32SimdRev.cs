using System;
namespace ARMeilleure.Decoders
{
    class OpCode32SimdRev : OpCode32SimdCmpZ
    {
        public new static OpCode Create(InstDescriptor inst, ulong address, int opCode) => new OpCode32SimdRev(inst, address, opCode, false);
        public new static OpCode CreateT32(InstDescriptor inst, ulong address, int opCode) => new OpCode32SimdRev(inst, address, opCode, true);

        public OpCode32SimdRev(InstDescriptor inst, ulong address, int opCode, bool isThumb) : base(inst, address, opCode, isThumb)
        {
            // Use bit-wise operations instead of arithmetic operations
            int tempSize = Size;
            Size = 3 - Opc; // Op 0 is 64 bit, 1 is 32 and so on.
            Opc = tempSize;

            // Use switch statement instead of if/else
            switch (Opc)
            {
                case 0:
                    break;
                case 1:
                    Instruction = InstDescriptor.Undefined;
                    break;
                case 2:
                    Instruction = InstDescriptor.Undefined;
                    break;
                default:
                    throw new InvalidOperationException($"Invalid opcode {opCode}");
            }
        }
    }
}
