using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class ImageForm : Form
    {
        private Image _baseImage = null;

        public ImageForm()
        {
            InitializeComponent();

            buttonOpenImage.Click += (sender, e) =>
            {
                OpenFileDialog openFileDialog = DialogUtilities.CreateOpenFileDialog(FileType.Image);
                DialogResult result = openFileDialog.ShowDialog();
                if (result != DialogResult.OK) return;
                string fileName = openFileDialog.FileName;
                _baseImage = Image.FromFile(fileName);
                pictureBoxImage.BackgroundImage = _baseImage;
            };

            trackBarTransparency.ValueChanged += (sender, e) =>
            {
                byte newAlpha = (byte)(trackBarTransparency.Value / 100.0 * 255.0);
                Image newImage = ImageUtilities.ChangeTransparency(_baseImage, newAlpha);
                pictureBoxImage.BackgroundImage = newImage;
            };
        }
    }
}
