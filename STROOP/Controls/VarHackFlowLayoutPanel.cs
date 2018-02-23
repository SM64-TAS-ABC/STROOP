using STROOP.Forms;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Controls
{
    public class VarHackFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        private static byte[] EMPTY_BYTES = new byte[VarHackConfig.StructSize];

        private readonly Object _objectLock;

        public VarHackFlowLayoutPanel()
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

        public void AddNewControl(string varName, uint address, Type memoryType, bool useHex, uint? pointerOffset)
        {
            if (Controls.Count >= VarHackConfig.MaxPossibleVars) return;
            VarHackContainer varHackContainer =
                VarHackContainer.Create(
                    this, Controls.Count, varName, address, memoryType, useHex, pointerOffset);
            lock (_objectLock)
            {
                Controls.Add(varHackContainer);
            }
        }

        public void AddNewControl(string specialType)
        {
            if (Controls.Count >= VarHackConfig.MaxPossibleVars) return;
            VarHackContainer varHackContainer =
                VarHackContainer.Create(this, Controls.Count, specialType);
            lock (_objectLock)
            {
                Controls.Add(varHackContainer);
            }
        }

        // Methods for buttons to modify the controls

        public void AddNewControl()
        {
            if (Controls.Count >= VarHackConfig.MaxPossibleVars) return;
            VarHackContainer varHackContainer = VarHackContainer.Create(this, Controls.Count, true);
            lock (_objectLock)
            {
                Controls.Add(varHackContainer);
            }
        }

        private List<XElement> GetCurrentXmlElements()
        {
            List<XElement> elements = new List<XElement>();
            lock (_objectLock)
            {
                foreach (Control control in Controls)
                {
                    VarHackContainer varHackContainer = control as VarHackContainer;
                    elements.Add(varHackContainer.ToXml());
                }
            }
            return elements;
        }

        public void OpenVariables()
        {
            List<XElement> elements = FileUtilities.OpenXmlElements(FileType.StroopVarHackVariables);
            List<VarHackContainer> varHackContainers =
                elements.ConvertAll(element => VarHackContainer.Create(element));
            lock (_objectLock)
            {
                varHackContainers.ForEach(varHackContainer => Controls.Add(varHackContainer));
            }
        }

        public void SaveVariables()
        {
            FileUtilities.SaveXmlElements(
                FileType.StroopVarHackVariables, "CustomVarHackData", GetCurrentXmlElements());
        }

        public void ClearVariables()
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
            InfoForm infoForm = new InfoForm();
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
            infoForm.SetText("Var Hack Info", "Little Endian Bytes", stringBuilder.ToString());
            infoForm.Show();
        }

        public void ShowVariableBytesInBigEndian()
        {
            InfoForm infoForm = new InfoForm();
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
            infoForm.SetText("Var Hack Info", "Big Endian Bytes", stringBuilder.ToString());
            infoForm.Show();
        }

        // Methods to modify memory

        public void ApplyVariablesToMemory()
        {
            lock (_objectLock)
            {
                for (int i = 0; i < VarHackConfig.MaxPossibleVars; i++)
                {
                    ApplyVariableToMemory(i);
                }
            }
        }

        private void ApplyVariableToMemory(int index)
        {
            uint address = VarHackConfig.VarHackMemoryAddress + (uint)index * VarHackConfig.StructSize;
            byte[] bytes;
            if (index < Controls.Count)
            {
                VarHackContainer varHackContainer = Controls[index] as VarHackContainer;
                bytes = varHackContainer.GetLittleEndianByteArray();
            }
            else
            {
                bytes = EMPTY_BYTES;
            }
            if (bytes == null) return;
            Config.Stream.WriteRamLittleEndian(bytes, address);
        }

        public void ClearVariablesInMemory()
        {
            byte[] emptyBytes = new byte[VarHackConfig.StructSize];
            for (int i = 0; i < VarHackConfig.MaxPossibleVars; i++)
            {
                uint address = VarHackConfig.VarHackMemoryAddress + (uint)i * VarHackConfig.StructSize;
                Config.Stream.WriteRamLittleEndian(emptyBytes, address);
            }
        }
        
        // Update method

        public void UpdateControls()
        {
            lock (_objectLock)
            {
                for (int i = 0; i < Controls.Count; i++)
                {
                    VarHackContainer varHackContainer = Controls[i] as VarHackContainer;
                    varHackContainer.UpdateControl();
                    if (varHackContainer.UpdatesContinuously())
                    {
                        ApplyVariableToMemory(i);
                    }
                }
            }
        }

    }
}
