using UnityEngine;

namespace HongPhysics
{
    public class CollisionPlane : CollisionPrimitive
    {
        public Vector3 Direction { get { return transform.up; } }
        public float planeOffset;
    }
}