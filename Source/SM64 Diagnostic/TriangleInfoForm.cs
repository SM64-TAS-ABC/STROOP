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
    public partial class TriangleInfoForm : Form
    {
        public TriangleInfoForm()
        {
            InitializeComponent();
            textBoxTriangleInfo.Click += (sender, e) => textBoxTriangleInfo.SelectAll();
        }

        public void SetCoordinates(short[] coordinates)
        {
            labelTitle.Text = "Triangle Coordinates";
            textBoxTriangleInfo.Text = StringifyCoordinates(coordinates);
        }

        public void SetEquation(float normalX, float normalY, float normalZ, float normalOffset)
        {
            labelTitle.Text = "Triangle Equation";
            textBoxTriangleInfo.Text =
                normalX + "x + " + normalY + "y + " + normalZ + "z + " + normalOffset + " = 0";
        }

        public void SetData(List<short[]> coordinateList)
        {
            labelTitle.Text = "Triangle Data";
            textBoxTriangleInfo.Text = String.Join(
                "\r\n\r\n",
                coordinateList.ConvertAll(
                    coordinates => StringifyCoordinates(coordinates, true)));
        }

        private String StringifyCoordinates(short[] coordinates, bool repeatCoordinates = false)
        {
            if (coordinates.Length != 9) throw new ArgumentOutOfRangeException();

            string text =
                coordinates[0] + "\t" + coordinates[1] + "\t" + coordinates[2] + "\r\n" +
                coordinates[3] + "\t" + coordinates[4] + "\t" + coordinates[5] + "\r\n" +
                coordinates[6] + "\t" + coordinates[7] + "\t" + coordinates[8];

            if (repeatCoordinates)
            {
                text += "\r\n" + coordinates[0] + "\t" + coordinates[1] + "\t" + coordinates[2];
            }

            return text;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void VariableViewerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
