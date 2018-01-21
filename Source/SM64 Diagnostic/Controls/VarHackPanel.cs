using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VarHackPanel : NoTearFlowLayoutPanel
    {
        public VarHackPanel()
        {
        }

        // Methods for buttons on the controls

        public void MoveUpControl(VarHackContainer varHackContainer)
        {
            int index = Controls.IndexOf(varHackContainer);
            if (index == 0) return;
            int newIndex = index - 1;
            Controls.SetChildIndex(varHackContainer, newIndex);
        }

        public void MoveDownControl(VarHackContainer varHackContainer)
        {
            int index = Controls.IndexOf(varHackContainer);
            if (index == Controls.Count - 1) return;
            int newIndex = index + 1;
            Controls.SetChildIndex(varHackContainer, newIndex);
        }

        public void RemoveControl(VarHackContainer varHackContainer)
        {
            Controls.Remove(varHackContainer);
        }

        // Methods for buttons to modify the controls

        public void AddNewControl()
        {
            VarHackContainer varHackContainer = new VarHackContainer(this, Controls.Count);
            Controls.Add(varHackContainer);
        }

        public void ClearControls()
        {
            Controls.Clear();
        }

        // Methods to show variable bytes

        public void ShowVariableBytesInLittleEndian()
        {
            TriangleInfoForm form = new TriangleInfoForm();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Control control in Controls)
            {
                VarHackContainer varHackContainer = control as VarHackContainer;
                byte[] bytes = varHackContainer.GetLittleEndianByteArray();
                string bytesString = VarHackContainer.ConvertBytesToString(bytes);
                stringBuilder.Append(bytesString);
            }
            form.SetTitleAndText("Little Endian Bytes", stringBuilder.ToString());
            form.Show();
        }

        public void ShowVariableBytesInBigEndian()
        {
            TriangleInfoForm form = new TriangleInfoForm();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Control control in Controls)
            {
                VarHackContainer varHackContainer = control as VarHackContainer;
                byte[] bytes = varHackContainer.GetBigEndianByteArray();
                string bytesString = VarHackContainer.ConvertBytesToString(bytes);
                stringBuilder.Append(bytesString);
            }
            form.SetTitleAndText("Big Endian Bytes", stringBuilder.ToString());
            form.Show();
        }

        // Methods to modify memory

        public void ApplyVariablesToMemory()
        {
            ClearVariablesInMemory();

            uint applyVariableAddress = 0x80370000;
            uint structSize = 0x20;

            for (int i = 0; i < Controls.Count; i++)
            {
                uint address = applyVariableAddress + (uint)i * structSize;
                VarHackContainer varHackContainer = Controls[i] as VarHackContainer;
                byte[] bytes = varHackContainer.GetLittleEndianByteArray();
                if (bytes == null) continue;
                Config.Stream.WriteRamLittleEndian(bytes, address);
            }
        }

        public void ClearVariablesInMemory()
        {
            uint applyVariableAddress = 0x80370000;
            uint structSize = 0x20;
            int maxPossibleVars = 432;

            byte[] emptyBytes = new byte[structSize];
            for (int i = 0; i < maxPossibleVars; i++)
            {
                uint address = applyVariableAddress + (uint)i * structSize;
                Config.Stream.WriteRamLittleEndian(emptyBytes, address);
            }
        }

    }
}
