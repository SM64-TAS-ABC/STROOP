using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class WaterMain
    {
        public static void Test()
        {
            List<Input> inputs = new List<Input>();
            ObjectSlots objSlots = new ObjectSlots(inputs);
            Config.Print(objSlots);
            for (int i = 0; i < 10; i++)
            {
                objSlots.Update();
                Config.Print(objSlots);
                Config.Print();
            }
        }
    }

    public class ObjectSlots
    {
        public List<List<WaterObject>> ObjectLists;

        public ObjectSlots(List<Input> inputs)
        {
            List<WaterObject> yorangeObjects = new List<WaterObject>();
            List<WaterObject> greenObjects = new List<WaterObject>();
            List<WaterObject> purpleObjects = new List<WaterObject>();
            List<WaterObject> brownObjects = new List<WaterObject>();
            ObjectLists =
                new List<List<WaterObject>>()
                {
                    yorangeObjects, greenObjects, purpleObjects, brownObjects,
                };

            TtcRng rng = new TtcRng();

            MarioObject marioObject = new MarioObject(rng, inputs);
            yorangeObjects.Add(marioObject);

            BobombBuddyObject bobombBuddyObject = new BobombBuddyObject(rng);
            greenObjects.Add(bobombBuddyObject);

            List<ObjectDataModel> bubbleSpawnerObjs = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Bubble Spawner");

            List<ObjectDataModel> bubbleObjs = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Underwater Bubble");

        }

        public void Update()
        {
            foreach (var objList in ObjectLists)
            {
                foreach (var obj in objList)
                {
                    obj.Update();
                }
            }

            foreach (var objList in ObjectLists)
            {
                for (int i = 0; i < objList.Count; i++)
                {
                    if (objList[i].ShouldBeDeleted)
                    {
                        objList.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public override string ToString()
        {
            List<WaterObject> linearList = ObjectLists.SelectMany(list => list).ToList();
            return string.Join("\r\n", linearList);
        }
    }

    public abstract class WaterObject
    {
        public TtcRng Rng;
        public bool ShouldBeDeleted;

        public WaterObject(TtcRng rng)
        {
            Rng = rng;
            ShouldBeDeleted = false;
        }

        public abstract void Update();

        public void MarkForDeletion()
        {
            ShouldBeDeleted = true;
        }
    }

    public class MarioObject : WaterObject
    {
        public List<Input> Inputs;
        public WaterState WaterState;
        public int WaterLevelIndex;

        public MarioObject(TtcRng rng, List<Input> inputs)
            : base(rng)
        {
            Inputs = new List<Input>();
            WaterState = new WaterState();
            WaterLevelIndex = WaterLevelCalculator.GetWaterLevelIndex();
        }

        public override void Update()
        {
            int index = WaterState.Index;
            Input input = index < Inputs.Count ? Inputs[index] : new Input(0, 0);
            int waterLevel = WaterLevelCalculator.GetWaterLevelFromIndex(WaterLevelIndex);
            WaterState.Update(input, waterLevel);
            WaterLevelIndex++;
        }

        public override string ToString()
        {
            return "Mario " + WaterState.ToString();
        }
    }

    public class BobombBuddyObject : WaterObject
    {
        public int BobombBuddyBlinkingTimer;

        public BobombBuddyObject(TtcRng rng)
            : base(rng)
        {
            BobombBuddyBlinkingTimer = 0;
        }

        public override void Update()
        {
            if (BobombBuddyBlinkingTimer > 0)
            {
                BobombBuddyBlinkingTimer = (BobombBuddyBlinkingTimer + 1) % 16;
            }
            else
            {
                if (Rng.PollRNG() <= 655)
                {
                    BobombBuddyBlinkingTimer++;
                }
            }
        }

        public override string ToString()
        {
            return "BobombBuddy " + BobombBuddyBlinkingTimer;
        }
    }

    public class BubbleSpawnerObject : WaterObject
    {
        public int Timer;
        public int TimerMax;

        public BubbleSpawnerObject(TtcRng rng)
            : base(rng)
        {
            Timer = 0;
            TimerMax = 0;
        }

        public override void Update()
        {
            if (Timer == 0)
            {
                TimerMax = 2 + (int)(9 * Rng.PollFloat());
            }

            Timer++;
        }

        public override string ToString()
        {
            return string.Format(
                "{0} Timer={1} TimerMax={2}",
                "BubbleSpawner", Timer, TimerMax);
        }
    }

    public class BubbleObject : WaterObject
    {
        public int Timer;
        public int VarF4;
        public int VarF8;
        public int VarFC;

        public BubbleObject(TtcRng rng)
            : base(rng)
        {
            Timer = 0;
        }

        public override void Update()
        {


            Timer++;
        }

        public override string ToString()
        {
            return string.Format(
                "{0} Timer={1} VarF4={2} VarF8={3} VarFC={4}",
                "Bubble", Timer, VarF4, VarF8, VarFC);
        }
    }
}
