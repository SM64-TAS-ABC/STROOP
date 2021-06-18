using STROOP.Forms;
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
        public static void Run()
        {
            List<int> loadingZoneFrames = new List<int>() { 1, 2, 3, 3, 5, 4 };
            List<int> bubbleSpawnerMaxFrames = new List<int>() { 10, 2, 3, 3, 9, 4 };
            bool isBubbleSpawnerPresent = true;
            int numInitialBubbles = 7;
            Simulate(loadingZoneFrames, bubbleSpawnerMaxFrames, isBubbleSpawnerPresent, numInitialBubbles);
        }

        public static void Simulate(
            List<int> loadingZoneFrames,
            List<int> bubbleSpawnerMaxFrames,
            bool isBubbleSpawnerPresent,
            int numInitialBubbles)
        {
            int startFrame = 1905;
            int frame = 0;
            bool isTownLoaded = false;
            int numFramesAreaLoaded = 0;
            ObjSlotManager objSlotManager = GetInitialObjSlotManager(isBubbleSpawnerPresent, numInitialBubbles);
            FrameTracker frameTracker = new FrameTracker(loadingZoneFrames);

            void print()
            {
                Config.Print(
                    "FRAME=" + (startFrame + frame) +
                    " isTownLoaded=" + isTownLoaded +
                    " numFramesAreaLoaded=" + numFramesAreaLoaded);
                Config.Print(objSlotManager);
                Config.Print();
            }

            print();

            while (true)
            {
                bool? shouldLoadTown = frameTracker.AdvanceFrame();
                if (shouldLoadTown.HasValue)
                {
                    PassThroughLoadingZone(objSlotManager, shouldLoadTown.Value);
                    isTownLoaded = shouldLoadTown.Value;
                    numFramesAreaLoaded = 0;
                }
                frame++;
                numFramesAreaLoaded++;
                if (isTownLoaded && numFramesAreaLoaded == 2)
                {
                    LoadYellowCoins(objSlotManager);
                }

                print();

                if (frame == 24) break;
            }
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
            objSlotManager.UnloadColor(ObjSlotColor.PINK);
            objSlotManager.UnloadColor(ObjSlotColor.BROWN);
        }

        public static void Load(ObjSlotManager objSlotManager, List<(ObjName objName, ObjSlotColor color)> objData)
        {
            foreach (var data in objData)
            {
                objSlotManager.Load(data);
            }
        }

        public static void LoadYellowCoins(ObjSlotManager objSlotManager)
        {
            for (int i = 0; i < 5; i++)
            {
                objSlotManager.Load((ObjName.YELLOW_COIN, ObjSlotColor.BLUE));
            }
        }

        public static ObjSlotManager GetInitialObjSlotManager(
            bool isBubbleSpawnerPresent,
            int numInitialBubbles)
        {
            ObjSlotManager objSlotManager = new ObjSlotManager();
            int counter = 0;
            foreach (var data in initialObjData)
            {
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

            public void Apply((ObjName objName, ObjSlotColor color) data)
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
            private readonly Dictionary<ObjSlotColor, List<ObjSlot>> _dictionary;
            private readonly List<ObjSlotColor> _colors;

            public ObjSlotManager()
            {
                _dictionary = new Dictionary<ObjSlotColor, List<ObjSlot>>();
                _colors = EnumUtilities.GetEnumValues<ObjSlotColor>(typeof(ObjSlotColor));
                foreach (ObjSlotColor color in _colors)
                {
                    _dictionary[color] = new List<ObjSlot>();
                }
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

            public void Load((ObjName objName, ObjSlotColor color) data)
            {
                List<ObjSlot> unloadedSlots = _dictionary[ObjSlotColor.GREY];
                List<ObjSlot> loadedSlots = _dictionary[data.color];
                ObjSlot objSlot = unloadedSlots[0];
                unloadedSlots.RemoveAt(0);
                objSlot.Apply(data);
                AddToEndOfList(objSlot);
            }

            public void FrameAdvance()
            {

            }

            public override string ToString()
            {
                List<ObjSlot> objSlots = new List<ObjSlot>();
                foreach (ObjSlotColor color in _colors)
                {
                    foreach (ObjSlot objSlot in _dictionary[color])
                    {
                        objSlots.Add(objSlot);
                    }
                }
                return string.Join("\r\n", objSlots);
            }
        }

        public class FrameTracker
        {
            private readonly List<int> _loadingZoneFrames;
            private bool _isTownLoaded;

            public FrameTracker(List<int> loadingZoneFrames)
            {
                _loadingZoneFrames = loadingZoneFrames;
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

        public static List<(ObjName objName, ObjSlotColor color)> initialObjData =
            new List<(ObjName objName, ObjSlotColor color)>()
            {
                (ObjName.COIN_RING, ObjSlotColor.PINK),
                (ObjName.COIN_LINE, ObjSlotColor.PINK),
                (ObjName.COIN_LINE, ObjSlotColor.PINK),

                (ObjName.SIGN, ObjSlotColor.RED),
                (ObjName.BLUE_COIN_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.CANNON_LID, ObjSlotColor.RED),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.SWITCH, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.SIGN, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.ROTATING_PLATFORM, ObjSlotColor.RED),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.SWITCH, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),

                (ObjName.MARIO, ObjSlotColor.YORANGE),

                (ObjName.BOB_OMB_BUDDY, ObjSlotColor.GREEN),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN),
                (ObjName.CHUCKYA, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.SKEETER, ObjSlotColor.GREEN),
                (ObjName.SKEETER, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN),
                (ObjName.CHUCKYA_ANCHOR, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN),

                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.ONE_UP, ObjSlotColor.BLUE),
                (ObjName.ONE_UP, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.STAR, ObjSlotColor.BLUE),
                (ObjName.SECRET_STAR_SPAWNER, ObjSlotColor.BLUE),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE),

                (ObjName.WATER_LEVEL_MANAGER, ObjSlotColor.PURPLE),
                (ObjName.MARIO_SPAWNER, ObjSlotColor.PURPLE),
            };

        public static List<(ObjName objName, ObjSlotColor color)> area1ObjData =
            new List<(ObjName objName, ObjSlotColor color)>()
            {
                (ObjName.SIGN, ObjSlotColor.RED),
                (ObjName.BOB_OMB_BUDDY, ObjSlotColor.GREEN),
                (ObjName.COIN_RING, ObjSlotColor.PINK),
                (ObjName.COIN_LINE, ObjSlotColor.PINK),
                (ObjName.BLUE_COIN_BLOCK, ObjSlotColor.RED),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN),
                (ObjName.CANNON_LID, ObjSlotColor.RED),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED),
                (ObjName.ONE_UP, ObjSlotColor.BLUE),
                (ObjName.ONE_UP, ObjSlotColor.BLUE),
                (ObjName.CHUCKYA, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO, ObjSlotColor.GREEN),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.SWITCH, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.PUSHABLE_BLOCK, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.AMP, ObjSlotColor.GREEN),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.BLUE_COIN, ObjSlotColor.BLUE),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.SECRET, ObjSlotColor.BLUE),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.COIN_LINE, ObjSlotColor.PINK),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.SIGN, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.SKEETER, ObjSlotColor.GREEN),
                (ObjName.SKEETER, ObjSlotColor.GREEN),
                (ObjName.ROTATING_PLATFORM, ObjSlotColor.RED),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.LONG_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED),
                (ObjName.EXPRESS_ELEVATOR, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.HIDDEN_WOODEN_BLOCK, ObjSlotColor.RED),
                (ObjName.SWITCH, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.WATER_LEVEL_MANAGER, ObjSlotColor.PURPLE),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.ARROW_LIFT, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.SHORT_WOODEN_BOARD, ObjSlotColor.RED),
                (ObjName.STAR, ObjSlotColor.BLUE),
                (ObjName.SECRET_STAR_SPAWNER, ObjSlotColor.BLUE),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE),
                (ObjName.TELEPORTER, ObjSlotColor.BLUE),
                (ObjName.MARIO_SPAWNER, ObjSlotColor.PURPLE),
                (ObjName.BUBBLE_SPAWNER, ObjSlotColor.PURPLE),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN),
                (ObjName.CHUCKYA_ANCHOR, ObjSlotColor.GREEN),
                (ObjName.HEAVE_HO_ANCHOR, ObjSlotColor.GREEN),
            };

        public static List<(ObjName objName, ObjSlotColor color)> area2ObjData =
            new List<(ObjName objName, ObjSlotColor color)>()
            {
                (ObjName.TREE, ObjSlotColor.ORANGE),
                (ObjName.TREE, ObjSlotColor.ORANGE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.RED_COIN, ObjSlotColor.BLUE),
                (ObjName.COIN_RING, ObjSlotColor.PINK),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE),
                (ObjName.ONE_UP_ACTIVATOR, ObjSlotColor.BLUE),
                (ObjName.ONE_UP, ObjSlotColor.BLUE),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.METAL_CAP_BLOCK, ObjSlotColor.RED),
                (ObjName.VANISH_CAP_BLOCK, ObjSlotColor.RED),
                (ObjName.COIN_LINE, ObjSlotColor.PINK),
                (ObjName.SKEETER, ObjSlotColor.GREEN),
                (ObjName.SKEETER, ObjSlotColor.GREEN),
                (ObjName.SWITCH, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.VANISH_CAP_BLOCK, ObjSlotColor.RED),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.FIRE_SPITTER, ObjSlotColor.GREEN),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.CORK_BOX, ObjSlotColor.RED),
                (ObjName.COIN_LINE, ObjSlotColor.PINK),
                (ObjName.COIN_LINE, ObjSlotColor.PINK),
                (ObjName.ITEM_BLOCK, ObjSlotColor.RED),
                (ObjName.POLE, ObjSlotColor.ORANGE),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.CRYSTAL_TAP, ObjSlotColor.RED),
                (ObjName.WATER_LEVEL_MANAGER, ObjSlotColor.PURPLE),
                (ObjName.STAR, ObjSlotColor.BLUE),
                (ObjName.RED_COIN_STAR_SPAWNER, ObjSlotColor.BLUE),
                (ObjName.BUBBLE_SPAWNER, ObjSlotColor.PURPLE),
                (ObjName.STAR_MARKER, ObjSlotColor.PURPLE),
            };
    }
}
