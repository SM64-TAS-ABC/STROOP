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
            _rngObjects = TtcUtilities.CreateRngObjects(_rng, dustFrames);

            //set up testing variables
            _startingFrame = startingFrame; //the frame directly preceding any object initialization
        }

        public TtcSimulation(List<int> dustFrames = null)
        {
            //set up objects
            _rng = new TtcRng(Config.Stream.GetUInt16(MiscConfig.RngAddress));
            _rngObjects = TtcUtilities.CreateRngObjectsFromGame(_rng, dustFrames);

            //set up testing variables
            _startingFrame = MupenUtilities.GetFrameCount(); //the frame directly preceding any object initialization
        }

        public TtcSimulation(TtcSaveState saveState)
        {
            (_rng, _rngObjects) = TtcUtilities.CreateRngObjectsFromSaveState(saveState);
            _startingFrame = 0;
        }

        public TtcSimulation(string saveStateString) : this(new TtcSaveState(saveStateString))
        {
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

        public bool FindIdealPendulumManipulation(uint pendulumAddress)
        {
            int? objectIndexNullable = ObjectUtilities.GetObjectIndex(pendulumAddress);
            if (!objectIndexNullable.HasValue) return false;
            int objectIndex = objectIndexNullable.Value;

            TtcPendulum pendulum = _rngObjects[objectIndex] as TtcPendulum;
            int pendulumAmplitudeStart = (int)WatchVariableSpecialUtilities.GetPendulumAmplitude(
                pendulum._accelerationDirection, pendulum._accelerationMagnitude, pendulum._angularVelocity, pendulum._angle);
            int? pendulumSwingIndexStartNullable = TableConfig.PendulumSwings.GetPendulumSwingIndex(pendulumAmplitudeStart);
            if (!pendulumSwingIndexStartNullable.HasValue) return false;
            int pendulumSwingIndexStart = pendulumSwingIndexStartNullable.Value;

            //iterate through frames to update objects
            int frame = _startingFrame;
            int counter = 0;
            while (frame < _startingFrame + 300)
            {
                frame++;
                counter++;
                foreach (TtcObject rngObject in _rngObjects)
                {
                    rngObject.SetFrame(frame);
                    rngObject.Update();
                }

                int pendulumAmplitude = (int)WatchVariableSpecialUtilities.GetPendulumAmplitude(
                    pendulum._accelerationDirection, pendulum._accelerationMagnitude, pendulum._angularVelocity, pendulum._angle);
                int? pendulumSwingIndexNullable = TableConfig.PendulumSwings.GetPendulumSwingIndex(pendulumAmplitude);
                if (!pendulumSwingIndexNullable.HasValue) return false;
                int pendulumSwingIndex = pendulumSwingIndexNullable.Value;

                if (pendulumSwingIndex > pendulumSwingIndexStart)
                {
                    return pendulum._waitingTimer == 0;
                }
                else if (pendulumSwingIndex < pendulumSwingIndexStart)
                {
                    return false;
                }
            }

            return false;
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
        
    }

}
