﻿using System;
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
            uint prevInst1 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress, 4), 0);
            uint prevInst2 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress + 4, 4), 0);
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
            prevInst1 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress, 4), 0);
            prevInst2 = BitConverter.ToUInt32(_stream.ReadRam(script.InsertAddress + 4, 4), 0);
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
            success &= _stream.WriteRam(scriptBytes, scriptAddress);

            scriptAddress += (uint)(scriptLength);
            script.PostInstrSpace = scriptAddress;

            scriptAddress += (uint)(2*sizeof(uint));

            uint jumpBackToInsertPointInst = JumpToAddressInst(script.InsertAddress + 8);
            success &= _stream.WriteRam(BitConverter.GetBytes(jumpBackToInsertPointInst), scriptAddress);

            script.Allocated = success;
            if (!script.Allocated)
                return false;

            // Write jump
            script.Allocated &= _stream.WriteRam(prevInstBytes, script.PostInstrSpace);
            script.Allocated &= _stream.WriteRam(script.JumpInstBytes, script.InsertAddress);

            return success;
        }
    }
}
