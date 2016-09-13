using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared.Math.M
{
    /// <summary>
    /// A utility-class that offers several mathematical algorithms.
    /// </summary>
    public static class MathUtils
    {
        #region VARIABLES
        private static float DEG_2_RAD = (float)(System.Math.PI / 180f);
        private static float RAD_2_DEG = (float)(180f / System.Math.PI);
        #endregion

        #region METHODS
        /// <summary>
        /// Translates an array of 3d-coordinates to screen-coodinates
        /// </summary>
        /// <param name="viewMatrix">The viewmatrix used to perform translation</param>
        /// <param name="screenSize">The size of the screen which is translated to</param>
        /// <param name="points">Array of 3d-coordinates</param>
        /// <returns>Array of translated screen-coodinates</returns>
        public static Vector2[] WorldToScreen(this Matrix viewMatrix, Vector2 screenSize, params Vector3[] points)
        {
            Vector2[] worlds = new Vector2[points.Length];
            for (int i = 0; i < worlds.Length; i++)
                worlds[i] = viewMatrix.WorldToScreen(screenSize, points[i]);
            return worlds;
        }
        /// <summary>
        /// Translates a 3d-coordinate to a screen-coodinate
        /// </summary>
        /// <param name="viewMatrix">The viewmatrix used to perform translation</param>
        /// <param name="screenSize">The size of the screen which is translated to</param>
        /// <param name="point3D">3d-coordinate of the point to translate</param>
        /// <returns>Translated screen-coodinate</returns>
        public static Vector2 WorldToScreen(this Matrix viewMatrix, Vector2 screenSize, Vector3 point3D)
        {
            Vector2 returnVector = Vector2.Zero;
            float w = viewMatrix[3, 0] * point3D.X + viewMatrix[3, 1] * point3D.Y + viewMatrix[3, 2] * point3D.Z + viewMatrix[3, 3];
            if (w >= 0.01f)
            {
                float inverseX = 1f / w;
                returnVector.X =
                    (screenSize.X / 2f) +
                    (0.5f * (
                    (viewMatrix[0, 0] * point3D.X + viewMatrix[0, 1] * point3D.Y + viewMatrix[0, 2] * point3D.Z + viewMatrix[0, 3])
                    * inverseX)
                    * screenSize.X + 0.5f);
                returnVector.Y =
                    (screenSize.Y / 2f) -
                    (0.5f * (
                    (viewMatrix[1, 0] * point3D.X + viewMatrix[1, 1] * point3D.Y + viewMatrix[1, 2] * point3D.Z + viewMatrix[1, 3])
                    * inverseX)
                    * screenSize.Y + 0.5f);
            }
            return returnVector;
        }
        /// <summary>
        /// Applies (adds) an offset to an array of 3d-coordinates
        /// </summary>
        /// <param name="offset">Offset to apply</param>
        /// <param name="points">Array if 3d-coordinates</param>
        /// <returns>Array of manipulated 3d-coordinates</returns>
        public static Vector3[] OffsetVectors(this Vector3 offset, params Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] += offset;
            return points;
        }
        /// <summary>
        /// Copies an array of vectors to a new array containing identical, new Vector3s (deep-copy)
        /// </summary>
        /// <param name="source">Source-array to copy from</param>
        /// <returns>New array containing identical yet new Vector3s</returns>
        public static Vector3[] CopyVectors(this Vector3[] source)
        {
            Vector3[] ret = new Vector3[source.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = new Vector3(source[i]);
            return ret;
        }
        /// <summary>
        /// Rotates a given point around another point
        /// </summary>
        /// <param name="pointToRotate">Point to rotate</param>
        /// <param name="centerPoint">Point to rotate around</param>
        /// <param name="angleInDegrees">Angle of rotation in degrees</param>
        /// <returns>Rotated point</returns>
        public static Vector2 RotatePoint(this Vector2 pointToRotate, Vector2 centerPoint, float angleInDegrees)
        {
            float angleInRadians = (float)(angleInDegrees * (System.Math.PI / 180f));
            float cosTheta = (float)System.Math.Cos(angleInRadians);
            float sinTheta = (float)System.Math.Sin(angleInRadians);
            return new Vector2
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
        /// <summary>
        /// Clamps a given angle
        /// </summary>
        /// <param name="qaAng">Angle to clamp</param>
        /// <returns>Clamped angle</returns>
        public static Vector3 ClampAngle(this Vector3 qaAng)
        {

            //if (qaAng.X > 89.0f && qaAng.X <= 180.0f)
            //    qaAng.X = 89.0f;

            //while (qaAng.X > 180.0f)
            //    qaAng.X = qaAng.X - 360.0f;

            //if (qaAng.X < -89.0f)
            //    qaAng.X = -89.0f;

            //while (qaAng.Y > 180.0f)
            //    qaAng.Y = qaAng.Y - 360.0f;

            //while (qaAng.Y < -180.0f)
            //    qaAng.Y = qaAng.Y + 360.0f;

            //return qaAng;
            if (qaAng[0] > 89.0f)
                qaAng[0] = 89.0f;

            if (qaAng[0] < -89.0f)
                qaAng[0] = -89.0f;

            while (qaAng[1] > 180)
                qaAng[1] -= 360;

            while (qaAng[1] < -180)
                qaAng[1] += 360;

            qaAng.Z = 0;

            return qaAng;
        }
        /// <summary>
        /// Calculates an angle that aims from the given source-Vector3 to the given destination-Vector3
        /// </summary>
        /// <param name="src">3d-coordinate of where to aim from</param>
        /// <param name="dst">3d-coordinate of where to aim to</param>
        /// <returns></returns>
        public static Vector3 CalcAngle(this Vector3 src, Vector3 dst)
        {
            Vector3 ret = new Vector3();
            Vector3 vDelta = src - dst;
            float fHyp = (float)System.Math.Sqrt((vDelta.X * vDelta.X) + (vDelta.Y * vDelta.Y));

            ret.X = RadiansToDegrees((float)System.Math.Atan(vDelta.Z / fHyp));
            ret.Y = RadiansToDegrees((float)System.Math.Atan(vDelta.Y / vDelta.X));

            if (vDelta.X >= 0.0f)
                ret.Y += 180.0f;
            return ret;
        }

        public static float GetRealDistance(this Vector3 diference, float distance)
        {
            var tempvector = new Vector2();
            tempvector.X = (float)System.Math.Sin(DegreesToRadians(diference.X)) * distance;
            tempvector.Y = (float)System.Math.Sin(DegreesToRadians(diference.Y)) * distance;
            return tempvector.Length();
        }
        /// <summary>
        /// Smooths an angle from src to dest
        /// </summary>
        /// <param name="src">Original angle</param>
        /// <param name="dest">Destination angle</param>
        /// <param name="smoothAmount">Value between 0 and 1 to apply as smooting where 0 is no modification and 1 is no smoothing</param>
        /// <returns></returns>
        public static Vector3 SmoothAngle(this Vector3 src, Vector3 dest, float smoothAmount)
        {
            return src + (dest - src) * (smoothAmount / 100);
        }
        public static Vector3 SmoothAngle(this Vector3 src, float smoothAmount)
        {
            return src * (smoothAmount / 100);
        }
        /// <summary>
        /// Converts the given angle in degrees to radians
        /// </summary>
        /// <param name="deg">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static float DegreesToRadians(float deg) { return (float)(deg * DEG_2_RAD); }
        /// <summary>
        /// Converts the given angle in radians to degrees
        /// </summary>
        /// <param name="rad">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static float RadiansToDegrees(float rad) { return (float)(rad * RAD_2_DEG); }
        /// <summary>
        /// Returns whether the given point is within a circle of the given radius around the given center
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <param name="circleCenter">Center of circle</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns></returns>
        public static bool PointInCircle(this Vector2 point, Vector2 circleCenter, float radius)
        {
            return (point - circleCenter).Length() < radius;
        }

        public static void SinCos(float rads, out float sin, out float cos)
        {
            sin = (float)System.Math.Sin(rads);
            cos = (float)System.Math.Cos(rads);
        }
        public static void TransformAABB(Matrix transform, Vector3 vecMinsIn, Vector3 vecMaxsIn, out Vector3 vecMinsOut, out Vector3 vecMaxsOut)
        {
            Vector3 localCenter = vecMinsIn + vecMaxsIn;
            localCenter *= 0.5f;

            Vector3 localExtents = vecMaxsIn - vecMinsIn;
            localExtents *= 0.5f;

            Vector3 worldCenter = VectorTransform(localCenter, transform);

            Vector3 worldExtents = new Vector3();
            worldExtents.X = System.Math.Abs(DotProduct(localExtents, transform, 0));
            worldExtents.Y = System.Math.Abs(DotProduct(localExtents, transform, 1));
            worldExtents.Z = System.Math.Abs(DotProduct(localExtents, transform, 2));

            vecMinsOut = worldCenter - worldExtents;
            vecMaxsOut = worldCenter + worldExtents;
        }

        public static float DotProduct(Vector3 in1, Matrix result, int row)
        {
            var matrixVector = new Vector3(result[row, 0], result[row, 1], result[row, 2]);
            return in1.dot(matrixVector);
        }

        public static Vector3 VectorRotate(Vector3 in1, Matrix qanglematrix)
        {
            var newvec = new Vector3();
            newvec[0] = DotProduct(in1, qanglematrix, 0);
            newvec[1] = DotProduct(in1, qanglematrix, 1);
            newvec[2] = DotProduct(in1, qanglematrix, 2);
            return newvec;
        }

        public static Vector3 VectorTransform(Vector3 in1, Matrix in2)
        {
            var result = new Vector3
            {
                [0] = DotProduct(in1, in2, 0) + in2[0, 3],
                [1] = DotProduct(in1, in2, 1) + in2[1, 3],
                [2] = DotProduct(in1, in2, 2) + in2[2, 3]
            };
            return result;
        }

        public static Matrix SetupTranslation(this Matrix m, Vector3 origin)
        {
            m[0, 3] = origin.X;
            m[1, 3] = origin.Y;
            m[2, 3] = origin.Z;
            return m;
        }

        public static void Vector3DMultiply(Matrix src1, Vector3 src2, ref Vector3 dst)
        {
            dst[0] = src1[0, 0] * src2[0] + src1[0, 1] * src2[1] + src1[0, 2] * src2[2];
            dst[1] = src1[1, 0] * src2[0] + src1[1, 1] * src2[1] + src1[1, 2] * src2[2];
            dst[2] = src1[2, 0] * src2[0] + src1[2, 1] * src2[1] + src1[2, 2] * src2[2];
        }

        #endregion

        public static float SmartSmoothCircle(float x, float additional)
        {
            return (float)System.Math.Sqrt((-System.Math.Pow(x, 2) + x)) + additional;
        }

        

        public static Vector3 CubicInterpolate(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
        {
            return p1 * (1 - t) * (1 - t) * (1 - t) +
            p2 * 3 * t * (1 - t) * (1 - t) +
            p3 * 3 * t * t * (1 - t) +
            p4 * t * t * t;
        }

        public  static Vector3 AngleVectors(this Vector3 angles)
        {
            Vector3 forward = new Vector3();
            float sp, sy, cp, cy;
            SinCos(DEG_2_RAD*(angles.X), out sp, out cp);
            SinCos(DEG_2_RAD * (angles.Y), out sy, out cy);
		        forward.X = cp* cy;
                forward.Y = cp* sy;
                forward.Z = -sp;
            return forward;
        }
    }
}
