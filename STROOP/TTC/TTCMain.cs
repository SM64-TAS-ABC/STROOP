using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.TTC
{
    /** This class creates all the RNGObjects and updates them
     *  for an arbitrary number of frames. Hence, this class is
     *  the hub of the project and where all the testing
     *  is done.
     */
    public class TTCMain
    {

        public static void TTCMainMethod()
        {

            //set up objects
            List<TTCObject> rngObjects = getRNGObjectsForRandomSetting();
            //Dust dust = (Dust)rngObjects.get(rngObjects.size()-1);

            //set up testing variables
            TTCRng.setRNG(0); //initial RNG during star selection screen
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
                foreach (TTCObject rngObject in rngObjects)
                {
                    rngObject.setFrame(frame);
                    rngObject.update();
                }
		    }
		
		    //print frame, RNG, and index
		    if (printRNG)
            {
			    StringUtilities.WriteLine(endingFrame + "\n");
                StringUtilities.WriteLine(TTCRng.getCurrentRNG() + "\n");
                StringUtilities.WriteLine("[" + TTCRng.getCurrentIndex() + "]\n");
		    }
		
		    //print each object's state
		    if (printObjects)
            {
                StringUtilities.WriteLine("");
                StringUtilities.WriteLine("");
                foreach (TTCObject rngObject in rngObjects)
                {
			        if (rngObject is TTCAmp) continue;
                    StringUtilities.WriteLine(rngObject + "\n");
		        }
                StringUtilities.WriteLine("");
                StringUtilities.WriteLine("");
            }
        }
		
	    private static List<TTCObject> getRNGObjectsForStillSetting()
        {
            List<TTCObject> rngObjects = new List<TTCObject>();
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TTCThwomp());
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TTCAmp());
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TTCBobomb());
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TTCDust().setIndex(i + 1));
            }
            return rngObjects;
        }

        private static List<TTCObject> getRNGObjectsForRandomSetting()
        {
            List<TTCObject> rngObjects = new List<TTCObject>();
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TTCRotatingBlock().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TTCRotatingTriangularPrism().setIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new TTCPendulum().setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TTCTreadmill(i == 0).setIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                if (i == 0) rngObjects.Add(new TTCPusher(20).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TTCPusher(0).setIndex(i + 1));
                if (i == 2) rngObjects.Add(new TTCPusher(50).setIndex(i + 1));
                if (i == 3) rngObjects.Add(new TTCPusher(100).setIndex(i + 1));
                if (i == 4) rngObjects.Add(new TTCPusher(0).setIndex(i + 1));
                if (i == 5) rngObjects.Add(new TTCPusher(10).setIndex(i + 1));
                if (i == 6) rngObjects.Add(new TTCPusher(0).setIndex(i + 1));
                if (i == 7) rngObjects.Add(new TTCPusher(0).setIndex(i + 1));
                if (i == 8) rngObjects.Add(new TTCPusher(0).setIndex(i + 1));
                if (i == 9) rngObjects.Add(new TTCPusher(30).setIndex(i + 1));
                if (i == 10) rngObjects.Add(new TTCPusher(10).setIndex(i + 1));
                if (i == 11) rngObjects.Add(new TTCPusher(20).setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TTCCog().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TTCSpinningTriangle(40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TTCSpinningTriangle(57344).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TTCPitBlock().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TTCHand(40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TTCHand(8192).setIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new TTCSpinner().setIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TTCWheel().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TTCElevator(445, 1045).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TTCElevator(-1454, -1254).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TTCCog().setIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TTCTreadmill(false).setIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TTCThwomp().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TTCAmp().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TTCBobomb().setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TTCDust().setIndex(i + 1));
            }
            return rngObjects;
        }
    }

}
