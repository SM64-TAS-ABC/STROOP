using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;
using static SM64_Diagnostic.Managers.CamHackManager;
using SM64_Diagnostic.Managers;
using static SM64_Diagnostic.Structs.Configurations.PositionControllerRelativeAngleConfig;

namespace SM64_Diagnostic.Utilities
{
    public static class ButtonUtilities
    {
        private struct TripleAddressAngle
        {
            public readonly uint XAddress;
            public readonly uint YAddress;
            public readonly uint ZAddress;
            public readonly ushort? Angle;

            public TripleAddressAngle(uint xAddress, uint yAddress, uint zAddress, ushort? angle = null)
            {
                XAddress = xAddress;
                YAddress = yAddress;
                ZAddress = zAddress;
                Angle = angle;
            }

            public (uint XAddress, uint YAddress, uint ZAddress) GetTripleAddress()
            {
                return (XAddress, YAddress, ZAddress);
            }
        }
        
        private enum Change { SET, ADD, MULTIPLY };

        private static bool ChangeValues(List<TripleAddressAngle> posAddressAngles,
            float xValue, float yValue, float zValue, Change change, bool useRelative = false,
            (bool affectX, bool affectY, bool affectZ)? affects = null)
        {
            if (posAddressAngles.Count == 0)
                return false;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var posAddressAngle in posAddressAngles)
            {
                float currentXValue = xValue;
                float currentYValue = yValue;
                float currentZValue = zValue;

                if (change == Change.ADD)
                {
                    HandleScaling(ref currentXValue, ref currentZValue);
                    HandleRelativeAngle(ref currentXValue, ref currentZValue, useRelative, posAddressAngle.Angle);
                    currentXValue += Config.Stream.GetSingle(posAddressAngle.XAddress);
                    currentYValue += Config.Stream.GetSingle(posAddressAngle.YAddress);
                    currentZValue += Config.Stream.GetSingle(posAddressAngle.ZAddress);
                }

                if (change == Change.MULTIPLY)
                {
                    currentXValue *= Config.Stream.GetSingle(posAddressAngle.XAddress);
                    currentYValue *= Config.Stream.GetSingle(posAddressAngle.YAddress);
                    currentZValue *= Config.Stream.GetSingle(posAddressAngle.ZAddress);
                }

                if (!affects.HasValue || affects.Value.affectX)
                {
                    success &= Config.Stream.SetValue(currentXValue, posAddressAngle.XAddress);
                }

                if (!affects.HasValue || affects.Value.affectY)
                {
                    success &= Config.Stream.SetValue(currentYValue, posAddressAngle.YAddress);
                }

                if (!affects.HasValue || affects.Value.affectZ)
                {
                    success &= Config.Stream.SetValue(currentZValue, posAddressAngle.ZAddress);
                }
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static void HandleScaling(ref float xOffset, ref float zOffset)
        {
            if (Config.ScaleDiagonalPositionControllerButtons)
            {
                (xOffset, zOffset) = ((float, float))MoreMath.ScaleValues(xOffset, zOffset);
            }
        }

        public static void HandleRelativeAngle(ref float xOffset, ref float zOffset, bool useRelative, double? relativeAngle)
        {
            if (useRelative)
            {
                if (!relativeAngle.HasValue)
                    throw new ArgumentNullException();

                switch (Config.PositionControllerRelativeAngle.Relativity)
                {
                    case RelativityType.Recommended:
                        // relativeAngle is already correct
                        break;
                    case RelativityType.Mario:
                        relativeAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        break;
                    case RelativityType.Custom:
                        relativeAngle = MoreMath.NormalizeAngleUshort(Config.PositionControllerRelativeAngle.CustomAngle);
                        break;
                }
                double thetaChange = MoreMath.NormalizeAngleDouble(relativeAngle.Value - 32768);
                (xOffset, _, zOffset) = ((float, float, float))MoreMath.OffsetSpherically(xOffset, 0, zOffset, 0, thetaChange, 0);
            }
        }

        public static bool GotoObjects(List<uint> objAddresses, (bool affectX, bool affectY, bool affectZ)? affects = null)
        {
            if (objAddresses.Count == 0)
                return false;

            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Mario.StructAddress + Config.Mario.XOffset,
                        Config.Mario.StructAddress + Config.Mario.YOffset,
                        Config.Mario.StructAddress + Config.Mario.ZOffset)
                };

            float xDestination = objAddresses.Average(obj => Config.Stream.GetSingle(obj + Config.ObjectSlots.ObjectXOffset));
            float yDestination = objAddresses.Average(obj => Config.Stream.GetSingle(obj + Config.ObjectSlots.ObjectYOffset));
            float zDestination = objAddresses.Average(obj => Config.Stream.GetSingle(obj + Config.ObjectSlots.ObjectZOffset));

            HandleGotoOffset(ref xDestination, ref yDestination, ref zDestination);

            return ChangeValues(posAddressAngles, xDestination, yDestination, zDestination, Change.SET, false, affects);
        }

        public static bool RetrieveObjects(List<uint> objAddresses, (bool affectX, bool affectY, bool affectZ)? affects = null)
        {
            List<TripleAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<TripleAddressAngle>(
                    objAddress => new TripleAddressAngle(
                        objAddress + Config.ObjectSlots.ObjectXOffset,
                        objAddress + Config.ObjectSlots.ObjectYOffset,
                        objAddress + Config.ObjectSlots.ObjectZOffset));

            float xDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float yDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float zDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            HandleRetrieveOffset(ref xDestination, ref yDestination, ref zDestination);

            return ChangeValues(posAddressAngles, xDestination, yDestination, zDestination, Change.SET, false, affects);
        }

        private static void HandleGotoOffset(ref float xPos, ref float yPos, ref float zPos)
        {
            float gotoAbove = Config.GotoRetrieve.GotoAboveOffset;
            float gotoInfront = Config.GotoRetrieve.GotoInfrontOffset;
            ushort marioYaw = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);

