using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Structs.Configurations;
using SM64Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class FileManager : DataManager
    {
        TabPage _tabControl;
        uint _triangleAddress = 0;
        CheckBox _useMisalignmentOffsetCheckbox;
        int _closestVertex = 0;

        uint TriangleAddress
        {
            get
            {
                return _triangleAddress;
            }
            set
            {
                if (_triangleAddress == value)
                    return;

                _triangleAddress = value;

                foreach (var dataContainer in _dataControls)
                {
                    if (dataContainer is WatchVariableControl)
                    {
                        var watchVar = dataContainer as WatchVariableControl;
                        watchVar.OtherOffsets = new List<uint>() { _triangleAddress };
                    }
                }
            }
        }

        public enum TriangleMode { Floor, Wall, Ceiling, Other };

        public TriangleMode Mode = TriangleMode.Floor;

        public FileManager(ProcessStream stream, List<WatchVariable> fileData, TabPage tabControl, NoTearFlowLayoutPanel noTearFlowLayoutPanelFile)
            : base(stream, fileData, noTearFlowLayoutPanelFile)
        {
            _tabControl = tabControl;

            SplitContainer splitContainerTriangles = tabControl.Controls["splitContainerFile"] as SplitContainer;

            /*
            _useMisalignmentOffsetCheckbox = splitContainerTriangles.Panel1.Controls["checkBoxVertexMisalignment"] as CheckBox;

            (splitContainerTriangles.Panel1.Controls["radioButtonTriFloor"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Floor);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriWall"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Wall);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriCeiling"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Ceiling);
            (splitContainerTriangles.Panel1.Controls["radioButtonTriOther"] as RadioButton).CheckedChanged
                += (sender, e) => Mode_CheckedChanged(sender, e, TriangleMode.Other);
                */

        }

        private void Mode_CheckedChanged(object sender, EventArgs e, TriangleMode mode)
        {
            if (!(sender as RadioButton).Checked)
                return;

            Mode = mode;
        }

        public override void Update(bool updateView)
        {
            if (updateView)
            {
                switch (Mode)
                {
                    case TriangleMode.Ceiling:
                        TriangleAddress = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.CeilingTriangleOffset);
                        break;

                    case TriangleMode.Floor:
                        TriangleAddress = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
                        break;

                    case TriangleMode.Wall:
                        TriangleAddress = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.WallTriangleOffset);
                        break;
                }

                base.Update(updateView);
            }
        }
    }
}
