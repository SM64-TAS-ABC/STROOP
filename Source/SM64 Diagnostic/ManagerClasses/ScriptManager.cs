using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;

namespace SM64_Diagnostic.ManagerClasses
{

    public class ScriptManager
    {
        ProcessStream _stream;
        ScriptParser _parser;
        readonly byte[] byteUintFF = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
        CheckBox _useRomHackChecBox;

        uint _freeMemPtr;

        public ScriptManager(ProcessStream stream, ScriptParser parser, CheckBox useRomHackChecBox)
        {
            _stream = stream;
            _parser = parser;
            _useRomHackChecBox = useRomHackChecBox;

            _freeMemPtr = _parser.FreeMemoryArea; 
        }

        public void Update()
        {
            // Don't do anything if the rom hack is not enabled
            if (!_useRomHackChecBox.Checked)
                return;

            _freeMemPtr = _parser.FreeMemoryArea;

            foreach (var script in _parser.Scripts)
                ExecuteScript(script);
        }

        public async void ExecuteScript(GameScript script)
        {
            // Copy jump bytes
            uint prevInst1 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress, 4), 0);
            uint prevInst2 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress + 4, 4), 0);
            byte[] prevInstBytes = new byte[8];
            BitConverter.GetBytes(prevInst1).CopyTo(prevInstBytes, 0);
            BitConverter.GetBytes(prevInst2).CopyTo(prevInstBytes, 4);

            if (!AllocateScript(script))
            {
                return;
            }

            // Write jump
            _stream.WriteRam(prevInstBytes, script.PostInstrSpace);
            _stream.WriteRam(script.JumpInstBytes, script.InsertAddress);
        }
        
        private uint JumpToAddressInst(uint address)
        {
            return (uint)(address) >> 2 & 0x03FFFFFF | 0x08000000;
        }

        private bool AllocateScript(GameScript script)
        {
            _stream.WriteRam(byteUintFF, _freeMemPtr);
            _freeMemPtr += 4;

            script.ExecutionSpace = _freeMemPtr;

            bool success = true;

            // Get jump instruction
            script.JumpInstr = JumpToAddressInst(script.ExecutionSpace);
            uint jumpInst2 = 0;
            script.JumpInstBytes = new byte[8];
            BitConverter.GetBytes(script.JumpInstr).ToArray().CopyTo(script.JumpInstBytes, 0);
            BitConverter.GetBytes(jumpInst2).ToArray().CopyTo(script.JumpInstBytes, 4);

            var scriptLength = script.Script.Length * sizeof(uint);
            byte[] scriptBytes = new byte[scriptLength];

            // Write script
            Buffer.BlockCopy(script.Script, 0, scriptBytes, 0, scriptLength);
            success &= _stream.WriteRam(scriptBytes, _freeMemPtr);

            _freeMemPtr += (uint)(scriptLength);
            script.PostInstrSpace = _freeMemPtr;

            _freeMemPtr += (uint)(2*sizeof(uint));

            uint jumpBackToInsertPointInst = JumpToAddressInst(script.InsertAddress + 8);
            success &= _stream.WriteRam(BitConverter.GetBytes(jumpBackToInsertPointInst), _freeMemPtr);

            _freeMemPtr += sizeof(uint) * 2;

            script.Allocated = true;

            return success;
        }
    }
}
