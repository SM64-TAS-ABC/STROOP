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
            TtcSimulation simulation = new TtcSimulation(
                rngValue: 0,
                startingFrame: 0,
                dustFrames: new List<int>());

            simulation.Print(
                endingFrame: 1070 - 100,
                printRng: false,
                printObjects: true);
        }
    }

}
