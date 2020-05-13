using UnityEngine;

namespace HongPhysics
{
    public class CollisionPrimitive : MonoBehaviour
    {
        public HRigidbody hRigidbody;
        public Matrix4x4 offset;
        //protected Matrix4x4 transform;

        //public void CalculateInternals()
        //{
        //    transform = hRigidbody.transform.worldToLocalMatrix * offset;
        //}

        //public Vector3 GetAxis(int index)
        //{
        //    return transform.GetColumn(index);
        //}

        //public Matrix4x4 GetTransform()
        //{
        //    return transform;
        //}
        protected virtual void Awake()
        {
            hRigidbody = GetComponent<HRigidbody>();
        }

        public Vector3 GetAxis(int index)
        {
            return transform.localToWorldMatrix.GetColumn(index);
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}