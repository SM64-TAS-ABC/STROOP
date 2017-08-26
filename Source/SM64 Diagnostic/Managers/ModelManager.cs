using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using System.Drawing;
using SM64_Diagnostic.Structs.Configurations;
using OpenTK;
using System.Data;

namespace SM64_Diagnostic.Managers
{
    public class ModelManager
    {
        private GLControl _glControl;
        private ModelGraphics _modelView;
        private DataGridView _dataGridViewVertices;
        private DataGridView _dataGridViewTriangles;
        private TextBox _textBoxAddress;

        public uint ModelObjectAddress;

        public uint ModelPointer
        {
            get
            {
                uint modelObjectAddress = ModelObjectAddress;
                return modelObjectAddress == 0 ? 0 : Config.Stream.GetUInt32(modelObjectAddress + Config.ObjectSlots.ModelPointerOffset);
            }
        }
        private uint _previousModelPointer = 0;

        private bool _isLoaded = false;
        public bool IsLoaded
        {
            get
            {
                return _isLoaded;
            }
        }

        public bool Visible
        {
            get
            {
                return _modelView.Control.Visible;
            }
            set
            {
                _modelView.Control.Visible = value;
            }
        }

        public ModelManager(Control tabControl)
        {
            SplitContainer splitContainer1 = tabControl.Controls["splitContainer1"] as SplitContainer;
            _glControl = splitContainer1.Panel2.Controls["glControlModelView"] as GLControl;
            _textBoxAddress = splitContainer1.Panel1.Controls["textBoxModelAddress"] as TextBox;
            SplitContainer splitContainerData = splitContainer1.Panel1.Controls["splitContainerModelTables"] as SplitContainer;
            _dataGridViewVertices = splitContainerData.Panel1.Controls["dataGridViewVertices"] as DataGridView;
            _dataGridViewTriangles = splitContainerData.Panel2.Controls["dataGridViewTriangles"] as DataGridView;

            _dataGridViewVertices.SelectionChanged += _dataGridViewVertices_SelectionChanged;
            _dataGridViewTriangles.SelectionChanged += _dataGridViewTriangles_SelectionChanged;

            UpdateModelPointer();
        }

        private void _dataGridViewVertices_SelectionChanged(object sender, EventArgs e)
        {
            bool[] selection = new bool[_dataGridViewVertices.Rows.Count];
            
            foreach (DataGridViewRow row in _dataGridViewVertices.SelectedRows)
            {
                selection[row.Index] = true;
            }

            _modelView.ChangeVertexSelection(selection);
        }

        private void _dataGridViewTriangles_SelectionChanged(object sender, EventArgs e)
        {
            bool[] selection = new bool[_dataGridViewTriangles.Rows.Count];

            foreach (DataGridViewRow row in _dataGridViewTriangles.SelectedRows)
            {
                selection[row.Index] = true;
            }

            _modelView.ChangeTriangleSelection(selection);
        }

        public void Load()
        {         
            // Create new graphics control
            _modelView = new ModelGraphics(_glControl);
            _modelView.Load();

            _isLoaded = true;
        }

        public List<short[]> GetVerticesFromModelPointer(ref uint modelPtr)
        {
            List<short[]> vertices = new List<short[]>();
            modelPtr += 2;
            int numberOfVertices = Math.Min(Config.Stream.GetUInt16(modelPtr), (ushort) 500);
            modelPtr += 2;

            for (int i = 0; i < numberOfVertices; i++)
            {
                short x = Config.Stream.GetInt16(modelPtr);
                short y = Config.Stream.GetInt16(modelPtr + 0x02);
                short z = Config.Stream.GetInt16(modelPtr + 0x04);
                modelPtr += 0x06;
                vertices.Add(new short[3] { x, y, z });
            }

            return vertices;
        }

        public List<int[]> GetTrianglesFromContinuedModelPonter(uint contModelPtr)
        {
            var triangles = new List<int[]>();

            for (int totalVertices = 0, group = 0; totalVertices < 500 / 2; group++)
            {
                ushort type = Config.Stream.GetUInt16(contModelPtr); // Type (unused, but here anyway for doc.)

                if (type == 0x41)
                    break;

                contModelPtr += 2;
                int numberOfTriangles = Config.Stream.GetUInt16(contModelPtr);
                contModelPtr += 2;

                totalVertices += numberOfTriangles;

                for (int i = 0; i < numberOfTriangles; i++)
                {
                    short v1 = Config.Stream.GetInt16(contModelPtr);
                    short v2 = Config.Stream.GetInt16(contModelPtr + 0x02);
                    short v3 = Config.Stream.GetInt16(contModelPtr + 0x04);
                    contModelPtr += 0x06;
                    triangles.Add(new int[] { v1, v2, v3, group, type});
                }
            }

            return triangles;
        }

        public void UpdateModelPointer()
        {
            if (ModelPointer == 0)
            {
                _textBoxAddress.Text = ModelPointer == 0 ? "0x00000000" : "(None)";
                _dataGridViewVertices.Rows.Clear();
                _dataGridViewTriangles.Rows.Clear();
                _modelView?.ClearModel();
                return;
            }

            _textBoxAddress.Text = String.Format("0x{0:X8}", ModelPointer);

            uint modelPtr = ModelPointer;
            List<short[]> vertices = GetVerticesFromModelPointer(ref modelPtr);
            List<int[]> triangles = GetTrianglesFromContinuedModelPonter(modelPtr);
            _modelView?.ChangeModel(vertices, triangles);

            // TODO: transformation

            _dataGridViewVertices.Rows.Clear();
            for (int i = 0; i < vertices.Count; i++)
            {
                short[] v = vertices[i];
                _dataGridViewVertices.Rows.Add(i, v[0], v[1], v[2]);
            }

            _dataGridViewTriangles.Rows.Clear();
            for (int i = 0; i < triangles.Count; i++)
            {
                int[] t = triangles[i];
                _dataGridViewTriangles.Rows.Add(t[3], t[4], t[0], t[1], t[2]);
            }

        }

        public virtual void Update(bool updateView = false)
        {
            if (!_isLoaded)
                return;

            uint currentModelPointer = ModelPointer;
            if (currentModelPointer != _previousModelPointer)
            {
                _previousModelPointer = currentModelPointer;
                UpdateModelPointer();
            }

            _modelView.Control.Invalidate();
        }
    }
}
