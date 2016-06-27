﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace SM64_Diagnostic.Structs
{
    public class ScriptParser
    {
        public List<GameScript> Scripts = new List<GameScript>();
        public uint FreeMemoryArea;

        public void AddScript(string scriptFile, uint insertAddress)
        {
            var newScript = new GameScript();
            newScript.Allocated = false;
            newScript.ExecuteMode = ExecuteModeType.Always;
            newScript.InsertAddress = insertAddress;

            string[] scriptLines = File.ReadAllLines(scriptFile);
            string fullScript = "";

            // Remove all single-line comments
            for (int i = 0; i < scriptLines.Length; i++)
            {
                if (scriptLines[i].Contains("//"))
                    fullScript += scriptLines[i].Substring(0, scriptLines[i].IndexOf("//"));
                else
                    fullScript += scriptLines[i];
            }

            // Remove all multi-line comments
            while (fullScript.Contains("*/"))
            {
                int startCommentIndex = fullScript.IndexOf("/*");
                int endCommentIndex = fullScript.IndexOf("*/");
                if (endCommentIndex == -1)
                {
                    fullScript = fullScript.Substring(0, startCommentIndex);
                }
                else
                {
                    fullScript = fullScript.Substring(0, startCommentIndex) 
                        + fullScript.Substring(endCommentIndex + 2, fullScript.Length - (endCommentIndex + 2));
                }
            }

            // Remove whitespace
            fullScript = Regex.Replace(fullScript, @"\s+", "");

            // Parse data bytes
            var scriptBytes = new List<uint>();
            for (int i = 0; i <= fullScript.Length - 4; i += 4)
            {
                scriptBytes.Add(uint.Parse(fullScript.Substring(i, 4), NumberStyles.HexNumber));
            }

            newScript.Script = scriptBytes.ToArray();
            Scripts.Add(newScript);
        }
    }
}
