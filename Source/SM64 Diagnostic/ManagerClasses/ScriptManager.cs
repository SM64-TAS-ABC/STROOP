using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.ManagerClasses
{

    public class ScriptManager
    {
        ProcessStream _stream;
        ScriptParser _parser;

        uint _freeMemPtr;

        public ScriptManager(ProcessStream stream, ScriptParser parser)
        {
            _stream = stream;
            _parser = parser;

            _freeMemPtr = _parser.FreeMemoryArea; 
        }

        public void Update()
        {
            foreach (var script in _parser.Scripts)
                ExecuteScript(script);
        }

        public void ExecuteScript(GameScript script)
        {
            bool scriptExecuted = true;

            // Copy jump bytes
            uint inst1 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress, 4), 0);
            uint inst2 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress + 4, 4), 0);

            // Get jump instruction
            uint jumpInst1 = JumpToAddressInst(script.ExecutionSpace);
            uint jumpInst2 = 0;
            byte[] jumpInstBytes = new byte[8];
            BitConverter.GetBytes(jumpInst1).ToArray().CopyTo(jumpInstBytes, 0);
            BitConverter.GetBytes(jumpInst2).ToArray().CopyTo(jumpInstBytes, 4);

            // Find out if the script was executeed
            scriptExecuted = (inst1 != jumpInst1) || (inst2 != jumpInst2);

            // wait for the script to be executed
            if (!scriptExecuted)
                return;


            // Write post instructions
            uint[] writeInstBackInsts = new uint[9];
            // [a0 = instAddress]
            // LUI a0, instAddress >> 16
            // ORI a0, a0, instAddress && 0xFFFF
            writeInstBackInsts[0] = (uint)(0x3C00 | (script.InsertAddress >> 16));
            writeInstBackInsts[1] = (uint)(0x3400 | (script.InsertAddress >> 16));
            // [a1 = *instAddress]
            // LUI a1, *instAddress >> 16
            // ORI a1, a1, *instAddress && 0xFFFF
            writeInstBackInsts[2] = (uint)(0x3C00 | (inst1 >> 16));
            writeInstBackInsts[3] = (uint)(0x3400 | (inst1 >> 16));
            // [instrAddress = *instAddress]
            // SW a1, a0(0)
            writeInstBackInsts[5] = 0xA8000000;
            // [a1 = *(instAddress+1)]
            // LUI a1, *(instAddress+1) >> 16
            // ORI a1, a1, *(instAddress+1) && 0xFFFF
            writeInstBackInsts[6] = (uint)(0x3C00 | (inst2 >> 16));
            writeInstBackInsts[7] = (uint)(0x3400 | (inst2 >> 16));
            // [instrAddress = *(instAddress+1)]
            // SW a1, a0(1)
            writeInstBackInsts[8] = 0xA8000001;

            byte[] writeInstrBackBytes = new byte[writeInstBackInsts.Length * sizeof(byte)];
            Buffer.BlockCopy(writeInstBackInsts, 0, writeInstrBackBytes, 0, writeInstrBackBytes.Length);
            _stream.WriteRam(writeInstrBackBytes, _freeMemPtr);

            if (!script.Allocated)
            {
                script.ExecutionSpace = _freeMemPtr;

                var scriptLength = script.Script.Length * sizeof(uint);
                byte[] scriptBytes = new byte[scriptLength];

                // Write script
                Buffer.BlockCopy(script.Script, 0, scriptBytes, 0, scriptLength);
                _stream.WriteRam(scriptBytes, _freeMemPtr);

                _freeMemPtr += (uint)(scriptLength + writeInstBackInsts.Length * sizeof(uint));

                uint jumpBackToInsertPointInst = JumpToAddressInst(script.InsertAddress);
                _stream.WriteRam(BitConverter.GetBytes(jumpBackToInsertPointInst), _freeMemPtr);

                _freeMemPtr += sizeof(uint);

                script.Allocated = true;
            }

            // Write jump
            _stream.WriteRam(jumpInstBytes, script.InsertAddress);
        }

        private uint JumpToAddressInst(uint address)
        {
            return (uint)(address) >> 2 & 0x03FFFFFF | 0x08000000;
        }
    }
}
