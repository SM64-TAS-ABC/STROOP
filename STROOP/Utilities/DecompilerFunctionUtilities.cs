using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public class DecompilerFunctionUtilities
    {
        static readonly byte[] ReturnBytes = new byte[] { 0x08, 0x00, 0xE0, 0x03 };
        static readonly int MaximumInstructions = 1000;

        public static int? FindEndAddress(uint startAddress, byte[] ramState)
        {
            startAddress &= ~0x80000000;

            int instructionCount;
            for (instructionCount = 0; instructionCount * 4 + startAddress < ramState.Length; instructionCount++)
            {
                byte[] instructionBytes = new byte[4];
                Array.Copy(ramState, startAddress + instructionCount * 4, instructionBytes, 0, 4);

                if (instructionBytes.SequenceEqual(ReturnBytes))
                    break;

                if (instructionCount > MaximumInstructions)
                    return null;
            }

            instructionCount += 2; // 

            return instructionCount;
        }

        public static string AddressToString(uint address)
        {
            address |= 0x80000000;
            return String.Format("fn{0:X8}", address);
        }

        public static List<uint?> GetCalls(uint startAddress, int instructionCount, byte[] ramState)
        {
            var result = new List<uint?>();
            startAddress &= ~0x80000000;

            for (int currentInstruction = (int) startAddress; currentInstruction < instructionCount * 4 + startAddress; currentInstruction += 4)
            {
                var instr = BitConverter.ToUInt32(ramState, currentInstruction);
                // JAL
                if ((instr & 0x0C000000U) == 0x0C000000U && ((~instr) & 0xF0000000U) == 0xF0000000U)
                {
                    var jalAddress = (instr & 0x03FFFFFFU) << 2;
                    jalAddress |= 0x80000000U;
                    if (!result.Contains(jalAddress))
                        result.Add(jalAddress);
                }
                // JALR
                if ((instr & 0x00000009U) == 0x00000009U && ((~instr) & 0xFC000036U) == 0xFC000036U)
                {
                    result.Add(null);
                }
            }

            return result;
        }
    }
}
