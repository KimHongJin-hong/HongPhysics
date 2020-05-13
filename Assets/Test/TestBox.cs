using HongPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : MonoBehaviour
{
    public BoundingBox aabb = new BoundingBox();
    public HRigidbody hRigidbody;

    private void OnDrawGizmos()
    {
        aabb.center = transform.position;
        aabb.halfSize = transform.localScale * 0.5f;
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawWireCube(aabb.center, aabb.halfSize * 2);
    }
}
