using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting.Providers;
using STROOP.Structs.Configurations;
using System.IO;
using STROOP.Controls;
using System.Windows.Forms.Integration;
using STROOP.Utilities;
using STROOP.Properties;

namespace STROOP.Managers
{
    public class DecompilerManager
    {
        static readonly string marioRamPath = @"Resources\DAKompiler\marioRam";
        static readonly string decompile_py;

        TextBox _textBoxAddress;
        DecompilerView _decompilerView;
        CompiledCode _pythonDecompiler;
        bool _stateCaptured = false;

        static DecompilerManager()
        {
            // Load python script
            using (Stream stream = new MemoryStream(Resources.decompile_py))
            using (StreamReader reader = new StreamReader(stream))
            {
                decompile_py = reader.ReadToEnd();
            }
        }

        public DecompilerManager(Control tabControl)
        {
            var splitContainerDecompiler = tabControl.Controls["splitContainerDecompiler"] as SplitContainer;
            _textBoxAddress = tabControl.Controls["textBoxDecompilerAddress"] as TextBox;
            var buttonDecompile = tabControl.Controls["buttonDecompilerDecompile"] as Button;

            ElementHost decompilerViewHost = splitContainerDecompiler.Panel2.Controls["decompilerViewHost"] as ElementHost;
            _decompilerView = decompilerViewHost.Child as DecompilerView;

            buttonDecompile.Click += (sender, e) => Decompile();

            CreateDecompileEngine();
        }

        public void Decompile(string strAddress = null)
        {
            if (strAddress == null)
                strAddress = _textBoxAddress.Text;

            // Remove "fn" from the front
            int fnIndex = strAddress.IndexOf("fn");
            if (fnIndex == 0)
                strAddress = strAddress.Substring(2);

            uint address;
            if (!ParsingUtilities.TryParseHex(strAddress, out address))
            {
                MessageBox.Show(String.Format("Address {0} is not valid!", _textBoxAddress.Text),
                    "Address Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DecompileFunction(address);
        }

        void CreateDecompileEngine()
        {
            ScriptEngine python = Python.CreateEngine();

            // Disable Zip
            var pc = HostingHelpers.GetLanguageContext(python) as PythonContext;
            var hooks = pc.SystemState.Get__dict__()["path_hooks"] as List;
            hooks.Clear();

            python.SetSearchPaths(new List<string>() { @"Resources\DAKompiler", @"Lib" });
            _pythonDecompiler = python.CreateScriptSourceFromString(decompile_py).Compile();
        }

        void DecompileFunction(uint address)
        {
            if (!_stateCaptured)
            UpdateSnapshot();

            _pythonDecompiler.DefaultScope.SetVariable("function_address", address | 0x80000000);
            string result;
            try
            {
                result = _pythonDecompiler.Execute<string>();
                //result = python.Execute<string>(decompile_py);
            }
            catch (Exception e)
            {
                result = $"#{e.Message}";
            }
                

            _decompilerView.Text = result;
        }

        void UpdateSnapshot()
        {
            var ramState = (byte[])Config.Stream.Ram.Clone();
            using (var file = new FileStream(marioRamPath, FileMode.Create))
            {
                // 
                for (int i = 0; i < ramState.Length; i += 4)
                {
                    file.WriteByte(ramState[i + 3]);
                    file.WriteByte(ramState[i + 2]);
                    file.WriteByte(ramState[i + 1]);
                    file.WriteByte(ramState[i]);
                }
            }
            _stateCaptured = true;
        }
    }
}
