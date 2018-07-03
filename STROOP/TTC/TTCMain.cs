using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** This class creates all the RNGObjects and updates them
     *  for an arbitrary number of frames. Hence, this class is
     *  the hub of the project and where all the testing
     *  is done.
     */
    public class TtcMain
    {

        public static void TtcMainMethod()
        {
            TtcSimulation simulation;

            if (KeyboardUtilities.IsCtrlHeld())
            {
                simulation = new TtcSimulation(0, 100);
            }
            else
            {
                simulation = new TtcSimulation();
            }

            simulation.Print(
                endingFrame: (int)GotoRetrieveConfig.GotoAboveOffset,
                printRng: false,
                printObjects: true);
        }
    }

}
