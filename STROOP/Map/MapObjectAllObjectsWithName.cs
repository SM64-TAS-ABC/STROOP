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
    public class MapObjectAllObjectsWithName : MapObjectIcon
    {
        private readonly string _objName;
        private readonly Image _objImage;
        private readonly Image _objMapImage;

        private readonly Dictionary<Image, int> texDictionary;

        public MapObjectAllObjectsWithName(string objName)
            : base()
        {
            _objName = objName;
            texDictionary = new Dictionary<Image, int>();

            List<ObjectBehaviorAssociation> associations = Config.ObjectAssociations.GetObjectAssociations(objName);
            if (associations.Count > 0)
            {
                _objImage = ImageUtilities.CreateMultiImage(associations.ConvertAll(a => a.Image.Image), 256, 256);
                _objMapImage = ImageUtilities.CreateMultiImage(associations.ConvertAll(a => a.MapImage.Image), 256, 256);
                InternalRotates = associations[0].RotatesOnMap;

                foreach (ObjectBehaviorAssociation a in associations)
                {
                    texDictionary[a.Image.Image] = MapUtilities.LoadTexture(a.Image.Image as Bitmap);
                    texDictionary[a.MapImage.Image] = MapUtilities.LoadTexture(a.MapImage.Image as Bitmap);
                }
            }
            else
            {
                _objImage = Config.ObjectAssociations.EmptyImage;
                _objMapImage = Config.ObjectAssociations.EmptyImage;
                InternalRotates = false;
            }
        }

        public string GetObjName()
        {
            return _objName;
        }

        public override Image GetInternalImage()
        {
            return _iconType == MapTrackerIconType.ObjectSlotImage ?
                _objImage :
                _objMapImage;
        }

        public override string GetName()
        {
            return "All " + _objName;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float x, float y, float z, float angle, Image image, int tex, uint objAddress)> data = GetData();
            data.Reverse();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, Image image, int tex, uint objAddress) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(x, z, UseRelativeCoordinates);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(image.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && dataPoint.objAddress == hoverData?.ObjAddress)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(tex, point, size, angleDegrees, opacity);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<(float x, float y, float z, float angle, Image image, int tex, uint objAddress)> data = GetData();
            data.Reverse();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, Image image, int tex, uint objAddress) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlOrthographicView(x, y, z, UseRelativeCoordinates);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(image.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && dataPoint.objAddress == hoverData?.ObjAddress)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(tex, point, size, angleDegrees, opacity);
            }
        }

        public override void DrawOn3DControl()
        {
            List<(float x, float y, float z, float angle, Image image, int tex, uint objAddress)> data = GetData();
            data.Reverse();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, Image image, int tex, uint objAddress) = dataPoint;

                Matrix4 viewMatrix = GetModelMatrix(x, y, z, angle, image);
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                Map3DVertex[] vertices = GetVertices();
                int vertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map3DVertex.Size),
                    vertices, BufferUsageHint.StaticDraw);
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
                GL.DeleteBuffer(vertexBuffer);
            }
        }
        
        public Matrix4 GetModelMatrix(float x, float y, float z, float ang, Image image)
        {
            SizeF _imageNormalizedSize = new SizeF(
                image.Width >= image.Height ? 1.0f : (float)image.Width / image.Height,
                image.Width <= image.Height ? 1.0f : (float)image.Height / image.Width);

            float angle = Rotates ? (float)MoreMath.AngleUnitsToRadians(ang - MapConfig.Map3DCameraYaw + 32768) : 0;
            Vector3 pos = new Vector3(x, y, z);

            float size = Size / 200;
            return Matrix4.CreateScale(size * _imageNormalizedSize.Width, size * _imageNormalizedSize.Height, 1)
                * Matrix4.CreateRotationZ(angle)
                * Matrix4.CreateScale(1.0f / Config.Map3DGraphics.NormalizedWidth, 1.0f / Config.Map3DGraphics.NormalizedHeight, 1)
                * Matrix4.CreateTranslation(MapUtilities.GetPositionOnViewFromCoordinate(pos));
        }
        
        private Map3DVertex[] GetVertices()
        {
            return new Map3DVertex[]
            {
                new Map3DVertex(new Vector3(-1, -1, 0), Color4, new Vector2(0, 1)),
                new Map3DVertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color4, new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, 1, 0), Color4, new Vector2(1, 0)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color4,  new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
            };
        }

        public List<(float x, float y, float z, float angle, Image image, int tex, uint objAddress)> GetData()
        {
            List<ObjectDataModel> objs = Config.ObjectSlotsManager.GetLoadedObjectsWithRegex(_objName);
            return objs.ConvertAll(obj =>
            {
                Image image = _iconType == MapTrackerIconType.ObjectSlotImage ?
                    obj.BehaviorAssociation.Image.Image :
                    obj.BehaviorAssociation.MapImage.Image;
                int tex = texDictionary[image];
                return (obj.X, obj.Y, obj.Z, (float)obj.FacingYaw, image, tex, obj.Address);
            });
        }

        public override bool ParticipatesInGlobalIconSize()
        {
            return true;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            List<(float x, float y, float z, float angle, Image image, int tex, uint objAddress)> data = GetData();
            foreach (var dataPoint in data)
            {
                double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
                double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, objAddress: dataPoint.objAddress);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            List<(float x, float y, float z, float angle, Image image, int tex, uint objAddress)> data = GetData();
            foreach (var dataPoint in data)
            {
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(dataPoint.x, dataPoint.y, dataPoint.z, UseRelativeCoordinates);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, objAddress: dataPoint.objAddress);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            ToolStripMenuItem selectObjectItem = new ToolStripMenuItem("Select Object in Object Tab");
            selectObjectItem.Click += (sender, e) => Config.ObjectSlotsManager.SelectSlotByAddress(hoverData.ObjAddress.Value);
            output.Insert(0, selectObjectItem);

            ToolStripMenuItem copyAddressItem = new ToolStripMenuItem("Copy Address");
            copyAddressItem.Click += (sender, e) => Clipboard.SetText(HexUtilities.FormatValue(hoverData.ObjAddress.Value));
            output.Insert(1, copyAddressItem);

            float x = Config.Stream.GetFloat(hoverData.ObjAddress.Value + ObjectConfig.XOffset);
            float y = Config.Stream.GetFloat(hoverData.ObjAddress.Value + ObjectConfig.YOffset);
            float z = Config.Stream.GetFloat(hoverData.ObjAddress.Value + ObjectConfig.ZOffset);
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(x, y, z, "Position");
            output.Insert(2, copyPositionItem);

            return output;
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("objectName", _objName),
            };
        }
    }
}
