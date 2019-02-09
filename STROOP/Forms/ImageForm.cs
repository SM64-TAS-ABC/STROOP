using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();

            buttonOpenImage.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = DialogUtilities.CreateOpenFileDialog(FileType.Image);
                DialogResult result = openFileDialog.ShowDialog();
                if (result != DialogResult.OK) return;
                string fileName = openFileDialog.FileName;
                InfoForm.ShowValue(fileName);
            };
            // rackBarTransparency
            // panelImage
        }
    }
}
