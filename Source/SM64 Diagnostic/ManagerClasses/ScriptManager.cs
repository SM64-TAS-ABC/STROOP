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
            uint jumpInst1 = (uint)(script.ExecutionSpace) >> 2 & 0x03FFFFFF | 0x08000000;
            uint jumpInst2 = 0;
            byte[] jumpInstBytes = new byte[8];
            BitConverter.GetBytes(jumpInst1).ToArray().CopyTo(jumpInstBytes, 0);
            BitConverter.GetBytes(jumpInst2).ToArray().CopyTo(jumpInstBytes, 4);

            // Find out if the script was executeed
            scriptExecuted = (inst1 != jumpInst1) || (inst2 != jumpInst2);

            // wait for the script to be executed
            if (!scriptExecuted)
                return;

            if (script.Allocated)
            {
                _stream.WriteRam(jumpInstBytes, script.InsertAddress);
                return;
            }

            uint[] postInstructions = new uint[4];

            script.ExecutionSpace = _freeMemPtr;

            var scriptLength = (uint)(script.Script.Length + postInstructions.Length) * 4;
            byte[] scriptBytes = new byte[scriptLength];

            // Write script
            //_stream.WriteRam(BitConverter.GetBytes())

                //Buffer.BlockCopy(script.Script, 0, result, 0, result.Length);

            //_freeMemPtr += ;
            script.Allocated = true;

            // Write jump
            _stream.WriteRam(jumpInstBytes, script.InsertAddress);
        }
    }
}
