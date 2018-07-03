﻿using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
            buttonOk.Click += (sender, e) => Close();
            textBoxTriangleInfo.DoubleClick += (sender, e) => textBoxTriangleInfo.SelectAll();
        }

        public void SetTriangleCoordinates(short[] coordinates)
        {
            this.Text = "Triangle Info";
            textBoxTitle.Text = "Triangle Coordinates";
            textBoxTriangleInfo.Text = StringifyCoordinates(coordinates);
        }

        public void SetTriangleEquation(float normalX, float normalY, float normalZ, float normalOffset)
        {
            this.Text = "Triangle Info";
            textBoxTitle.Text = "Triangle Equation";
            textBoxTriangleInfo.Text =
                normalX + "x + " + normalY + "y + " + normalZ + "z + " + normalOffset + " = 0";
        }

        public void SetTriangleData(List<short[]> coordinateList, bool repeatFirstVertex)
        {
            this.Text = "Triangle Info";
            textBoxTitle.Text = "Triangle Data";
            textBoxTriangleInfo.Text = String.Join(
                "\r\n\r\n",
                coordinateList.ConvertAll(
                    coordinates => StringifyCoordinates(coordinates, repeatFirstVertex)));
        }

        public void SetTriangleVertices(List<short[]> coordinateList)
        {
            this.Text = "Triangle Info";
            textBoxTitle.Text = "Triangle Vertices";
            List<short[]> vertexList = new List<short[]>();
            coordinateList.ForEach(
                coordinates =>
                {
                    vertexList.Add(new short[] { coordinates[0], coordinates[1], coordinates[2] });
                    vertexList.Add(new short[] { coordinates[3], coordinates[4], coordinates[5] });
                    vertexList.Add(new short[] { coordinates[6], coordinates[7], coordinates[8] });
                });

            List<short[]> uniqueVertexList = new List<short[]>();
            vertexList.ForEach(
                vertex =>
                {
                    bool hasAlready = uniqueVertexList.Any(v => Enumerable.SequenceEqual(v, vertex));
                    if (!hasAlready) uniqueVertexList.Add(vertex);
                });

            uniqueVertexList.Sort(
                (short[] v1, short[] v2) =>
                {
                    int diff = v1[0] - v2[0];
                    if (diff != 0) return diff;
                    diff = v1[1] - v2[1];
                    if (diff != 0) return diff;
                    diff = v1[2] - v2[2];
                    return diff;
                });

            textBoxTriangleInfo.Text = String.Join(
                "\r\n",
                uniqueVertexList.ConvertAll(
                    coordinate => StringifyCoordinate(coordinate)));
        }

        public void SetTriangles(List<TriangleDataModel> triangleList)
        {
            this.Text = "Triangle Info";
            textBoxTitle.Text = "Triangles";
            textBoxTriangleInfo.Text = TriangleDataModel.GetFieldNameString() + "\n" + String.Join("\n", triangleList);
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

        private String StringifyCoordinate(short[] coordinate)
        {
            if (coordinate.Length != 3) throw new ArgumentOutOfRangeException();
            string text = coordinate[0] + "\t" + coordinate[1] + "\t" + coordinate[2];
            return text;
        }

        public void SetDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string keyName = null, string valueName = null)
        {
            this.Text = "Dictionary Info";
            textBoxTitle.Text = "Dictionary";
            String text = "";
            if (keyName != null && valueName != null)
            {
                text += (keyName + "\t" + valueName + "\r\n");
            }
            foreach (KeyValuePair<TKey, TValue> entry in dictionary)
            {
                text += (entry.Key + "\t" + entry.Value + "\r\n");
            }
            textBoxTriangleInfo.Text = text;
        }

        public void SetText(string formTitle, string textTitle, string text)
        {
            this.Text = formTitle;
            textBoxTitle.Text = textTitle;
            textBoxTriangleInfo.Text = text;
        }

        public static void ShowText(string formTitle, string textTitle, string text)
        {
            InfoForm infoForm = new InfoForm();
            infoForm.SetText(formTitle, textTitle, text);
            infoForm.Show();
        }

        public static void ShowValue(object value)
        {
            InfoForm infoForm = new InfoForm();
            infoForm.SetText("Info", "Text", value.ToString());
            infoForm.Show();
        }
    }
}
