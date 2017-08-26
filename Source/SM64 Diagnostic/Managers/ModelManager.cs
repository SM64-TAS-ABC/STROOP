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

        private uint? _modelPointer = null;
        public uint? ModelPointer
        {
            get
            {
                return _modelPointer;
            }
            set
            {
                if (_modelPointer == value)
                    return;

                _modelPointer = value;

                UpdateModelPointer();
            }
        }

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
            _glControl = tabControl.Controls["glControlModelView"] as GLControl;
            _textBoxAddress = tabControl.Controls["textBoxModelAddress"] as TextBox;
            var splitContainerData = tabControl.Controls["splitContainerModel"] as SplitContainer;
            _dataGridViewVertices = splitContainerData.Panel1.Controls["dataGridViewVertices"] as DataGridView;
            _dataGridViewTriangles = splitContainerData.Panel2.Controls["dataGridViewTriangles"] as DataGridView;

            _dataGridViewVertices.SelectionChanged += _dataGridViewVertices_SelectionChanged;
            _dataGridViewTriangles.SelectionChanged += _dataGridViewTriangles_SelectionChanged;

            UpdateModelPointer();
        }

        private void _dataGridViewVertices_SelectionChanged(object sender, EventArgs e)
        {
            var selection = new bool[_dataGridViewVertices.Rows.Count];
            
            foreach (DataGridViewRow row in _dataGridViewVertices.SelectedRows)
            {
                selection[row.Index] = true;
            }

            _modelView.ChangeVertexSelection(selection);
        }

        private void _dataGridViewTriangles_SelectionChanged(object sender, EventArgs e)
        {
            var selection = new bool[_dataGridViewTriangles.Rows.Count];

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
            var vertices = new List<short[]>();
            modelPtr += 2;
            int numberOfVertices = Math.Min(Config.Stream.GetUInt16(modelPtr), (ushort) 500);
            modelPtr += 2;

            for (int i = 0; i < numberOfVertices; i++)
            {
                var x = Config.Stream.GetInt16(modelPtr);
                var y = Config.Stream.GetInt16(modelPtr + 0x02);
                var z = Config.Stream.GetInt16(modelPtr + 0x04);
                modelPtr += 0x06;
                vertices.Add(new short[3] { x, y, z });
            }

            return vertices;
        }

        public List<int[]> GetTrianglesFromContinuedModelPonter(uint contModelPtr)
        {
            var triangles = new List<int[]>();
            var type = Config.Stream.GetUInt16(contModelPtr); // Type (unused, but here anyway for doc.)
            contModelPtr += 2;
            int numberOfTriangles = Math.Min(Config.Stream.GetUInt16(contModelPtr), (ushort)(500 / 3));
            contModelPtr += 2;

            for (int i = 0; i < numberOfTriangles; i++)
            {
                var v1 = Config.Stream.GetInt16(contModelPtr);
                var v2 = Config.Stream.GetInt16(contModelPtr + 0x02);
                var v3 = Config.Stream.GetInt16(contModelPtr + 0x04);
                contModelPtr += 0x06;
                triangles.Add(new int[3] { v1, v2, v3 });
            }

            return triangles;
        }

        public void UpdateModelPointer()
        {
            if (!ModelPointer.HasValue || ModelPointer == 0)
            {
                _textBoxAddress.Text = ModelPointer == 0 ? "0x00000000" : "(None)";
                _dataGridViewVertices.Rows.Clear();
                return;
            }

            _textBoxAddress.Text = String.Format("0x{0:X8}", ModelPointer.Value);

            var modelPtr = ModelPointer.Value;
            var vertices = GetVerticesFromModelPointer(ref modelPtr);
            var triangles = GetTrianglesFromContinuedModelPonter(modelPtr);
            _modelView.ChangeModel(vertices, triangles);


            // TODO: transformation

            _dataGridViewVertices.Rows.Clear();
            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];
                _dataGridViewVertices.Rows.Add(i, v[0], v[1], v[2]);
            }

            _dataGridViewTriangles.Rows.Clear();
            for (int i = 0; i < triangles.Count; i++)
            {
                var t = triangles[i];
                _dataGridViewTriangles.Rows.Add(t[0], t[1], t[2]);
            }

        }

        public virtual void Update(bool updateView = false)
        {
            if (!_isLoaded)
                return;

            _modelView.Control.Invalidate();
        }
    }
}
