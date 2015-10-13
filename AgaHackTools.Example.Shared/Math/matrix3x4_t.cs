using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared.Math
{
    public struct matrix3x4_t
    {
        public float[][] m_flMatVal;
        //public static matrix3x4_t() { }

        public matrix3x4_t(int row, int col)
        {
            m_flMatVal = new float[3][];
            m_flMatVal[0] = new float[4];
            m_flMatVal[2] = new float[4];
            m_flMatVal[3] = new float[4];
        }
        public matrix3x4_t(bool init = true)
        {
            m_flMatVal = new float[3][];
            m_flMatVal[0] = new float[4];
            m_flMatVal[2] = new float[4];
            m_flMatVal[3] = new float[4];
        }
        public static matrix3x4_t matrix3x4(
            float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23)
        {
            var matrix= new matrix3x4_t();
            matrix.m_flMatVal[0][0] = m00; matrix.m_flMatVal[0][1] = m01; matrix.m_flMatVal[0][2] = m02; matrix.m_flMatVal[0][3] = m03;
            matrix.m_flMatVal[1][0] = m10; matrix.m_flMatVal[1][1] = m11; matrix.m_flMatVal[1][2] = m12; matrix.m_flMatVal[1][3] = m13;
            matrix.m_flMatVal[2][0] = m20; matrix.m_flMatVal[2][1] = m21; matrix.m_flMatVal[2][2] = m22; matrix.m_flMatVal[2][3] = m23;
            return matrix;
        }
    //-----------------------------------------------------------------------------
    // Creates a matrix where the X axis = forward
    // the Y axis = left, and the Z axis = up
    //-----------------------------------------------------------------------------
    public void Init(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, Vector3 vecOrigin )
    {
        m_flMatVal[0][0] = xAxis.X; m_flMatVal[0][1] = yAxis.X; m_flMatVal[0][2] = zAxis.X; m_flMatVal[0][3] = vecOrigin.X;
        m_flMatVal[1][0] = xAxis.Y; m_flMatVal[1][1] = yAxis.Y; m_flMatVal[1][2] = zAxis.Y; m_flMatVal[1][3] = vecOrigin.Y;
        m_flMatVal[2][0] = xAxis.Z; m_flMatVal[2][1] = yAxis.Z; m_flMatVal[2][2] = zAxis.Z; m_flMatVal[2][3] = vecOrigin.Z;
    }
    public static matrix3x4_t matrix3x4(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis, Vector3 vecOrigin )
    {
            var matrix = new matrix3x4_t();
        matrix.m_flMatVal[0][0] = xAxis.X; matrix.m_flMatVal[0][1] = yAxis.X; matrix.m_flMatVal[0][2] = zAxis.X; matrix.m_flMatVal[0][3] = vecOrigin.X;
        matrix.m_flMatVal[1][0] = xAxis.Y; matrix.m_flMatVal[1][1] = yAxis.Y; matrix.m_flMatVal[1][2] = zAxis.Y; matrix.m_flMatVal[1][3] = vecOrigin.Y;
        matrix.m_flMatVal[2][0] = xAxis.Z; matrix.m_flMatVal[2][1] = yAxis.Z; matrix.m_flMatVal[2][2] = zAxis.Z; matrix.m_flMatVal[2][3] = vecOrigin.Z;
        return matrix;
    }
    //    __inline void dump(const char* name)
    //{
    //    printf("%s: \n", name);
    //    printf("\t%f\t%f\t%f\t%f\n", m_flMatVal[0][0], m_flMatVal[0][1], m_flMatVal[0][2], m_flMatVal[0][3]);
    //    printf("\t%f\t%f\t%f\t%f\n", m_flMatVal[1][0], m_flMatVal[1][1], m_flMatVal[1][2], m_flMatVal[1][3]);
    //    printf("\t%f\t%f\t%f\t%f\n", m_flMatVal[2][0], m_flMatVal[2][1], m_flMatVal[2][2], m_flMatVal[2][3]);
    //}



    //-----------------------------------------------------------------------------
    // Creates a matrix where the X axis = forward
    // the Y axis = left, and the Z axis = up
    //-----------------------------------------------------------------------------
    //matrix3x4_t(



    //    const Vector& xAxis, const Vector& yAxis, const Vector& zAxis, const Vector &vecOrigin )
    //{
    //    Init(xAxis, yAxis, zAxis, vecOrigin);
    //}

    public void SetOrigin(Vector3  p )
    {
        m_flMatVal[0][3] = p.X;
        m_flMatVal[1][3] = p.Y;
        m_flMatVal[2][3] = p.Z;
    }

    public void Invalidate()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                m_flMatVal[i][j] = (uint)0x7FC00000;
            }
        }
    }

    //    float* operator[]( int i )				{ Assert(( i >= 0 ) && ( i< 3 )); return m_flMatVal[i]; }
    //const float*operator[] (int i) const  { Assert(( i >= 0 ) && ( i< 3 )); return m_flMatVal[i]; }
    //	float* Base() { return &m_flMatVal[0][0]; }
    //const float* Base() const				{ return &m_flMatVal[0][0]; }


    }
}
