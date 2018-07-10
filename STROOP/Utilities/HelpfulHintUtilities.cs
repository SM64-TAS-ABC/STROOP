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
    public static class HelpfulHintUtilities
    {
        private static readonly List<string> helpfulHints =
            new List<string>()
            {
                // Right clicking
                "Right click on object slots to access object functions from any tab, e.g. \"Goto\", \"Clone\", \"Unload\".",
                "Right click on the Goto/Retrieve buttons to get more options, such as \"Goto Laterally\" or \"Goto Y\".",
                "Right click on variables for more options, such as highlighting, locking, and how many decimal places to round to.",
                "Right click on a variable and choose Panel Options to get options for all variables in the list. From there, you can filter which variables are shown.",
                "Right click on an angle variable to change its units. You can choose In-Game Angle Units, HAU (Hexadecimal Angle Units), Degrees, Radians, or Revolutions.",
                "Right click on a variable that's part of an x,y,z coordinate triple for options to copy/paste 3 coordinate values at once. Especially helpful when inputting data " +
                    "into a spreadsheet, or applying data from a spreadsheet.",
                "When variables are locked, you can right click on any variable and choose \"Remove All Locks\" to remove all locks, including those not currently visible.",

                // Left clicking while holding a key
                "Click on a variable while holding Z to zero that variable.",
                "Click on a variable while holding Escape to delete that variable.",
                "Click on a variable while holding -/+ to decrement/increment that variable.",
                "Click on a variable while holding Ctrl to add that variable to the Custom tab.",
                "Click on a variable while holding Alt to enable custom functionality for that variable, i.e. fixing its address, renaming, deleting.",
                "Click on a variable while holding Backtick to add that variable to the Var Hack tab, so that it can be displayed on screen.",
                "Click on a variable while holding H to highlight that variable.",
                "Click on a variable while holding L to lock that variable.",
                "Click on a variable while holding F to fix that variable's address. For example, fixing an object variable's address will cause that variable to keep referring to the same object " +
                    " instead of the currently selected object.",
                "Click on a variable while holding Shift to inform STROOP that you want to move that variable. Then click on another variable while holding shift to move the first variable there.",
                "Click on an object slot while holding Alt to mark that slot. Then it will have a black border to help distinguish it.",
                "Click on an object slot while holding Ctrl to toggle whether that slot is selected. You can use this to select multiple slots at once.",
                "Click on an object slot while holding Shift to select all slots from the one currently selected to the one you clicked on.",

                // Tabs
                "In the Object tab, you can remove an object's shadow by setting its Shadow Opacity to 0.",
                "In the Object tab, you can toggle whether an object is visible via the checkbox in the Visible variable.",
                "In the Mario tab, you can toggle whether Mario is visible or not via the \"Toggle Visibility\" button. This doesn't affect his movement, and can be useful for getting certain " +
                    "screenshots.",
                "In the File tab, use the \"Everything\" button to unlock all stars/doors/caps/cannons/coins. Use this to quickly gain access to everything when using a new ROM hack or game version.",
                "In the Map tab, clicking on an object slot will show that object on the map.",
                "In the Triangles tab, you can neutralize all triangles via the \"Neutralize All Triangles\" button. This can be useful if you don't want lava or the death barrier to affect you.",
                "In the Triangles tab, you can get the coordinates for all level triangles via the \"Get Level Tris\" button.",
                "In the Water tab, you can change the water level for any water in the game. You can also change the default water level, which is usually -11000.",
                "In the Options tab, you can change what language STROOP is using. Choose between US and JP.",
                "In the Options tab, you can turn off which object overlays are shown, such as the Held Object or Interaction Object.",
                "In the Options tab, you can how the \"Goto\" and \"Retrieve\" buttons work. For example, you can set whether you want to go to objects 300 units above or exactly at them.",
                "In the Options tab, you can set a specific angle for relative position controllers. For example, you could set the Mario position controller's relative settings to consider " +
                    "angle 1000 as forwards.",
                "In the custom tab, you can show whatever variables you want and record their values as well. You can add variables to the Custom tab either by right clicking and choose " +
                    "\"Add to Custom Tab\" or clicking on a variable while holding Ctrl.",
                "In the custom tab, you can change the size of variables, which applies to all tabs. This is helpful if you want to show several variables on screen while screen recording.",
                "In the custom tab, you can save your current list of variables as a file to your computer. Then later you can open that file to show those same variables.",

            };

        public static string GetRandomHelpfulHint()
        {
            Random random = new Random();
            int randomIndex = (int)(helpfulHints.Count * random.NextDouble());
            return helpfulHints[randomIndex];
        }
    }
}
