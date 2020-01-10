﻿using System;
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
using STROOP.Models;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public abstract class MapWallObject : MapTriangleObject
    {
        private float? _relativeHeight;

        public MapWallObject()
            : base()
        {
            Size = 50;
            Opacity = 0.5;
            Color = Color.Green;

            _relativeHeight = null;
        }

        public override void DrawOn2DControl()
        {
            float marioHeight = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float? height = _relativeHeight.HasValue ? marioHeight - _relativeHeight.Value : (float?)null;
            List<(float x1, float z1, float x2, float z2, bool xProjection)> wallData = GetTriangles()
                .ConvertAll(tri => MapUtilities.Get2DWallDataFromTri(tri, height))
                .FindAll(wallDataNullable => wallDataNullable.HasValue)
                .ConvertAll(wallDataNullable => wallDataNullable.Value);

            foreach ((float x1, float z1, float x2, float z2, bool xProjection) in wallData)
            {
                float angle = (float)MoreMath.AngleTo_Radians(x1, z1, x2, z2);
                float projectionDist = Size / (float)Math.Abs(xProjection ? Math.Cos(angle) : Math.Sin(angle));
                List<List<(float x, float z)>> quads = new List<List<(float x, float z)>>();
                Action<float, float> addQuad = (float xAdd, float zAdd) =>
                {
                    quads.Add(new List<(float x, float z)>
                    {
                        (x1, z1),
                        (x1 + xAdd, z1 + zAdd),
                        (x2 + xAdd, z2 + zAdd),
                        (x2, z2)
                    });
                };
                if (xProjection)
                {
                    addQuad(projectionDist, 0);
                    addQuad(-1 * projectionDist, 0);
                }
                else
                {
                    addQuad(0, projectionDist);
                    addQuad(0, -1 * projectionDist);
                }

                List<List<(float x, float z)>> quadsForControl =
                    quads.ConvertAll(quad => quad.ConvertAll(
                        vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.z)));

                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                // Draw quad
                GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
                GL.Begin(PrimitiveType.Quads);
                foreach (List<(float x, float z)> quad in quadsForControl)
                {
                    foreach ((float x, float z) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                }
                GL.End();

                // Draw outline
                if (OutlineWidth != 0)
                {
                    GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                    GL.LineWidth(OutlineWidth);
                    foreach (List<(float x, float z)> quad in quadsForControl)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        foreach ((float x, float z) in quad)
                        {
                            GL.Vertex2(x, z);
                        }
                        GL.End();
                    }
                }

                GL.Color4(1, 1, 1, 1.0f);
            }
        }

        protected BetterContextMenuStrip CreateWallContextMenuStrip()
        {
            ToolStripMenuItem itemSetRelativeHeight = new ToolStripMenuItem("Set Relative Height");
            itemSetRelativeHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter relative height of wall hitbox compared to wall triangle.");
                float? relativeHeightNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!relativeHeightNullable.HasValue) return;
                _relativeHeight = relativeHeightNullable.Value;
            };

            ToolStripMenuItem itemClearRelativeHeight = new ToolStripMenuItem("Clear Relative Height");
            itemClearRelativeHeight.Click += (sender, e) =>
            {
                _relativeHeight = null;
            };

            BetterContextMenuStrip contextMenuStrip = new BetterContextMenuStrip();
            contextMenuStrip.AddToEndingList(itemSetRelativeHeight);
            contextMenuStrip.AddToEndingList(itemClearRelativeHeight);

            return contextMenuStrip;
        }

        public override void DrawOn3DControl()
        {
            float relativeHeight = _relativeHeight ?? 0;
            List<TriangleDataModel> tris = GetTriangles();

            List<List<(float x, float y, float z)>> centerSurfaces =
                tris.ConvertAll(tri => tri.Get3DVertices()
                    .ConvertAll(vertex => OffsetVertex(vertex, 0, relativeHeight, 0)));

            List<List<(float x, float y, float z)>> GetFrontOrBackSurfaces(bool front) =>
                tris.ConvertAll(tri =>
                {
                    bool xProjection = tri.XProjection;
                    float angle = (float)Math.Atan2(tri.NormX, tri.NormZ);
                    float projectionMag = Size / (float)Math.Abs(xProjection ? Math.Sin(angle) : Math.Cos(angle));
                    float projectionDist = front ? projectionMag : -1 * projectionMag;
                    float xOffset = xProjection ? projectionDist : 0;
                    float yOffset = relativeHeight;
                    float zOffset = xProjection ? 0 : projectionDist;
                    return tri.Get3DVertices().ConvertAll(vertex =>
                    {
                        return OffsetVertex(vertex, xOffset, yOffset, zOffset);
                    });
                });
            List<List<(float x, float y, float z)>> frontSurfaces = GetFrontOrBackSurfaces(true);
            List<List<(float x, float y, float z)>> backSurfaces = GetFrontOrBackSurfaces(false);

            List<List<(float x, float y, float z)>> GetSideSurfaces(int index1, int index2) =>
                tris.ConvertAll(tri =>
                {
                    bool xProjection = tri.XProjection;
                    float angle = (float)Math.Atan2(tri.NormX, tri.NormZ);
                    float projectionMag = Size / (float)Math.Abs(xProjection ? Math.Sin(angle) : Math.Cos(angle));
                    float xOffsetMag = xProjection ? projectionMag : 0;
                    float zOffsetMag = xProjection ? 0 : projectionMag;
                    List<(float x, float y, float z)> vertices = tri.Get3DVertices();
                    return new List<(float x, float y, float z)>
                    {
                        OffsetVertex(vertices[index1], xOffsetMag, relativeHeight, zOffsetMag),
                        OffsetVertex(vertices[index2], xOffsetMag, relativeHeight, zOffsetMag),
                        OffsetVertex(vertices[index2], -1 * xOffsetMag, relativeHeight, -1 * zOffsetMag),
                        OffsetVertex(vertices[index1], -1 * xOffsetMag, relativeHeight, -1 * zOffsetMag)
                    };
                });
            List<List<(float x, float y, float z)>> side1Surfaces = GetSideSurfaces(0, 1);
            List<List<(float x, float y, float z)>> side2Surfaces = GetSideSurfaces(1, 2);
            List<List<(float x, float y, float z)>> side3Surfaces = GetSideSurfaces(2, 0);

            List<List<(float x, float y, float z)>> allSurfaces =
                centerSurfaces
                .Concat(frontSurfaces)
                .Concat(backSurfaces)
                .Concat(side1Surfaces)
                .Concat(side2Surfaces)
                .Concat(side3Surfaces)
                .ToList();

            List<Map3DVertex[]> vertexArrayForSurfaces = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), Color4)).ToArray());
            List<Map3DVertex[]> vertexArrayForEdges = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), OutlineColor)).ToArray());

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            vertexArrayForSurfaces.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });

            if (OutlineWidth != 0)
            {
                vertexArrayForEdges.ForEach(vertexes =>
                {
                    int buffer = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    GL.LineWidth(OutlineWidth);
                    Config.Map3DGraphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer);
                });
            }
        }
    }
}
