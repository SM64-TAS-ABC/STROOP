using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using System.Drawing;
using STROOP.Structs.Configurations;
using OpenTK;
using System.Data;

namespace STROOP.Managers
{
    public class ModelManager
    {
        private GLControl _glControl;
        private ModelGraphics _modelView;
        private DataGridView _dataGridViewVertices;
        private DataGridView _dataGridViewTriangles;
        private TextBox _textBoxAddress;
        private Label _labelModelVertices;
        private Label _labelModelTriangles;
        private CheckBox _checkBoxLevel;

        public uint ModelObjectAddress;

        public uint ModelPointer
        {
            get
            {
                uint modelObjectAddress = ModelObjectAddress;
                return modelObjectAddress == 0 ? 0 : Config.Stream.GetUInt32(modelObjectAddress + ObjectConfig.ModelPointerOffset);
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

        /// <summary>
        /// Mode of camera movement in the view. ManualMode indicates the camera 
        /// should fly around with user input. Otherwise a value of false indicates
        /// the camera rotates around the model (automatically).
        /// </summary>
        public bool ManualMode
        {
            get
            {
                if (_modelView == null)
                    return false;

                return _modelView.ManualMode;
            }
            set
            {
                if (_modelView != null)
                    _modelView.ManualMode = value;
            }
        }

        public ModelManager(Control tabControl)
        {
            SplitContainer splitContainerModel = tabControl.Controls["splitContainerModel"] as SplitContainer;
            _glControl = splitContainerModel.Panel2.Controls["glControlModelView"] as GLControl;
            _textBoxAddress = splitContainerModel.Panel1.Controls["textBoxModelAddress"] as TextBox;

            SplitContainer splitContainerTables = splitContainerModel.Panel1.Controls["splitContainerModelTables"] as SplitContainer;
            _dataGridViewVertices = splitContainerTables.Panel1.Controls["dataGridViewVertices"] as DataGridView;
            _labelModelVertices = splitContainerTables.Panel1.Controls["labelModelVertices"] as Label;
            _dataGridViewTriangles = splitContainerTables.Panel2.Controls["dataGridViewTriangles"] as DataGridView;
            _labelModelTriangles = splitContainerTables.Panel2.Controls["labelModelTriangles"] as Label;

            _dataGridViewVertices.SelectionChanged += _dataGridViewVertices_SelectionChanged;
            _dataGridViewTriangles.SelectionChanged += _dataGridViewTriangles_SelectionChanged;

            _checkBoxLevel = splitContainerModel.Panel1.Controls["checkBoxModelLevel"] as CheckBox;
            _checkBoxLevel.Click += CheckBoxLevel_CheckedChanged;

            UpdateModelPointer();
        }

        private void UpdateCounts()
        {
            _labelModelVertices.Text = "Vertices: " + _dataGridViewVertices.Rows.Count;
            _labelModelTriangles.Text = "Triangles: " + _dataGridViewTriangles.Rows.Count;
        }

        private void CheckBoxLevel_CheckedChanged(object sender, EventArgs e)
        {
            SwitchLevelModel();

            _textBoxAddress.Text = "(Level)";
            UpdateCounts();
            _checkBoxLevel.Checked = true;
        }

        private void SwitchLevelModel()
        {
            List<TriangleDataModel> triangleStructs = TriangleUtilities.GetLevelTriangles();

            // Build vertice and triangle list from triangle set
            List<int[]> triangles = new List<int[]>();
            List<short[]> vertices = new List<short[]>();
            List<int> surfaceTypes = new List<int>();
            triangleStructs.ForEach(t =>
            {
                var vIndex = vertices.Count;
                triangles.Add(new int[] { vIndex, vIndex + 1, vIndex + 2 });
                surfaceTypes.Add(t.SurfaceType);
                vertices.Add(new short[] { t.X1, t.Y1, t.Z1 });
                vertices.Add(new short[] { t.X2, t.Y2, t.Z2 });
                vertices.Add(new short[] { t.X3, t.Y3, t.Z3 });
            });

            _modelView?.ChangeModel(vertices, triangles);

            // Update tables
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
                _dataGridViewTriangles.Rows.Add(0, surfaceTypes[i], t[0], t[1], t[2]);
            }
            _dataGridViewTriangles.SelectAll();

            ModelObjectAddress = _previousModelPointer = 0;
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
                _textBoxAddress.Text = "(None)";
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
            _dataGridViewTriangles.SelectAll();
            _checkBoxLevel.Checked = false;
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
            UpdateCounts();

            _modelView.Control.Invalidate();
        }
    }
}
