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

        public void DuplicateControl(VarHackContainer varHackContainer)
        {
            if (Controls.Count >= VarHackConfig.MaxPossibleVars) return;
            int index = Controls.IndexOf(varHackContainer);
            VarHackContainer duplicate = varHackContainer.Clone();
            lock (_objectLock)
            {
                Controls.Add(duplicate);
                Controls.SetChildIndex(duplicate, index + 1);
            }
        }

        public void ConvertToHexIntVersions(VarHackContainer varHackContainer)
        {
            if (Controls.Count >= VarHackConfig.MaxPossibleVars) return;
            int index = Controls.IndexOf(varHackContainer);
            (VarHackContainer v1, VarHackContainer v2) = varHackContainer.GetHexIntVersions();
            lock (_objectLock)
            {
                Controls.Add(v1);
                Controls.Add(v2);
                Controls.SetChildIndex(v1, index + 1);
                Controls.SetChildIndex(v2, index + 2);
                Controls.Remove(varHackContainer);
            }
        }

        public void ApplyVariableToMemory(VarHackContainer varHackContainer)
        {
            int index = Controls.IndexOf(varHackContainer);
            ApplyVariableToMemory(index);
        }

        // Methods from a watch var control

        public void AddNewControl(string varName, uint address, Type memoryType, bool useHex, uint? pointerOffset)
        {
            if (Controls.Count >= VarHackConfig.MaxPossibleVars) return;
            VarHackContainer varHackContainer =
                VarHackContainer.CreateWithParameters(
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
                VarHackContainer.CreateSpecial(this, Controls.Count, specialType);
            lock (_objectLock)
            {
                Controls.Add(varHackContainer);
            }
        }

        // Methods for buttons to modify the controls

        public void AddNewControl()
        {
            if (Controls.Count >= VarHackConfig.MaxPossibleVars) return;
            VarHackContainer varHackContainer = VarHackContainer.CreateDefault(this, Controls.Count);
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
            List<XElement> elements = DialogUtilities.OpenXmlElements(FileType.StroopVarHackVariables);
            List<VarHackContainer> varHackContainers =
                elements.ConvertAll(element => VarHackContainer.CreateFromXml(this, element));
            lock (_objectLock)
            {
                varHackContainers.ForEach(varHackContainer => Controls.Add(varHackContainer));
            }
        }

        public void SaveVariables()
        {
            DialogUtilities.SaveXmlElements(
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
            Config.Stream.WriteRam(bytes, address, EndianessType.Little);
        }

        public void ClearVariablesInMemory()
        {
            byte[] emptyBytes = new byte[VarHackConfig.StructSize];
            for (int i = 0; i < VarHackConfig.MaxPossibleVars; i++)
            {
                uint address = VarHackConfig.VarHackMemoryAddress + (uint)i * VarHackConfig.StructSize;
                Config.Stream.WriteRam(emptyBytes, address, EndianessType.Little);
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
                    if (varHackContainer.UpdatesContinuously())
                    {
                        ApplyVariableToMemory(i);
                    }
                }
            }
        }

    }
}
