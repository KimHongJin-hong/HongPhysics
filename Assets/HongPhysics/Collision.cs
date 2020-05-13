using UnityEngine;

namespace HongPhysics
{
    public class Contact
    {
        public HRigidbody[] hRigidbodies = new HRigidbody[2];

        public float friction;
        public float restitution;
        public float penetration;
        public Vector3 contactPoint;
        public Vector3 contactNormal;

        public void SetBodyData(HRigidbody one, HRigidbody two, float friction, float restitution)
        {
            hRigidbodies[0] = one;
            hRigidbodies[1] = two;
            this.friction = friction;
            this.restitution = restitution;
        }
    }

    [System.Serializable]
    public class CollisionData
    {
        //public Contact[] contacts = new Contact[4]; //List ?
        //TODO 1. 접촉의 최대를 두어야 한다
        //     2. 접촉의 최대를 두었으면 인스턴스를 생성하고 가지고있는다 ( 접촉 할 때 마다 인스턴스 생성 방지 )
        public Contact[] contacts = null;
        public int currentIndex = 0;
        public int contactLeft = 0;
        public int contactCount = 0;
        public float friction;
        public float restitution;
        public float tolerance;

        public CollisionData(uint maxContacts)
        {
            Reset(maxContacts);
        }

        public bool HasMoreContacts()
        {
            return contacts.Length > 0;
        }

        public void Reset(uint maxContacts)
        {
            contacts = new Contact[(int)maxContacts];

            for(int i=0;i< maxContacts; i++)
                contacts[i] = new Contact();

            contactLeft = (int)maxContacts;
            contactCount = 0;
            currentIndex = 0;
        }

        public void AddContact()
        {
            contactLeft -= 1;
            contactCount += 1;
            currentIndex += 1;
        }

        public Contact GetCurrentContact()
        {
            return contacts[currentIndex];
        }
    }

    public class Collision
    {

    }

    public class CollisionDetector
    {
        public static bool BoxAndSphere(CollisionBox box, CollisionSphere sphere, CollisionData data)
        {
            Vector3 center = sphere.GetAxis(3); // sphere의 포지션을 추출
            Vector3 relCenter = box.GetTransform().InverseTransformPoint(center); // box local 기준의 좌표로 변경

            if (System.Math.Abs(relCenter.x) - sphere.Radius > box.HalfSize.x || // box의 space에서 각 축에 sphere center를 투영하여 거리를 계산합니다.
                System.Math.Abs(relCenter.y) - sphere.Radius > box.HalfSize.y || 
                System.Math.Abs(relCenter.z) - sphere.Radius > box.HalfSize.z)
            {
                return false;
            }

            Vector3 closestPt = new Vector3(); // 구와 제일 가까운 점을 찾는다 (큐브 안에서)
            float dist = relCenter.x;
            if (dist > box.HalfSize.x) dist = box.HalfSize.x;
            if (dist < -box.HalfSize.x) dist = -box.HalfSize.x;
            closestPt.x = dist;

            dist = relCenter.y;
            if (dist > box.HalfSize.y) dist = box.HalfSize.y;
            if (dist < -box.HalfSize.y) dist = -box.HalfSize.y;
            closestPt.y = dist;

            dist = relCenter.z;
            if (dist > box.HalfSize.z) dist = box.HalfSize.z;
            if (dist < -box.HalfSize.z) dist = -box.HalfSize.z;
            closestPt.z = dist;
            
            dist = (closestPt - relCenter).sqrMagnitude; 
            if (dist > sphere.Radius * sphere.Radius) return false;
            // 구와 제일 가까운 점과 구의 중심의 거리가 구의 반지름보다 작으면 당연하게 충돌이 일어나지 않는다.

            Vector3 closestPtWorld = box.GetTransform().TransformPoint(closestPt); 
            //Vector3 closestPtWorld = box.GetTransform().localToWorldMatrix * closestPt; 
            // 모든 충돌 테스트를 마친 closestPoint의 좌표계를 월드 좌표계로 바꾼다 ( 위에서 Box Local Coordinates를 사용했었음 )

            Contact contact = data.GetCurrentContact();
            contact.contactNormal = (closestPtWorld - center).normalized;
            contact.contactPoint = closestPtWorld;
            contact.penetration = sphere.Radius - (float)System.Math.Sqrt(dist);
            contact.SetBodyData(box.hRigidbody, sphere.hRigidbody, data.friction, data.restitution);

            data.AddContact();
            //TODO CollisionData 에서 Contacts 데이터를 받아와 시뮬레이션 한다.
            return true;
        }

        public static bool SphereAndHalfSpace(CollisionSphere sphere, CollisionPlane plane, CollisionData data)
        {
            if (data.contactLeft <= 0) return false;

            Vector3 position = sphere.GetAxis(3);

            float ballDistance = Vector3.Dot(plane.Direction, position) - sphere.Radius - plane.planeOffset;

            if (ballDistance >= 0) return false;

            Contact contact = data.GetCurrentContact();
            contact.contactNormal = plane.Direction;
            contact.penetration = -ballDistance;
            contact.contactPoint = position - plane.Direction * (ballDistance + sphere.Radius);
            contact.SetBodyData(sphere.hRigidbody, null, data.friction, data.restitution);

            data.AddContact();
            return true;
        }
    }
}