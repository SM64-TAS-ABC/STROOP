using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    // TODO add new offset types
    public enum BaseAddressTypeEnum
    {
        None,

        Absolute,
        Relative,

        Mario,
        MarioObj,
        Camera,
        File,
        Object,
        Triangle,
        TriangleExertionForceTable,
        InputCurrent,
        InputJustPressed,
        InputBuffered,
        Graphics,
        Animation,
        Waypoint,
        Water,
        Area,

        HackedArea,
        CamHack,
        GfxNode,
        GhostHack,
    };
}
