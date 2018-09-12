using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;

namespace STROOP.Utilities
{
    public static class KeyboardUtilities
    {
        public static int? GetCurrentlyInputtedNumber()
        {
            if (Keyboard.IsKeyDown(Key.D1)) return 1;
            if (Keyboard.IsKeyDown(Key.D2)) return 2;
            if (Keyboard.IsKeyDown(Key.D3)) return 3;
            if (Keyboard.IsKeyDown(Key.D4)) return 4;
            if (Keyboard.IsKeyDown(Key.D5)) return 5;
            if (Keyboard.IsKeyDown(Key.D6)) return 6;
            if (Keyboard.IsKeyDown(Key.D7)) return 7;
            if (Keyboard.IsKeyDown(Key.D8)) return 8;
            if (Keyboard.IsKeyDown(Key.D9)) return 9;
            if (Keyboard.IsKeyDown(Key.D0)) return 0;
            return null;
        }

        public static bool IsNumberHeld()
        {
            return GetCurrentlyInputtedNumber() != null;
        }

        public static bool IsCtrlHeld()
        {
            return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        }

        public static bool IsShiftHeld()
        {
            return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }

        public static bool IsAltHeld()
        {
            return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
        }

        public static bool IsDeletishKeyHeld()
        {
            return Keyboard.IsKeyDown(Key.Delete) ||
                Keyboard.IsKeyDown(Key.Back) ||
                Keyboard.IsKeyDown(Key.Escape);
        }
    }
}
