using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Utilities;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.Managers
{
    public class DebugManager
    {
        ProcessStream _stream;

        public DebugManager(ProcessStream stream, Control tabControl)
        {
            _stream = stream;

            var panel = tabControl.Controls["NoTearFlowLayoutPanelDebugDisplayType"];
            (panel.Controls["radioButtonDbgOff"] as RadioButton).Click += radioButtonDbgOff_CheckedChanged;
            (panel.Controls["radioButtonDbgObjCnt"] as RadioButton).Click += radioButtonDbgObjCnt_CheckedChanged;
            (panel.Controls["radioButtonDbgChkInfo"] as RadioButton).Click += radioButtonDbgChkInfo_CheckedChanged;
            (panel.Controls["radioButtonDbgMapInfo"] as RadioButton).Click += radioButtonDbgMapInfo_CheckedChanged;
            (panel.Controls["radioButtonDbgStgInfo"] as RadioButton).Click += radioButtonDbgStgInfo_CheckedChanged;
            (panel.Controls["radioButtonDbgFxInfo"] as RadioButton).Click += radioButtonDbgFxInfo_CheckedChanged;
            (panel.Controls["radioButtonDbgEnemyInfo"] as RadioButton).Click += radioButtonDbgEnemyInfo_CheckedChanged;
        }

        private void radioButtonDbgOff_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug off
            _stream.WriteRam(new byte[] { 0 }, Config.Debug.Toggle);

            // Set mode
            _stream.WriteRam(new byte[] { 0 }, Config.Debug.Setting);
        }

        private void radioButtonDbgObjCnt_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _stream.WriteRam(new byte[] { 0 }, Config.Debug.Setting);
        }

        private void radioButtonDbgChkInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _stream.WriteRam(new byte[] { 1 }, Config.Debug.Setting);
        }

        private void radioButtonDbgMapInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _stream.WriteRam(new byte[] { 2 }, Config.Debug.Setting);
        }

        private void radioButtonDbgStgInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _stream.WriteRam(new byte[] { 3 }, Config.Debug.Setting);
        }

        private void radioButtonDbgFxInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _stream.WriteRam(new byte[] { 4 }, Config.Debug.Setting);
        }

        private void radioButtonDbgEnemyInfo_CheckedChanged(object sender, EventArgs e)
        {
            // Turn debug on
            _stream.WriteRam(new byte[] { 1 }, Config.Debug.Toggle);

            // Set mode
            _stream.WriteRam(new byte[] { 5 }, Config.Debug.Setting);
        }
    }
}
