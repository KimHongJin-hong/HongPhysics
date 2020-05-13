using UnityEngine;

namespace HongPhysics
{
    public class CollisionBox : CollisionPrimitive
    {
        public Vector3 HalfSize { get {
                return transform.localScale * 0.5f;
            } }

        public float mass = 1;

        protected override void Awake()
        {
            base.Awake();
            Vector3 squares = new Vector3(HalfSize.x * HalfSize.x, HalfSize.y * HalfSize.y, HalfSize.z * HalfSize.z);
            hRigidbody.mass = mass * HalfSize.x * HalfSize.y * HalfSize.z;
            hRigidbody.inverseInertiaTensor.m00 = 0.3f * hRigidbody.mass * ((squares.y * squares.y) + (squares.z * squares.z));
            hRigidbody.inverseInertiaTensor.m11 = 0.3f * hRigidbody.mass * ((squares.x * squares.x) + (squares.z * squares.z));
            hRigidbody.inverseInertiaTensor.m22 = 0.3f * hRigidbody.mass * ((squares.x * squares.x) + (squares.y * squares.y));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position, HalfSize * 2);
        }
    }
}
