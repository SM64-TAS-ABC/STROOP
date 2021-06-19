﻿using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class LoadingZoneMain
    {
        public static Random r = new Random();

        public static Dictionary<UnloadableId, bool> UnloadStrategy =
            new Dictionary<UnloadableId, bool>()
            {
                [UnloadableId.LOADED_ALWAYS] = false, // do not change

                [UnloadableId.SKEETER_CLOSE] = true,
                [UnloadableId.SKEETER_FAR] = false,

                [UnloadableId.CORK_BOX_EXPRESS_ELEVATOR] = false,
                [UnloadableId.CORK_BOX_EDGE_1] = false,
                [UnloadableId.CORK_BOX_EDGE_2] = false,
                [UnloadableId.CORK_BOX_EDGE_3] = false,
                [UnloadableId.CORK_BOX_EDGE_4] = false,
                [UnloadableId.CORK_BOX_EDGE_BIG] = false,

                [UnloadableId.BLUE_COIN] = false,
                [UnloadableId.BLUE_COIN_BLOCK] = false,

                [UnloadableId.ITEM_BLOCK_PENTAGON_PLATFORM] = false,
                [UnloadableId.ITEM_BLOCK_EXPRESS_ELEVATOR] = false,
                [UnloadableId.ITEM_BLOCK_SLIDE_KICK] = false,
                [UnloadableId.ITEM_BLOCK_HIGH_CORNER] = false,

                [UnloadableId.ONE_UP_TUNNEL_1] = false,
                [UnloadableId.ONE_UP_TUNNEL_2] = false,

                [UnloadableId.SECRET_EXPRESS_ELEVATOR] = false,
                [UnloadableId.SECRET_BLOCK_HOLE] = false,
                [UnloadableId.SECRET_PENTAGON_PLATFORM] = false,
                [UnloadableId.SECRET_HIGH_CORNER] = false,
                [UnloadableId.SECRET_WATER_BLOCK] = false,
            };

        public static void Run()
        {
            HashSet<int> results = new HashSet<int>();
            while (true)
            {
                List<int> loadingZoneFrames = GenerateRandomLoadingZoneFrames();
                List<int> bubbleSpawnerMaxTimers = GenerateRandomBubbleSpawnerMaxTimers();
                foreach (bool isBubbleSpawnerPresent in new List<bool>() { false, true })
                {
                    for (int numInitialBubbles = 5; numInitialBubbles <= 12; numInitialBubbles++)
                    {
                        (bool success, int result, ObjName objName, int numTransitions) =
                            Simulate(loadingZoneFrames, bubbleSpawnerMaxTimers, isBubbleSpawnerPresent, numInitialBubbles, false);
                        if (!results.Contains(result))
                        {
                            Config.Print(result + " " + objName);
                            results.Add(result);
                        }
                        if (success)
                        {
                            Config.Print("-------------------------------------");
                            Config.Print("numTransitions = " + numTransitions);
                            Config.Print("loadingZoneFrames = " + string.Join(",", loadingZoneFrames));
                            Config.Print("bubbleSpawnerMaxTimers = " + string.Join(",", bubbleSpawnerMaxTimers));
                            Config.Print("isBubbleSpawnerPresent = " + isBubbleSpawnerPresent);
                            Config.Print("numInitialBubbles = " + numInitialBubbles);
                            Config.Print("-------------------------------------");
                        }
                    }
                }
            }
        }

        public static List<int> GenerateRandomLoadingZoneFrames()
        {
            List<int> loadingZoneFrames = new List<int>() { 1 };
            int max = r.Next(2, 8);
            for (int i = 0; i < 15; i++)
            {
                loadingZoneFrames.Add(r.Next(2, max));
            }
            return loadingZoneFrames;
        }

        public static List<int> GenerateRandomBubbleSpawnerMaxTimers()
        {
            List<int> bubbleSpawnerMaxTimers = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                bubbleSpawnerMaxTimers.Add(r.Next(2, 6));
            }
            return bubbleSpawnerMaxTimers;
        }

        public static void RunTest()
        {
            List<int> loadingZoneFrames = new List<int>() { 1, 2, 3, 3, 10 };
            List<int> bubbleSpawnerMaxTimers = new List<int>() { 10, 2, 3, 3, 9, 5 };
            bool isBubbleSpawnerPresent = true;
            int numInitialBubbles = 7;
            Simulate(loadingZoneFrames, bubbleSpawnerMaxTimers, isBubbleSpawnerPresent, numInitialBubbles, true);
        }

        public static (bool success, int result, ObjName objName, int numTransitions) Simulate(
            List<int> loadingZoneFrames,
            List<int> bubbleSpawnerMaxTimers,
            bool isBubbleSpawnerPresent,
            int numInitialBubbles,
            bool shouldPrint)
        {
            int startFrame = 1905;
            int frame = 0;
            bool isTownLoaded = false;
            int numFramesAreaLoaded = 0;
            int numTransitions = 0;

            FrameTracker frameTracker = new FrameTracker(loadingZoneFrames);
            BubbleTracker bubbleTracker = new BubbleTracker(bubbleSpawnerMaxTimers);
            ObjSlotManager objSlotManager = InitializeObjSlotManager(isBubbleSpawnerPresent, numInitialBubbles, bubbleTracker);
            ObjSlot heldSlot = objSlotManager.FindSlot(ObjName.CHUCKYA);

            void print()
            {
                Config.Print(
                    "FRAME=" + (startFrame + frame) +
                    " isTownLoaded=" + isTownLoaded +
                    " numFramesAreaLoaded=" + numFramesAreaLoaded);
                Config.Print(objSlotManager);
                Config.Print();
            }

            if (shouldPrint) print();

            (bool success, int result, ObjName objName, int numTransitions) returnValue = (false, -1, ObjName.UNKNOWN, numTransitions);

            while (true)
            {
                bool? shouldLoadTown = frameTracker.AdvanceFrame();
                if (shouldLoadTown.HasValue)
                {
                    PassThroughLoadingZone(objSlotManager, shouldLoadTown.Value);
                    isTownLoaded = shouldLoadTown.Value;
                    numFramesAreaLoaded = 0;
                    numTransitions++;
                }
                frame++;
                numFramesAreaLoaded++;
                if (isTownLoaded && numFramesAreaLoaded == 2)
                {
                    LoadYellowCoins(objSlotManager);
                }
                objSlotManager.FrameAdvance();

                if (shouldPrint) print();

                if (isTownLoaded && heldSlot.Color != ObjSlotColor.GREY)
                {
                    returnValue = (false, objSlotManager.GetCurrentSlotIndex(heldSlot), heldSlot.ObjName, numTransitions);
                    if (heldSlot.ObjName == ObjName.STAR)
                    {
                        return (true, heldSlot.InitialIndex, heldSlot.ObjName, numTransitions);
                    }
                }

                if (frame == 24) break;
            }

            return returnValue;
        }

        public static void PassThroughLoadingZone(ObjSlotManager objSlotManager, bool loadsTown)
        {
            UnloadEverything(objSlotManager);
            Load(objSlotManager, loadsTown ? area2ObjData : area1ObjData);
        }

        public static void UnloadEverything(ObjSlotManager objSlotManager)
        {
            objSlotManager.UnloadColor(ObjSlotColor.GREEN);
            objSlotManager.UnloadColor(ObjSlotColor.BLUE);
            objSlotManager.UnloadColor(ObjSlotColor.PURPLE);
            objSlotManager.UnloadColor(ObjSlotColor.RED);
            objSlotManager.UnloadColor(ObjSlotColor.ORANGE);
            objSlotManager.UnloadColor(ObjSlotColor.PINK);
            objSlotManager.UnloadColor(ObjSlotColor.BROWN);
        }

        public static void Load(ObjSlotManager objSlotManager, List<(ObjName objName, ObjSlotColor color, UnloadableId id)> objData)
        {
            foreach (var data in objData)
            {
                if (UnloadStrategy[data.id]) continue;
                objSlotManager.Load(data);
            }
        }

        public static void LoadYellowCoins(ObjSlotManager objSlotManager)
        {
            for (int i = 0; i < 5; i++)
            {
                objSlotManager.Load((ObjName.YELLOW_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS));
            }
        }

        public static ObjSlotManager InitializeObjSlotManager(
            bool isBubbleSpawnerPresent,
            int numInitialBubbles,
            BubbleTracker bubbleTracker)
        {
            ObjSlotManager objSlotManager = new ObjSlotManager(bubbleTracker);
            int counter = 0;
            foreach (var data in initialObjData)
            {
                if (UnloadStrategy[data.id]) continue;
                objSlotManager.AddToEndOfList(new ObjSlot(counter++, data.objName, data.color));
            }
            if (isBubbleSpawnerPresent)
            {
                objSlotManager.AddToEndOfList(new ObjSlot(counter++, ObjName.BUBBLE_SPAWNER, ObjSlotColor.PURPLE));
            }
            for (int i = 0; i < numInitialBubbles; i++)
            {
                objSlotManager.AddToEndOfList(new ObjSlot(counter++, ObjName.BUBBLE, ObjSlotColor.BROWN));
            }
            while (counter < 240)
            {
                objSlotManager.AddToEndOfList(new ObjSlot(counter++, ObjName.UNKNOWN, ObjSlotColor.GREY));
            }
            return objSlotManager;
        }

        public enum ObjSlotColor
        { 
            PINK,
            RED,
            ORANGE,
            YORANGE,
            GREEN,
            BLUE,
            PURPLE,
            BROWN,
            GREY,
        };

        public enum ObjName
        {
            COIN_RING,
            COIN_LINE,
            SIGN,
            BLUE_COIN_BLOCK,
            ITEM_BLOCK,
            CANNON_LID,
            PUSHABLE_BLOCK,
            CORK_BOX,
            SWITCH,
            ROTATING_PLATFORM,
            LONG_WOODEN_BOARD,
            EXPRESS_ELEVATOR,
            HIDDEN_WOODEN_BLOCK,
            CRYSTAL_TAP,
            ARROW_LIFT,
            SHORT_WOODEN_BOARD,
            MARIO,
            BOB_OMB_BUDDY,
            AMP,
            HEAVE_HO,
            CHUCKYA,
            FIRE_SPITTER,
            SKEETER,
            HEAVE_HO_ANCHOR,
            CHUCKYA_ANCHOR,
            BLUE_COIN,
            ONE_UP,
            SECRET,
            STAR,
            SECRET_STAR_SPAWNER,
            TELEPORTER,
            WATER_LEVEL_MANAGER,
            MARIO_SPAWNER,

            METAL_CAP_BLOCK,
            VANISH_CAP_BLOCK,
            TREE,
            POLE,
            RED_COIN,
            ONE_UP_ACTIVATOR,
            RED_COIN_STAR_SPAWNER,
            YELLOW_COIN,
            STAR_MARKER,

            BUBBLE_SPAWNER,
            BUBBLE,

            UNKNOWN,
        };

        public enum UnloadableId
        {
            LOADED_ALWAYS,

            SKEETER_CLOSE,
            SKEETER_FAR,

            CORK_BOX_EXPRESS_ELEVATOR,
            CORK_BOX_EDGE_1,
            CORK_BOX_EDGE_2,
            CORK_BOX_EDGE_3,
            CORK_BOX_EDGE_4,
            CORK_BOX_EDGE_BIG,

            BLUE_COIN,
            BLUE_COIN_BLOCK,

            ITEM_BLOCK_PENTAGON_PLATFORM,
            ITEM_BLOCK_EXPRESS_ELEVATOR,
            ITEM_BLOCK_SLIDE_KICK,
            ITEM_BLOCK_HIGH_CORNER,

            ONE_UP_TUNNEL_1,
            ONE_UP_TUNNEL_2,

            SECRET_EXPRESS_ELEVATOR,
            SECRET_BLOCK_HOLE,
            SECRET_PENTAGON_PLATFORM,
            SECRET_HIGH_CORNER,
            SECRET_WATER_BLOCK,
        };

        public class ObjSlot
        {
            public readonly int InitialIndex;
            public ObjName ObjName;
            public ObjSlotColor Color;

            public ObjSlot(int initialIndex, ObjName objName, ObjSlotColor color)
            {
                InitialIndex = initialIndex;
                ObjName = objName;
                Color = color;
            }

            public void Apply((ObjName objName, ObjSlotColor color, UnloadableId id) data)
            {
                ObjName = data.objName;
                Color = data.color;
            }

            public override string ToString()
            {
                return string.Format("({0},{1},{2})", InitialIndex, ObjName, Color);
            }
        }

        public class ObjSlotManager
        {
            private readonly BubbleTracker _bubbleTracker;
            private readonly Dictionary<ObjSlotColor, List<ObjSlot>> _dictionary;
            private readonly List<ObjSlotColor> _colors;

            private int bubbleSpawnerMaxTimer;
            private int bubbleSpawnerTimer;

            public ObjSlotManager(BubbleTracker bubbleTracker)
            {
                _bubbleTracker = bubbleTracker;
                _dictionary = new Dictionary<ObjSlotColor, List<ObjSlot>>();
                _colors = EnumUtilities.GetEnumValues<ObjSlotColor>(typeof(ObjSlotColor));
                foreach (ObjSlotColor color in _colors)
                {
                    _dictionary[color] = new List<ObjSlot>();
                }

                bubbleSpawnerMaxTimer = int.MaxValue;
                bubbleSpawnerTimer = 0;
            }

            public void AddToEndOfList(ObjSlot objSlot)
            {
                _dictionary[objSlot.Color].Add(objSlot);
            }

            public void AddToStartOfList(ObjSlot objSlot)
            {
                _dictionary[objSlot.Color].Insert(0, objSlot);
            }

            public void UnloadColor(ObjSlotColor color)
            {
                List<ObjSlot> listToUnload = _dictionary[color];
                while (listToUnload.Count > 0)
                {
                    ObjSlot objSlot = listToUnload[0];
                    listToUnload.RemoveAt(0);
                    objSlot.Color = ObjSlotColor.GREY;
                    AddToStartOfList(objSlot);
                }
            }

            public void Load((ObjName objName, ObjSlotColor color, UnloadableId id) data)
            {
                List<ObjSlot> unloadedSlots = _dictionary[ObjSlotColor.GREY];
                List<ObjSlot> loadedSlots = _dictionary[data.color];
                ObjSlot objSlot = unloadedSlots[0];
                unloadedSlots.RemoveAt(0);
                objSlot.Apply(data);
                AddToEndOfList(objSlot);

                if (data.objName == ObjName.BUBBLE_SPAWNER)
                {
                    bubbleSpawnerMaxTimer = _bubbleTracker.GetNextMaxTimer();
                    bubbleSpawnerTimer = 0;
                }
            }

            public void Unload(ObjSlot objSlot)
            {
                List<ObjSlot> listToUnloadFrom = _dictionary[objSlot.Color];
                listToUnloadFrom.Remove(objSlot);
                objSlot.Color = ObjSlotColor.GREY;
                AddToStartOfList(objSlot);
            }

            public void FrameAdvance()
            {
                ObjSlot bubbleSpawner = _dictionary[ObjSlotColor.PURPLE].FirstOrDefault(
                    objSlot => objSlot.ObjName == ObjName.BUBBLE_SPAWNER);
                if (bubbleSpawner == null)
                {
                    Load((ObjName.BUBBLE_SPAWNER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS));
                    bubbleSpawnerTimer++;
                }
                else
                {
                    bubbleSpawnerTimer++;
                    if (bubbleSpawnerTimer > bubbleSpawnerMaxTimer)
                    {
                        Load((ObjName.BUBBLE, ObjSlotColor.BROWN, UnloadableId.LOADED_ALWAYS));
                        Unload(bubbleSpawner);
                    }
                }
            }

            public ObjSlot FindSlot(ObjName objName)
            {
                foreach (ObjSlotColor color in _colors)
                {
                    foreach (ObjSlot objSlot in _dictionary[color])
                    {
                        if (objSlot.ObjName == objName) return objSlot;
                    }
                }
                return null;
            }

            public int GetCurrentSlotIndex(ObjSlot goalObjSlot)
            {
                int counter = 0;
                foreach (ObjSlotColor color in _colors)
                {
                    foreach (ObjSlot objSlot in _dictionary[color])
                    {
                        if (objSlot == goalObjSlot) return counter;
                        counter++;
                    }
                }
                return -1;
            }

            public override string ToString()
            {
                List<string> strings = new List<string>();
                strings.Add(string.Format("timer={0} maxTimer={1}", bubbleSpawnerTimer, bubbleSpawnerMaxTimer));
                foreach (ObjSlotColor color in _colors)
                {
                    foreach (ObjSlot objSlot in _dictionary[color])
                    {
                        strings.Add(objSlot.ToString());
                    }
                }
                return string.Join("\r\n", strings);
            }
        }

        public class FrameTracker
        {
            private readonly List<int> _loadingZoneFrames;
            private bool _isTownLoaded;

            public FrameTracker(List<int> loadingZoneFrames)
            {
                _loadingZoneFrames = new List<int>(loadingZoneFrames);
                _isTownLoaded = false;
            }

            public bool? AdvanceFrame()
            {
                if (_loadingZoneFrames.Count == 0) return null;

                if (_loadingZoneFrames[0] > 1)
                {
                    _loadingZoneFrames[0]--;
                    return null;
                }
                else
                {
                    _loadingZoneFrames.RemoveAt(0);
                    _isTownLoaded = !_isTownLoaded;
                    return _isTownLoaded;
                }
            }
        }

        public class BubbleTracker
        {
            private readonly List<int> _bubbleSpawnerMaxTimers;

            public BubbleTracker(List<int> bubbleSpawnerMaxTimers)
            {
                _bubbleSpawnerMaxTimers = new List<int>(bubbleSpawnerMaxTimers);
            }

            public int GetNextMaxTimer()
            {
                if (_bubbleSpawnerMaxTimers.Count == 0) return int.MaxValue;

                int firstValue = _bubbleSpawnerMaxTimers[0];
                _bubbleSpawnerMaxTimers.RemoveAt(0);
                return firstValue;
            }
        }

        public static List<(ObjName objName, ObjSlotColor color, UnloadableId id)> initialObjData =
            new List<(ObjName objName, ObjSlotColor color, UnloadableId id)>()
            {
                (ObjName.COIN_RING, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_LINE, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_LINE, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),

                (ObjName.SIGN, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.BLUE_COIN_BLOCK, ObjSlotColor.RED, UnloadableId.BLUE_COIN_BLOCK),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CANNON_LID, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EXPRESS_ELEVATOR),
                (ObjName.SWITCH, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_4),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_3),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_BIG),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_PENTAGON_PLATFORM),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_EXPRESS_ELEVATOR),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_2),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_SLIDE_KICK),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_HIGH_CORNER),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SIGN, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_1),
                (ObjName.ROTATING_PLATFORM, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SWITCH, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),

                (ObjName.MARIO, ObjSlotColor.YORANGE, UnloadableId.LOADED_ALWAYS),

                (ObjName.BOB_OMB_BUDDY, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.CHUCKYA, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.SKEETER, ObjSlotColor.GREEN, UnloadableId.SKEETER_FAR),
                (ObjName.SKEETER, ObjSlotColor.GREEN, UnloadableId.SKEETER_CLOSE),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.CHUCKYA_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),

                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.ONE_UP, ObjSlotColor.BLUE, UnloadableId.ONE_UP_TUNNEL_2),
                (ObjName.ONE_UP, ObjSlotColor.BLUE, UnloadableId.ONE_UP_TUNNEL_1),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_EXPRESS_ELEVATOR),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_BLOCK_HOLE),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_PENTAGON_PLATFORM),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_HIGH_CORNER),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_WATER_BLOCK),
                (ObjName.STAR, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.SECRET_STAR_SPAWNER, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),

                (ObjName.WATER_LEVEL_MANAGER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
                (ObjName.MARIO_SPAWNER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
            };

        public static List<(ObjName objName, ObjSlotColor color, UnloadableId id)> area1ObjData =
            new List<(ObjName objName, ObjSlotColor color, UnloadableId id)>()
            {
                (ObjName.SIGN, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.BOB_OMB_BUDDY, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_RING, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_LINE, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.BLUE_COIN_BLOCK, ObjSlotColor.RED, UnloadableId.BLUE_COIN_BLOCK),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.CANNON_LID, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ONE_UP, ObjSlotColor.BLUE, UnloadableId.ONE_UP_TUNNEL_2),
                (ObjName.ONE_UP, ObjSlotColor.BLUE, UnloadableId.ONE_UP_TUNNEL_1),
                (ObjName.CHUCKYA, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_EXPRESS_ELEVATOR),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_BLOCK_HOLE),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_PENTAGON_PLATFORM),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_HIGH_CORNER),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EXPRESS_ELEVATOR),
                (ObjName.SWITCH, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_4),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_3),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_BIG),
                (ObjName.AMP, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE, UnloadableId.BLUE_COIN),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_PENTAGON_PLATFORM),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_EXPRESS_ELEVATOR),
                (ObjName.SECRET, ObjSlotColor.BLUE, UnloadableId.SECRET_WATER_BLOCK),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_2),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_SLIDE_KICK),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.ITEM_BLOCK_HIGH_CORNER),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_LINE, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SIGN, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.CORK_BOX_EDGE_1),
                (ObjName.SKEETER, ObjSlotColor.GREEN, UnloadableId.SKEETER_FAR),
                (ObjName.SKEETER, ObjSlotColor.GREEN, UnloadableId.SKEETER_CLOSE),
                (ObjName.ROTATING_PLATFORM, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SWITCH, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.WATER_LEVEL_MANAGER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.STAR, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.SECRET_STAR_SPAWNER, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.MARIO_SPAWNER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
                (ObjName.BUBBLE_SPAWNER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.CHUCKYA_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
            };

        public static List<(ObjName objName, ObjSlotColor color, UnloadableId id)> area2ObjData =
            new List<(ObjName objName, ObjSlotColor color, UnloadableId id)>()
            {
                (ObjName.TREE, ObjSlotColor.ORANGE, UnloadableId.LOADED_ALWAYS),
                (ObjName.TREE, ObjSlotColor.ORANGE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_RING, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.ONE_UP, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.METAL_CAP_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.VANISH_CAP_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_LINE, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.SKEETER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.SKEETER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.SWITCH, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.VANISH_CAP_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CORK_BOX, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_LINE, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.COIN_LINE, ObjSlotColor.PINK, UnloadableId.LOADED_ALWAYS),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.POLE, ObjSlotColor.ORANGE, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED, UnloadableId.LOADED_ALWAYS),
                (ObjName.WATER_LEVEL_MANAGER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
                (ObjName.STAR, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.RED_COIN_STAR_SPAWNER, ObjSlotColor.BLUE, UnloadableId.LOADED_ALWAYS),
                (ObjName.BUBBLE_SPAWNER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
                (ObjName.STAR_MARKER, ObjSlotColor.PURPLE, UnloadableId.LOADED_ALWAYS),
            };
    }
}
