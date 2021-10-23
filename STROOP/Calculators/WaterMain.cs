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
            ObjSlotManager objSlotManager = new ObjSlotManager(inputs);
            Config.Print(objSlotManager);
            //for (int i = 0; i < 10; i++)
            //{
            //    objSlotManager.Update();
            //    Config.Print(objSlotManager);
            //    Config.Print();
            //}
        }
    }

    public class ObjSlotManager
    {
        public int GlobalTimer;
        TtcRng Rng;
        public List<List<WaterObject>> ObjectLists;

        public ObjSlotManager(List<Input> inputs)
        {
            GlobalTimer = Config.Stream.GetInt(MiscConfig.GlobalTimerAddress);

            List<WaterObject> yorangeObjects = new List<WaterObject>();
            List<WaterObject> greenObjects = new List<WaterObject>();
            List<WaterObject> purpleObjects = new List<WaterObject>();
            List<WaterObject> brownObjects = new List<WaterObject>();
            ObjectLists =
                new List<List<WaterObject>>()
                {
                    yorangeObjects, greenObjects, purpleObjects, brownObjects,
                };

            Rng = new TtcRng();

            MarioObject marioObject = new MarioObject(this, Rng, inputs);
            yorangeObjects.Add(marioObject);

            List<ObjectDataModel> bobombBuddyObjs = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Bob-omb Buddy (Opens Cannon)");
            foreach (var bobombBuddyObj in bobombBuddyObjs)
            {
                int blinkingTimer = Config.Stream.GetInt(bobombBuddyObj.Address + 0xF4);
                BobombBuddyObject bobombBuddyObject = new BobombBuddyObject(this, Rng, blinkingTimer);
                greenObjects.Add(bobombBuddyObject);
            }

            List<ObjectDataModel> bubbleSpawnerObjs = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Bubble Spawner");
            foreach (var bubbleSpawnerObj in bubbleSpawnerObjs)
            {
                int timer = Config.Stream.GetInt(bubbleSpawnerObj.Address + ObjectConfig.TimerOffset);
                int timerMax = Config.Stream.GetInt(bubbleSpawnerObj.Address + 0xF4);
                BubbleSpawnerObject bubbleSpawnerObject = new BubbleSpawnerObject(this, Rng, timer, timerMax);
                purpleObjects.Add(bubbleSpawnerObject);
            }

            List<ObjectDataModel> bubbleObjs = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Underwater Bubble");
            foreach (var bubbleObj in bubbleObjs)
            {
                float x = Config.Stream.GetFloat(bubbleObj.Address + ObjectConfig.XOffset);
                float y = Config.Stream.GetFloat(bubbleObj.Address + ObjectConfig.YOffset);
                float z = Config.Stream.GetFloat(bubbleObj.Address + ObjectConfig.ZOffset);
                int timer = Config.Stream.GetInt(bubbleObj.Address + ObjectConfig.TimerOffset);

                float varF4 = Config.Stream.GetFloat(bubbleObj.Address + 0xF4);
                float varF8 = Config.Stream.GetFloat(bubbleObj.Address + 0xF8);
                float varFC = Config.Stream.GetFloat(bubbleObj.Address + 0xFC);
                float var100 = Config.Stream.GetFloat(bubbleObj.Address + 0x100);

                BubbleObject bubbleObject = new BubbleObject(this, Rng, x, y, z, timer, varF4, varF8, varFC, var100);
                brownObjects.Add(bubbleObject);
            }
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
            List<WaterObject> objList = ObjectLists.SelectMany(list => list).ToList();
            List<string> stringList = objList.ConvertAll(obj => obj.ToString());
            stringList.Insert(0, GlobalTimer.ToString());
            stringList.Insert(1, Rng.ToString());
            return string.Join("\r\n", stringList);
        }
    }

    public abstract class WaterObject
    {
        public ObjSlotManager ObjSlotManager;
        public TtcRng Rng;
        public bool ShouldBeDeleted;

        public WaterObject(ObjSlotManager objectSlotsManager, TtcRng rng)
        {
            ObjSlotManager = objectSlotsManager;
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

        public MarioObject(ObjSlotManager objSlotManager, TtcRng rng, List<Input> inputs)
            : base(objSlotManager, rng)
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

        public BobombBuddyObject(ObjSlotManager objSlotManager, TtcRng rng, int bobombBuddyBlinkingTimer)
            : base(objSlotManager, rng)
        {
            BobombBuddyBlinkingTimer = bobombBuddyBlinkingTimer;
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

        public BubbleSpawnerObject(ObjSlotManager objSlotManager, TtcRng rng, int timer, int timerMax)
            : base(objSlotManager, rng)
        {
            Timer = timer;
            TimerMax = timerMax;
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
        public float X;
        public float Y;
        public float Z;
        public int Timer;
        public float VarF4;
        public float VarF8;
        public float VarFC;
        public float Var100;

        public BubbleObject(
                ObjSlotManager objSlotManager, TtcRng rng,
                float x, float y, float z, int timer,
                float varF4, float varF8, float varFC, float var100)
            : base(objSlotManager, rng)
        {
            X = x;
            Y = y;
            Z = z;
            Timer = timer;
            VarF4 = varF4;
            VarF8 = varF8;
            VarFC = varFC;
            Var100 = var100;
        }

        public override void Update()
        {
            if (Timer == 0)
            {
                bhv_bubble_wave_init();

                VarF4 = -50 + Rng.PollFloat() * 100;
                VarF8 = -50 + Rng.PollFloat() * 100;
                X += VarF4;
                Z += VarF8;
                VarFC = Rng.PollFloat() * 50;
                Y += VarFC;

                bhvSmallWaterWave398();
            }

            if (Timer < 60)
            {
                bhvSmallWaterWave398();
                bhv_small_water_wave_loop();
            }

            Timer++;

            if (Timer == 61)
            {
                MarkForDeletion();
            }
        }

        public void bhv_bubble_wave_init()
        {
            VarFC = 0x800 + (int)(Rng.PollFloat() * 2048.0f);
            Var100 = 0x800 + (int)(Rng.PollFloat() * 2048.0f);
        }

        public void bhvSmallWaterWave398()
        {
            Y += 7;
            VarF4 = -2 + Rng.PollFloat() * 5;
            VarF8 = -2 + Rng.PollFloat() * 5;
            X += VarF4;
            Z += VarF8;
        }

        public void bhv_small_water_wave_loop()
        {
            
        }

        public override string ToString()
        {
            return string.Format(
                "{0} X={1} Y={2} Z={3} Timer={4}",
                "Bubble", (double)X, (double)Y, (double)Z, Timer);
        }
    }
}
