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
                rngObjects.Add(new TtcTreadmill(rng, i + 2).SetIndex(i + 6));
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
                rngObjects.Add(new TtcRotatingBlock(rng, TtcObjectConfig.TtcRotatingBlockAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcRotatingTriangularPrism(rng, TtcObjectConfig.TtcRotatingTriangularPrismAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                rngObjects.Add(new TtcPendulum(rng, TtcObjectConfig.TtcPendulumAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, TtcObjectConfig.TtcTreadmill1Address + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 12; i++)
            {
                rngObjects.Add(new TtcPusher(rng, TtcObjectConfig.TtcPusherAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 5; i++)
            {
                rngObjects.Add(new TtcCog(rng, TtcObjectConfig.TtcCog1Address + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcSpinningTriangle(rng, TtcObjectConfig.TtcSpinningTriangleAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcPitBlock(rng, TtcObjectConfig.TtcPitBlockAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcHand(rng, TtcObjectConfig.TtcHandAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 14; i++)
            {
                rngObjects.Add(new TtcSpinner(rng, TtcObjectConfig.TtcSpinnerAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 6; i++)
            {
                rngObjects.Add(new TtcWheel(rng, TtcObjectConfig.TtcWheelAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcElevator(rng, TtcObjectConfig.TtcElevatorAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcCog(rng, TtcObjectConfig.TtcCog2Address + getOffset(i)).SetIndex(i + 6));
            }
            for (int i = 0; i < 2; i++)
            {
                rngObjects.Add(new TtcTreadmill(rng, TtcObjectConfig.TtcTreadmill2Address + getOffset(i)).SetIndex(i + 6));
            }
            for (int i = 0; i < 1; i++)
            {
                rngObjects.Add(new TtcThwomp(rng, TtcObjectConfig.TtcThwompAddress + getOffset(i)).SetIndex(i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) rngObjects.Add(new TtcAmp(rng, TtcObjectConfig.TtcAmp1Address).SetIndex(i + 1));
                if (i == 1) rngObjects.Add(new TtcAmp(rng, TtcObjectConfig.TtcAmp2Address).SetIndex(i + 1));
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
            for (int i = 0; i < 1; i++)
            {
                TtcDust dust = new TtcDust(rng).SetIndex(i + 1) as TtcDust;
                // if (dustFrames != null) dust.AddDustFrames(dustFrames);
                rngObjects.Add(dust);
            }

            if (!iter.IsDone()) throw new ArgumentOutOfRangeException();

            return (rng, rngObjects);
        }

        public static List<uint> GetObjectAddresses()
        {
            List<uint> addresses = new List<uint>();
            Func<int, uint> getOffset = (int i) => (uint)i * 0x260;
            for (int i = 0; i < 6; i++)
            {
                addresses.Add(TtcObjectConfig.TtcRotatingBlockAddress + getOffset(i));
            }
            for (int i = 0; i < 2; i++)
            {
                addresses.Add(TtcObjectConfig.TtcRotatingTriangularPrismAddress + getOffset(i));
            }
            for (int i = 0; i < 4; i++)
            {
                addresses.Add(TtcObjectConfig.TtcPendulumAddress + getOffset(i));
            }
            for (int i = 0; i < 5; i++)
            {
                addresses.Add(TtcObjectConfig.TtcTreadmill1Address + getOffset(i));
            }
            for (int i = 0; i < 12; i++)
            {
                addresses.Add(TtcObjectConfig.TtcPusherAddress + getOffset(i));
            }
            for (int i = 0; i < 5; i++)
            {
                addresses.Add(TtcObjectConfig.TtcCog1Address + getOffset(i));
            }
            for (int i = 0; i < 2; i++)
            {
                addresses.Add(TtcObjectConfig.TtcSpinningTriangleAddress + getOffset(i));
            }
            for (int i = 0; i < 1; i++)
            {
                addresses.Add(TtcObjectConfig.TtcPitBlockAddress + getOffset(i));
            }
            for (int i = 0; i < 2; i++)
            {
                addresses.Add(TtcObjectConfig.TtcHandAddress + getOffset(i));
            }
            for (int i = 0; i < 14; i++)
            {
                addresses.Add(TtcObjectConfig.TtcSpinnerAddress + getOffset(i));
            }
            for (int i = 0; i < 6; i++)
            {
                addresses.Add(TtcObjectConfig.TtcWheelAddress + getOffset(i));
            }
            for (int i = 0; i < 2; i++)
            {
                addresses.Add(TtcObjectConfig.TtcElevatorAddress + getOffset(i));
            }
            for (int i = 0; i < 1; i++)
            {
                addresses.Add(TtcObjectConfig.TtcCog2Address + getOffset(i));
            }
            for (int i = 0; i < 2; i++)
            {
                addresses.Add(TtcObjectConfig.TtcTreadmill2Address + getOffset(i));
            }
            for (int i = 0; i < 1; i++)
            {
                addresses.Add(TtcObjectConfig.TtcThwompAddress + getOffset(i));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) addresses.Add(TtcObjectConfig.TtcAmp1Address);
                if (i == 1) addresses.Add(TtcObjectConfig.TtcAmp2Address);
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
                addresses.Add(bobombs[i].Address);
            }
            return addresses;
        }

        public static void ApplySaveState(TtcSaveState saveState)
        {
            (TtcRng rng, List<TtcObject> objects) = CreateRngObjectsFromSaveState(saveState);
            List<uint> addresses = GetObjectAddresses();
            if (objects.Count != addresses.Count + 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            Config.Stream.SetValue(rng.GetRng(), MiscConfig.RngAddress);
            for (int i = 0; i < addresses.Count; i++)
            {
                objects[i].ApplyToAddress(addresses[i]);
            }
        }
    }

}
