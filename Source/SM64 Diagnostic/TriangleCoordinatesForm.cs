using SM64_Diagnostic.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic
{
    public partial class TriangleCoordinatesForm : Form
    {
        short[] _coordinates;

        public TriangleCoordinatesForm(short[] coordinates)
        {
            InitializeComponent();
            _coordinates = coordinates;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void VariableViewerForm_Load(object sender, EventArgs e)
        {
            textBoxN64AddValue.Text =
                _coordinates[0] + "\t" + _coordinates[1] + "\t" + _coordinates[2] + "\r\n" +
                _coordinates[3] + "\t" + _coordinates[4] + "\t" + _coordinates[5] + "\r\n" +
                _coordinates[6] + "\t" + _coordinates[7] + "\t" + _coordinates[8];
        }
    }
}
