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
        private static bool readyToNotify = false;

        public static void NotifyCoordinate(Coordinate coordinate, WatchVariableNumberWrapper watchVarWrapper)
        {
            switch (coordinate)
            {
                case Coordinate.X:
                    coordinateVarList.Clear();
                    coordinateVarList.Add(watchVarWrapper);
                    break;
                case Coordinate.Y:
                    if (coordinateVarList.Count == 1)
                        coordinateVarList.Add(watchVarWrapper);
                    break;
                case Coordinate.Z:
                    if (coordinateVarList.Count == 1 || coordinateVarList.Count == 2)
                    {
                        coordinateVarList.Add(watchVarWrapper);
                        readyToNotify = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (readyToNotify)
            {
                foreach (WatchVariableNumberWrapper coordinateVar in coordinateVarList)
                {
                    List<WatchVariableNumberWrapper> coordinateVarListCopy = new List<WatchVariableNumberWrapper>(coordinateVarList);
                    coordinateVar.EnableCoordinateContextMenuStripItemFunctionality(coordinateVarListCopy);
                }
                coordinateVarList.Clear();
                readyToNotify = false;
            }
        }

    }
}
