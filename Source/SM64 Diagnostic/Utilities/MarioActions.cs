using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Utilities
{
    public static class MarioActions
    {
        static uint _prevMarioGraphic = 0x00;

        private struct PositionAddressAngle
        {
            public readonly uint XAddress;
            public readonly uint YAddress;
            public readonly uint ZAddress;
            public readonly ushort Angle;

            public PositionAddressAngle(uint xAddress, uint yAddress, uint zAddress, ushort angle = 0)
            {
                XAddress = xAddress;
                YAddress = yAddress;
                ZAddress = zAddress;
                Angle = angle;
            }
        }

        private static bool MoveThings(ProcessStream stream, List<PositionAddressAngle> posAddressAngles,
            float xValue, float yValue, float zValue, bool translate, bool useRelative = false)
        {
            if (posAddressAngles.Count == 0)
                return false;

            bool success = true;
            stream.Suspend();

            foreach (var posAddressAngle in posAddressAngles)
            {
                float currentXValue = xValue;
                float currentYValue = yValue;
                float currentZValue = zValue;

                if (translate)
                {
                    handleScaling(ref currentXValue, ref currentZValue);
                    handleRelativeAngle(ref currentXValue, ref currentZValue, useRelative, posAddressAngle.Angle);
                    currentXValue += stream.GetSingle(posAddressAngle.XAddress);
                    currentYValue += stream.GetSingle(posAddressAngle.YAddress);
                    currentZValue += stream.GetSingle(posAddressAngle.ZAddress);
                }

                success &= stream.SetValue(currentXValue, posAddressAngle.XAddress);
                success &= stream.SetValue(currentYValue, posAddressAngle.YAddress);
                success &= stream.SetValue(currentZValue, posAddressAngle.ZAddress);
            }

            stream.Resume();
            return success;
        }

        public static bool GoToObjects(ProcessStream stream, List<uint> objAddresses)
        {
            if (objAddresses.Count == 0)
                return false;

            List<PositionAddressAngle> posAddressAngles = new List<PositionAddressAngle>();
            posAddressAngles.Add(new PositionAddressAngle(
                Config.Mario.StructAddress + Config.Mario.XOffset,
                Config.Mario.StructAddress + Config.Mario.YOffset,
                Config.Mario.StructAddress + Config.Mario.ZOffset));

            float xDestination = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.ObjectXOffset));
            float yDestination = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.ObjectYOffset));
            float zDestination = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.ObjectZOffset));

            yDestination += Config.Mario.MoveToObjectYOffset;

            return MoveThings(stream, posAddressAngles, xDestination, yDestination, zDestination, false);
        }

        public static bool RetrieveObjects(ProcessStream stream, List<uint> objAddresses)
        {
            List<PositionAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<PositionAddressAngle>(
                    objAddress => new PositionAddressAngle(
                        objAddress + Config.ObjectSlots.ObjectXOffset,
                        objAddress + Config.ObjectSlots.ObjectYOffset,
                        objAddress + Config.ObjectSlots.ObjectZOffset));

            float xDestination = stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float yDestination = stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float zDestination = stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            yDestination += Config.ObjectSlots.MoveToMarioYOffset;

            return MoveThings(stream, posAddressAngles, xDestination, yDestination, zDestination, false);
        }

        public static bool TranslateObjects(ProcessStream stream, List<uint> objAddresses,
            float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<PositionAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<PositionAddressAngle>(
                    objAddress => new PositionAddressAngle(
                        objAddress + Config.ObjectSlots.ObjectXOffset,
                        objAddress + Config.ObjectSlots.ObjectYOffset,
                        objAddress + Config.ObjectSlots.ObjectZOffset,
                        stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset)));

            return MoveThings(stream, posAddressAngles, xOffset, yOffset, zOffset, true, useRelative);
        }

        public static bool TranslateObjectHomes(ProcessStream stream, List<uint> objAddresses,
            float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<PositionAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<PositionAddressAngle>(
                    objAddress => new PositionAddressAngle(
                        objAddress + Config.ObjectSlots.HomeXOffset,
                        objAddress + Config.ObjectSlots.HomeYOffset,
                        objAddress + Config.ObjectSlots.HomeZOffset,
                        stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset)));

            return MoveThings(stream, posAddressAngles, xOffset, yOffset, zOffset, true, useRelative);
        }

        public static bool RotateObjects(ProcessStream stream, List<uint> objAddresses,
            int yawOffset, int pitchOffset, int rollOffset)
        {
            if (objAddresses.Count == 0)
                return false;

            bool success = true;
            stream.Suspend();

            foreach (var objAddress in objAddresses)
            {
                ushort yawFacing, pitchFacing, rollFacing, yawMoving, pitchMoving, rollMoving;
                yawFacing = stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset);
                pitchFacing = stream.GetUInt16(objAddress + Config.ObjectSlots.PitchFacingOffset);
                rollFacing = stream.GetUInt16(objAddress + Config.ObjectSlots.RollFacingOffset);
                yawMoving = stream.GetUInt16(objAddress + Config.ObjectSlots.YawMovingOffset);
                pitchMoving = stream.GetUInt16(objAddress + Config.ObjectSlots.PitchMovingOffset);
                rollMoving = stream.GetUInt16(objAddress + Config.ObjectSlots.RollMovingOffset);

                yawFacing += (ushort)yawOffset;
                pitchFacing += (ushort)pitchOffset;
                rollFacing += (ushort)rollOffset;
                yawMoving += (ushort)yawOffset;
                pitchMoving += (ushort)pitchOffset;
                rollMoving += (ushort)rollOffset;

                success &= stream.SetValue(yawFacing, objAddress + Config.ObjectSlots.YawFacingOffset);
                success &= stream.SetValue(pitchFacing, objAddress + Config.ObjectSlots.PitchFacingOffset);
                success &= stream.SetValue(rollFacing, objAddress + Config.ObjectSlots.RollFacingOffset);
                success &= stream.SetValue(yawMoving, objAddress + Config.ObjectSlots.YawMovingOffset);
                success &= stream.SetValue(pitchMoving, objAddress + Config.ObjectSlots.PitchMovingOffset);
                success &= stream.SetValue(rollMoving, objAddress + Config.ObjectSlots.RollMovingOffset);
            }

            stream.Resume();
            return success;
        }

        public static bool ScaleObjects(ProcessStream stream, List<uint> objAddresses,
            float widthChange, float heightChange, float depthChange)
        {
            List<PositionAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<PositionAddressAngle>(
                    objAddress => new PositionAddressAngle(
                        objAddress + Config.ObjectSlots.ScaleWidthOffset,
                        objAddress + Config.ObjectSlots.ScaleHeightOffset,
                        objAddress + Config.ObjectSlots.ScaleDepthOffset));

            return MoveThings(stream, posAddressAngles, widthChange, heightChange, depthChange, true, false);
        }

        public static bool GoToObjectsHome(ProcessStream stream, List<uint> objAddresses)
        {
            if (objAddresses.Count == 0)
                return false;

            List<PositionAddressAngle> posAddressAngles = new List<PositionAddressAngle>();
            posAddressAngles.Add(new PositionAddressAngle(
                Config.Mario.StructAddress + Config.Mario.XOffset,
                Config.Mario.StructAddress + Config.Mario.YOffset,
                Config.Mario.StructAddress + Config.Mario.ZOffset));

            float xDestination = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.HomeXOffset));
            float yDestination = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.HomeYOffset));
            float zDestination = objAddresses.Average(obj => stream.GetSingle(obj + Config.ObjectSlots.HomeZOffset));

            yDestination += Config.Mario.MoveToObjectYOffset;

            return MoveThings(stream, posAddressAngles, xDestination, yDestination, zDestination, false);
        }

        public static bool RetrieveObjectsHome(ProcessStream stream, List<uint> objAddresses)
        {
            List<PositionAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<PositionAddressAngle>(
                    objAddress => new PositionAddressAngle(
                        objAddress + Config.ObjectSlots.HomeXOffset,
                        objAddress + Config.ObjectSlots.HomeYOffset,
                        objAddress + Config.ObjectSlots.HomeZOffset));

            float xDestination = stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float yDestination = stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float zDestination = stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            yDestination += Config.ObjectSlots.MoveToMarioYOffset;

            return MoveThings(stream, posAddressAngles, xDestination, yDestination, zDestination, false);
        }

        public static bool CloneObject(ProcessStream stream, uint objAddress)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;
            stream.Suspend();

            uint lastObject = stream.GetUInt32(marioAddress + Config.Mario.HoldingObjectPointerOffset);
            
            // Set clone action flags
            if (lastObject == 0x00000000U && !Config.DisableActionUpdateWhenCloning)
            {
                // Set Next action
                uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
                uint nextAction = Config.MarioActions.GetAfterCloneValue(currentAction);
                success &= stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);
            }

            // Set new holding value
            success &= stream.SetValue(objAddress, marioAddress + Config.Mario.HoldingObjectPointerOffset);

            stream.Resume();
            return success;
        }

        public static bool UnCloneObject(ProcessStream stream, uint objAddress)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            // Set mario's next action
            uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
            uint nextAction = Config.MarioActions.GetAfterUncloneValue(currentAction);
            success &= stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);

            // Clear mario's holding object
            success &= stream.SetValue(0x00000000U, marioAddress + Config.Mario.HoldingObjectPointerOffset);

            stream.Resume();
            return success;
        }

        public static bool UnloadObject(ProcessStream stream, List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            foreach (var address in addresses)
            {
                var test = stream.GetUInt16(address + Config.ObjectSlots.ObjectActiveOffset);
                success &= stream.SetValue((short) 0x0000, address + Config.ObjectSlots.ObjectActiveOffset);
            }
            return success;
        }

        public static bool ReviveObject(ProcessStream stream, List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            stream.Suspend();

            foreach (var address in addresses)
            {
                // Find process group
                uint scriptAddress = stream.GetUInt32(address + Config.ObjectSlots.BehaviorScriptOffset);
                if (scriptAddress == 0x00000000)
                    continue;
                uint firstScriptAction = stream.GetUInt32(scriptAddress);
                if ((firstScriptAction & 0xFF000000U) != 0x00000000U)
                    continue;
                byte processGroup = (byte)((firstScriptAction & 0x00FF0000U) >> 16);

                // Read first object in group
                var groupConfig = Config.ObjectGroups;
                uint groupAddress = groupConfig.FirstGroupingAddress + processGroup * groupConfig.ProcessGroupStructSize;

                // Loop through and find last object in group
                uint lastGroupObj = groupAddress;
                while (stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset) != groupAddress)
                    lastGroupObj = stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset);

                // Remove object from current group
                uint nextObj = stream.GetUInt32(address + groupConfig.ProcessNextLinkOffset);
                uint prevObj = stream.GetUInt32(groupConfig.VactantPointerAddress);
                if (prevObj == address)
                {
                    // Set new vacant pointer
                    success &= stream.SetValue(nextObj, groupConfig.VactantPointerAddress);
                }
                else
                {
                    for (int i = 0; i < Config.ObjectSlots.MaxSlots; i++)
                    {
                        uint obj = stream.GetUInt32(prevObj + groupConfig.ProcessNextLinkOffset);
                        if (obj == address)
                            break;
                        prevObj = obj;
                    }
                    success &= stream.SetValue(nextObj, prevObj + groupConfig.ProcessNextLinkOffset);
                }


                // Insert object in new group
                nextObj = stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset);
                success &= stream.SetValue(address, nextObj + groupConfig.ProcessPreviousLinkOffset);
                success &= stream.SetValue(address, lastGroupObj + groupConfig.ProcessNextLinkOffset);
                success &= stream.SetValue(lastGroupObj, address + groupConfig.ProcessPreviousLinkOffset);
                success &= stream.SetValue(nextObj, address + groupConfig.ProcessNextLinkOffset);

                success &= stream.SetValue((short)0x0101, address + Config.ObjectSlots.ObjectActiveOffset);

                if (addresses.Count > 1)
                    if (!stream.RefreshRam() || !success)
                        break;
            }

            stream.Resume();
            return success;
        }

        public static bool DebilitateObject(ProcessStream stream, List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            stream.Suspend();
            foreach (var address in addresses)
            {
                success &= stream.SetValue(0x800EE5F8, address + Config.ObjectSlots.ReleaseStatusOffset);
            }
            stream.Resume();
            return success;
        }

        public static bool InteractObject(ProcessStream stream, List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            stream.Suspend();
            foreach (var address in addresses)
            {
                success &= stream.SetValue(0xFFFFFFFF, address + Config.ObjectSlots.InteractionStatusOffset);
            }
            stream.Resume();
            return success;
        }

        public static bool ToggleHandsfree(ProcessStream stream)
        {
            bool success = true;
            var marioAddress = Config.Mario.StructAddress;

            stream.Suspend();

            var holdingObj = stream.GetUInt32(marioAddress + Config.Mario.HoldingObjectPointerOffset);

            if (holdingObj != 0x00000000U)
            {
                uint currentAction = stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
                uint nextAction = Config.MarioActions.GetHandsfreeValue(currentAction);
                success = stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);
            }

            stream.Resume();

            return success;
        }

        public static bool ToggleVisibility(ProcessStream stream)
        {
            bool success = true;
            stream.Suspend();

            var marioObjRef = stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
            if (marioObjRef != 0x00000000U)
            {
                var marioGraphics = stream.GetUInt32(marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                if (marioGraphics == 0)
                { 
                    success &= stream.SetValue(_prevMarioGraphic, marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                }
                else
                {
                    _prevMarioGraphic = marioGraphics;
                    success &= stream.SetValue(0x00000000U, marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                }
            }

            stream.Resume();

            return success;
        }

        public static bool TranslateMario(ProcessStream stream, float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<PositionAddressAngle> posAddressAngles = new List<PositionAddressAngle>();
            posAddressAngles.Add(new PositionAddressAngle(
                Config.Mario.StructAddress + Config.Mario.XOffset,
                Config.Mario.StructAddress + Config.Mario.YOffset,
                Config.Mario.StructAddress + Config.Mario.ZOffset,
                stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset)));

            return MoveThings(stream, posAddressAngles, xOffset, yOffset, zOffset, true, useRelative);
        }

        public static bool TranslateHOLP(ProcessStream stream, float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<PositionAddressAngle> posAddressAngles = new List<PositionAddressAngle>();
            posAddressAngles.Add(new PositionAddressAngle(
                Config.Mario.StructAddress + Config.HolpX,
                Config.Mario.StructAddress + Config.HolpY,
                Config.Mario.StructAddress + Config.HolpZ,
                stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset)));

            return MoveThings(stream, posAddressAngles, xOffset, yOffset, zOffset, true, useRelative);
        }

        public static bool MarioChangeYaw(ProcessStream stream, int yawOffset)
        {
            var marioAddress = Config.Mario.StructAddress;

            ushort yaw = stream.GetUInt16(marioAddress + Config.Mario.YawFacingOffset);
            yaw += (ushort)yawOffset;

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(yaw, marioAddress + Config.Mario.YawFacingOffset);

            stream.Resume();
            return success;
        }

        public static bool MarioChangeHspd(ProcessStream stream, float hspdOffset)
        {
            var marioAddress = Config.Mario.StructAddress;

            float hspd = stream.GetSingle(marioAddress + Config.Mario.HSpeedOffset);
            hspd += hspdOffset;

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(hspd, marioAddress + Config.Mario.HSpeedOffset);

            stream.Resume();
            return success;
        }

        public static bool MarioChangeVspd(ProcessStream stream, float vspdOffset)
        {
            var marioAddress = Config.Mario.StructAddress;

            float vspd = stream.GetSingle(marioAddress + Config.Mario.VSpeedOffset);
            vspd += vspdOffset;

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(vspd, marioAddress + Config.Mario.VSpeedOffset);

            stream.Resume();
            return success;
        }

        public static bool RefillHp(ProcessStream stream)
        {
            return stream.SetValue(Config.Hud.FullHp, Config.Hud.HpAddress);
        }

        public static bool Die(ProcessStream stream)
        {
            return stream.SetValue((short)255, Config.Hud.HpAddress);
        }

        public static bool StandardHud(ProcessStream stream)
        {
            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(Config.Hud.FullHp, Config.Hud.HpAddress);
            success &= stream.SetValue(Config.Hud.StandardCoins, Config.Hud.CoinCountAddress);
            success &= stream.SetValue(Config.Hud.StandardLives, Config.Hud.LiveCountAddress);
            success &= stream.SetValue(Config.Hud.StandardStars, Config.Hud.StarCountAddress);
            success &= stream.SetValue(Config.Hud.FullHpInt, Config.Hud.DisplayHpAddress);
            success &= stream.SetValue(Config.Hud.StandardCoins, Config.Hud.DisplayCoinCountAddress);
            success &= stream.SetValue((short)Config.Hud.StandardLives, Config.Hud.DisplayLiveCountAddress);
            success &= stream.SetValue(Config.Hud.StandardStars, Config.Hud.DisplayStarCountAddress);

            stream.Resume();
            return success;
        }

        public static bool GoToTriangle(ProcessStream stream, uint triangleAddress, int vertex, bool _useMisalignmentOffset = false)
        {
            if (triangleAddress == 0x0000)
                return false;

            float newX, newY, newZ;
            switch(vertex)
            {
                case 1:
                    newX = stream.GetInt16(triangleAddress + Config.TriangleOffsets.X1);
                    newY = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1);
                    newZ = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z1);
                    break;

                case 2:
                    newX = stream.GetInt16(triangleAddress + Config.TriangleOffsets.X2);
                    newY = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2);
                    newZ = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z2);
                    break;

                case 3:
                    newX = stream.GetInt16(triangleAddress + Config.TriangleOffsets.X3);
                    newY = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3);
                    newZ = stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z3);
                    break;

                default:
                    throw new Exception("There are only 3 vertices in a triangle. You are an idiot :).");
            }

            if (_useMisalignmentOffset)
            {
                newX += (newX >= 0) ? 0.5f : -0.5f;
                newZ += (newZ >= 0) ? 0.5f : -0.5f;
            }

            // Move mario to triangle (while in same Pu)
            return PuUtilities.MoveToInCurrentPu(stream, newX, newY, newZ);
        }

        public static bool RetrieveTriangle(ProcessStream stream, uint triangleAddress)
        {
            if (triangleAddress == 0x0000)
                return false;

            float normX, normY, normZ, oldNormOffset;
            normX = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormX);
            normY = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormY);
            normZ = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormZ);
            oldNormOffset = stream.GetSingle(triangleAddress + Config.TriangleOffsets.Offset);

            // Get Mario position
            float marioX, marioY, marioZ;
            var marioAddress = Config.Mario.StructAddress;
            marioX = stream.GetSingle(marioAddress + Config.Mario.XOffset);
            marioY = stream.GetSingle(marioAddress + Config.Mario.YOffset);
            marioZ = stream.GetSingle(marioAddress + Config.Mario.ZOffset);

            float normOffset = -(normX * marioX + normY * marioY + normZ * marioZ);
            float normDiff = normOffset - oldNormOffset;

            short yOffset = (short)(-normDiff * normY);

            short v1Y, v2Y, v3Y;
            v1Y = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1) + yOffset);
            v2Y = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2) + yOffset);
            v3Y = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3) + yOffset);

            short yMin = (short)(Math.Min(Math.Min(v1Y, v2Y), v3Y) - 5);
            short yMax = (short)(Math.Max(Math.Max(v1Y, v2Y), v3Y) + 5);

            bool success = true;
            stream.Suspend();
            
            success &= stream.SetValue(v1Y, triangleAddress + Config.TriangleOffsets.Y1);
            success &= stream.SetValue(v2Y, triangleAddress + Config.TriangleOffsets.Y2);
            success &= stream.SetValue(v3Y, triangleAddress + Config.TriangleOffsets.Y3);
            success &= stream.SetValue(yMin, triangleAddress + Config.TriangleOffsets.YMin);
            success &= stream.SetValue(yMax, triangleAddress + Config.TriangleOffsets.YMax);
            success &= stream.SetValue(normOffset, triangleAddress + Config.TriangleOffsets.Offset);

            stream.Resume();
            return success;
        }

        public static bool NeutralizeTriangle(ProcessStream stream, uint triangleAddress)
        {
            if (triangleAddress == 0x0000)
                return false;

            short neutralizedSurfaceType = (short)(Config.NeutralizeTriangleWith21 ? 21 : 0);

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(neutralizedSurfaceType, triangleAddress + Config.TriangleOffsets.SurfaceType);

            stream.Resume();
            return success;
        }

        public static bool AnnihilateTriangle(ProcessStream stream, uint triangleAddress)
        {
            if (triangleAddress == 0x0000)
                return false;

            short xzCoordinate = 16000;
            short yCoordinate = 30000;
            short v1X = xzCoordinate;
            short v1Y = yCoordinate;
            short v1Z = xzCoordinate;
            short v2X = xzCoordinate;
            short v2Y = yCoordinate;
            short v2Z = xzCoordinate;
            short v3X = xzCoordinate;
            short v3Y = yCoordinate;
            short v3Z = xzCoordinate;
            float normX = 0;
            float normY = 0;
            float normZ = 0;
            float normOffset = 16000;

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(v1X, triangleAddress + Config.TriangleOffsets.X1);
            success &= stream.SetValue(v1Y, triangleAddress + Config.TriangleOffsets.Y1);
            success &= stream.SetValue(v1Z, triangleAddress + Config.TriangleOffsets.Z1);
            success &= stream.SetValue(v2X, triangleAddress + Config.TriangleOffsets.X2);
            success &= stream.SetValue(v2Y, triangleAddress + Config.TriangleOffsets.Y2);
            success &= stream.SetValue(v2Z, triangleAddress + Config.TriangleOffsets.Z2);
            success &= stream.SetValue(v3X, triangleAddress + Config.TriangleOffsets.X3);
            success &= stream.SetValue(v3Y, triangleAddress + Config.TriangleOffsets.Y3);
            success &= stream.SetValue(v3Z, triangleAddress + Config.TriangleOffsets.Z3);
            success &= stream.SetValue(normX, triangleAddress + Config.TriangleOffsets.NormX);
            success &= stream.SetValue(normY, triangleAddress + Config.TriangleOffsets.NormY);
            success &= stream.SetValue(normZ, triangleAddress + Config.TriangleOffsets.NormZ);
            success &= stream.SetValue(normOffset, triangleAddress + Config.TriangleOffsets.Offset);

            stream.Resume();
            return success;
        }

        public static bool MoveTriangle(ProcessStream stream, uint triangleAddress,
            float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            if (triangleAddress == 0x0000)
                return false;

            handleScaling(ref xOffset, ref zOffset);

            float normX, normY, normZ, oldNormOffset;
            normX = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormX);
            normY = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormY);
            normZ = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormZ);
            oldNormOffset = stream.GetSingle(triangleAddress + Config.TriangleOffsets.Offset);

            ushort relativeAngle = MoreMath.getUphillAngle(normX, normY, normZ);
            handleRelativeAngle(ref xOffset, ref zOffset, useRelative, relativeAngle);

            float newNormOffset = oldNormOffset - normX * xOffset - normY * yOffset - normZ * zOffset;

            short newX1, newY1, newZ1, newX2, newY2, newZ2, newX3, newY3, newZ3;
            newX1 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.X1) + xOffset);
            newY1 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1) + yOffset);
            newZ1 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z1) + zOffset);
            newX2 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.X2) + xOffset);
            newY2 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2) + yOffset);
            newZ2 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z2) + zOffset);
            newX3 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.X3) + xOffset);
            newY3 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3) + yOffset);
            newZ3 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z3) + zOffset);

            short newYMin = (short)(Math.Min(Math.Min(newY1, newY2), newY3) - 5);
            short newYMax = (short)(Math.Max(Math.Max(newY1, newY2), newY3) + 5);

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(newNormOffset, triangleAddress + Config.TriangleOffsets.Offset);
            success &= stream.SetValue(newX1, triangleAddress + Config.TriangleOffsets.X1);
            success &= stream.SetValue(newY1, triangleAddress + Config.TriangleOffsets.Y1);
            success &= stream.SetValue(newZ1, triangleAddress + Config.TriangleOffsets.Z1);
            success &= stream.SetValue(newX2, triangleAddress + Config.TriangleOffsets.X2);
            success &= stream.SetValue(newY2, triangleAddress + Config.TriangleOffsets.Y2);
            success &= stream.SetValue(newZ2, triangleAddress + Config.TriangleOffsets.Z2);
            success &= stream.SetValue(newX3, triangleAddress + Config.TriangleOffsets.X3);
            success &= stream.SetValue(newY3, triangleAddress + Config.TriangleOffsets.Y3);
            success &= stream.SetValue(newZ3, triangleAddress + Config.TriangleOffsets.Z3);
            success &= stream.SetValue(newYMin, triangleAddress + Config.TriangleOffsets.YMin);
            success &= stream.SetValue(newYMax, triangleAddress + Config.TriangleOffsets.YMax);

            stream.Resume();
            return success;
        }

        public static bool MoveTriangleNormal(ProcessStream stream, uint triangleAddress, float normalChange)
        {
            if (triangleAddress == 0x0000)
                return false;

            float normX, normY, normZ, oldNormOffset;
            normX = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormX);
            normY = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormY);
            normZ = stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormZ);
            oldNormOffset = stream.GetSingle(triangleAddress + Config.TriangleOffsets.Offset);

            float newNormOffset = oldNormOffset - normalChange;

            double xChange = normalChange * normX;
            double yChange = normalChange * normY;
            double zChange = normalChange * normZ;

            short newX1, newY1, newZ1, newX2, newY2, newZ2, newX3, newY3, newZ3;
            newX1 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.X1) + xChange);
            newY1 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1) + yChange);
            newZ1 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z1) + zChange);
            newX2 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.X2) + xChange);
            newY2 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2) + yChange);
            newZ2 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z2) + zChange);
            newX3 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.X3) + xChange);
            newY3 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3) + yChange);
            newZ3 = (short)(stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z3) + zChange);

            short newYMin = (short)(Math.Min(Math.Min(newY1, newY2), newY3) - 5);
            short newYMax = (short)(Math.Max(Math.Max(newY1, newY2), newY3) + 5);

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue(newNormOffset, triangleAddress + Config.TriangleOffsets.Offset);
            success &= stream.SetValue(newX1, triangleAddress + Config.TriangleOffsets.X1);
            success &= stream.SetValue(newY1, triangleAddress + Config.TriangleOffsets.Y1);
            success &= stream.SetValue(newZ1, triangleAddress + Config.TriangleOffsets.Z1);
            success &= stream.SetValue(newX2, triangleAddress + Config.TriangleOffsets.X2);
            success &= stream.SetValue(newY2, triangleAddress + Config.TriangleOffsets.Y2);
            success &= stream.SetValue(newZ2, triangleAddress + Config.TriangleOffsets.Z2);
            success &= stream.SetValue(newX3, triangleAddress + Config.TriangleOffsets.X3);
            success &= stream.SetValue(newY3, triangleAddress + Config.TriangleOffsets.Y3);
            success &= stream.SetValue(newZ3, triangleAddress + Config.TriangleOffsets.Z3);
            success &= stream.SetValue(newYMin, triangleAddress + Config.TriangleOffsets.YMin);
            success &= stream.SetValue(newYMax, triangleAddress + Config.TriangleOffsets.YMax);

            stream.Resume();
            return success;
        }

        public static bool TranslateCamera(ProcessStream stream, float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<PositionAddressAngle> posAddressAngles = new List<PositionAddressAngle>();
            posAddressAngles.Add(new PositionAddressAngle(
                Config.Camera.CameraX,
                Config.Camera.CameraY,
                Config.Camera.CameraZ,
                (UInt16)(stream.GetUInt32(Config.Camera.CameraRot))));

            return MoveThings(stream, posAddressAngles, xOffset, yOffset, zOffset, true, useRelative);
        }

        public static bool TranslateCameraSpherically(ProcessStream stream, float radiusOffset, float thetaOffset, float phiOffset, float pivotX, float pivotY, float pivotZ)
        {
            handleScaling(ref thetaOffset, ref phiOffset);

            float oldX, oldY, oldZ;
            oldX = stream.GetSingle(Config.Camera.CameraX);
            oldY = stream.GetSingle(Config.Camera.CameraY);
            oldZ = stream.GetSingle(Config.Camera.CameraZ);

            double newX, newY, newZ;
            (newX, newY, newZ) = MoreMath.OffsetSphericallyAboutPivot(oldX, oldY, oldZ, radiusOffset, thetaOffset, phiOffset, pivotX, pivotY, pivotZ);

            bool success = true;
            stream.Suspend();

            success &= stream.SetValue((float)newX, Config.Camera.CameraX);
            success &= stream.SetValue((float)newY, Config.Camera.CameraY);
            success &= stream.SetValue((float)newZ, Config.Camera.CameraZ);

            stream.Resume();
            return success;
        }

        public static void handleScaling(ref float xOffset, ref float zOffset)
        {
            if (Config.ScaleDiagonalPositionControllerButtons)
            {
                (xOffset, zOffset) = ((float, float))MoreMath.ScaleValues(xOffset, zOffset);
            }
        }

        public static void handleRelativeAngle(ref float xOffset, ref float zOffset, bool useRelative, ushort relativeAngle)
        {
            if (useRelative)
            {
                double thetaChange = relativeAngle - 32768;
                (xOffset, _, zOffset) = ((float, float, float))MoreMath.OffsetSpherically(xOffset, 0, zOffset, 0, thetaChange, 0);
            }
        }
    }
}
