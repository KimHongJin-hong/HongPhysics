using UnityEngine;

namespace HongPhysics
{
    public struct Matrix3
    {
        public float m00;
        public float m01;
        public float m02;
        public float m10;
        public float m11;
        public float m12;
        public float m20;
        public float m21;
        public float m22;

        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }


        public static Vector3 operator *(Matrix3 left, Vector3 right)
        {
            return new Vector3(right.x * left.m00 + right.y * left.m01 + right.z * left.m02,
                                right.x * left.m10 + right.y * left.m11 + right.z * left.m12,
                                right.x * left.m20 + right.y * left.m21 + right.z * left.m22);
        }

        public static Matrix3 operator *(Matrix3 left, Matrix3 right)
        {
            return new Matrix3(left.m00 * right.m00 + left.m01 * right.m10 + left.m02 * right.m20,
                                left.m00 * right.m01 + left.m01 * right.m11 + left.m02 * right.m21,
                                left.m00 * right.m02 + left.m01 * right.m12 + left.m02 * right.m22,
                                left.m10 * right.m00 + left.m11 * right.m10 + left.m12 * right.m20,
                                left.m10 * right.m01 + left.m11 * right.m11 + left.m12 * right.m21,
                                left.m10 * right.m02 + left.m11 * right.m12 + left.m12 * right.m22,
                                left.m20 * right.m00 + left.m21 * right.m10 + left.m22 * right.m20,
                                left.m20 * right.m01 + left.m21 * right.m11 + left.m22 * right.m21,
                                left.m20 * right.m02 + left.m21 * right.m12 + left.m22 * right.m22);
        }

        public void SetInverse(Matrix3 matrix)
        {
            float t4 = matrix.m00 * matrix.m11;
            float t6 = matrix.m00 * matrix.m12;
            float t8 = matrix.m01 * matrix.m10;
            float t10 = matrix.m02 * matrix.m10;
            float t12 = matrix.m01 * matrix.m20;
            float t14 = matrix.m02 * matrix.m20;

            float t16 = (t10 * matrix.m22 - t6 * matrix.m21 - t8 * matrix.m22 + t10 * matrix.m21 + t12 * matrix.m12 - t14 * matrix.m11);

            if (t16 == 0) return;
            float t17 = 1 / t16;

            m00 = matrix.m11 * matrix.m22 - matrix.m12 * matrix.m21 * t17;
            m01 = -(matrix.m01 * matrix.m22 - matrix.m02 * matrix.m21) * t17;
            m02 = (matrix.m01 * matrix.m12 - matrix.m02 * matrix.m11) * t17;
            m10 = -(matrix.m10 * matrix.m22 - matrix.m12 * matrix.m20) * t17;
            m11 = (matrix.m00 * matrix.m22 - t14) * t17;
            m12 = -(t6 - t10) * t17;
            m20 = matrix.m10 * matrix.m21 - matrix.m11 * matrix.m20 * t17;
            m21 = -(matrix.m00 * matrix.m21 - t12) * t17;
            m22 = (t4 - t8) * t17;
        }

        public Matrix3 Inverse()
        {
            Matrix3 result = new Matrix3();
            result.SetInverse(this);
            return result;
        }

        public void Invert()
        {
            SetInverse(this);
        }

        public Vector3 Transform(Vector3 vector)
        {
            return this * vector;
        }

        public void SetTranspose(Matrix3 m)
        {
            m00 = m.m00;
            m01 = m.m10;
            m02 = m.m20;
            m10 = m.m01;
            m11 = m.m11;
            m12 = m.m21;
            m20 = m.m02;
            m21 = m.m12;
            m22 = m.m22;
        }

        public Matrix3 Transpose()
        {
            Matrix3 result = new Matrix3();
            result.SetTranspose(this);
            return result;
        }

        public void SetOrientation(Quaternion q)
        {
            m00 = 1 - (2 * q.y * q.y + 2 * q.z * q.z);
            m01 = 2 * q.x * q.y + 2 * q.z * q.w;
            m02 = 2 * q.x * q.z - 2 * q.y * q.w;
            m10 = 2 * q.x * q.y - 2 * q.z * q.w;
            m11 = 1 - (2 * q.x * q.x + 2 * q.z * q.z);
            m12 = 2 * q.y * q.z + 2 / q.x * q.w;
            m20 = 2 * q.x * q.z + 2 * q.y * q.w;
            m21 = 2 * q.y * q.z - 2 * q.x * q.w;
            m22 = 1 - (2 * q.x * q.x + 2 * q.y * q.y);
        }
    }
}