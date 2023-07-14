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
        public enum PuGridlineSetting { SETTING1, SETTING2, SETTING3 };
        private PuGridlineSetting _setting;
        private List<ToolStripMenuItem> _settingItemList;

        private bool _useMarioAsOrigin;
        private ToolStripMenuItem _itemUseMarioAsOrigin;

        private int puSize => 65536;
        private int halfPuSize => 32768;
        private int halfCourseSize => 8192;

        public MapObjectPuGridlines()
            : base()
        {
            Size = 1;
            LineWidth = 1;
            LineColor = Color.Black;

            _setting = PuGridlineSetting.SETTING1;
            _useMarioAsOrigin = false;
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            switch (_setting)
            {
                case PuGridlineSetting.SETTING1:
                    {
                        float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

                        long goThroughValueX = 0;
                        long goThroughValueZ = 0;
                        if (_useMarioAsOrigin)
                        {
                            (int puXIndex, int puYIndex, int puZIndex) = PuUtilities.GetMarioPuIndexes();
                            goThroughValueX = puXIndex * (long)puSize;
                            goThroughValueZ = puZIndex * (long)puSize;
                        }

                        long size = (long)Math.Max(Size, 1);
                        long gap = puSize * size;
                        List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueX, false, true, true);
                        List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueZ, false, true, true);

                        float xMin = Config.CurrentMapGraphics.MapViewXMin;
                        float xMax = Config.CurrentMapGraphics.MapViewXMax;
                        float zMin = Config.CurrentMapGraphics.MapViewZMin;
                        float zMax = Config.CurrentMapGraphics.MapViewZMax;

                        List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                        foreach (long x in xValues)
                        {
                            vertices.Add((x, marioY, zMin));
                            vertices.Add((x, marioY, zMax));
                        }
                        foreach (long z in zValues)
                        {
                            vertices.Add((xMin, marioY, z));
                            vertices.Add((xMax, marioY, z));
                        }
                        return vertices;
                    }
                case PuGridlineSetting.SETTING2:
                    {
                        float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

                        List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                            puSize, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.SPACED_AROUND_ZERO, 0, false, true, true);
                        List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                            puSize, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.SPACED_AROUND_ZERO, 0, false, true, true);

                        float xMin = Config.CurrentMapGraphics.MapViewXMin;
                        float xMax = Config.CurrentMapGraphics.MapViewXMax;
                        float zMin = Config.CurrentMapGraphics.MapViewZMin;
                        float zMax = Config.CurrentMapGraphics.MapViewZMax;

                        List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                        foreach (long x in xValues)
                        {
                            vertices.Add((x, marioY, zMin));
                            vertices.Add((x, marioY, zMax));
                        }
                        foreach (long z in zValues)
                        {
                            vertices.Add((xMin, marioY, z));
                            vertices.Add((xMax, marioY, z));
                        }
                        return vertices;
                    }
                case PuGridlineSetting.SETTING3:
                    {
                        float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

                        List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                            puSize, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);
                        List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                            puSize, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);

                        List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                        foreach (long x in xValues)
                        {
                            foreach (long z in zValues)
                            {
                                float x1 = ExtendedLevelBoundariesUtilities.GetNext(x, -halfCourseSize, false, false);
                                float x2 = ExtendedLevelBoundariesUtilities.GetNext(x, halfCourseSize, false, false);
                                float z1 = ExtendedLevelBoundariesUtilities.GetNext(z, -halfCourseSize, false, false);
                                float z2 = ExtendedLevelBoundariesUtilities.GetNext(z, halfCourseSize, false, false);

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
            if (_setting != PuGridlineSetting.SETTING1)
            {
                return new List<(float x, float y, float z)>();
            }

            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            long goThroughValueX = 0;
            long goThroughValueZ = 0;
            if (_useMarioAsOrigin)
            {
                (int puXIndex, int puYIndex, int puZIndex) = PuUtilities.GetMarioPuIndexes();
                goThroughValueX = puXIndex * (long)puSize;
                goThroughValueZ = puZIndex * (long)puSize;
            }

            long size = (long)Math.Max(Size, 1);
            long gap = puSize * size;
            List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueX, false, true, true);
            List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueZ, false, true, true);

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            foreach (long x in xValues)
            {
                foreach (long z in zValues)
                {
                    vertices.Add((x, marioY, z));
                }
            }
            return vertices;
        }

        protected override List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            switch (_setting)
            {
                case PuGridlineSetting.SETTING1:
                    {
                        float xCenter = Config.CurrentMapGraphics.MapViewCenterXValue;
                        float yCenter = Config.CurrentMapGraphics.MapViewCenterYValue;
                        float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;

                        float xMin = Config.CurrentMapGraphics.MapViewXMin;
                        float xMax = Config.CurrentMapGraphics.MapViewXMax;
                        float yMin = Config.CurrentMapGraphics.MapViewYMin;
                        float yMax = Config.CurrentMapGraphics.MapViewYMax;
                        float zMin = Config.CurrentMapGraphics.MapViewZMin;
                        float zMax = Config.CurrentMapGraphics.MapViewZMax;

                        long goThroughValueX = 0;
                        long goThroughValueY = 0;
                        long goThroughValueZ = 0;
                        if (_useMarioAsOrigin)
                        {
                            (int puXIndex, int puYIndex, int puZIndex) = PuUtilities.GetMarioPuIndexes();
                            goThroughValueX = puXIndex * (long)puSize;
                            goThroughValueY = puYIndex * (long)puSize;
                            goThroughValueZ = puZIndex * (long)puSize;
                        }

                        long size = (long)Math.Max(Size, 1);
                        long gap = puSize * size;
                        List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueX, false, true, true);
                        List<long> yValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewYMin, (long)Config.CurrentMapGraphics.MapViewYMax,
                            gap, true, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueY, false, true, true);
                        List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueZ, false, true, true);

                        if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 32768))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            foreach (long x in xValues)
                            {
                                vertices.Add((x, yMin, zCenter));
                                vertices.Add((x, yMax, zCenter));
                            }
                            foreach (long y in yValues)
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
                            foreach (long z in zValues)
                            {
                                vertices.Add((xCenter, yMin, z));
                                vertices.Add((xCenter, yMax, z));
                            }
                            foreach (long y in yValues)
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
                        float yCenter = Config.CurrentMapGraphics.MapViewCenterYValue;
                        float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;

                        float xMin = Config.CurrentMapGraphics.MapViewXMin;
                        float xMax = Config.CurrentMapGraphics.MapViewXMax;
                        float yMin = Config.CurrentMapGraphics.MapViewYMin;
                        float yMax = Config.CurrentMapGraphics.MapViewYMax;
                        float zMin = Config.CurrentMapGraphics.MapViewZMin;
                        float zMax = Config.CurrentMapGraphics.MapViewZMax;

                        long size = (long)Math.Max(Size, 1);
                        long gap = puSize * size;
                        List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.SPACED_AROUND_ZERO, 0, false, true, true);
                        List<long> yValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewYMin, (long)Config.CurrentMapGraphics.MapViewYMax,
                            gap, true, ExtendedLevelBoundariesUtilities.ValueOffsetType.SPACED_AROUND_ZERO, 0, false, true, true);
                        List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.SPACED_AROUND_ZERO, 0, false, true, true);

                        if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 32768))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            foreach (long x in xValues)
                            {
                                vertices.Add((x, yMin, zCenter));
                                vertices.Add((x, yMax, zCenter));
                            }
                            foreach (long y in yValues)
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
                            foreach (long z in zValues)
                            {
                                vertices.Add((xCenter, yMin, z));
                                vertices.Add((xCenter, yMax, z));
                            }
                            foreach (long y in yValues)
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
                        float yCenter = Config.CurrentMapGraphics.MapViewCenterYValue;
                        float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;

                        float xMin = Config.CurrentMapGraphics.MapViewXMin;
                        float xMax = Config.CurrentMapGraphics.MapViewXMax;
                        float yMin = Config.CurrentMapGraphics.MapViewYMin;
                        float yMax = Config.CurrentMapGraphics.MapViewYMax;
                        float zMin = Config.CurrentMapGraphics.MapViewZMin;
                        float zMax = Config.CurrentMapGraphics.MapViewZMax;

                        long size = (long)Math.Max(Size, 1);
                        long gap = puSize * size;
                        List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);
                        List<long> yValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewYMin, (long)Config.CurrentMapGraphics.MapViewYMax,
                            gap, true, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);
                        List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                            (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                            gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, true);

                        if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                            (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                            Config.CurrentMapGraphics.MapViewYawValue == 32768))
                        {
                            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                            foreach (long x in xValues)
                            {
                                foreach (long y in yValues)
                                {
                                    float x1 = ExtendedLevelBoundariesUtilities.GetNext(x, -halfCourseSize, false, false);
                                    float x2 = ExtendedLevelBoundariesUtilities.GetNext(x, halfCourseSize, false, false);
                                    float y1 = ExtendedLevelBoundariesUtilities.GetNext(y, -halfCourseSize, true, false);
                                    float y2 = ExtendedLevelBoundariesUtilities.GetNext(y, halfCourseSize, true, false);

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
                            foreach (long z in zValues)
                            {
                                foreach (long y in yValues)
                                {
                                    float z1 = ExtendedLevelBoundariesUtilities.GetNext(z, -halfCourseSize, false, false);
                                    float z2 = ExtendedLevelBoundariesUtilities.GetNext(z, halfCourseSize, false, false);
                                    float y1 = ExtendedLevelBoundariesUtilities.GetNext(y, -halfCourseSize, true, false);
                                    float y2 = ExtendedLevelBoundariesUtilities.GetNext(y, halfCourseSize, true, false);

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

        protected override List<(float x, float y, float z)> GetGridlineIntersectionPositionsOrthographicView()
        {
            if (_setting != PuGridlineSetting.SETTING1)
            {
                return new List<(float x, float y, float z)>();
            }

            float xCenter = Config.CurrentMapGraphics.MapViewCenterXValue;
            float yCenter = Config.CurrentMapGraphics.MapViewCenterYValue;
            float zCenter = Config.CurrentMapGraphics.MapViewCenterZValue;

            float xMin = Config.CurrentMapGraphics.MapViewXMin;
            float xMax = Config.CurrentMapGraphics.MapViewXMax;
            float yMin = Config.CurrentMapGraphics.MapViewYMin;
            float yMax = Config.CurrentMapGraphics.MapViewYMax;
            float zMin = Config.CurrentMapGraphics.MapViewZMin;
            float zMax = Config.CurrentMapGraphics.MapViewZMax;

            long goThroughValueX = 0;
            long goThroughValueY = 0;
            long goThroughValueZ = 0;
            if (_useMarioAsOrigin)
            {
                (int puXIndex, int puYIndex, int puZIndex) = PuUtilities.GetMarioPuIndexes();
                goThroughValueX = puXIndex * (long)puSize;
                goThroughValueY = puYIndex * (long)puSize;
                goThroughValueZ = puZIndex * (long)puSize;
            }

            long size = (long)Math.Max(Size, 1);
            long gap = puSize * size;
            List<long> xValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewXMin, (long)Config.CurrentMapGraphics.MapViewXMax,
                gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueX, false, true, true);
            List<long> yValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewYMin, (long)Config.CurrentMapGraphics.MapViewYMax,
                gap, true, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueY, false, true, true);
            List<long> zValues = ExtendedLevelBoundariesUtilities.GetValuesInRange(
                (long)Config.CurrentMapGraphics.MapViewZMin, (long)Config.CurrentMapGraphics.MapViewZMax,
                gap, false, ExtendedLevelBoundariesUtilities.ValueOffsetType.GO_THROUGH_VALUE, goThroughValueZ, false, true, true);

            if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                Config.CurrentMapGraphics.MapViewYawValue == 32768))
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                foreach (long x in xValues)
                {
                    foreach (long y in yValues)
                    {
                        vertices.Add((x, y, zCenter));
                    }
                }
                return vertices;
            }
            else if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                Config.CurrentMapGraphics.MapViewYawValue == 49152))
            {
                List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
                foreach (long z in zValues)
                {
                    foreach (long y in yValues)
                    {
                        vertices.Add((xCenter, y, z));
                    }
                }
                return vertices;
            }
            else
            {
                return new List<(float x, float y, float z)>();
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
                _settingItemList = itemList;

                _itemUseMarioAsOrigin = new ToolStripMenuItem("Use Mario as Origin");
                _itemUseMarioAsOrigin.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeUseMarioAsOrigin: true, newUseMarioAsOrigin: !_useMarioAsOrigin);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                itemList.ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(_itemUseMarioAsOrigin);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetGridlinesToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangePuGridlinesSetting)
            {
                _setting = (PuGridlineSetting)Enum.Parse(typeof(PuGridlineSetting), settings.NewPuGridlinesSetting);
                List<PuGridlineSetting> enumValues = EnumUtilities.GetEnumValues<PuGridlineSetting>(typeof(PuGridlineSetting));
                for (int i = 0; i < 3; i++)
                {
                    _settingItemList[i].Checked = _setting == enumValues[i];
                }
            }

            if (settings.ChangeUseMarioAsOrigin)
            {
                _useMarioAsOrigin = settings.NewUseMarioAsOrigin;
                _itemUseMarioAsOrigin.Checked = _useMarioAsOrigin;
            }
        }
    }
}
