

using STROOP.Structs.Configurations;

namespace STROOP.Structs
{
    public static class InGameFunctionCall
    {
        private static uint LUI(uint reg, ushort value)
        {
            return ((0b0011110000000000 | reg) << 16 | value);
        }

        private static uint ORI(uint resultReg, uint operandReg, ushort value)
        {
            return ((0b0011010000000000 | resultReg << 5 | operandReg) << 16 | value);
        }

        private static uint JAL(uint address)
        {
            return (0x0C000000 | ((address & 0xFFFFFF) / 4));
        }

        private static uint J(uint address)
        {
            return (0x08000000 | ((address & 0xFFFFFF) / 4));
        }

        private static void WriteWords(ref uint address, params uint[] words) {
            for(int i=0; i<words.Length; i++)
            {
                Config.Stream.SetValue(words[i], address);
                address += 4;
            }
        }

        private static void WriteRegisterAssign(ref uint address, uint register, uint value)
        {
            WriteWords(ref address, LUI(register, (ushort)(value >> 16)), ORI(register, register, (ushort)(value & 0xFFFF)));
        }

        // Inject asm in the game that executes a function call once and then removes
        // itself. Example:
        //
        // WriteInGameFunctionCall(0x8031F690, 0, 33, 0); // play music with index 33 (JP version)
        //
        // It works by changing the function pointer of the level script update function 
        // to some custom asm. Since this is an indirect call to something that hasn't 
        // been code yet, it will work even if the emulator isn't in pure interpreter
        // mode and caches code. A memcpy at the end removes the injected asm and also forces
        // the emulator to remove the cache so another function can be injected later.
        public static void WriteInGameFunctionCall(uint address, params uint[] arguments)
        {
            const int maxArguments = 4;
            if (arguments.Length > maxArguments)
            {
                throw new System.Exception("trying to call function with " + arguments.Length + " arguments, max is "+maxArguments);
            }
            uint startAddress = 0x803FFF00; // some free 0-memory (hopefully)
            uint currAddress = startAddress;
            const uint A0 = 4; // register index for argument 0

            // Stack frame:
            //ADDIU SP, SP, 0xFFE0
            //SW RA, 0x0014 (SP)
            WriteWords(ref currAddress, 0x27BDFFE0, 0xAFBF0014);

            // Restore level script pointer:
            //LI T0, 0x8037EB04
            //LUI AT, 0x8039
            //SW T0, 0xB900(AT)
            WriteWords(ref currAddress, 0x3C088037, 0x3508EB04, 0x3C018039, 0xAC28B900);

            // Write function call
            for (int i = 0; i < arguments.Length; i++)
            {
                uint reg = (uint) (A0 + i);
                WriteRegisterAssign(ref currAddress, reg, arguments[i]);
                //WriteWords(ref baseAddress, LUI(reg, (ushort) (arguments[i] >> 16)), ORI(reg, reg, (ushort) (arguments[i] & 0xFFFF)));
            }
            WriteWords(ref currAddress, JAL(address), 0x00000000); // NOP for delay slot

            // Erase self and return as if nothing happened:
            //LI RA, 0x8037EB0C
            //LUI A0, 0x803D
            //ADDIU A1, A0, 0x63FF
            //ADDIU A0, A0, 0x6400
            //J memcpy
            //ADDIU A2, R0, eraseBytes
            uint memcpyAddress = RomVersionConfig.Switch(0x803273F0, 0x803264C0);
            const uint eraseBytes = 16 * 4 + maxArguments * 8; //fixed instructions + 2 instructions per argument
            WriteWords(ref currAddress, 0x3C1F8037, 0x37FFEB0C);
            WriteRegisterAssign(ref currAddress, A0  , currAddress);
            WriteRegisterAssign(ref currAddress, A0+1, currAddress-1);
            WriteWords(ref currAddress, J(memcpyAddress), 0x24060000 | eraseBytes);

            // Hijack level script function pointer to point to injected asm
            Config.Stream.SetValue(startAddress | 0x80000000, 0x8038B900);
        }
    }
}
