using UnityEngine;

namespace HongPhysics
{
    public struct BoundingSphere : IBoundVolume
    {
        private Vector3 center;
        private float radius;

        public BoundingSphere(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public BoundingSphere(BoundingSphere one, BoundingSphere two)
        {
            Vector3 centerOffset = two.center - one.center;
            float distance = centerOffset.sqrMagnitude;
            float radiusDiff = two.radius - one.radius;

            //A 구가 B 구안에 정확히 포함되어 있을때
            if (radiusDiff * radiusDiff >= distance)
            {
                if (one.radius > two.radius)
                {
                    center = one.center;
                    radius = one.radius;
                }
                else
                {
                    center = two.center;
                    radius = two.radius;
                }
            }
            // 아닐경우 두 구를 포함하는 새로운 구를 만든다
            else
            {
                distance = (float)System.Math.Sqrt(distance);
                radius = (distance + one.radius + two.radius) * 0.5f;

                center = one.center;
                if (distance > 0)
                {
                    center += centerOffset * ((radius - one.radius) / distance);
                }
            }
        }

        public bool Overlaps(IBoundVolume boundVolume)
        {
            BoundingSphere other = (BoundingSphere)boundVolume;
            float distance = (center - other.center).sqrMagnitude;
            return distance < (radius + other.radius) * (radius + other.radius);
        }

        public float GetGrowth(IBoundVolume other)
        {
            BoundingSphere newSphere = new BoundingSphere(this, (BoundingSphere)other);
            return (newSphere.radius * newSphere.radius) - (radius * radius);
        }

        public float GetSize()
        {
            return 1.333333f * (float)Mathf.PI * radius * radius * radius;
        }

    }
}
