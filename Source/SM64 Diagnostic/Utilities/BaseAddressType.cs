using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    // TODO add new offset types
    public enum BaseAddressType
    {
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
        Special,
    };
}
