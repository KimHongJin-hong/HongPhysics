using HongPhysics;
using UnityEngine;

public class Test : MonoBehaviour
{
    private BVHNode root;
    public TestBox rootBox;
    public TestBox addBox;
    public TestBox[] testBoxes;
    private void Awake()
    {
        root = new BVHNode(null, rootBox.aabb, rootBox.hRigidbody);
        foreach(TestBox box in testBoxes)
        {
            root.Insert(box.hRigidbody, box.aabb);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            root.Insert(addBox.hRigidbody, addBox.aabb);
        root.Update(1.25f);
    }

    private void OnDrawGizmos()
    {
        if(root != null)
        {
            root.DrawGizmo();
        }
    }
}