            double xOffset, zOffset;
            (xOffset, zOffset) = MoreMath.GetComponentsFromVector(-1 * gotoInfront, marioYaw);

            xPos += (float)xOffset;
            yPos += gotoAbove;
            zPos += (float)zOffset;
        }

        private static void HandleRetrieveOffset(ref float xPos, ref float yPos, ref float zPos)
        {
            float retrieveAbove = Config.GotoRetrieve.RetrieveAboveOffset;
            float retrieveInfront = Config.GotoRetrieve.RetrieveInfrontOffset;
            ushort marioYaw = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);

            double xOffset, zOffset;
            (xOffset, zOffset) = MoreMath.GetComponentsFromVector(retrieveInfront, marioYaw);

            xPos += (float)xOffset;
            yPos += retrieveAbove;
            zPos += (float)zOffset;
        }

        public static bool TranslateObjects(List<uint> objAddresses,
            float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<TripleAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<TripleAddressAngle>(
                    objAddress => new TripleAddressAngle(
                        objAddress + Config.ObjectSlots.ObjectXOffset,
                        objAddress + Config.ObjectSlots.ObjectYOffset,
                        objAddress + Config.ObjectSlots.ObjectZOffset,
                        Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset)));

            return ChangeValues(posAddressAngles, xOffset, yOffset, zOffset, Change.ADD, useRelative);
        }

        public static bool TranslateObjectHomes(List<uint> objAddresses,
            float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<TripleAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<TripleAddressAngle>(
                    objAddress => new TripleAddressAngle(
                        objAddress + Config.ObjectSlots.HomeXOffset,
                        objAddress + Config.ObjectSlots.HomeYOffset,
                        objAddress + Config.ObjectSlots.HomeZOffset,
                        Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset)));

            return ChangeValues(posAddressAngles, xOffset, yOffset, zOffset, Change.ADD, useRelative);
        }

        public static bool RotateObjects(List<uint> objAddresses,
            int yawOffset, int pitchOffset, int rollOffset)
        {
            if (objAddresses.Count == 0)
                return false;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var objAddress in objAddresses)
            {
                ushort yawFacing, pitchFacing, rollFacing, yawMoving, pitchMoving, rollMoving;
                yawFacing = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset);
                pitchFacing = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.PitchFacingOffset);
                rollFacing = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.RollFacingOffset);
                yawMoving = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.YawMovingOffset);
                pitchMoving = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.PitchMovingOffset);
                rollMoving = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.RollMovingOffset);

                yawFacing += (ushort)yawOffset;
                pitchFacing += (ushort)pitchOffset;
                rollFacing += (ushort)rollOffset;
                yawMoving += (ushort)yawOffset;
                pitchMoving += (ushort)pitchOffset;
                rollMoving += (ushort)rollOffset;

                success &= Config.Stream.SetValue(yawFacing, objAddress + Config.ObjectSlots.YawFacingOffset);
                success &= Config.Stream.SetValue(pitchFacing, objAddress + Config.ObjectSlots.PitchFacingOffset);
                success &= Config.Stream.SetValue(rollFacing, objAddress + Config.ObjectSlots.RollFacingOffset);
                success &= Config.Stream.SetValue(yawMoving, objAddress + Config.ObjectSlots.YawMovingOffset);
                success &= Config.Stream.SetValue(pitchMoving, objAddress + Config.ObjectSlots.PitchMovingOffset);
                success &= Config.Stream.SetValue(rollMoving, objAddress + Config.ObjectSlots.RollMovingOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool ScaleObjects(List<uint> objAddresses,
            float widthChange, float heightChange, float depthChange, bool multiply)
        {
            List<TripleAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<TripleAddressAngle>(
                    objAddress => new TripleAddressAngle(
                        objAddress + Config.ObjectSlots.ScaleWidthOffset,
                        objAddress + Config.ObjectSlots.ScaleHeightOffset,
                        objAddress + Config.ObjectSlots.ScaleDepthOffset));

            return ChangeValues(posAddressAngles, widthChange, heightChange, depthChange, multiply ? Change.MULTIPLY : Change.ADD);
        }

        public static bool GotoObjectsHome(List<uint> objAddresses, (bool affectX, bool affectY, bool affectZ)? affects = null)
        {
            if (objAddresses.Count == 0)
                return false;

            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Mario.StructAddress + Config.Mario.XOffset,
                        Config.Mario.StructAddress + Config.Mario.YOffset,
                        Config.Mario.StructAddress + Config.Mario.ZOffset)
                };

            float xDestination = objAddresses.Average(obj => Config.Stream.GetSingle(obj + Config.ObjectSlots.HomeXOffset));
            float yDestination = objAddresses.Average(obj => Config.Stream.GetSingle(obj + Config.ObjectSlots.HomeYOffset));
            float zDestination = objAddresses.Average(obj => Config.Stream.GetSingle(obj + Config.ObjectSlots.HomeZOffset));

            HandleGotoOffset(ref xDestination, ref yDestination, ref zDestination);

            return ChangeValues(posAddressAngles, xDestination, yDestination, zDestination, Change.SET, false, affects);
        }

        public static bool RetrieveObjectsHome(List<uint> objAddresses, (bool affectX, bool affectY, bool affectZ)? affects = null)
        {
            List<TripleAddressAngle> posAddressAngles =
                objAddresses.ConvertAll<TripleAddressAngle>(
                    objAddress => new TripleAddressAngle(
                        objAddress + Config.ObjectSlots.HomeXOffset,
                        objAddress + Config.ObjectSlots.HomeYOffset,
                        objAddress + Config.ObjectSlots.HomeZOffset));

            float xDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float yDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float zDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            HandleRetrieveOffset(ref xDestination, ref yDestination, ref zDestination);

            return ChangeValues(posAddressAngles, xDestination, yDestination, zDestination, Change.SET, false, affects);
        }

        public static bool CloneObject(uint objAddress, bool updateAction = true)
        {
            var marioAddress = Config.Mario.StructAddress;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            uint lastObject = Config.Stream.GetUInt32(marioAddress + Config.Mario.HeldObjectPointerOffset);
            
            // Set clone action flags
            if (lastObject == 0x00000000U && updateAction)
            {
                // Set Next action
                uint currentAction = Config.Stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
                uint nextAction = Config.MarioActions.GetAfterCloneValue(currentAction);
                success &= Config.Stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);
            }

            // Set new held value
            success &= Config.Stream.SetValue(objAddress, marioAddress + Config.Mario.HeldObjectPointerOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool UnCloneObject(bool updateAction = true)
        {
            var marioAddress = Config.Mario.StructAddress;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            // Set mario's next action
            if (updateAction)
            {
                uint currentAction = Config.Stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
                uint nextAction = Config.MarioActions.GetAfterUncloneValue(currentAction);
                success &= Config.Stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);
            }

            // Clear mario's held object
            success &= Config.Stream.SetValue(0x00000000U, marioAddress + Config.Mario.HeldObjectPointerOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool UnloadObject(List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var address in addresses)
            {
                var test = Config.Stream.GetUInt16(address + Config.ObjectSlots.ObjectActiveOffset);
                success &= Config.Stream.SetValue((short) 0x0000, address + Config.ObjectSlots.ObjectActiveOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool ReviveObject(List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var address in addresses)
            {
                // Find process group
                uint scriptAddress = Config.Stream.GetUInt32(address + Config.ObjectSlots.BehaviorScriptOffset);
                if (scriptAddress == 0x00000000)
                    continue;
                uint firstScriptAction = Config.Stream.GetUInt32(scriptAddress);
                if ((firstScriptAction & 0xFF000000U) != 0x00000000U)
                    continue;
                byte processGroup = (byte)((firstScriptAction & 0x00FF0000U) >> 16);

                // Read first object in group
                var groupConfig = Config.ObjectGroups;
                uint groupAddress = groupConfig.FirstGroupingAddress + processGroup * groupConfig.ProcessGroupStructSize;

                // Loop through and find last object in group
                uint lastGroupObj = groupAddress;
                while (Config.Stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset) != groupAddress)
                    lastGroupObj = Config.Stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset);

                // Remove object from current group
                uint nextObj = Config.Stream.GetUInt32(address + groupConfig.ProcessNextLinkOffset);
                uint prevObj = Config.Stream.GetUInt32(groupConfig.VactantPointerAddress);
                if (prevObj == address)
                {
                    // Set new vacant pointer
                    success &= Config.Stream.SetValue(nextObj, groupConfig.VactantPointerAddress);
                }
                else
                {
                    for (int i = 0; i < Config.ObjectSlots.MaxSlots; i++)
                    {
                        uint obj = Config.Stream.GetUInt32(prevObj + groupConfig.ProcessNextLinkOffset);
                        if (obj == address)
                            break;
                        prevObj = obj;
                    }
                    success &= Config.Stream.SetValue(nextObj, prevObj + groupConfig.ProcessNextLinkOffset);
                }

                // Insert object in new group
                nextObj = Config.Stream.GetUInt32(lastGroupObj + groupConfig.ProcessNextLinkOffset);
                success &= Config.Stream.SetValue(address, nextObj + groupConfig.ProcessPreviousLinkOffset);
                success &= Config.Stream.SetValue(address, lastGroupObj + groupConfig.ProcessNextLinkOffset);
                success &= Config.Stream.SetValue(lastGroupObj, address + groupConfig.ProcessPreviousLinkOffset);
                success &= Config.Stream.SetValue(nextObj, address + groupConfig.ProcessNextLinkOffset);

                success &= Config.Stream.SetValue((short)0x0101, address + Config.ObjectSlots.ObjectActiveOffset);

                if (addresses.Count > 1)
                    if (!Config.Stream.RefreshRam() || !success)
                        break;
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool ReleaseObject(List<uint> addresses, bool useThrownValue = true)
        {
            if (addresses.Count == 0)
                return false;

            uint releasedValue = useThrownValue ? Config.ObjectSlots.ReleaseStatusThrownValue : Config.ObjectSlots.ReleaseStatusDroppedValue;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var address in addresses)
            {
                success &= Config.Stream.SetValue(releasedValue, address + Config.ObjectSlots.ReleaseStatusOffset);
                success &= Config.Stream.SetValue(Config.ObjectSlots.StackIndexReleasedValue, address + Config.ObjectSlots.StackIndexOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool UnReleaseObject(List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var address in addresses)
            {
                uint initialReleaseStatus = Config.Stream.GetUInt32(address + Config.ObjectSlots.InitialReleaseStatusOffset);
                success &= Config.Stream.SetValue(initialReleaseStatus, address + Config.ObjectSlots.ReleaseStatusOffset);
                success &= Config.Stream.SetValue(Config.ObjectSlots.StackIndexUnReleasedValue, address + Config.ObjectSlots.StackIndexOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool InteractObject(List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var address in addresses)
            {
                success &= Config.Stream.SetValue(0xFFFFFFFF, address + Config.ObjectSlots.InteractionStatusOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool UnInteractObject(List<uint> addresses)
        {
            if (addresses.Count == 0)
                return false;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            foreach (var address in addresses)
            {
                success &= Config.Stream.SetValue(0x00000000, address + Config.ObjectSlots.InteractionStatusOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool ToggleHandsfree()
        {
            var marioAddress = Config.Mario.StructAddress;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            var heldObj = Config.Stream.GetUInt32(marioAddress + Config.Mario.HeldObjectPointerOffset);

            if (heldObj != 0x00000000U)
            {
                uint currentAction = Config.Stream.GetUInt32(marioAddress + Config.Mario.ActionOffset);
                uint nextAction = Config.MarioActions.GetHandsfreeValue(currentAction);
                success = Config.Stream.SetValue(nextAction, marioAddress + Config.Mario.ActionOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool ToggleVisibility()
        {
            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            var marioObjRef = Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
            if (marioObjRef != 0x00000000U)
            {
                var marioGraphics = Config.Stream.GetUInt32(marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                if (marioGraphics == 0)
                { 
                    success &= Config.Stream.SetValue(Config.ObjectSlots.MarioGraphic, marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                }
                else
                {
                    success &= Config.Stream.SetValue(0x00000000U, marioObjRef + Config.ObjectSlots.BehaviorGfxOffset);
                }
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool TranslateMario(float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Mario.StructAddress + Config.Mario.XOffset,
                        Config.Mario.StructAddress + Config.Mario.YOffset,
                        Config.Mario.StructAddress + Config.Mario.ZOffset,
                        Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset))
                };

            return ChangeValues(posAddressAngles, xOffset, yOffset, zOffset, Change.ADD, useRelative);
        }

        public static bool SetMarioPosition(float xValue, float yValue, float zValue)
        {
            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Mario.StructAddress + Config.Mario.XOffset,
                        Config.Mario.StructAddress + Config.Mario.YOffset,
                        Config.Mario.StructAddress + Config.Mario.ZOffset)
                };

            return ChangeValues(posAddressAngles, xValue, yValue, zValue, Change.SET);
        }

        public static bool GotoHOLP((bool affectX, bool affectY, bool affectZ)? affects = null)
        {
            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Mario.StructAddress + Config.Mario.XOffset,
                        Config.Mario.StructAddress + Config.Mario.YOffset,
                        Config.Mario.StructAddress + Config.Mario.ZOffset)
                };

            float xDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPXOffset);
            float yDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPYOffset);
            float zDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HOLPZOffset);

            return ChangeValues(posAddressAngles, xDestination, yDestination, zDestination, Change.SET, false, affects);
        }

        public static bool RetrieveHOLP((bool affectX, bool affectY, bool affectZ)? affects = null)
        {
            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Mario.StructAddress + Config.Mario.HOLPXOffset,
                        Config.Mario.StructAddress + Config.Mario.HOLPYOffset,
                        Config.Mario.StructAddress + Config.Mario.HOLPZOffset)
                };

            float xDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float yDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float zDestination = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            return ChangeValues(posAddressAngles, xDestination, yDestination, zDestination, Change.SET, false, affects);
        }

        public static bool TranslateHOLP(float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Mario.StructAddress + Config.Mario.HOLPXOffset,
                        Config.Mario.StructAddress + Config.Mario.HOLPYOffset,
                        Config.Mario.StructAddress + Config.Mario.HOLPZOffset,
                        Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset))
                };

            return ChangeValues(posAddressAngles, xOffset, yOffset, zOffset, Change.ADD, useRelative);
        }

        public static bool MarioChangeYaw(int yawOffset)
        {
            var marioAddress = Config.Mario.StructAddress;

            ushort yaw = Config.Stream.GetUInt16(marioAddress + Config.Mario.YawFacingOffset);
            yaw += (ushort)yawOffset;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(yaw, marioAddress + Config.Mario.YawFacingOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool MarioChangeHspd(float hspdOffset)
        {
            var marioAddress = Config.Mario.StructAddress;

            float hspd = Config.Stream.GetSingle(marioAddress + Config.Mario.HSpeedOffset);
            hspd += hspdOffset;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(hspd, marioAddress + Config.Mario.HSpeedOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool MarioChangeVspd(float vspdOffset)
        {
            var marioAddress = Config.Mario.StructAddress;

            float vspd = Config.Stream.GetSingle(marioAddress + Config.Mario.VSpeedOffset);
            vspd += vspdOffset;

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(vspd, marioAddress + Config.Mario.VSpeedOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool FullHp()
        {
            return Config.Stream.SetValue(Config.Hud.FullHp, Config.Mario.StructAddress + Config.Hud.HpCountOffset);
        }

        public static bool Die()
        {
            return Config.Stream.SetValue((short)255, Config.Mario.StructAddress + Config.Hud.HpCountOffset);
        }

        public static bool StandardHud()
        {
            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(Config.Hud.FullHp, Config.Mario.StructAddress + Config.Hud.HpCountOffset);
            success &= Config.Stream.SetValue(Config.Hud.StandardCoins, Config.Mario.StructAddress + Config.Hud.CoinCountOffset);
            success &= Config.Stream.SetValue(Config.Hud.StandardLives, Config.Mario.StructAddress + Config.Hud.LifeCountOffset);
            success &= Config.Stream.SetValue(Config.Hud.StandardStars, Config.Mario.StructAddress + Config.Hud.StarCountOffset);

            success &= Config.Stream.SetValue(Config.Hud.FullHpInt, Config.Mario.StructAddress + Config.Hud.HpDisplayOffset);
            success &= Config.Stream.SetValue(Config.Hud.StandardCoins, Config.Mario.StructAddress + Config.Hud.CoinDisplayOffset);
            success &= Config.Stream.SetValue((short)Config.Hud.StandardLives, Config.Mario.StructAddress + Config.Hud.LifeDisplayOffset);
            success &= Config.Stream.SetValue(Config.Hud.StandardStars, Config.Mario.StructAddress + Config.Hud.StarDisplayOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool Coins99()
        {
            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue((short)99, Config.Mario.StructAddress + Config.Hud.CoinCountOffset);
            success &= Config.Stream.SetValue((short)99, Config.Mario.StructAddress + Config.Hud.CoinDisplayOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool Lives100()
        {
            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue((sbyte)100, Config.Mario.StructAddress + Config.Hud.LifeCountOffset);
            success &= Config.Stream.SetValue((short)100, Config.Mario.StructAddress + Config.Hud.LifeDisplayOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool GotoTriangle(uint triangleAddress, int vertex, bool _useMisalignmentOffset = false)
        {
            if (triangleAddress == 0x0000)
                return false;

            float newX, newY, newZ;
            switch(vertex)
            {
                case 1:
                    newX = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X1);
                    newY = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1);
                    newZ = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z1);
                    break;

                case 2:
                    newX = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X2);
                    newY = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2);
                    newZ = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z2);
                    break;

                case 3:
                    newX = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X3);
                    newY = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3);
                    newZ = Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z3);
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
            return PuUtilities.MoveToInCurrentPu(newX, newY, newZ);
        }

        public static bool RetrieveTriangle(uint triangleAddress)
        {
            if (triangleAddress == 0x0000)
                return false;

            float normX, normY, normZ, oldNormOffset;
            normX = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormX);
            normY = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormY);
            normZ = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormZ);
            oldNormOffset = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormOffset);

            // Get Mario position
            float marioX, marioY, marioZ;
            var marioAddress = Config.Mario.StructAddress;
            marioX = Config.Stream.GetSingle(marioAddress + Config.Mario.XOffset);
            marioY = Config.Stream.GetSingle(marioAddress + Config.Mario.YOffset);
            marioZ = Config.Stream.GetSingle(marioAddress + Config.Mario.ZOffset);

            float normOffset = -(normX * marioX + normY * marioY + normZ * marioZ);
            float normDiff = normOffset - oldNormOffset;

            short yOffset = (short)(-normDiff * normY);

            short v1Y, v2Y, v3Y;
            v1Y = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1) + yOffset);
            v2Y = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2) + yOffset);
            v3Y = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3) + yOffset);

            short yMin = (short)(Math.Min(Math.Min(v1Y, v2Y), v3Y) - 5);
            short yMax = (short)(Math.Max(Math.Max(v1Y, v2Y), v3Y) + 5);

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(v1Y, triangleAddress + Config.TriangleOffsets.Y1);
            success &= Config.Stream.SetValue(v2Y, triangleAddress + Config.TriangleOffsets.Y2);
            success &= Config.Stream.SetValue(v3Y, triangleAddress + Config.TriangleOffsets.Y3);
            success &= Config.Stream.SetValue(yMin, triangleAddress + Config.TriangleOffsets.YMin);
            success &= Config.Stream.SetValue(yMax, triangleAddress + Config.TriangleOffsets.YMax);
            success &= Config.Stream.SetValue(normOffset, triangleAddress + Config.TriangleOffsets.NormOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool NeutralizeTriangle(uint triangleAddress, bool use21 = true)
        {
            if (triangleAddress == 0x0000)
                return false;

            short neutralizedSurfaceType = (short)(use21 ? 21 : 0);

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(neutralizedSurfaceType, triangleAddress + Config.TriangleOffsets.SurfaceType);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool AnnihilateTriangle(uint triangleAddress)
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
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(v1X, triangleAddress + Config.TriangleOffsets.X1);
            success &= Config.Stream.SetValue(v1Y, triangleAddress + Config.TriangleOffsets.Y1);
            success &= Config.Stream.SetValue(v1Z, triangleAddress + Config.TriangleOffsets.Z1);
            success &= Config.Stream.SetValue(v2X, triangleAddress + Config.TriangleOffsets.X2);
            success &= Config.Stream.SetValue(v2Y, triangleAddress + Config.TriangleOffsets.Y2);
            success &= Config.Stream.SetValue(v2Z, triangleAddress + Config.TriangleOffsets.Z2);
            success &= Config.Stream.SetValue(v3X, triangleAddress + Config.TriangleOffsets.X3);
            success &= Config.Stream.SetValue(v3Y, triangleAddress + Config.TriangleOffsets.Y3);
            success &= Config.Stream.SetValue(v3Z, triangleAddress + Config.TriangleOffsets.Z3);
            success &= Config.Stream.SetValue(normX, triangleAddress + Config.TriangleOffsets.NormX);
            success &= Config.Stream.SetValue(normY, triangleAddress + Config.TriangleOffsets.NormY);
            success &= Config.Stream.SetValue(normZ, triangleAddress + Config.TriangleOffsets.NormZ);
            success &= Config.Stream.SetValue(normOffset, triangleAddress + Config.TriangleOffsets.NormOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool MoveTriangle(uint triangleAddress,
            float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            if (triangleAddress == 0x0000)
                return false;

            HandleScaling(ref xOffset, ref zOffset);

            float normX, normY, normZ, oldNormOffset;
            normX = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormX);
            normY = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormY);
            normZ = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormZ);
            oldNormOffset = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormOffset);

            ushort relativeAngle = MoreMath.getUphillAngle(normX, normY, normZ);
            HandleRelativeAngle(ref xOffset, ref zOffset, useRelative, relativeAngle);

            float newNormOffset = oldNormOffset - normX * xOffset - normY * yOffset - normZ * zOffset;

            short newX1, newY1, newZ1, newX2, newY2, newZ2, newX3, newY3, newZ3;
            newX1 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X1) + xOffset);
            newY1 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1) + yOffset);
            newZ1 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z1) + zOffset);
            newX2 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X2) + xOffset);
            newY2 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2) + yOffset);
            newZ2 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z2) + zOffset);
            newX3 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X3) + xOffset);
            newY3 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3) + yOffset);
            newZ3 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z3) + zOffset);

            short newYMin = (short)(Math.Min(Math.Min(newY1, newY2), newY3) - 5);
            short newYMax = (short)(Math.Max(Math.Max(newY1, newY2), newY3) + 5);

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(newNormOffset, triangleAddress + Config.TriangleOffsets.NormOffset);
            success &= Config.Stream.SetValue(newX1, triangleAddress + Config.TriangleOffsets.X1);
            success &= Config.Stream.SetValue(newY1, triangleAddress + Config.TriangleOffsets.Y1);
            success &= Config.Stream.SetValue(newZ1, triangleAddress + Config.TriangleOffsets.Z1);
            success &= Config.Stream.SetValue(newX2, triangleAddress + Config.TriangleOffsets.X2);
            success &= Config.Stream.SetValue(newY2, triangleAddress + Config.TriangleOffsets.Y2);
            success &= Config.Stream.SetValue(newZ2, triangleAddress + Config.TriangleOffsets.Z2);
            success &= Config.Stream.SetValue(newX3, triangleAddress + Config.TriangleOffsets.X3);
            success &= Config.Stream.SetValue(newY3, triangleAddress + Config.TriangleOffsets.Y3);
            success &= Config.Stream.SetValue(newZ3, triangleAddress + Config.TriangleOffsets.Z3);
            success &= Config.Stream.SetValue(newYMin, triangleAddress + Config.TriangleOffsets.YMin);
            success &= Config.Stream.SetValue(newYMax, triangleAddress + Config.TriangleOffsets.YMax);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool MoveTriangleNormal(uint triangleAddress, float normalChange)
        {
            if (triangleAddress == 0x0000)
                return false;

            float normX, normY, normZ, oldNormOffset;
            normX = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormX);
            normY = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormY);
            normZ = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormZ);
            oldNormOffset = Config.Stream.GetSingle(triangleAddress + Config.TriangleOffsets.NormOffset);

            float newNormOffset = oldNormOffset - normalChange;

            double xChange = normalChange * normX;
            double yChange = normalChange * normY;
            double zChange = normalChange * normZ;

            short newX1, newY1, newZ1, newX2, newY2, newZ2, newX3, newY3, newZ3;
            newX1 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X1) + xChange);
            newY1 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y1) + yChange);
            newZ1 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z1) + zChange);
            newX2 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X2) + xChange);
            newY2 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y2) + yChange);
            newZ2 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z2) + zChange);
            newX3 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.X3) + xChange);
            newY3 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Y3) + yChange);
            newZ3 = (short)(Config.Stream.GetInt16(triangleAddress + Config.TriangleOffsets.Z3) + zChange);

            short newYMin = (short)(Math.Min(Math.Min(newY1, newY2), newY3) - 5);
            short newYMax = (short)(Math.Max(Math.Max(newY1, newY2), newY3) + 5);

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(newNormOffset, triangleAddress + Config.TriangleOffsets.NormOffset);
            success &= Config.Stream.SetValue(newX1, triangleAddress + Config.TriangleOffsets.X1);
            success &= Config.Stream.SetValue(newY1, triangleAddress + Config.TriangleOffsets.Y1);
            success &= Config.Stream.SetValue(newZ1, triangleAddress + Config.TriangleOffsets.Z1);
            success &= Config.Stream.SetValue(newX2, triangleAddress + Config.TriangleOffsets.X2);
            success &= Config.Stream.SetValue(newY2, triangleAddress + Config.TriangleOffsets.Y2);
            success &= Config.Stream.SetValue(newZ2, triangleAddress + Config.TriangleOffsets.Z2);
            success &= Config.Stream.SetValue(newX3, triangleAddress + Config.TriangleOffsets.X3);
            success &= Config.Stream.SetValue(newY3, triangleAddress + Config.TriangleOffsets.Y3);
            success &= Config.Stream.SetValue(newZ3, triangleAddress + Config.TriangleOffsets.Z3);
            success &= Config.Stream.SetValue(newYMin, triangleAddress + Config.TriangleOffsets.YMin);
            success &= Config.Stream.SetValue(newYMax, triangleAddress + Config.TriangleOffsets.YMax);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool TranslateCamera(float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            List<TripleAddressAngle> posAddressAngles =
                new List<TripleAddressAngle> {
                    new TripleAddressAngle(
                        Config.Camera.CameraStructAddress + Config.Camera.XOffset,
                        Config.Camera.CameraStructAddress + Config.Camera.YOffset,
                        Config.Camera.CameraStructAddress + Config.Camera.ZOffset,
                        Config.Stream.GetUInt16(Config.Camera.CameraStructAddress + Config.Camera.YawFacingOffset))
                };

            return ChangeValues(posAddressAngles, xOffset, yOffset, zOffset, Change.ADD, useRelative);
        }

        public static bool TranslateCameraSpherically(float radiusOffset, float thetaOffset, float phiOffset, (float, float, float) pivotPoint)
        {
            float pivotX, pivotY, pivotZ;
            (pivotX, pivotY, pivotZ) = pivotPoint;

            HandleScaling(ref thetaOffset, ref phiOffset);

            float oldX, oldY, oldZ;
            oldX = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.XOffset);
            oldY = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.YOffset);
            oldZ = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.ZOffset);

            double newX, newY, newZ;
            (newX, newY, newZ) = MoreMath.OffsetSphericallyAboutPivot(oldX, oldY, oldZ, radiusOffset, thetaOffset, phiOffset, pivotX, pivotY, pivotZ);

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue((float)newX, Config.Camera.CameraStructAddress + Config.Camera.XOffset);
            success &= Config.Stream.SetValue((float)newY, Config.Camera.CameraStructAddress + Config.Camera.YOffset);
            success &= Config.Stream.SetValue((float)newZ, Config.Camera.CameraStructAddress + Config.Camera.ZOffset);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        private static ushort getCamHackYawFacing(CamHackMode camHackMode)
        {
            switch (camHackMode)
            {
                case CamHackMode.REGULAR:
                    return Config.Stream.GetUInt16(Config.Camera.CameraStructAddress + Config.Camera.YawFacingOffset);

                case CamHackMode.RELATIVE_ANGLE:
                case CamHackMode.ABSOLUTE_ANGLE:
                case CamHackMode.FIXED_POS:
                case CamHackMode.FIXED_ORIENTATION:
                    float camHackPosX = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraXOffset);
                    float camHackPosZ = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraZOffset);
                    float camHackFocusX = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusXOffset);
                    float camHackFocusZ = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusZOffset);
                    return MoreMath.AngleTo_AngleUnitsRounded(camHackPosX, camHackPosZ, camHackFocusX, camHackFocusZ);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static TripleAddressAngle getCamHackFocusTripleAddressController(CamHackMode camHackMode)
        {
            uint camHackObject = Config.Stream.GetUInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.ObjectOffset);
            switch (camHackMode)
            {
                case CamHackMode.REGULAR:
                    return new TripleAddressAngle(
                        Config.Camera.CameraStructAddress + Config.Camera.FocusXOffset,
                        Config.Camera.CameraStructAddress + Config.Camera.FocusYOffset,
                        Config.Camera.CameraStructAddress + Config.Camera.FocusZOffset,
                        getCamHackYawFacing(camHackMode));
                
                case CamHackMode.RELATIVE_ANGLE:
                case CamHackMode.ABSOLUTE_ANGLE:
                case CamHackMode.FIXED_POS:
                    if (camHackObject == 0) // focused on Mario
                    {
                        return new TripleAddressAngle(
                            Config.Mario.StructAddress + Config.Mario.XOffset,
                            Config.Mario.StructAddress + Config.Mario.YOffset,
                            Config.Mario.StructAddress + Config.Mario.ZOffset,
                            getCamHackYawFacing(camHackMode));
                    }
                    else // focused on object
                    {
                        return new TripleAddressAngle(
                            camHackObject + Config.ObjectSlots.ObjectXOffset,
                            camHackObject + Config.ObjectSlots.ObjectYOffset,
                            camHackObject + Config.ObjectSlots.ObjectZOffset,
                            getCamHackYawFacing(camHackMode));
                    }
                
                case CamHackMode.FIXED_ORIENTATION:
                    return new TripleAddressAngle(
                        Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusXOffset,
                        Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusYOffset,
                        Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusZOffset,
                        getCamHackYawFacing(camHackMode));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool TranslateCameraHack(CamHackMode camHackMode, float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            switch (camHackMode)
            {
                case CamHackMode.REGULAR:
                {
                    return TranslateCamera(xOffset, yOffset, zOffset, useRelative);
                }

                case CamHackMode.FIXED_POS:
                case CamHackMode.FIXED_ORIENTATION:
                {
                    return ChangeValues(
                        new List<TripleAddressAngle> {
                            new TripleAddressAngle(
                                Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraXOffset,
                                Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraYOffset,
                                Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraZOffset,
                                getCamHackYawFacing(camHackMode))
                        },
                        xOffset,
                        yOffset,
                        zOffset,
                        Change.ADD,
                        useRelative);
                }

                case CamHackMode.RELATIVE_ANGLE:
                case CamHackMode.ABSOLUTE_ANGLE:
                {
                    HandleScaling(ref xOffset, ref zOffset);

                    HandleRelativeAngle(ref xOffset, ref zOffset, useRelative, getCamHackYawFacing(camHackMode));
                    float xDestination = xOffset + Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraXOffset);
                    float yDestination = yOffset + Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraYOffset);
                    float zDestination = zOffset + Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraZOffset);

                    float xFocus = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusXOffset);
                    float yFocus = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusYOffset);
                    float zFocus = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusZOffset);

                    double radius, theta, height;
                    (radius, theta, height) = MoreMath.EulerToCylindricalAboutPivot(xDestination, yDestination, zDestination, xFocus, yFocus, zFocus);

                    ushort relativeYawOffset = 0;
                    if (camHackMode == CamHackMode.RELATIVE_ANGLE)
                    {
                        uint camHackObject = Config.Stream.GetUInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.ObjectOffset);
                        relativeYawOffset = camHackObject == 0
                            ? Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset)
                            : Config.Stream.GetUInt16(camHackObject + Config.ObjectSlots.YawFacingOffset);
                    }

                    bool success = true;
                    bool streamAlreadySuspended = Config.Stream.IsSuspended;
                    if (!streamAlreadySuspended) Config.Stream.Suspend();

                    success &= Config.Stream.SetValue((float)radius, Config.CameraHack.CameraHackStruct + Config.CameraHack.RadiusOffset);
                    success &= Config.Stream.SetValue(MoreMath.NormalizeAngleUshort(theta + 32768 - relativeYawOffset), Config.CameraHack.CameraHackStruct + Config.CameraHack.ThetaOffset);
                    success &= Config.Stream.SetValue((float)height, Config.CameraHack.CameraHackStruct + Config.CameraHack.RelativeHeightOffset);

                    if (!streamAlreadySuspended) Config.Stream.Resume();
                    return success;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool TranslateCameraHackSpherically(CamHackMode camHackMode, float radiusOffset, float thetaOffset, float phiOffset)
        {
            switch (camHackMode)
            {
                case CamHackMode.REGULAR:
                {
                    float xFocus = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.FocusXOffset);
                    float yFocus = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.FocusYOffset);
                    float zFocus = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.FocusZOffset);
                    return TranslateCameraSpherically(radiusOffset, thetaOffset, phiOffset, (xFocus, yFocus, zFocus));
                }

                case CamHackMode.FIXED_POS:
                case CamHackMode.FIXED_ORIENTATION:
                {
                    HandleScaling(ref thetaOffset, ref phiOffset);

                    TripleAddressAngle focusTripleAddressAngle = getCamHackFocusTripleAddressController(camHackMode);
                    uint focusXAddress, focusYAddress, focusZAddress;
                    (focusXAddress, focusYAddress, focusZAddress) = focusTripleAddressAngle.GetTripleAddress();

                    float xFocus = Config.Stream.GetSingle(focusTripleAddressAngle.XAddress);
                    float yFocus = Config.Stream.GetSingle(focusTripleAddressAngle.YAddress);
                    float zFocus = Config.Stream.GetSingle(focusTripleAddressAngle.ZAddress);

                    float xCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraXOffset);
                    float yCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraYOffset);
                    float zCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraZOffset);

                    double xDestination, yDestination, zDestination;
                    (xDestination, yDestination, zDestination) =
                        MoreMath.OffsetSphericallyAboutPivot(xCamPos, yCamPos, zCamPos, radiusOffset, thetaOffset, phiOffset, xFocus, yFocus, zFocus);

                    return ChangeValues(
                        new List<TripleAddressAngle> {
                            new TripleAddressAngle(
                                Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraXOffset,
                                Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraYOffset,
                                Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraZOffset)
                        },
                        (float)xDestination,
                        (float)yDestination,
                        (float)zDestination,
                        Change.SET);
                }

                case CamHackMode.RELATIVE_ANGLE:
                case CamHackMode.ABSOLUTE_ANGLE:
                {
                    HandleScaling(ref thetaOffset, ref phiOffset);

                    float xCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraXOffset);
                    float yCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraYOffset);
                    float zCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraZOffset);

                    float xFocus = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusXOffset);
                    float yFocus = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusYOffset);
                    float zFocus = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.FocusZOffset);

                    double xDestination, yDestination, zDestination;
                    (xDestination, yDestination, zDestination) =
                        MoreMath.OffsetSphericallyAboutPivot(xCamPos, yCamPos, zCamPos, radiusOffset, thetaOffset, phiOffset, xFocus, yFocus, zFocus);

                    double radius, theta, height;
                    (radius, theta, height) = MoreMath.EulerToCylindricalAboutPivot(xDestination, yDestination, zDestination, xFocus, yFocus, zFocus);

                    ushort relativeYawOffset = 0;
                    if (camHackMode == CamHackMode.RELATIVE_ANGLE)
                    {
                        uint camHackObject = Config.Stream.GetUInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.ObjectOffset);
                        relativeYawOffset = camHackObject == 0
                            ? Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset)
                            : Config.Stream.GetUInt16(camHackObject + Config.ObjectSlots.YawFacingOffset);
                    }

                    bool success = true;
                    bool streamAlreadySuspended = Config.Stream.IsSuspended;
                    if (!streamAlreadySuspended) Config.Stream.Suspend();

                    success &= Config.Stream.SetValue((float)radius, Config.CameraHack.CameraHackStruct + Config.CameraHack.RadiusOffset);
                    success &= Config.Stream.SetValue(MoreMath.NormalizeAngleUshort(theta + 32768 - relativeYawOffset), Config.CameraHack.CameraHackStruct + Config.CameraHack.ThetaOffset);
                    success &= Config.Stream.SetValue((float)height, Config.CameraHack.CameraHackStruct + Config.CameraHack.RelativeHeightOffset);

                    if (!streamAlreadySuspended) Config.Stream.Resume();
                    return success;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool TranslateCameraHackFocus(CamHackMode camHackMode, float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            return ChangeValues(
                new List<TripleAddressAngle> { getCamHackFocusTripleAddressController(camHackMode) },
                xOffset,
                yOffset,
                zOffset,
                Change.ADD,
                useRelative);
        }

        public static bool TranslateCameraHackFocusSpherically(CamHackMode camHackMode, float radiusOffset, float thetaOffset, float phiOffset)
        {
            HandleScaling(ref thetaOffset, ref phiOffset);

            TripleAddressAngle focusTripleAddressAngle = getCamHackFocusTripleAddressController(camHackMode);
            uint focusXAddress, focusYAddress, focusZAddress;
            (focusXAddress, focusYAddress, focusZAddress) = focusTripleAddressAngle.GetTripleAddress();

            float xFocus = Config.Stream.GetSingle(focusTripleAddressAngle.XAddress);
            float yFocus = Config.Stream.GetSingle(focusTripleAddressAngle.YAddress);
            float zFocus = Config.Stream.GetSingle(focusTripleAddressAngle.ZAddress);

            float xCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraXOffset);
            float yCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraYOffset);
            float zCamPos = Config.Stream.GetSingle(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraZOffset);

            double xDestination, yDestination, zDestination;
            (xDestination, yDestination, zDestination) =
                MoreMath.OffsetSphericallyAboutPivot(xFocus, yFocus, zFocus, radiusOffset, thetaOffset, phiOffset, xCamPos, yCamPos, zCamPos);

            return ChangeValues(
                new List<TripleAddressAngle> { focusTripleAddressAngle },
                (float)xDestination,
                (float)yDestination,
                (float)zDestination,
                Change.SET);
        }

        public static bool TranslateCameraHackBoth(CamHackMode camHackMode, float xOffset, float yOffset, float zOffset, bool useRelative)
        {
            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            if (camHackMode != CamHackMode.RELATIVE_ANGLE && camHackMode != CamHackMode.ABSOLUTE_ANGLE)
            {
                success &= TranslateCameraHack(camHackMode, xOffset, yOffset, zOffset, useRelative);
            }
            success &= TranslateCameraHackFocus(camHackMode, xOffset, yOffset, zOffset, useRelative);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        public static bool SetHudVisibility(bool hudOn)
        {
            byte currentHudVisibility = Config.Stream.GetByte(Config.Mario.StructAddress + Config.Hud.VisibilityOffset);
            byte newHudVisibility = MoreMath.ApplyValueToMaskedByte(currentHudVisibility, Config.Hud.VisibilityMask, hudOn);

            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            success &= Config.Stream.SetValue(newHudVisibility, Config.Mario.StructAddress + Config.Hud.VisibilityOffset);
            success &= Config.Stream.SetValue((short)(hudOn ? 1 : 0), Config.LevelIndexAddress);

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }
    }
}
