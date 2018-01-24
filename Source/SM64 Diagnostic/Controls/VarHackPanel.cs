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
        private readonly Object _objectLock;

        public VarHackPanel()
        {
            _objectLock = new Object();
        }

        // Methods for buttons on the controls

        public void MoveUpControl(VarHackContainer varHackContainer)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(varHackContainer);
                if (index == 0) return;
                int newIndex = index - 1;
                Controls.SetChildIndex(varHackContainer, newIndex);
            }
        }

        public void MoveDownControl(VarHackContainer varHackContainer)
        {
            lock (_objectLock)
            {
                int index = Controls.IndexOf(varHackContainer);
                if (index == Controls.Count - 1) return;
                int newIndex = index + 1;
                Controls.SetChildIndex(varHackContainer, newIndex);
            }
        }

        public void RemoveControl(VarHackContainer varHackContainer)
        {
            lock (_objectLock)
            {
                Controls.Remove(varHackContainer);
            }
        }

        // Methods from a watch var control

        public void AddNewControlWithParameters(string varName, uint address, Type memoryType, bool useHex, uint? pointerOffset)
        {
            if (Controls.Count >= Config.VarHack.MaxPossibleVars) return;
            VarHackContainer varHackContainer = new VarHackContainer(this, Controls.Count, varName, address, memoryType, useHex, pointerOffset);
            lock (_objectLock)
            {
                Controls.Add(varHackContainer);
            }
        }

        // Methods for buttons to modify the controls

        public void AddNewControl()
        {
            if (Controls.Count >= Config.VarHack.MaxPossibleVars) return;
            VarHackContainer varHackContainer = new VarHackContainer(this, Controls.Count, true);
            lock (_objectLock)
            {
                Controls.Add(varHackContainer);
            }
        }

        public void ClearControls()
        {
            lock (_objectLock)
            {
                Controls.Clear();
            }
        }

        public void SetPositions(int xPos, int yPos, int yDelta)
        {
            lock (_objectLock)
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    VarHackContainer varHackContainer = Controls[i] as VarHackContainer;
                    varHackContainer.SetPosition(xPos, yPos - i * yDelta);
                }
            }
        }

        // Methods to show variable bytes

        public void ShowVariableBytesInLittleEndian()
        {
            TriangleInfoForm form = new TriangleInfoForm();
            StringBuilder stringBuilder = new StringBuilder();
            lock (_objectLock)
            {
                foreach (Control control in Controls)
                {
                    VarHackContainer varHackContainer = control as VarHackContainer;
                    byte[] bytes = varHackContainer.GetLittleEndianByteArray();
                    string bytesString = VarHackContainer.ConvertBytesToString(bytes);
                    stringBuilder.Append(bytesString);
                }
            }
            form.SetTitleAndText("Little Endian Bytes", stringBuilder.ToString());
            form.Show();
        }

        public void ShowVariableBytesInBigEndian()
        {
            TriangleInfoForm form = new TriangleInfoForm();
            StringBuilder stringBuilder = new StringBuilder();
            lock (_objectLock)
            {
                foreach (Control control in Controls)
                {
                    VarHackContainer varHackContainer = control as VarHackContainer;
                    byte[] bytes = varHackContainer.GetBigEndianByteArray();
                    string bytesString = VarHackContainer.ConvertBytesToString(bytes);
                    stringBuilder.Append(bytesString);
                }
            }
            form.SetTitleAndText("Big Endian Bytes", stringBuilder.ToString());
            form.Show();
        }

        // Methods to modify memory

        public void ApplyVariablesToMemory()
        {
            lock (_objectLock)
            {
                byte[] emptyBytes = new byte[Config.VarHack.StructSize];
                for (int i = 0; i < Config.VarHack.MaxPossibleVars; i++)
                {
                    uint address = Config.VarHack.VarHackMemoryAddress + (uint)i * Config.VarHack.StructSize;
                    byte[] bytes;
                    if (i < Controls.Count)
                    {
                        VarHackContainer varHackContainer = Controls[i] as VarHackContainer;
                        bytes = varHackContainer.GetLittleEndianByteArray();
                    }
                    else
                    {
                        bytes = emptyBytes;
                    }
                    if (bytes == null) continue;
                    Config.Stream.WriteRamLittleEndian(bytes, address);
                }
            }
        }

        public void ClearVariablesInMemory()
        {
            byte[] emptyBytes = new byte[Config.VarHack.StructSize];
            for (int i = 0; i < Config.VarHack.MaxPossibleVars; i++)
            {
                uint address = Config.VarHack.VarHackMemoryAddress + (uint)i * Config.VarHack.StructSize;
                Config.Stream.WriteRamLittleEndian(emptyBytes, address);
            }
        }

        public void UpdateControls()
        {
            lock (_objectLock)
            {
                foreach (Control control in Controls)
                {
                    VarHackContainer varHackContainer = control as VarHackContainer;
                    varHackContainer.UpdateControl();
                }
            }
        }

    }
}
