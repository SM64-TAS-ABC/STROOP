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
            Controls.Clear();
        }

        public void ApplyVariables()
        {

            uint applyVariableAddress = 0x80370000;

            System.Diagnostics.Trace.WriteLine("-----------------------------------------");
            foreach (VarHackContainer varHackContainer in Controls)
            {
                System.Diagnostics.Trace.WriteLine(varHackContainer.ToString());
                System.Diagnostics.Trace.WriteLine("");
                Config.Stream.WriteRamLittleEndian(
                    varHackContainer.GetByteArray(), applyVariableAddress);
            }
            System.Diagnostics.Trace.WriteLine("-----------------------------------------");

        }
    }
}
