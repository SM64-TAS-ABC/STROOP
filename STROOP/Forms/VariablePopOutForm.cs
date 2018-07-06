using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class VariablePopOutForm : Form
    {
        public static int? WIDTH = null;
        public static int? HEIGHT = null;

        public VariablePopOutForm()
        {
            InitializeComponent();
            if (WIDTH.HasValue) Width = WIDTH.Value;
            if (HEIGHT.HasValue) Height = HEIGHT.Value;
            Resize += (sender, e) =>
            {
                WIDTH = Width;
                HEIGHT = Height;
            };
        }

        public void Initialize()
        {
            
        }
        
    }
}
