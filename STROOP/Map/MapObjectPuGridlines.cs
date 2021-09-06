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
    public class MapObjectPuGridlines : MapObjectGridlines
    {
        private enum PuGridlineSetting { SETTING1, SETTING2, SETTING3 };
        private PuGridlineSetting _setting;

        public MapObjectPuGridlines()
            : base()
        {
            Size = 1;
            LineWidth = 1;
            LineColor = Color.Black;

            _setting = PuGridlineSetting.SETTING1;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            if (!Config.MapGui.checkBoxMapOptionsEnablePuView.Checked)
            {
                return new List<(float x, float y, float z)>();
            }

            switch (_setting)
            {
                case PuGridlineSetting.SETTING1:
                    {
                        float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

                        float size = Math.Max(Size, 1);
                        long spacing = (long)(65536 * size);

                        long xMin = ((((long)Config.CurrentMapGraphics.MapViewXMin) / spacing) - 1) * spacing;
                        long xMax = ((((long)Config.CurrentMapGraphics.MapViewXMax) / spacing) + 1) * spacing;
                        long zMin = ((((long)Config.CurrentMapGraphics.MapViewZMin) / spacing) - 1) * spacing;
                        long zMax = ((((long)Config.CurrentMapGraphics.MapViewZMax) / spacing) + 1) * spacing;

                        List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                        for (long x = xMin; x <= xMax; x += spacing)
                        {
                            vertices.Add((x, marioY, zMin));
                            vertices.Add((x, marioY, zMax));
                        }
                        for (long z = zMin; z <= zMax; z += spacing)
                        {
                            vertices.Add((xMin, marioY, z));
                            vertices.Add((xMax, marioY, z));
                        }
                        return vertices;
                    }
                case PuGridlineSetting.SETTING2:
                    {
                        float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

                        int xMin = ((((int)Config.CurrentMapGraphics.MapViewXMin) / 65536) - 1) * 65536 - 32768;
                        int xMax = ((((int)Config.CurrentMapGraphics.MapViewXMax) / 65536) + 1) * 65536 + 32768;
                        int zMin = ((((int)Config.CurrentMapGraphics.MapViewZMin) / 65536) - 1) * 65536 - 32768;
                        int zMax = ((((int)Config.CurrentMapGraphics.MapViewZMax) / 65536) + 1) * 65536 + 32768;

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
                        float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

                        int xMin = ((((int)Config.CurrentMapGraphics.MapViewXMin) / 65536) - 1) * 65536;
                        int xMax = ((((int)Config.CurrentMapGraphics.MapViewXMax) / 65536) + 1) * 65536;
                        int zMin = ((((int)Config.CurrentMapGraphics.MapViewZMin) / 65536) - 1) * 65536;
                        int zMax = ((((int)Config.CurrentMapGraphics.MapViewZMax) / 65536) + 1) * 65536;

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

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsTopDownView()
        {
            if (!Config.MapGui.checkBoxMapOptionsEnablePuView.Checked)
            {
                return new List<(float x, float y, float z)>();
            }

            if (_setting != PuGridlineSetting.SETTING1)
            {
                return new List<(float x, float y, float z)>();
            }

            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            float size = Math.Max(Size, 1);
            long spacing = (long)(65536 * size);

            long xMin = ((((long)Config.CurrentMapGraphics.MapViewXMin) / spacing) - 1) * spacing;
            long xMax = ((((long)Config.CurrentMapGraphics.MapViewXMax) / spacing) + 1) * spacing;
            long zMin = ((((long)Config.CurrentMapGraphics.MapViewZMin) / spacing) - 1) * spacing;
            long zMax = ((((long)Config.CurrentMapGraphics.MapViewZMax) / spacing) + 1) * spacing;

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            for (long x = xMin; x <= xMax; x += spacing)
            {
                for (long z = zMin; z <= zMax; z += spacing)
                {
                    vertices.Add((x, marioY, z));
                }
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            if (!Config.MapGui.checkBoxMapOptionsEnablePuView.Checked)
            {
                return new List<(float x, float y, float z)>();
            }

            switch (_setting)
            {
                case PuGridlineSetting.SETTING1:
                    {
                        float xCenter = Config.CurrentMapGraphics.MapViewCenterXValue;
                        float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;
                        int xMin = ((((int)Config.CurrentMapGraphics.MapViewXMin) / 65536) - 1) * 65536;
                        int xMax = ((((int)Config.CurrentMapGraphics.MapViewXMax) / 65536) + 1) * 65536;
                        int yMin = ((((int)Config.CurrentMapGraphics.MapViewYMin) / 65536) - 1) * 65536;
                        int yMax = ((((int)Config.CurrentMapGraphics.MapViewYMax) / 65536) + 1) * 65536;
                        int zMin = ((((int)Config.CurrentMapGraphics.MapViewZMin) / 65536) - 1) * 65536;
                        int zMax = ((((int)Config.CurrentMapGraphics.MapViewZMax) / 65536) + 1) * 65536;

                        if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 32768))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            for (int x = xMin; x <= xMax; x += 65536)
                            {
                                vertices.Add((x, yMin, zCenter));
                                vertices.Add((x, yMax, zCenter));
                            }
                            for (int y = yMin; y <= yMax; y += 65536)
                            {
                                vertices.Add((xMin, y, zCenter));
                                vertices.Add((xMax, y, zCenter));
                            }
                            return vertices;
                        }
                        else if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 49152))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            for (int z = zMin; z <= zMax; z += 65536)
                            {
                                vertices.Add((xCenter, yMin, z));
                                vertices.Add((xCenter, yMax, z));
                            }
                            for (int y = yMin; y <= yMax; y += 65536)
                            {
                                vertices.Add((zCenter, y, zMin));
                                vertices.Add((xCenter, y, zMax));
                            }
                            return vertices;
                        }
                        else
                        {
                            return new List<(float x, float y, float z)>();
                        }
                    }
                case PuGridlineSetting.SETTING2:
                    {
                        float xCenter = Config.CurrentMapGraphics.MapViewCenterXValue;
                        float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;
                        int xMin = ((((int)Config.CurrentMapGraphics.MapViewXMin) / 65536) - 1) * 65536 - 32768;
                        int xMax = ((((int)Config.CurrentMapGraphics.MapViewXMax) / 65536) + 1) * 65536 + 32768;
                        int yMin = ((((int)Config.CurrentMapGraphics.MapViewYMin) / 65536) - 1) * 65536 - 32768;
                        int yMax = ((((int)Config.CurrentMapGraphics.MapViewYMax) / 65536) + 1) * 65536 + 32768;
                        int zMin = ((((int)Config.CurrentMapGraphics.MapViewZMin) / 65536) - 1) * 65536 - 32768;
                        int zMax = ((((int)Config.CurrentMapGraphics.MapViewZMax) / 65536) + 1) * 65536 + 32768;

                        if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 32768))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            for (int x = xMin; x <= xMax; x += 65536)
                            {
                                vertices.Add((x, yMin, zCenter));
                                vertices.Add((x, yMax, zCenter));
                            }
                            for (int y = yMin; y <= yMax; y += 65536)
                            {
                                vertices.Add((xMin, y, zCenter));
                                vertices.Add((xMax, y, zCenter));
                            }
                            return vertices;
                        }
                        else if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 49152))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            for (int z = zMin; z <= zMax; z += 65536)
                            {
                                vertices.Add((xCenter, yMin, z));
                                vertices.Add((xCenter, yMax, z));
                            }
                            for (int y = yMin; y <= yMax; y += 65536)
                            {
                                vertices.Add((zCenter, y, zMin));
                                vertices.Add((xCenter, y, zMax));
                            }
                            return vertices;
                        }
                        else
                        {
                            return new List<(float x, float y, float z)>();
                        }
                    }
                case PuGridlineSetting.SETTING3:
                    {
                        float xCenter = Config.CurrentMapGraphics.MapViewCenterXValue;
                        float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;
                        int xMin = ((((int)Config.CurrentMapGraphics.MapViewXMin) / 65536) - 1) * 65536;
                        int xMax = ((((int)Config.CurrentMapGraphics.MapViewXMax) / 65536) + 1) * 65536;
                        int yMin = ((((int)Config.CurrentMapGraphics.MapViewYMin) / 65536) - 1) * 65536;
                        int yMax = ((((int)Config.CurrentMapGraphics.MapViewYMax) / 65536) + 1) * 65536;
                        int zMin = ((((int)Config.CurrentMapGraphics.MapViewZMin) / 65536) - 1) * 65536;
                        int zMax = ((((int)Config.CurrentMapGraphics.MapViewZMax) / 65536) + 1) * 65536;

                        if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 32768))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            for (int x = xMin; x <= xMax; x += 65536)
                            {
                                for (int y = yMin; y <= yMax; y += 65536)
                                {
                                    float x1 = x - 8192;
                                    float x2 = x + 8192;
                                    float y1 = y - 8192;
                                    float y2 = y + 8192;

                                    vertices.Add((x1, y1, zCenter));
                                    vertices.Add((x1, y2, zCenter));

                                    vertices.Add((x2, y1, zCenter));
                                    vertices.Add((x2, y2, zCenter));

                                    vertices.Add((x1, y1, zCenter));
                                    vertices.Add((x2, y1, zCenter));

                                    vertices.Add((x1, y2, zCenter));
                                    vertices.Add((x2, y2, zCenter));
                                }
                            }
                            return vertices;
                        }
                        else if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 49152))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            for (int z = zMin; z <= zMax; z += 65536)
                            {
                                for (int y = yMin; y <= yMax; y += 65536)
                                {
                                    float z1 = z - 8192;
                                    float z2 = z + 8192;
                                    float y1 = y - 8192;
                                    float y2 = y + 8192;

                                    vertices.Add((xCenter, y1, z1));
                                    vertices.Add((xCenter, y2, z1));

                                    vertices.Add((xCenter, y1, z2));
                                    vertices.Add((xCenter, y2, z2));

                                    vertices.Add((xCenter, y1, z1));
                                    vertices.Add((xCenter, y1, z2));

                                    vertices.Add((xCenter, y2, z1));
                                    vertices.Add((xCenter, y2, z2));
                                }
                            }
                            return vertices;
                        }
                        else
                        {
                            return new List<(float x, float y, float z)>();
                        }
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
                Action<PuGridlineSetting> setterAction = (PuGridlineSetting setting) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePuGridlinesSetting: true, newPuGridlinesSetting: setting.ToString());
                    GetParentMapTracker().ApplySettings(settings);
                };
                PuGridlineSetting startingValue = _setting;
                (List<ToolStripMenuItem> itemList, Action<PuGridlineSetting> valueAction) =
                    ControlUtilities.CreateCheckableItems(
                        itemNames, itemValues, setterAction, startingValue);

                _contextMenuStrip = new ContextMenuStrip();
                itemList.ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetLineToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangePuGridlinesSetting)
            {
                _setting = (PuGridlineSetting)Enum.Parse(typeof(PuGridlineSetting), settings.NewPuGridlinesSetting);
            }
        }
    }
}
