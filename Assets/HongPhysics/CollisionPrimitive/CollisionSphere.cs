using UnityEngine;

namespace HongPhysics
{
    public class CollisionSphere : CollisionPrimitive
    {
        [SerializeField] float radius;

        public float Radius { get { return radius * transform.localScale.x; } }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}