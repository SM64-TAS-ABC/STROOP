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

    public class TtcSimulation
    {
        private readonly TtcRng _rng;
        private readonly List<TtcObject> _rngObjects;
        private readonly int _startingFrame;

        public TtcSimulation(ushort rngValue, int startingFrame, List<int> dustFrames = null)
        {
            //set up objects
            _rng = new TtcRng(rngValue); //initial RNG during star selection screen
            _rngObjects = CreateRngObjects(_rng, dustFrames);

            //set up testing variables
            _startingFrame = startingFrame; //the frame directly preceding any object initialization
        }

        public TtcSimulation(List<int> dustFrames = null)
        {
            //set up objects
            _rng = new TtcRng(Config.Stream.GetUInt16(MiscConfig.RngAddress));
            _rngObjects = CreateRngObjectsFromGame(_rng, dustFrames);

            //set up testing variables
            _startingFrame = MupenUtilities.GetFrameCount(); //the frame directly preceding any object initialization
        }

        public string GetObjectsString(int endingFrame)
        {
            //iterate through frames to update objects
            int frame = _startingFrame;
            int counter = 0;
            while (frame < endingFrame)
            {
                frame++;
                counter++;
                foreach (TtcObject rngObject in _rngObjects)
                {
                    rngObject.SetFrame(frame);
                    rngObject.Update();
                }
            }

            List<string> lines = new List<string>();
            foreach (TtcObject rngObject in _rngObjects)
            {
                lines.Add(rngObject.ToString());
            }
            lines.Add("RNG Value = " + _rng.GetRng());
            lines.Add("RNG Index = " + _rng.GetIndex());
            lines.Add("");
            lines.Add(String.Format("iterated through {0} frames, from {1} to {2}", counter, _startingFrame, endingFrame));
            lines.Add("frame = " + frame);
            lines.Add("");
            return String.Join("\r\n", lines);
        }

        public int? FindIdealCogConfiguration(int numFramesMin, int numFramesMax)
        {
            TtcCog upperCog = _rngObjects[31] as TtcCog;
            TtcCog lowerCog = _rngObjects[32] as TtcCog;
            List<CogConfiguration> cogConfigurations = new List<CogConfiguration>();
            List<int> goodUpperCogAngles = new List<int>() { 46432, 57360, 2752, 13664, 24592, 35536 };
            List<int> goodLowerCogAngles = new List<int>() { 42576, 53504, 64416, 9808, 20736, 31648 };
            List<int> goodLowerCogAnglesAdjusted = goodLowerCogAngles.ConvertAll(angle => angle + 32);

            List<int> upperCogEndingYaws = new List<int>() { 46432, 45232 };

            int numCogConfigurations = 9;
            int lowerCogGoodAngle = 62988;
            List<int> lowerCogGoodAngles = Enumerable.Range(0, 6).ToList()
                .ConvertAll(index => lowerCogGoodAngle + 65536 / 6 * index)
                .ConvertAll(angle => (int)MoreMath.NormalizeAngleUshort(angle));

            //iterate through frames to update objects
            int frame = _startingFrame;
            int counter = 0;
            while (frame < _startingFrame + numFramesMax)
            {
                frame++;
                counter++;
                foreach (TtcObject rngObject in _rngObjects)
                {
                    rngObject.SetFrame(frame);
                    rngObject.Update();
                }

                if (cogConfigurations.Count >= numCogConfigurations)
                    cogConfigurations.RemoveAt(0);
                cogConfigurations.Add(new CogConfiguration(upperCog, lowerCog));

                if (counter >= numFramesMin)
                {
                    if (cogConfigurations.Count < numCogConfigurations) continue;

                    int upperCogAngleDist = goodUpperCogAngles.Min(
                        angle => (int)MoreMath.GetAngleDistance(
                            angle, MoreMath.NormalizeAngleTruncated(cogConfigurations[8].UpperCogAngle)));
                    int lowerCogAngleDist = goodLowerCogAnglesAdjusted.Min(
                        angle => (int)MoreMath.GetAngleDistance(
                            angle, MoreMath.NormalizeAngleTruncated(cogConfigurations[5].LowerCogAngle)));

                    int lowerCogMinAngularVelocity = 50;
                    int lowerCogMaxAngularVelocity = 400;

                    if (upperCogAngleDist == 0 &&
                        cogConfigurations[8].UpperCogAngle == 46432 &&
                        cogConfigurations[7].UpperCogTargetAngularVelocity == 1200 &&
                        cogConfigurations[8].UpperCogCurrentAngularVelocity == 1200 &&
                        cogConfigurations[0].UpperCogTargetAngularVelocity == 1200 &&
                        lowerCogAngleDist <= 48 &&

                        cogConfigurations[2].LowerCogCurrentAngularVelocity <= 200 &&
                        cogConfigurations[3].LowerCogCurrentAngularVelocity <= 200 &&
                        cogConfigurations[4].LowerCogCurrentAngularVelocity <= 200 &&
                        cogConfigurations[5].LowerCogCurrentAngularVelocity <= 200 &&

                        cogConfigurations[2].LowerCogCurrentAngularVelocity >= 50 &&
                        cogConfigurations[3].LowerCogCurrentAngularVelocity >= 50 &&
                        cogConfigurations[4].LowerCogCurrentAngularVelocity >= 50 &&
                        cogConfigurations[5].LowerCogCurrentAngularVelocity >= 50

                        /*
                        cogConfigurations[2].LowerCogCurrentAngularVelocity == 200 &&
                        cogConfigurations[3].LowerCogCurrentAngularVelocity == 150 &&
                        cogConfigurations[4].LowerCogCurrentAngularVelocity == 100 &&
                        cogConfigurations[5].LowerCogCurrentAngularVelocity == 50
                        */)
                    {
                        return frame;
                    }
                }

                //if (!upperCogEndingYaws.Any(yaw => yaw == upperCog._endingYaw))
                //    return null;
            }

            return null;
        }

        private class CogConfiguration
        {
            public readonly int UpperCogAngle;
            public readonly int UpperCogCurrentAngularVelocity;
            public readonly int UpperCogTargetAngularVelocity;
            public readonly int LowerCogAngle;
            public readonly int LowerCogCurrentAngularVelocity;
            public readonly int LowerCogTargetAngularVelocity;

            public CogConfiguration(TtcCog upperCog, TtcCog lowerCog)
            {
                UpperCogAngle = upperCog._angle;
                UpperCogCurrentAngularVelocity = upperCog._currentAngularVelocity;
                UpperCogTargetAngularVelocity = upperCog._targetAngularVelocity;
                LowerCogAngle = lowerCog._angle;
                LowerCogCurrentAngularVelocity = lowerCog._currentAngularVelocity;
                LowerCogTargetAngularVelocity = lowerCog._targetAngularVelocity;
            }
        }

        private static List<TtcObject> CreateRngObjects(TtcRng rng, List<int> dustFrames)
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
                rngObjects.Add(new TtcTreadmill(rng, i == 0).SetIndex(i + 1));
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
                rngObjects.Add(new TtcTreadmill(rng, false).SetIndex(i + 6));
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

        private static List<TtcObject> CreateRngObjectsFromGame(TtcRng rng, List<int> dustFrames)
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
    }

}
