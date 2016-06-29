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
            //if (!scriptExecuted)
                return;

            scriptExecuted = false;

            // Write post instructions
            uint[] writeInstBackInsts = new uint[8];
            const byte reg1 = 0x18;
            const byte reg2 = 0x19;
            // [a0 = instAddress]
            // LUI a0, instAddress >> 16
            // ORI a0, a0, instAddress && 0xFFFF
            writeInstBackInsts[0] = (uint)(0x3C008000 | (script.InsertAddress >> 16) | reg1 << 16);
            writeInstBackInsts[1] = (uint)(0x34000000 | ((UInt16) script.InsertAddress) | reg1 << 16 | reg1 << 21);
            // [a1 = *instAddress]
            // LUI a1, *instAddress >> 16
            // ORI a1, a1, *instAddress && 0xFFFF
            writeInstBackInsts[2] = (uint)(0x3C000000 | (inst1 >> 16) | reg2 << 16);
            writeInstBackInsts[3] = (uint)(0x34000000 | ((UInt16)inst1) | reg2 << 16 | reg2 << 21);
            // [instrAddress = *instAddress]
            // SW a1, a0(0)
            writeInstBackInsts[4] = 0xAC000000 | reg2 << 16 | reg1 << 21;
            // [a1 = *(instAddress+1)]
            // LUI a1, *(instAddress+1) >> 16
            // ORI a1, a1, *(instAddress+1) && 0xFFFF
            writeInstBackInsts[5] = (uint)(0x3C000000 | (inst2 >> 16) | reg2 << 16);
            writeInstBackInsts[6] = (uint)(0x34000000 | ((UInt16)inst2) | reg2 << 16 | reg2 << 21);
            // [instrAddress = *(instAddress+1)]
            // SW a1, a0(1)
            writeInstBackInsts[7] = 0xAC000004 | reg2 << 16 | reg1 << 21;

            byte[] writeInstrBackBytes = writeInstBackInsts.SelectMany((i) => BitConverter.GetBytes(i)).ToArray();
          
            if (!script.Allocated)
            {
                script.ExecutionSpace = _freeMemPtr;
                jumpInst1 = JumpToAddressInst(script.ExecutionSpace);
                BitConverter.GetBytes(jumpInst1).ToArray().CopyTo(jumpInstBytes, 0);

                var scriptLength = script.Script.Length * sizeof(uint);
                byte[] scriptBytes = new byte[scriptLength];
 
                // Write script
                Buffer.BlockCopy(script.Script, 0, scriptBytes, 0, scriptLength);
                _stream.WriteRam(scriptBytes, _freeMemPtr);

                _freeMemPtr += (uint)(scriptLength);
                script.PostInstrSpace = _freeMemPtr;

                _freeMemPtr += (uint) (writeInstBackInsts.Length * sizeof(uint));

                uint jumpBackToInsertPointInst = JumpToAddressInst(script.InsertAddress);
                _stream.WriteRam(BitConverter.GetBytes(jumpBackToInsertPointInst), _freeMemPtr);

                _freeMemPtr += sizeof(uint) * 2;

                script.Allocated = true;
            }

            // Write jump
            _stream.WriteRam(writeInstrBackBytes, script.PostInstrSpace);
            _stream.WriteRam(jumpInstBytes, script.InsertAddress);
        }
        
        private uint JumpToAddressInst(uint address)
        {
            return (uint)(address) >> 2 & 0x03FFFFFF | 0x08000000;
        }
    }
}
