using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared.Math
{
    public struct Vector3
    {
        #region VARIABLES

        public float X;
        public float Y;
        public float Z;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Returns a new Vector3 at (0,0,0)
        /// </summary>
        public static Vector3 Zero
        {
            get { return new Vector3(0, 0, 0); }
        }
        /// <summary>
        /// Returns a new Vector3 at (1,0,0)
        /// </summary>
        public static Vector3 UnitX
        {
            get { return new Vector3(1, 0, 0); }
        }
        /// <summary>
        /// Returns a new Vector3 at (0,1,0)
        /// </summary>
        public static Vector3 UnitY
        {
            get { return new Vector3(0, 1, 0); }
        }
        /// <summary>
        /// Returns a new Vector3 at (0,0,1)
        /// </summary>
        public static Vector3 UnitZ
        {
            get { return new Vector3(0, 0, 1); }
        }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new Vector3 using the given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        /// <summary>
        /// Initializes a new Vector3 by copying the values of the given Vector3
        /// </summary>
        /// <param name="vec"></param>
        public Vector3(Vector3 vec) : this(vec.X, vec.Y, vec.Z) { }
        /// <summary>
        /// Initializes a new Vector3 using the given float-array
        /// </summary>
        /// <param name="values"></param>
        public Vector3(float[] values) : this(values[0], values[1], values[2]) { }
        #endregion

        #region METHODS
        /// <summary>
        /// Returns the length of this Vector3
        /// </summary>
        /// <returns></returns>
        public float Length()
        {
            return (float)System.Math.Abs(System.Math.Sqrt(System.Math.Pow(X, 2) + System.Math.Pow(Y, 2) + System.Math.Pow(Z, 2)));
        }
        public float LengthSqr()
        {
            return (float)(System.Math.Pow(X, 2) + System.Math.Pow(Y, 2) + System.Math.Pow(Z, 2));
        }
        /// <summary>
        /// Returns the distance from this Vector3 to the given Vector3
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float DistanceTo(Vector3 other)
        {
            return (this - other).Length();
        }

        public override bool Equals(object obj)
        {
            Vector3 vec = (Vector3)obj;
            return this.GetHashCode() == vec.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[X={0}, Y={1}, Z={2}]", this.X.ToString(), this.Y.ToString(), this.Z.ToString());
        }

        public float dot(Vector3 dot)
        {
            return (X * dot.X + Y * dot.Y + Z * dot.Z);
        }
        public VectorAligned Aligned()
        {
            return new VectorAligned {X = this.X,Y=this.Y,Z= this.Z,W=0};
        }
        #endregion METHODS

        //        public Matrix AngleMatrix()
        //        {
        //            var matrix = new Matrix(3, 4);
        //            float sr, sp, sy, cr, cp, cy;
        //            MathUtils.SinCos(MathUtils.DegreesToRadians(Y), out sy, out cy);
        //            MathUtils.SinCos(MathUtils.DegreesToRadians(X), out sp, out cp);
        //            MathUtils.SinCos(MathUtils.DegreesToRadians(Z), out sr, out cr);
        //
        //            // matrix = (YAW * PITCH) * ROLL
        //            matrix[0, 0] = cp * cy;
        //            matrix[1, 0] = cp * sy;
        //            matrix[2, 0] = -sp;
        //
        //            float crcy = cr * cy;
        //            float crsy = cr * sy;
        //            float srcy = sr * cy;
        //            float srsy = sr * sy;
        //            matrix[0, 1] = sp * srcy - crsy;
        //            matrix[1, 1] = sp * srsy + crcy;
        //            matrix[2, 1] = sr * cp;
        //
        //            matrix[0, 2] = (sp * crcy + srsy);
        //            matrix[1, 2] = (sp * crsy - srcy);
        //            matrix[2, 2] = cr * cp;
        //
        //            matrix[0, 3] = 0.0f;
        //            matrix[1, 3] = 0.0f;
        //            matrix[2, 3] = 0.0f;
        //            return matrix;
        //        }

        //        public Vector3 AbsVector3()
        //        {
        //            return new Vector3(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
        //        }
        //        #endregion

        #region OPERATORS
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }
        public static Vector3 operator *(Vector3 v1, float scalar)
        {
            return new Vector3(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);
        }
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }
        #endregion
    }
}
