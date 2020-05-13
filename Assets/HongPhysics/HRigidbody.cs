using UnityEngine;

namespace HongPhysics
{
    public class HRigidbody : MonoBehaviour
    {
        public bool useGravity = false;
        public float mass;
        public Matrix3 inverseInertiaTensor;
        private float inverseMass;
        private Vector3 velocity;
        private Vector3 angularVelocity;

        private Vector3 position;
        private Quaternion rotation;

        //한 프레임에서 작용한 힘들을 모아놓는 것
        private Vector3 forceAccumulate;
        private Vector3 torqueAccumulate;
        private Vector3 lastFrameAcceleration;
        private Vector3 acceleration;
        private Matrix3 inverseInertiaTensorWorld;
        // Damping : 
        private float linearDamping;
        private float angularDamping;

        private void Awake()
        {
            inverseMass = 1 / mass;
            
            position = transform.position;
            rotation = transform.rotation;
        }

        private void CalculateDerivedData()
        {
            rotation.Normalize();
            transform.SetPositionAndRotation(position, rotation);
            TransformInertiaTensor(inverseInertiaTensor, transform.worldToLocalMatrix);
        }

        public void Integrate(float duration)
        {
            lastFrameAcceleration = acceleration;
            lastFrameAcceleration += (forceAccumulate * inverseMass);

            Vector3 angularAcceleration = inverseInertiaTensorWorld.Transform(torqueAccumulate);

            velocity += lastFrameAcceleration * duration;
            angularVelocity += angularAcceleration * duration;

            position += velocity * duration;
            rotation *= Quaternion.Euler(angularVelocity * duration);

            CalculateDerivedData();
            ClearAccumulate();
        }

        public void AddForce(Vector3 force)
        {
            forceAccumulate += force;
        }

        public void AddForceAtPoint(Vector3 force, Vector3 point)
        {
            Vector3 pt = point;
            pt -= position;
            forceAccumulate += force;
            torqueAccumulate += Vector3.Cross(pt, force);
        }

        private void TransformInertiaTensor(Matrix3 iitBody, Matrix4x4 rotmat)
        {
            float t4 = rotmat.m00 * iitBody.m00 + rotmat.m01 * iitBody.m10 + rotmat.m02 * iitBody.m20;
            float t9 = rotmat.m00 * iitBody.m01 + rotmat.m01 * iitBody.m11 + rotmat.m02 * iitBody.m21;
            float t14 = rotmat.m00 * iitBody.m02 + rotmat.m01 * iitBody.m12 + rotmat.m02 * iitBody.m22;
            float t28 = rotmat.m10 * iitBody.m00 + rotmat.m11 * iitBody.m10 + rotmat.m12 * iitBody.m20;
            float t33 = rotmat.m10 * iitBody.m01 + rotmat.m11 * iitBody.m11 + rotmat.m12 * iitBody.m21;
            float t38 = rotmat.m10 * iitBody.m02 + rotmat.m11 * iitBody.m12 + rotmat.m12 * iitBody.m22;
            float t52 = rotmat.m20 * iitBody.m00 + rotmat.m21 * iitBody.m10 + rotmat.m22 * iitBody.m20;
            float t57 = rotmat.m20 * iitBody.m01 + rotmat.m21 * iitBody.m11 + rotmat.m22 * iitBody.m21;
            float t62 = rotmat.m20 * iitBody.m02 + rotmat.m21 * iitBody.m12 + rotmat.m22 * iitBody.m22;

            inverseInertiaTensorWorld.m00 = t4 * rotmat.m00 + t9 * rotmat.m01 + t14 * rotmat.m02;
            inverseInertiaTensorWorld.m01 = t4 * rotmat.m10 + t9 * rotmat.m11 + t14 * rotmat.m12;
            inverseInertiaTensorWorld.m02 = t4 * rotmat.m20 + t9 * rotmat.m21 + t14 * rotmat.m22;
            inverseInertiaTensorWorld.m10 = t28 * rotmat.m00 + t33 * rotmat.m01 + t38 * rotmat.m02;
            inverseInertiaTensorWorld.m11 = t28 * rotmat.m10 + t33 * rotmat.m11 + t38 * rotmat.m12;
            inverseInertiaTensorWorld.m12 = t28 * rotmat.m20 + t33 * rotmat.m21 + t38 * rotmat.m22;
            inverseInertiaTensorWorld.m20 = t52 * rotmat.m00 + t57 * rotmat.m01 + t62 * rotmat.m02;
            inverseInertiaTensorWorld.m21 = t52 * rotmat.m10 + t57 * rotmat.m11 + t62 * rotmat.m12;
            inverseInertiaTensorWorld.m22 = t52 * rotmat.m20 + t57 * rotmat.m21 + t62 * rotmat.m22;
        }

        private void ClearAccumulate()
        {
            forceAccumulate = Vector3.zero;
            torqueAccumulate = Vector3.zero;
        }
    }
}