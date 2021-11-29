using STROOP.Forms;
using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class PendulumData
    {
        public class COL_VERTEX
        {
            public int X;
            public int Y;
            public int Z;

            public COL_VERTEX(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        public static List<COL_VERTEX> COL_VERTICES = new List<COL_VERTEX>()
        {
            new COL_VERTEX(-144, -771, 67),
            new COL_VERTEX(145, -771, 67),
            new COL_VERTEX(106, -704, 67),
            new COL_VERTEX(39, -665, 67),
            new COL_VERTEX(106, -704, -66),
            new COL_VERTEX(39, -665, -66),
            new COL_VERTEX(-105, -916, -66),
            new COL_VERTEX(145, -771, -66),
            new COL_VERTEX(-38, -665, -66),
            new COL_VERTEX(-38, -665, 67),
            new COL_VERTEX(145, -848, 67),
            new COL_VERTEX(145, -848, -66),
            new COL_VERTEX(106, -916, 67),
            new COL_VERTEX(106, -916, -66),
            new COL_VERTEX(39, -954, 67),
            new COL_VERTEX(-38, -954, 67),
            new COL_VERTEX(39, -954, -66),
            new COL_VERTEX(-105, -916, 67),
            new COL_VERTEX(-38, -954, -66),
            new COL_VERTEX(-144, -848, 67),
            new COL_VERTEX(-144, -771, -66),
            new COL_VERTEX(-105, -704, -66),
            new COL_VERTEX(-144, -848, -66),
            new COL_VERTEX(-105, -704, 67),
            new COL_VERTEX(20, -665, 20),
            new COL_VERTEX(-19, -665, 20),
            new COL_VERTEX(-19, -665, -19),
            new COL_VERTEX(20, -665, -19),
            new COL_VERTEX(-14, 0, 15),
            new COL_VERTEX(-19, 0, 20),
            new COL_VERTEX(-19, 0, -19),
            new COL_VERTEX(20, 0, 20),
            new COL_VERTEX(20, 0, -19),
            new COL_VERTEX(15, 0, 15),
            new COL_VERTEX(15, 0, -14),
            new COL_VERTEX(-14, 0, -14),
        };

        public class COL_TRI
        {
            public int Index1;
            public int Index2;
            public int Index3;

            public COL_TRI(int index1, int index2, int index3)
            {
                Index1 = index1;
                Index2 = index2;
                Index3 = index3;
            }
        }

        public static List<COL_TRI> COL_TRIS = new List<COL_TRI>()
        {
            new COL_TRI(0, 1, 2),
            new COL_TRI(0, 2, 3),
            new COL_TRI(2, 4, 5),
            new COL_TRI(2, 5, 3),
            new COL_TRI(1, 4, 2),
            new COL_TRI(6, 5, 4),
            new COL_TRI(6, 4, 7),
            new COL_TRI(1, 7, 4),
            new COL_TRI(6, 8, 5),
            new COL_TRI(8, 3, 5),
            new COL_TRI(0, 3, 9),
            new COL_TRI(8, 9, 3),
            new COL_TRI(0, 10, 1),
            new COL_TRI(7, 1, 10),
            new COL_TRI(6, 7, 11),
            new COL_TRI(7, 10, 11),
            new COL_TRI(0, 12, 10),
            new COL_TRI(12, 11, 10),
            new COL_TRI(22, 0, 20),
            new COL_TRI(6, 11, 13),
            new COL_TRI(12, 13, 11),
            new COL_TRI(0, 14, 12),
            new COL_TRI(13, 12, 14),
            new COL_TRI(6, 13, 16),
            new COL_TRI(13, 14, 16),
            new COL_TRI(0, 15, 14),
            new COL_TRI(16, 14, 15),
            new COL_TRI(6, 16, 18),
            new COL_TRI(16, 15, 18),
            new COL_TRI(0, 17, 15),
            new COL_TRI(18, 15, 17),
            new COL_TRI(18, 17, 6),
            new COL_TRI(0, 19, 17),
            new COL_TRI(6, 17, 19),
            new COL_TRI(6, 20, 21),
            new COL_TRI(6, 21, 8),
            new COL_TRI(6, 22, 20),
            new COL_TRI(6, 19, 22),
            new COL_TRI(22, 19, 0),
            new COL_TRI(0, 9, 23),
            new COL_TRI(20, 0, 23),
            new COL_TRI(20, 23, 21),
            new COL_TRI(21, 23, 9),
            new COL_TRI(21, 9, 8),
            new COL_TRI(24, 29, 25),
            new COL_TRI(25, 29, 30),
            new COL_TRI(25, 30, 26),
            new COL_TRI(24, 31, 29),
            new COL_TRI(26, 30, 32),
            new COL_TRI(28, 34, 35),
            new COL_TRI(26, 32, 27),
            new COL_TRI(27, 32, 31),
            new COL_TRI(27, 31, 24),
            new COL_TRI(28, 33, 34),
        };

        public class Mat4
        {
            private float[][] _matrix;

            public Mat4()
            {
                _matrix = new float[][]
                {
                    new float[] { 1, 0, 0, 0 },
                    new float[] { 0, 1, 0, 0 },
                    new float[] { 0, 0, 1, 0 },
                    new float[] { 0, 0, 0, 1 },
                };
            }
        }

        //public static void load_object_collision_model()
        //{
        //    short[] vertexData = new short[600];

        //    transform_object_vertices(&collisionData, vertexData);

        //    // TERRAIN_LOAD_CONTINUE acts as an "end" to the terrain data.
        //    while (*collisionData != TERRAIN_LOAD_CONTINUE)
        //    {
        //        load_object_surfaces(&collisionData, vertexData);
        //    }
        //}

        //public static void transform_object_vertices(short[] vertexData)
        //{
        //    short[] vertices;
        //    float vx, vy, vz;
        //    int numVertices;

        //    Mat4 objectTransform;
        //    Mat4 m;

        //    objectTransform = &gCurrentObject->transform;

        //    numVertices = *(*data);
        //    (*data)++;

        //    vertices = *data;

        //    if (gCurrentObject->header.gfx.throwMatrix == NULL)
        //    {
        //        gCurrentObject->header.gfx.throwMatrix = objectTransform;
        //        obj_build_transform_from_pos_and_angle(gCurrentObject, O_POS_INDEX, O_FACE_ANGLE_INDEX);
        //    }

        //    obj_apply_scale_to_matrix(gCurrentObject, m, *objectTransform);

        //    // Go through all vertices, rotating and translating them to transform the object.
        //    while (numVertices--)
        //    {
        //        vx = *(vertices++);
        //        vy = *(vertices++);
        //        vz = *(vertices++);

        //        //! No bounds check on vertex data
        //        *vertexData++ = (s16)(vx * m[0][0] + vy * m[1][0] + vz * m[2][0] + m[3][0]);
        //        *vertexData++ = (s16)(vx * m[0][1] + vy * m[1][1] + vz * m[2][1] + m[3][1]);
        //        *vertexData++ = (s16)(vx * m[0][2] + vy * m[1][2] + vz * m[2][2] + m[3][2]);
        //    }

        //    *data = vertices;
        //}
    }
}
