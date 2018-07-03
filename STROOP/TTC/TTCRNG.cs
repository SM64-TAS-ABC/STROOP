using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** RNGManger is a static class that manages the RNG.
     *  This includes keeping track of the RNG and its index,
     *  letting objects poll the next RNG value, and being able
     *  to set any RNG or index.
     */
    public class TtcRng
    {

        //start off with RNG 0 by default
        private int index;

        public TtcRng(int indexIn)
        {
            index = indexIn;
        }

        /** Get the current RNG (will not update RNG).
         */
        public ushort getCurrentRNG()
        {
            return RngIndexer.GetRngValue(index);
        }

        /** Get the current index (will not update RNG).
         */
        public int getCurrentIndex()
        {
            return index;
        }

        /** Poll (or call) RNG. This method updates RNG to the
         *  next RNG value and returns that new value,
         *  exactly mimicking the in-game RNG.
         */
        public ushort pollRNG()
        {
            index = (index + 1) % 65114;
            return getCurrentRNG();
        }

        /** Sets the RNG/index using an inputted index.
         */
        public void setOrder(int indexIn)
        {
            index = indexIn;
        }

        /** Sets the RNG/index using an inputted RNG value.
         */
        public void setRNG(ushort rngValueIn)
        {
            index = RngIndexer.GetRngIndex(rngValueIn);
        }

    }


}
