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

        public TtcSaveState GetSaveState()
        {
            return new TtcSaveState(_rng.GetRng(), _rngObjects);
        }

        public void AddDustFrames(List<int> dustFrames)
        {
            TtcDust dust = (TtcDust)_rngObjects.FirstOrDefault(obj => obj is TtcDust);
            if (dust == null) throw new ArgumentOutOfRangeException();
            dust.AddDustFrames(dustFrames);
        }

        public void TurnOffBobombs()
        {
            foreach (TtcObject obj in _rngObjects)
            {
                if (obj is TtcBobomb bobomb)
                {
                    bobomb.SetWithinMarioRange(0);
                }
            }
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
            lines.Add(new TtcSaveState(_rng.GetRng(), _rngObjects).ToString());
            return String.Join("\r\n", lines);
        }

        public int? FindIdealCogConfiguration(int numFramesMin, int numFramesMax)
        {
            TtcCog upperCog = _rngObjects[31] as TtcCog;
            TtcCog lowerCog = _rngObjects[32] as TtcCog;
            List<CogConfiguration> cogConfigurations = new List<CogConfiguration>();

            int numCogConfigurations = 9;
            int lowerCogGoodAngle = 9892;
            List<int> lowerCogGoodAngles = Enumerable.Range(0, 6).ToList()
                .ConvertAll(index => lowerCogGoodAngle + 65536 / 6 * index)
                .ConvertAll(angle => (int)MoreMath.NormalizeAngleTruncated(angle));

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

                    int lowerCogAngleDist = lowerCogGoodAngles.Min(
                        angle => (int)MoreMath.GetAngleDistance(
                            angle, MoreMath.NormalizeAngleTruncated(cogConfigurations[5].LowerCogAngle)));

                    bool upperCogPreGoal =
                        cogConfigurations[8].UpperCogAngle == 46432 && // right angle
                        cogConfigurations[7].UpperCogTargetAngularVelocity == 1200 && // was targeting 1200
                        cogConfigurations[8].UpperCogCurrentAngularVelocity == 1200; // moved at 1200 speed

                    bool upperCogGoal =
                        cogConfigurations[8].UpperCogAngle == 46432 && // right angle
                        cogConfigurations[7].UpperCogTargetAngularVelocity == 1200 && // was targeting 1200
                        cogConfigurations[8].UpperCogCurrentAngularVelocity == 1200 && // moved at 1200 speed
                        cogConfigurations[0].UpperCogTargetAngularVelocity == 1200; // had been targeting 1200 for some time

                    int lowerCogMinAngularVelocity = 0;
                    int lowerCogMaxAngularVelocity = 400;

                    bool lowerCogGoal =
                        lowerCogAngleDist <= 64 && // close to some right angle
                        // was moving slowly leading up to right angle
                        cogConfigurations[1].LowerCogCurrentAngularVelocity <= lowerCogMaxAngularVelocity &&
                        cogConfigurations[2].LowerCogCurrentAngularVelocity <= lowerCogMaxAngularVelocity &&
                        cogConfigurations[3].LowerCogCurrentAngularVelocity <= lowerCogMaxAngularVelocity &&
                        cogConfigurations[4].LowerCogCurrentAngularVelocity <= lowerCogMaxAngularVelocity &&
                        cogConfigurations[5].LowerCogCurrentAngularVelocity <= lowerCogMaxAngularVelocity &&
                        cogConfigurations[1].LowerCogCurrentAngularVelocity >= lowerCogMinAngularVelocity &&
                        cogConfigurations[2].LowerCogCurrentAngularVelocity >= lowerCogMinAngularVelocity &&
                        cogConfigurations[3].LowerCogCurrentAngularVelocity >= lowerCogMinAngularVelocity &&
                        cogConfigurations[4].LowerCogCurrentAngularVelocity >= lowerCogMinAngularVelocity &&
                        cogConfigurations[5].LowerCogCurrentAngularVelocity >= lowerCogMinAngularVelocity;

                    if (upperCogGoal && lowerCogGoal)
                    //if (upperCogPreGoal)
                    {
                        return frame;
                    }
                }

                //if (!upperCogEndingYaws.Any(yaw => yaw == upperCog._endingYaw))
                //    return null;
            }

            return null;
        }

        public (bool success, TtcSaveState saveState, int endFrame) FindIdealPendulumManipulation(uint pendulumAddress)
        {
            int? objectIndexNullable = ObjectUtilities.GetObjectIndex(pendulumAddress);
            if (!objectIndexNullable.HasValue) return (false, null, 0);
            int objectIndex = objectIndexNullable.Value;

            TtcPendulum pendulum = _rngObjects[objectIndex] as TtcPendulum;
            int pendulumAmplitudeStart = (int)WatchVariableSpecialUtilities.GetPendulumAmplitude(
                pendulum._accelerationDirection, pendulum._accelerationMagnitude, pendulum._angularVelocity, pendulum._angle);
            int? pendulumSwingIndexStartNullable = TableConfig.PendulumSwings.GetPendulumSwingIndex(pendulumAmplitudeStart);
            if (!pendulumSwingIndexStartNullable.HasValue) return (false, null, 0);
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
                if (!pendulumSwingIndexNullable.HasValue) return (false, null, 0);
                int pendulumSwingIndex = pendulumSwingIndexNullable.Value;

                if (pendulumSwingIndex > pendulumSwingIndexStart)
                {
                    if (pendulum._waitingTimer == 0)
                    {
                        return (true, GetSaveState(), frame);
                    }
                    else
                    {
                        return (false, null, 0);
                    }
                }
                else if (pendulumSwingIndex < pendulumSwingIndexStart)
                {
                    return (false, null, 0);
                }
            }

            return (false, null, 0);
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

        public void FindHandMovement()
        {
            ushort startAngle = 48700;
            ushort endAngle = 3912;
            ushort resetAngle = 44000;
            int margin = 100;

            TtcHand hand = _rngObjects[37] as TtcHand;

            bool goingForItBool = false;
            int goingForItFrame = 0;
            int bestDist = int.MinValue;
            int totalDist = (int)MoreMath.GetAngleDistance(startAngle, endAngle);

            int frame = _startingFrame;
            for (int counter = 0; true; counter++)
            {
                if (frame % 4000000 == 0)
                {
                    Config.Print("...frame {0}", frame);
                    return;
                }

                frame++;
                foreach (TtcObject rngObject in _rngObjects)
                {
                    rngObject.SetFrame(frame);
                    rngObject.Update();
                }

                bool atStartAngle = MoreMath.GetAngleDistance(hand._angle, startAngle) <= margin;
                bool atEndAngle = MoreMath.GetAngleDistance(hand._angle, endAngle) <= margin;
                bool atResetAngle = MoreMath.GetAngleDistance(hand._angle, resetAngle) <= margin;

                if (goingForItBool)
                {
                    if (atStartAngle)
                    {
                        goingForItBool = true;
                        goingForItFrame = frame;
                        //Config.Print("Start again on frame {0}", frame);
                    }
                    else if (atEndAngle)
                    {
                        //Config.Print("End on frame {0}", frame);
                        Config.Print("Success from {0} to {1}", goingForItFrame, frame);
                        return;
                    }
                    else if (atResetAngle)
                    {
                        goingForItBool = false;
                        //Config.Print("Reset on frame {0}", frame);
                    }
                }
                else
                {
                    if (atStartAngle)
                    {
                        goingForItBool = true;
                        goingForItFrame = frame;
                        //Config.Print("Start on frame {0}", frame);
                    }
                    else if (atEndAngle)
                    {

                    }
                    else if (atResetAngle)
                    {

                    }
                }

                if (goingForItBool)
                {
                    int currentDist = (int)MoreMath.GetAngleDifference(startAngle, hand._angle);
                    if (currentDist > bestDist)
                    {
                        bestDist = currentDist;
                        Config.Print(
                            "Frame {0} has dist {1} of {2} ({3})",
                            frame,
                            currentDist,
                            totalDist,
                            MoreMath.GetPercentString(currentDist, totalDist, 2));
                    }
                }
            }
        }

        public List<int> FindKeyHandFrames()
        {
            List<int> pendulumAnglesForDust = new List<int>()
            {
                -103861, -37756, 26919, 93440,
            };
            List<int> output = new List<int>();

            TtcPendulum pendulum = GetClosePendulum();
            int initialAmplitude = pendulum.GetAmplitude();

            int frame = _startingFrame;
            for (int counter = 0; true; counter++)
            {
                frame++;
                foreach (TtcObject rngObject in _rngObjects)
                {
                    rngObject.SetFrame(frame);
                    rngObject.Update();
                }
                
                if (pendulumAnglesForDust.Contains(pendulum._angle))
                {
                    output.Add(frame);
                }

                if (pendulum.GetAmplitude() != initialAmplitude)
                {
                    output.Add(frame);
                    if (output.Count != 5) throw new ArgumentOutOfRangeException();
                    return output;
                }
            }
        }

        public TtcPendulum GetClosePendulum()
        {
            return _rngObjects[8] as TtcPendulum;
        }

        public TtcHand GetUpperHand()
        {
            return _rngObjects[37] as TtcHand;
        }
    }

}
