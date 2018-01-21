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

        public void AddNewControl()
        {
            VarHackContainer varHackContainer = new VarHackContainer(this);
            Controls.Add(varHackContainer);
        }

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

        public void ClearControls()
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

            //Controls.Clear();
        }

        public void ApplyVariables()
        {
            uint applyVariableAddress = 0x80370000;
            uint structSize = 0x20;

            for (int i = 0; i < Controls.Count; i++)
            {
                uint address = applyVariableAddress + (uint)i * structSize;
                VarHackContainer varHackContainer = Controls[i] as VarHackContainer;
                byte[] bytes = varHackContainer.GetLittleEndianByteArray();
                Config.Stream.WriteRamLittleEndian(bytes, address);
            }
        }
    }
}
