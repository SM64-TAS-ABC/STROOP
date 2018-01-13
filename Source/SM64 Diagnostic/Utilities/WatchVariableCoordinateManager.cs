using SM64_Diagnostic.Controls;
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
    public static class WatchVariableCoordinateManager
    {
        private static List<WatchVariableNumberWrapper> coordinateVarList = new List<WatchVariableNumberWrapper>();

        public static void NotifyVarXCoordinate(WatchVariableCoordinate coordinate, WatchVariableNumberWrapper varX)
        {
            switch (coordinate)
            {
                case WatchVariableCoordinate.X:
                    coordinateVarList.Clear();
                    coordinateVarList.Add(varX);
                    break;
                case WatchVariableCoordinate.Y:
                    if (coordinateVarList.Count == 1) coordinateVarList.Add(varX);
                    break;
                case WatchVariableCoordinate.Z:
                    if (coordinateVarList.Count == 2) coordinateVarList.Add(varX);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (coordinateVarList.Count == 3)
            {
                foreach (WatchVariableNumberWrapper coordinateVar in coordinateVarList)
                {
                    List<WatchVariableNumberWrapper> coordinateVarListCopy = new List<WatchVariableNumberWrapper>(coordinateVarList);
                    coordinateVar.AddCoordinateContextMenuStripItemFunctionality(coordinateVarListCopy);
                }
                coordinateVarList.Clear();
            }
        }

    }
}
