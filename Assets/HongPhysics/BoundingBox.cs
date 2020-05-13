using UnityEngine;

namespace HongPhysics
{
    //BVH는 그냥 공간분할이다. 잠재적으로 접촉이 가능한 오브젝트들을 묶어놓고 그 트리를 탐색하면서 충돌 검사를 하기 위해 분할 하는것
    //접촉 불가능한 오브젝트들을 검사할 필요가 없으니까.
    [System.Serializable]
    public class BoundingBox : IBoundVolume
    {
        public Vector3 center;
        public Vector3 halfSize;

        public BoundingBox()
        {

        }

        public BoundingBox(Vector3 center, Vector3 halfSize)
        {
            this.center = center;
            this.halfSize = halfSize;
        }

        public BoundingBox(BoundingBox one, BoundingBox two)
        {
            Vector3 aLower = one.GetLower();
            Vector3 aUpper = one.GetUpper();

            Vector3 bLower = two.GetLower();
            Vector3 bUpper = two.GetUpper();

            Vector3 lower = new Vector3(Mathf.Min(aLower.x, bLower.x), Mathf.Min(aLower.y, bLower.y), Mathf.Min(aLower.z, bLower.z));
            Vector3 upper = new Vector3(Mathf.Max(aUpper.x, bUpper.x), Mathf.Max(aUpper.y, bUpper.y), Mathf.Max(aUpper.z, bUpper.z));


            halfSize = (upper - lower) * 0.5f;
            center = lower + halfSize;
        }

        public bool Overlaps(IBoundVolume other)
        {
            BoundingBox box = (BoundingBox)other;

            Vector3 aLower = GetLower();
            Vector3 aUpper = GetUpper();

            Vector3 bLower = box.GetLower();
            Vector3 bUpper = box.GetUpper();

            return aLower.x <= bUpper.x && aLower.y <= bUpper.y && aLower.z <= bUpper.z &&
                   aUpper.x >= bLower.x && aUpper.y >= bLower.y && aUpper.z >= bLower.z;
        }

        public float GetGrowth(IBoundVolume other)
        {
            BoundingBox newAABB = new BoundingBox(this, (BoundingBox)other);
            return newAABB.GetPerimeter() - GetPerimeter();
        }

        public float GetPerimeter()
        {
            Vector3 size = halfSize * 2;
            return 4 * (size.x + size.y + size.z);
        }

        public float GetSize()
        {
            Vector3 size = halfSize * 2;
            return Mathf.Abs(size.x * size.y * size.z);
        }

        public Vector3 GetLower()
        {
            return center - halfSize;
        }

        public Vector3 GetUpper()
        {
            return center + halfSize;
        }

        public bool Contain(BoundingBox aabb)
        {
            Vector3 aLower = GetLower();
            Vector3 aUpper = GetUpper();
            Vector3 bLower = aabb.GetLower();
            Vector3 bUpper = aabb.GetUpper();
            return (aUpper.x >= bUpper.x && aUpper.y >= bUpper.y && aUpper.z >= bUpper.z &&
                aLower.x <= bLower.x && aLower.y <= bLower.y && aLower.z <= bLower.z);
        }

        public bool FatAABBContain(BoundingBox aabb, float fat)
        {
            Vector3 aLower = GetLower() * fat;
            Vector3 aUpper = GetUpper() * fat;
            Vector3 bLower = aabb.GetLower();
            Vector3 bUpper = aabb.GetUpper();

            return (aUpper.x >= bUpper.x && aUpper.y >= bUpper.y && aUpper.z >= bUpper.z &&
                aLower.x <= bLower.x && aLower.y <= bLower.y && aLower.z <= bLower.z);
        }
    }
}