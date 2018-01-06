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
    public static class VarXCoordinateUtilities
    {
        private static List<VarXNumber> coordinateVarList = new List<VarXNumber>();

        public static void NotifyVarXCoordinate(VarXCoordinate coordinate, VarXNumber varX)
        {
            switch (coordinate)
            {
                case VarXCoordinate.X:
                    coordinateVarList.Clear();
                    coordinateVarList.Add(varX);
                    break;
                case VarXCoordinate.Y:
                    if (coordinateVarList.Count == 1) coordinateVarList.Add(varX);
                    break;
                case VarXCoordinate.Z:
                    if (coordinateVarList.Count == 2) coordinateVarList.Add(varX);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (coordinateVarList.Count == 3)
            {
                foreach (VarXNumber coordinateVar in coordinateVarList)
                {
                    List<VarXNumber> coordinateVarListCopy = new List<VarXNumber>(coordinateVarList);
                    coordinateVar.AddCoordinateContextMenuStripItemFunctionality(coordinateVarListCopy);
                }
                coordinateVarList.Clear();
            }
        }

    }
}
