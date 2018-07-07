using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace STROOP.Ttc
{

    public static class TtcUtilities
    {

        public static List<TtcObject> CreateRngObjects(TtcRng rng, List<int> dustFrames = null)
        {
            List<TtcObject> rngObjects = new List<TtcObject>();
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcRotatingBlock(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcRotatingTriangularPrism(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new TtcPendulum(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, i == 0 ? 0 : 1).SetIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                if (i == 0) rngObjects.Add(new TtcPusher(rng, 20).SetIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcPusher(rng, 0).SetIndex(i + 1));
                if (i == 2) rngObjects.Add(new TtcPusher(rng, 50).SetIndex(i + 1));
                if (i == 3) rngObjects.Add(new TtcPusher(rng, 100).SetIndex(i + 1));
                if (i == 4) rngObjects.Add(new TtcPusher(rng, 0).SetIndex(i + 1));
                if (i == 5) rngObjects.Add(new TtcPusher(rng, 10).SetIndex(i + 1));
                if (i == 6) rngObjects.Add(new TtcPusher(rng, 0).SetIndex(i + 1));
                if (i == 7) rngObjects.Add(new TtcPusher(rng, 0).SetIndex(i + 1));
                if (i == 8) rngObjects.Add(new TtcPusher(rng, 0).SetIndex(i + 1));
                if (i == 9) rngObjects.Add(new TtcPusher(rng, 30).SetIndex(i + 1));
                if (i == 10) rngObjects.Add(new TtcPusher(rng, 10).SetIndex(i + 1));
                if (i == 11) rngObjects.Add(new TtcPusher(rng, 20).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcCog(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcSpinningTriangle(rng, 40960).SetIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcSpinningTriangle(rng, 57344).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcPitBlock(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcHand(rng, 40960).SetIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcHand(rng, 8192).SetIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new TtcSpinner(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcWheel(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcElevator(rng, 445, 1045).SetIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcElevator(rng, -1454, -1254).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcCog(rng).SetIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, 1).SetIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcThwomp(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcAmp(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcBobomb(rng).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                TtcDust dust = new TtcDust(rng).SetIndex(i + 1) as TtcDust;
                if (dustFrames != null) dust.AddDustFrames(dustFrames);
                rngObjects.Add(dust);
            }
            return rngObjects;
        }

        public static List<TtcObject> CreateRngObjectsFromGame(TtcRng rng, List<int> dustFrames = null)
        {
            Func<int, uint> getOffset = (int i) => (uint)i * 0x260;

            List<TtcObject> rngObjects = new List<TtcObject>();
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcRotatingBlock(rng, 0x8033D488 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcRotatingTriangularPrism(rng, 0x8033E2C8 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new TtcPendulum(rng, 0x8033E788 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, 0x8033F108 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                rngObjects.Add(new TtcPusher(rng, 0x8033FCE8 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcCog(rng, 0x80341968 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcSpinningTriangle(rng, 0x80342548 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcPitBlock(rng, 0x80342A08 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcHand(rng, 0x80342C68 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new TtcSpinner(rng, 0x80343128 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcWheel(rng, 0x80345268 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcElevator(rng, 0x80347608 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcCog(rng, 0x8034B3C8 + getOffset(i)).SetIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, 0x8034D508 + getOffset(i)).SetIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcThwomp(rng, 0x8034E808 + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcAmp(rng, 0x80347AC8).SetIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcAmp(rng, 0x8034A328).SetIndex(i + 1));
            }
            List<ObjectDataModel> bobombs = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Bob-omb");
            bobombs.Sort((obj1, obj2) =>
            {
                string label1 = Config.ObjectSlotsManager.GetSlotLabelFromObject(obj1);
                string label2 = Config.ObjectSlotsManager.GetSlotLabelFromObject(obj2);
                int pos1 = ParsingUtilities.ParseInt(label1);
                int pos2 = ParsingUtilities.ParseInt(label2);
                return pos1 - pos2;
            });
            for (int i = 0; i < bobombs.Count; i++)
            {
                rngObjects.Add(new TtcBobomb(rng, bobombs[i].Address).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                TtcDust dust = new TtcDust(rng).SetIndex(i + 1) as TtcDust;
                if (dustFrames != null) dust.AddDustFrames(dustFrames);
                rngObjects.Add(dust);
            }
            return rngObjects;
        }

        public static (TtcRng, List<TtcObject>) CreateRngObjectsFromSaveState(TtcSaveState saveState)
        {
            TtcSaveStateByteIterator iter = saveState.GetIterator();
            TtcRng rng = new TtcRng(iter.GetUShort());

            List<TtcObject> rngObjects = new List<TtcObject>();
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcRotatingBlock(rng, iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcRotatingTriangularPrism(rng, iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new TtcPendulum(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                rngObjects.Add(new TtcPusher(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcCog(rng, iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcSpinningTriangle(rng, iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcPitBlock(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcHand(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new TtcSpinner(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcWheel(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcElevator(rng, 445, 1045, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcElevator(rng, -1454, -1254, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcCog(rng, iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcThwomp(rng, iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcAmp(rng, iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcBobomb(rng, iter.GetInt(), iter.GetInt()).SetIndex(i + 1));
            }

            if (!iter.IsDone()) throw new ArgumentOutOfRangeException();

            return (rng, rngObjects);
        }
    }

}
