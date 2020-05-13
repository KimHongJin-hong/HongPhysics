using UnityEngine;

namespace HongPhysics
{
    public class Solver : MonoBehaviour
    {
        public Vector3 gravity;
        public HRigidbody[] hRigidbodies;
        public Vector3 force;
        public Vector3 point;
        //public CollisionPrimitive[] collisionPrimitives;
        public CollisionSphere testBall;
        public CollisionBox[] testBoxes;

        public CollisionData testData;

        private void Awake()
        {
            testData = new CollisionData(3);
            testData.friction = 0.9f;
            testData.restitution = 0.1f;
            testData.tolerance = 0.1f;
        }

        private void FixedUpdate()
        {
            foreach (var item in hRigidbodies)
            {
                if (item.useGravity)
                {
                    item.AddForce(gravity);
                }
                item.Integrate(Time.deltaTime);
            }

            foreach (var item in testBoxes)
            {
                if (CollisionDetector.BoxAndSphere(item, testBall, testData))
                {
                    var contact = testData.contacts[0];
                    contact.hRigidbodies[0].AddForceAtPoint(contact.contactNormal * 100, contact.contactPoint);
                    contact.hRigidbodies[1].AddForceAtPoint(-contact.contactNormal * 100, contact.contactPoint);
                    testData.Reset(1);
                }
            }

            //for (int i = 0; i < testBoxes.Length; i++)
            //{
            //    CollisionDetector.BoxAndSphere
            //}
        }
    }
}