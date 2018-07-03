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
            List<RNGObject> rngObjects = getRNGObjectsForRandomSetting();
            //Dust dust = (Dust)rngObjects.get(rngObjects.size()-1);

            //set up testing variables
            RNGManager.setRNG(0); //initial RNG during star selection screen
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
                foreach (RNGObject rngObject in rngObjects)
                {
                    rngObject.setFrame(frame);
                    rngObject.update();
                }
		    }
		
		    //print frame, RNG, and index
		    if (printRNG)
            {
			    StringUtilities.WriteLine(endingFrame + "\n");
                StringUtilities.WriteLine(RNGManager.getCurrentRNG() + "\n");
                StringUtilities.WriteLine("[" + RNGManager.getCurrentIndex() + "]\n");
		    }
		
		    //print each object's state
		    if (printObjects)
            {
                StringUtilities.WriteLine("");
                StringUtilities.WriteLine("");
                foreach (RNGObject rngObject in rngObjects)
                {
			        if (rngObject is Amp) continue;
                    StringUtilities.WriteLine(rngObject + "\n");
		        }
                StringUtilities.WriteLine("");
                StringUtilities.WriteLine("");
            }
        }
		
	    private static List<RNGObject> getRNGObjectsForStillSetting()
        {
            List<RNGObject> rngObjects = new List<RNGObject>();
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new Thwomp());
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new Amp());
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new Bobomb());
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new Dust().setIndex(i + 1));
            }
            return rngObjects;
        }

        private static List<RNGObject> getRNGObjectsForRandomSetting()
        {
            List<RNGObject> rngObjects = new List<RNGObject>();
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new RotatingBlock().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new RotatingTriangularPrism().setIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new Pendulum().setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new Treadmill(i == 0).setIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                if (i == 0) rngObjects.Add(new Pusher(20).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new Pusher(0).setIndex(i + 1));
                if (i == 2) rngObjects.Add(new Pusher(50).setIndex(i + 1));
                if (i == 3) rngObjects.Add(new Pusher(100).setIndex(i + 1));
                if (i == 4) rngObjects.Add(new Pusher(0).setIndex(i + 1));
                if (i == 5) rngObjects.Add(new Pusher(10).setIndex(i + 1));
                if (i == 6) rngObjects.Add(new Pusher(0).setIndex(i + 1));
                if (i == 7) rngObjects.Add(new Pusher(0).setIndex(i + 1));
                if (i == 8) rngObjects.Add(new Pusher(0).setIndex(i + 1));
                if (i == 9) rngObjects.Add(new Pusher(30).setIndex(i + 1));
                if (i == 10) rngObjects.Add(new Pusher(10).setIndex(i + 1));
                if (i == 11) rngObjects.Add(new Pusher(20).setIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new Cog().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new SpinningTriangle(40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new SpinningTriangle(57344).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new PitBlock().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new Hand(40960).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new Hand(8192).setIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new Spinner().setIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new Wheel().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new Elevator(445, 1045).setIndex(i + 1));
                if (i == 1) rngObjects.Add(new Elevator(-1454, -1254).setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new Cog().setIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new Treadmill(false).setIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new Thwomp().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new Amp().setIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new Bobomb().setIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new Dust().setIndex(i + 1));
            }
            return rngObjects;
        }
    }

}
