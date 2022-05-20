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
using System.Windows.Forms;
using STROOP.Map.Map3D;
using STROOP.Models;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectAllMapObjectsWithName : MapObject
    {
        private readonly string _objName;
        private readonly List<MapObject> _subMapObjs;

        public MapObjectAllMapObjectsWithName(string objName, List<MapObject> subMapObjs)
            : base()
        {
            _objName = objName;
            _subMapObjs = subMapObjs;

            GetProperties();
        }

        public override Image GetInternalImage()
        {
            return _subMapObjs[0].GetInternalImage();
        }

        public override string GetName()
        {
            return "All " + _subMapObjs[0].GetType().Name.Substring("MapObject".Length) + " for " + _objName;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<MapObject> mapObjs = GetCurrentMapObjects();
            mapObjs.Reverse();
            foreach (MapObject mapObj in mapObjs)
            {
                mapObj.DrawOn2DControlTopDownView(hoverData);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<MapObject> mapObjs = GetCurrentMapObjects();
            mapObjs.Reverse();
            foreach (MapObject mapObj in mapObjs)
            {
                mapObj.DrawOn2DControlOrthographicView(hoverData);
            }
        }

        public override void DrawOn3DControl()
        {
            List<MapObject> mapObjs = GetCurrentMapObjects();
            mapObjs.Reverse();
            foreach (MapObject mapObj in mapObjs)
            {
                mapObj.DrawOn3DControl();
            }
        }

        private List<MapObject> GetCurrentMapObjects()
        {
            List<MapObject> mapObjs = Config.ObjectSlotsManager.GetLoadedObjectsWithRegex(_objName)
                .ConvertAll(obj => obj.Address)
                .ConvertAll(address => ObjectUtilities.GetObjectIndex(address).Value)
                .ConvertAll(index => _subMapObjs[index]);
            mapObjs.ForEach(mapObj => SetProperties(mapObj));
            return mapObjs;
        }

        private void GetProperties()
        {
            MapObject mapObj = _subMapObjs[0];
            Size = mapObj.Size;
            Opacity = mapObj.Opacity;
            LineWidth = mapObj.LineWidth;
            Color = mapObj.Color;
            LineColor = mapObj.LineColor;
            InternalRotates = mapObj.InternalRotates;
            Scales = mapObj.Scales;
        }

        private void SetProperties(MapObject mapObj)
        {
            mapObj.Size = Size;
            mapObj.Opacity = Opacity;
            mapObj.LineWidth = LineWidth;
            mapObj.Color = Color;
            mapObj.LineColor = LineColor;
            mapObj.InternalRotates = InternalRotates;
            mapObj.Scales = Scales;
        }

        public override bool ParticipatesInGlobalIconSize()
        {
            return true;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public bool ContainsMapObject(MapObject mapObject)
        {
            return _subMapObjs.Contains(mapObject);
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            return _subMapObjs[0].GetContextMenuStrip();
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            foreach (MapObject mapObj in _subMapObjs)
            {
                mapObj.ApplySettings(settings);
            }
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);
            return output;
        }
    }
}
