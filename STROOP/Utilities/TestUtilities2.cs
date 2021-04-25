using STROOP.Controls;
using STROOP.Forms;
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
            XDocument doc = Config.MapGui.flowLayoutPanelMapTrackers.ToXDocument();

            //MapObject mapObj1 = new MapMarioObject();
            //MapObjectSettings settings = new MapObjectSettings(changeTriangleWithinDist: true, newTriangleWithinDist: 100);
            //mapObj1.ApplySettings(settings);

            //MapObject mapObj2 = new MapHolpObject();

            //MapTracker mapTracker = new MapTracker(new List<MapObject>() { mapObj1, mapObj2 });

            //var xml = mapTracker.ToXElement();
            //string s = xml.ToString();

            //MapObjectSettings settings = new MapObjectSettings(
            //    changeTriangleWithinDist: true, newTriangleWithinDist: 100);
            //MapObjectSettingsAccumulator accumulator = new MapObjectSettingsAccumulator();
            //accumulator.ApplySettings(settings);
            //XElement xElement = accumulator.ToXElement();
            //MapObjectSettings output = MapObjectSettings.FromXElement(xElement);
        }
    }
} 
