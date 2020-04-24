using MSCLoader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSFDTestbed
{
    public class Interaction : MonoBehaviour
    {
        public RaycastHit hitInfo;

        public bool hasHit = false;
        public float rayDistance = 1.35f;
        public int layerMask;
        public static float boltTimeDelay;

        // Use this for initialization
        void Start()
        {
            layerMask = LayerMask.GetMask("Bolts");
            hitInfo = new RaycastHit();
        }

        void FixedUpdate()
        {
            if (Camera.main != null) hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, rayDistance, layerMask);
        }

        public bool GetHit(Collider collider) => hasHit && hitInfo.collider == collider;
        public bool GetHitAny(Collider[] colliders) => hasHit && colliders.Any(collider => collider == hitInfo.collider);
        public bool GetHitAny(List<Collider> colliders) => hasHit && colliders.Any(collider => collider == hitInfo.collider);
    }
}