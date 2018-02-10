using STROOP.Controls;
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
    public static class WatchVariableCoordinateManager
    {
        private static List<WatchVariableNumberWrapper> coordinateVarList = new List<WatchVariableNumberWrapper>();

        public static void NotifyCoordinate(WatchVariableCoordinate coordinate, WatchVariableNumberWrapper watchVarWrapper)
        {
            switch (coordinate)
            {
                case WatchVariableCoordinate.X:
                    coordinateVarList.Clear();
                    coordinateVarList.Add(watchVarWrapper);
                    break;
                case WatchVariableCoordinate.Y:
                    if (coordinateVarList.Count == 1) coordinateVarList.Add(watchVarWrapper);
                    break;
                case WatchVariableCoordinate.Z:
                    if (coordinateVarList.Count == 2) coordinateVarList.Add(watchVarWrapper);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (coordinateVarList.Count == 3)
            {
                foreach (WatchVariableNumberWrapper coordinateVar in coordinateVarList)
                {
                    List<WatchVariableNumberWrapper> coordinateVarListCopy = new List<WatchVariableNumberWrapper>(coordinateVarList);
                    coordinateVar.EnableCoordinateContextMenuStripItemFunctionality(coordinateVarListCopy);
                }
                coordinateVarList.Clear();
            }
        }

    }
}
