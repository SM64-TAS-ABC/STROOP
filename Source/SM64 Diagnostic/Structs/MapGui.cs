using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace SM64_Diagnostic.Structs
{
    public class MapGui
    {
        public GLControl GLControl;
        public Label QpuValueLabel;
        public Label PuValueLabel;
        public Label MapIdLabel;
        public Label MapNameLabel;
        public Label MapSubNameLabel;
        public TrackBar MapIconSizeTrackbar;
        public TrackBar MapZoomTrackbar;
        public CheckBox MapShowMario;
        public CheckBox MapShowInactiveObjects;
        public CheckBox MapShowObjects;
        public CheckBox MapShowHolp;
    }
}
