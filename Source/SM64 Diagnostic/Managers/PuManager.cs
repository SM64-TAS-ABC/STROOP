using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Managers
{
    public class PuManager
    {
        Control _puController;
        ProcessStream _stream;

        enum PuControl { Home, PuUp, PuDown, PuLeft, PuRight, QpuUp, QpuDown, QpuLeft, QpuRight };

        public PuManager(Control puController)
        {
            _stream = Config.Stream;
            _puController = puController;

            // Pu Controller initialize and register click events
            _puController.Controls["buttonPuConHome"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.Home);
            _puController.Controls["buttonPuConZnQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuUp);
            _puController.Controls["buttonPuConZpQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuDown);
            _puController.Controls["buttonPuConXnQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuLeft);
            _puController.Controls["buttonPuConXpQpu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.QpuRight);
            _puController.Controls["buttonPuConZnPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuUp);
            _puController.Controls["buttonPuConZpPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuDown);
            _puController.Controls["buttonPuConXnPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuLeft);
            _puController.Controls["buttonPuConXpPu"].Click += (sender, e) => PuControl_Click(sender, e, PuControl.PuRight);
        }

        private void PuControl_Click(object sender, EventArgs e, PuControl controlType)
        {
            switch (controlType)
            {
                case PuControl.Home:
                    PuUtilities.MoveToPu(_stream, 0, 0, 0);
                    break;
                case PuControl.PuUp:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, -1);
                    break;
                case PuControl.PuDown:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, 1);
                    break;
                case PuControl.PuLeft:
                    PuUtilities.MoveToRelativePu(_stream, -1, 0, 0);
                    break;
                case PuControl.PuRight:
                    PuUtilities.MoveToRelativePu(_stream, 1, 0, 0);
                    break;
                case PuControl.QpuUp:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, -4);
                    break;
                case PuControl.QpuDown:
                    PuUtilities.MoveToRelativePu(_stream, 0, 0, 4);
                    break;
                case PuControl.QpuLeft:
                    PuUtilities.MoveToRelativePu(_stream, -4, 0, 0);
                    break;
                case PuControl.QpuRight:
                    PuUtilities.MoveToRelativePu(_stream, 4, 0, 0);
                    break;
            }
        }

        public void Update(bool updateView)
        {
            if (!updateView)
                return;

            //base.Update();
            //ProcessSpecialVars();

            _puController.Controls["labelPuConPuValue"].Text = PuUtilities.GetPuPosString(_stream);
            _puController.Controls["labelPuConQpuValue"].Text = PuUtilities.GetQpuPosString(_stream);
        }
    }
}
