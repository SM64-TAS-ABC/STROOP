using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class ObjectSlotsConfig
    {
        public static ObjectSlot HoverObjectSlot;

        public static readonly int MaxSlots = 240;

        public static uint LinkStartAddress { get { return Config.SwitchRomVersion(LinkStartAddressUS, LinkStartAddressJP); } }
        public static readonly uint LinkStartAddressUS = 0x8033D488;
        public static readonly uint LinkStartAddressJP = 0x8033C118;

        public static readonly uint StructSize = 0x0260;
        public static readonly uint HeaderOffset = 0x00;
        public static readonly uint NextLinkOffset = 0x08;
        public static readonly uint PreviousLinkOffset = 0x04;
        public static readonly uint ParentOffset = 0x68;

        public static uint UnusedSlotAddress { get { return Config.SwitchRomVersion(UnusedSlotAddressUS, UnusedSlotAddressJP); } }
        public static readonly uint UnusedSlotAddressUS = 0x80360E88;
        public static readonly uint UnusedSlotAddressJP = 0x8035FB18;

        public static readonly uint BehaviorScriptOffset = 0x020C;
        public static readonly uint BehaviorGfxOffset = 0x14;
        public static readonly uint BehaviorSubtypeOffset = 0x0144;
        public static readonly uint BehaviorAppearance = 0xF0;
        public static readonly uint ModelPointerOffset = 0x218;
        public static readonly uint ObjectActiveOffset = 0x74;
        public static readonly uint TimerOffset = 0x154;
        public static readonly uint AnimationOffset = 0x3C;

        public static readonly uint DustSpawnerBehavior = 0x130024AC;
        public static readonly uint DustBallBehavior = 0x130024DC;
        public static readonly uint DustBehavior = 0x13002500;

        public static readonly uint ObjectXOffset = 0xA0;
        public static readonly uint ObjectYOffset = 0xA4;
        public static readonly uint ObjectZOffset = 0xA8;

        public static readonly uint YSpeedOffset = 0xB0;
        public static readonly uint HSpeedOffset = 0xB8;

        public static readonly uint HomeXOffset = 0x164;
        public static readonly uint HomeYOffset = 0x168;
        public static readonly uint HomeZOffset = 0x16C;

        public static readonly uint GraphicsXOffset = 0x20;
        public static readonly uint GraphicsYOffset = 0x24;
        public static readonly uint GraphicsZOffset = 0x28;
        public static readonly uint GraphicsYawOffset = 0x1C;
        public static readonly uint GraphicsPitchOffset = 0x1A;
        public static readonly uint GraphicsRollOffset = 0x1E;

        public static readonly uint HitboxRadius = 0x1F8;
        public static readonly uint HitboxHeight = 0x1FC;
        public static readonly uint HitboxDownOffset = 0x208;

        public static readonly uint YawFacingOffset = 0xD6;
        public static readonly uint PitchFacingOffset = 0xD2;
        public static readonly uint RollFacingOffset = 0xDA;
        public static readonly uint YawMovingOffset = 0xCA;
        public static readonly uint PitchMovingOffset = 0xC6;
        public static readonly uint RollMovingOffset = 0xCE;

        public static readonly uint ScaleWidthOffset = 0x2C;
        public static readonly uint ScaleHeightOffset = 0x30;
        public static readonly uint ScaleDepthOffset = 0x34;

        public static readonly uint ReleaseStatusOffset = 0x1CC;
        public static readonly uint StackIndexOffset = 0x1D0;
        public static readonly uint StackIndexReleasedValue = 0;
        public static readonly uint StackIndexUnReleasedValue = 1;
        public static readonly uint InitialReleaseStatusOffset = 0x1D4;
        public static readonly uint ReleaseStatusThrownValue = 0x800EE5F8;
        public static readonly uint ReleaseStatusDroppedValue = 0x800EE5F0;
        public static readonly uint InteractionStatusOffset = 0x134;

        public static readonly uint PendulumAccelerationDirection = 0xF4;
        public static readonly uint PendulumAccelerationMagnitude = 0x100;
        public static readonly uint PendulumAngularVelocity = 0xFC;
        public static readonly uint PendulumAngle = 0xF8;

        public static readonly uint WaypointOffset = 0x100;
        public static readonly uint PitchToWaypointOffset = 0x10A;
        public static readonly uint RacingPenguinEffortOffset = 0x110;
        public static readonly uint KoopaTheQuickHSpeedMultiplierOffset = 0xF4;

        public static readonly uint FlyGuyOscillationTimerOffset = 0xF8;

        public static readonly uint ScuttlebugTargetAngleOffset = 0x162;
    }
}
