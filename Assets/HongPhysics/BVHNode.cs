using UnityEngine;

namespace HongPhysics
{
    public class BVHNode
    {
        public BVHNode parent;
        public BVHNode left;
        public BVHNode right;

        public BoundingBox aabb;
        public HRigidbody body;

        public BVHNode(BVHNode parent, BoundingBox aabb, HRigidbody body)
        {
            this.parent = parent;
            this.aabb = aabb;
            this.body = body;
        }

        public bool IsLeaf()
        {
            return body != null;
        }

        public void Insert(HRigidbody newBody, BoundingBox newAABB)
        {
            if (IsLeaf())
            {
                left = new BVHNode(this, aabb, body);
                right = new BVHNode(this, newAABB, newBody);
                this.body = null;
                RecalculateAABB();
            }
            else
            {
                if (left.aabb.GetGrowth(newAABB) < right.aabb.GetGrowth(newAABB))
                    left.Insert(newBody, newAABB);
                else
                    right.Insert(newBody, newAABB);
            }
        }

        public void Remove()
        {
            if (parent != null)
            {
                BVHNode sibling = GetSibling();

                if (sibling != null)
                {
                    parent.aabb = sibling.aabb;
                    parent.body = sibling.body;
                    parent.left = sibling.left;
                    parent.right = sibling.right;

                    sibling.parent = null;
                    sibling.body = null;
                    sibling.left = null;
                    sibling.right = null;
                    sibling = null;
                    parent.RecalculateAABB();
                }


                if (left != null)
                {
                    left.parent = null;
                    left = null;
                }

                if (right != null)
                {
                    right.parent = null;
                    right = null;
                }
            }
        }

        public void RecalculateAABB()
        {
            if (IsLeaf()) return;

            aabb = new BoundingBox(left.aabb, right.aabb);

            if (parent != null) parent.RecalculateAABB();
        }

        public void DrawGizmo()
        {
            if (IsLeaf())
                return;

            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(aabb.center, aabb.halfSize * 2);

            if (left != null)
                left.DrawGizmo();
            if (right != null)
                right.DrawGizmo();
        }

        public BVHNode GetSibling()
        {
            return this == parent.left ? parent.right : parent.left;
        }

        public void Update(float fat)
        {
            if (IsLeaf())
            {
                if (!parent.aabb.FatAABBContain(aabb, fat))
                {
                    //TODO 지금 노드를 지우고 root에 새로 삽입한다.
                    Remove();
                    GetRoot().Insert(body, aabb);
                }
            }
            else
            {
                if (left != null)
                    left.Update(fat);
                if (right != null)
                    right.Update(fat);
            }
        }

        public BVHNode GetRoot()
        {
            if (parent != null)
                return parent.GetRoot();
            return this;
        }
    }
}