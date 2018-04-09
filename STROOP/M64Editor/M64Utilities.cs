using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using STROOP.Structs;
using System.ComponentModel;
using STROOP.Utilities;
using System.Windows.Forms;
using STROOP.Forms;

namespace STROOP.M64Editor
{
    public static class M64Utilities
    {

        public static Dictionary<string, int> InputStringToIndex
            = new Dictionary<string, int>()
            {
                ["X"] = 0,
                ["Y"] = 1,
                ["A"] = 2,
                ["B"] = 3,
                ["Z"] = 4,
                ["S"] = 5,
                ["R"] = 6,
                ["C^"] = 7,
                ["Cv"] = 8,
                ["C<"] = 9,
                ["C>"] = 10,
                ["L"] = 11,
                ["D^"] = 12,
                ["Dv"] = 13,
                ["D<"] = 14,
                ["D>"] = 15,
            };

        public static void ClearSpecificInput(M64InputFrame inputFrame, string headerText)
        {
            switch (headerText)
            {
                case "X":
                    inputFrame.X = 0;
                    break;
                case "Y":
                    inputFrame.Y = 0;
                    break;
                case "A":
                    inputFrame.A = false;
                    break;
                case "B":
                    inputFrame.B = false;
                    break;
                case "Z":
                    inputFrame.Z = false;
                    break;
                case "S":
                    inputFrame.Start = false;
                    break;
                case "R":
                    inputFrame.R = false;
                    break;
                case "C^":
                    inputFrame.C_Up = false;
                    break;
                case "Cv":
                    inputFrame.C_Down = false;
                    break;
                case "C<":
                    inputFrame.C_Left = false;
                    break;
                case "C>":
                    inputFrame.C_Right = false;
                    break;
                case "L":
                    inputFrame.L = false;
                    break;
                case "D^":
                    inputFrame.D_Up = false;
                    break;
                case "Dv":
                    inputFrame.D_Down = false;
                    break;
                case "D<":
                    inputFrame.D_Left = false;
                    break;
                case "D>":
                    inputFrame.D_Right = false;
                    break;
            }
        }
    }
}
