using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace STROOP.Map
{
    public class MapPuGridlinesObject : MapLineObject
    {
        private enum PuGridlineSetting { SETTING1, SETTING2, SETTING3 };
        private PuGridlineSetting _setting;

        public MapPuGridlinesObject()
            : base()
        {
            OutlineWidth = 1;
            OutlineColor = Color.Black;

            _setting = PuGridlineSetting.SETTING1;
        }

        protected override List<(float x, float y, float z)> GetVertices()
        {
            switch (_setting)
            {
                case PuGridlineSetting.SETTING1:
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);

                        int xMin = ((((int)Config.MapGraphics.MapViewXMin) / 65536) - 1) * 65536;
                        int xMax = ((((int)Config.MapGraphics.MapViewXMax) / 65536) + 1) * 65536;
                        int zMin = ((((int)Config.MapGraphics.MapViewZMin) / 65536) - 1) * 65536;
                        int zMax = ((((int)Config.MapGraphics.MapViewZMax) / 65536) + 1) * 65536;

                        List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                        for (int x = xMin; x <= xMax; x += 65536)
                        {
                            vertices.Add((x, marioY, zMin));
                            vertices.Add((x, marioY, zMax));
                        }
                        for (int z = zMin; z <= zMax; z += 65536)
                        {
                            vertices.Add((xMin, marioY, z));
                            vertices.Add((xMax, marioY, z));
                        }
                        return vertices;
                    }
                case PuGridlineSetting.SETTING2:
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);

                        int xMin = ((((int)Config.MapGraphics.MapViewXMin) / 65536) - 1) * 65536 - 32768;
                        int xMax = ((((int)Config.MapGraphics.MapViewXMax) / 65536) + 1) * 65536 + 32768;
                        int zMin = ((((int)Config.MapGraphics.MapViewZMin) / 65536) - 1) * 65536 - 32768;
                        int zMax = ((((int)Config.MapGraphics.MapViewZMax) / 65536) + 1) * 65536 + 32768;

                        List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                        for (int x = xMin; x <= xMax; x += 65536)
                        {
                            vertices.Add((x, marioY, zMin));
                            vertices.Add((x, marioY, zMax));
                        }
                        for (int z = zMin; z <= zMax; z += 65536)
                        {
                            vertices.Add((xMin, marioY, z));
                            vertices.Add((xMax, marioY, z));
                        }
                        return vertices;
                    }
                case PuGridlineSetting.SETTING3:
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);

                        int xMin = ((((int)Config.MapGraphics.MapViewXMin) / 65536) - 1) * 65536;
                        int xMax = ((((int)Config.MapGraphics.MapViewXMax) / 65536) + 1) * 65536;
                        int zMin = ((((int)Config.MapGraphics.MapViewZMin) / 65536) - 1) * 65536;
                        int zMax = ((((int)Config.MapGraphics.MapViewZMax) / 65536) + 1) * 65536;

                        List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                        for (int x = xMin; x <= xMax; x += 65536)
                        {
                            for (int z = zMin; z <= zMax; z += 65536)
                            {
                                float x1 = x - 8192;
                                float x2 = x + 8192;
                                float z1 = z - 8192;
                                float z2 = z + 8192;

                                vertices.Add((x1, marioY, z1));
                                vertices.Add((x1, marioY, z2));

                                vertices.Add((x2, marioY, z1));
                                vertices.Add((x2, marioY, z2));

                                vertices.Add((x1, marioY, z1));
                                vertices.Add((x2, marioY, z1));

                                vertices.Add((x1, marioY, z2));
                                vertices.Add((x2, marioY, z2));
                            }
                        }
                        return vertices;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override string GetName()
        {
            return "PU Gridlines";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                List<string> itemNames = new List<string>() { "Setting 1", "Setting 2", "Setting 3" };
                List<PuGridlineSetting> itemValues = EnumUtilities.GetEnumValues<PuGridlineSetting>(typeof(PuGridlineSetting));
                Action<PuGridlineSetting> setterAction = (PuGridlineSetting setting) => _setting = setting;
                PuGridlineSetting startingValue = _setting;
                (List<ToolStripMenuItem> itemList, Action<PuGridlineSetting> valueAction) =
                    ControlUtilities.CreateCheckableItems(
                        itemNames, itemValues, setterAction, startingValue);
                _contextMenuStrip = new ContextMenuStrip();
                itemList.ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }
    }
}
