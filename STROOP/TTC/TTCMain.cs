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
            TtcRng rng = new TtcRng(0); //initial RNG during star selection screen
            List<TtcObject> rngObjects = getRNGObjectsForRandomSetting(rng);
            //Dust dust = (Dust)rngObjects.get(rngObjects.size()-1);

            //set up testing variables
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
                StringUtilities.WriteLine(rng.getCurrentRNG() + "\n");
                StringUtilities.WriteLine("[" + rng.getCurrentIndex() + "]\n");
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
		
        private static List<TtcObject> getRNGObjectsForRandomSetting(TtcRng rng)
        {
            List<TtcObject> rngObjects = new List<TtcObject>();
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcRotatingBlock(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcRotatingTriangularPrism(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new TtcPendulum(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, i == 0).setIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                if (i == 0) rngObjects.Add(new TtcPusher(rng, 20).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcPusher(rng, 0).setIndex(i + 1));
                if (i == 2) rngObjects.Add(new TtcPusher(rng, 50).setIndex(i + 1));
                if (i == 3) rngObjects.Add(new TtcPusher(rng, 100).setIndex(i + 1));
                if (i == 4) rngObjects.Add(new TtcPusher(rng, 0).setIndex(i + 1));
                if (i == 5) rngObjects.Add(new TtcPusher(rng, 10).setIndex(i + 1));
                if (i == 6) rngObjects.Add(new TtcPusher(rng, 0).setIndex(i + 1));
                if (i == 7) rngObjects.Add(new TtcPusher(rng, 0).setIndex(i + 1));
                if (i == 8) rngObjects.Add(new TtcPusher(rng, 0).setIndex(i + 1));
                if (i == 9) rngObjects.Add(new TtcPusher(rng, 30).setIndex(i + 1));
                if (i == 10) rngObjects.Add(new TtcPusher(rng, 10).setIndex(i + 1));
                if (i == 11) rngObjects.Add(new TtcPusher(rng, 20).setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcCog(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcSpinningTriangle(rng, 40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcSpinningTriangle(rng, 57344).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcPitBlock(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcHand(rng, 40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcHand(rng, 8192).setIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new TtcSpinner(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcWheel(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcElevator(rng, 445, 1045).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcElevator(rng , - 1454, -1254).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcCog(rng).setIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, false).setIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcThwomp(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcAmp(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcBobomb(rng).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcDust(rng).setIndex(i + 1));
            }
            return rngObjects;
        }
    }

}
