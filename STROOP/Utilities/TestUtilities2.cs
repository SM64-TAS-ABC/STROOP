using STROOP.Controls;
using STROOP.Forms;
using STROOP.M64;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Utilities
{
    public static class TestUtilities2
    {
        public static void Test()
        {
            TrackPlatform trackPlatform = new TrackPlatform();
            for (int i = 0; i < 100; i++)
            {
                trackPlatform.Update(true);
                Config.Print("{0}: {1}", i, trackPlatform.oPosX);
            }
            Config.Print(trackPlatform);
        }
    }
}
