using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Utilities;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Extensions;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{

    public class InjectionManager
    {
        ScriptParser _parser;
        readonly byte[] byteUintFF = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
        CheckBox _useRomHackChecBox;
      
        uint _freeMemPtr;

        public InjectionManager(ScriptParser parser, CheckBox useRomHackChecBox)
        {
            _parser = parser;
            _useRomHackChecBox = useRomHackChecBox;

            _freeMemPtr = _parser.FreeMemoryArea; 

            // Find spots to allocate script memory
            foreach (var script in _parser.Scripts)
            {
                script.ExecutionSpace = _freeMemPtr;
                _freeMemPtr += (uint)(script.Script.Length * sizeof(uint));
                _freeMemPtr += (uint)(4 * sizeof(uint));
            }
        }

        public void Update()
        {
            // Don't do anything if the rom hack is not enabled
            if (!_useRomHackChecBox.Checked)
                return;

            foreach (var script in _parser.Scripts)
                ExecuteScript(script);
        }

        public void ExecuteScript(GameScript script)
        {
            // Copy jump bytes
            uint prevInst1 = Config.Stream.GetUInt32(script.InsertAddress);
            uint prevInst2 = Config.Stream.GetUInt32(script.InsertAddress + 4);
            byte[] prevInstBytes = new byte[8];
            BitConverter.GetBytes(prevInst1).CopyTo(prevInstBytes, 0);
            BitConverter.GetBytes(prevInst2).CopyTo(prevInstBytes, 4);

            if (script.Allocated)
                script.Allocated &= prevInstBytes.SequenceEqual(script.JumpInstBytes);

            if (script.Allocated)
                return;

            // Finish loading a state
            Task.Delay(100).Wait();

            // Copy jump bytes (They may have changed)
            prevInst1 = Config.Stream.GetUInt32(script.InsertAddress);
            prevInst2 = Config.Stream.GetUInt32(script.InsertAddress + 4);
            prevInstBytes = new byte[8];
            BitConverter.GetBytes(prevInst1).CopyTo(prevInstBytes, 0);
            BitConverter.GetBytes(prevInst2).CopyTo(prevInstBytes, 4);

            AllocateScript(script, prevInstBytes);
        }
        
        private uint JumpToAddressInst(uint address)
        {
            return (uint)(address) >> 2 & 0x03FFFFFF | 0x08000000;
        }

        private bool AllocateScript(GameScript script, byte[] prevInstBytes)
        {
            uint scriptAddress = script.ExecutionSpace;
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
            success &= Config.Stream.WriteRam(scriptBytes, scriptAddress, EndiannessType.Little);

            scriptAddress += (uint)(scriptLength);
            script.PostInstrSpace = scriptAddress;

            scriptAddress += (uint)(2*sizeof(uint));

            uint jumpBackToInsertPointInst = JumpToAddressInst(script.InsertAddress + 8);
            success &= Config.Stream.WriteRam(BitConverter.GetBytes(jumpBackToInsertPointInst), scriptAddress, EndiannessType.Little);

            script.Allocated = success;
            if (!script.Allocated)
                return false;

            // Write jump
            script.Allocated &= Config.Stream.WriteRam(prevInstBytes, script.PostInstrSpace, EndiannessType.Little);
            script.Allocated &= Config.Stream.WriteRam(script.JumpInstBytes, script.InsertAddress, EndiannessType.Little);

            return success;
        }
    }
}
