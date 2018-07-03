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

            //set up objects
            List<TtcObject> rngObjects = getRNGObjectsForRandomSetting();
            //Dust dust = (Dust)rngObjects.get(rngObjects.size()-1);

            //set up testing variables
            TtcRng.setRNG(0); //initial RNG during star selection screen
            int startingFrame = 0; //the frame directly preceding any object initialization
            int endingFrame = 100; //the frame you want to see printed
            bool printRNG = false; //whether to print frame/RNG/index
            bool printObjects = true; //whether to print the state of every object
            // dust.addDustFrames(); //which frames dust is present on (empty parameters = no dust)

            //iterate through frames to update objects
            int frame = startingFrame;
		    while (frame<endingFrame)
            {
			    frame++;
                foreach (TtcObject rngObject in rngObjects)
                {
                    rngObject.setFrame(frame);
                    rngObject.update();
                }
		    }
		
		    //print frame, RNG, and index
		    if (printRNG)
            {
			    StringUtilities.WriteLine(endingFrame + "\n");
                StringUtilities.WriteLine(TtcRng.getCurrentRNG() + "\n");
                StringUtilities.WriteLine("[" + TtcRng.getCurrentIndex() + "]\n");
		    }
		
		    //print each object's state
		    if (printObjects)
            {
                StringUtilities.WriteLine("");
                StringUtilities.WriteLine("");
                foreach (TtcObject rngObject in rngObjects)
                {
			        if (rngObject is TtcAmp) continue;
                    StringUtilities.WriteLine(rngObject + "\n");
		        }
                StringUtilities.WriteLine("");
                StringUtilities.WriteLine("");
            }
        }
		
	    private static List<TtcObject> getRNGObjectsForStillSetting()
        {
            List<TtcObject> rngObjects = new List<TtcObject>();
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcThwomp());
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcAmp());
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcBobomb());
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcDust().setIndex(i + 1));
            }
            return rngObjects;
        }

        private static List<TtcObject> getRNGObjectsForRandomSetting()
        {
            List<TtcObject> rngObjects = new List<TtcObject>();
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcRotatingBlock().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcRotatingTriangularPrism().setIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new TtcPendulum().setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcTreadmill(i == 0).setIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                if (i == 0) rngObjects.Add(new TtcPusher(20).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcPusher(0).setIndex(i + 1));
                if (i == 2) rngObjects.Add(new TtcPusher(50).setIndex(i + 1));
                if (i == 3) rngObjects.Add(new TtcPusher(100).setIndex(i + 1));
                if (i == 4) rngObjects.Add(new TtcPusher(0).setIndex(i + 1));
                if (i == 5) rngObjects.Add(new TtcPusher(10).setIndex(i + 1));
                if (i == 6) rngObjects.Add(new TtcPusher(0).setIndex(i + 1));
                if (i == 7) rngObjects.Add(new TtcPusher(0).setIndex(i + 1));
                if (i == 8) rngObjects.Add(new TtcPusher(0).setIndex(i + 1));
                if (i == 9) rngObjects.Add(new TtcPusher(30).setIndex(i + 1));
                if (i == 10) rngObjects.Add(new TtcPusher(10).setIndex(i + 1));
                if (i == 11) rngObjects.Add(new TtcPusher(20).setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcCog().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcSpinningTriangle(40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcSpinningTriangle(57344).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcPitBlock().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcHand(40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcHand(8192).setIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new TtcSpinner().setIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcWheel().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcElevator(445, 1045).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcElevator(-1454, -1254).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcCog().setIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcTreadmill(false).setIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcThwomp().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcAmp().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcBobomb().setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcDust().setIndex(i + 1));
            }
            return rngObjects;
        }
    }

}
