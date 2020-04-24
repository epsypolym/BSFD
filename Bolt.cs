using MSCLoader;
using UnityEngine;

namespace BSFDTestbed
{
    public class Bolt : MonoBehaviour
    {
        public int insAmount;
        public int currentBoltStep;
        public int maxBoltSteps = 8;
        public int boltSize;
        public float boltMoveAmount;

        public bool boltable;
        public bool unboltable;

        Interaction boltInteraction;
        bool GUIuse;
        Collider selfCollider;

        // Use this for initialization
        void Start()
        {
            boltInteraction = BSFDTestbed.boltInteraction;
            GUIuse = BSFDTestbed.GUIuse;
            selfCollider = GetComponent<Collider>();
        }

        void BoltEvent(int amount)
        {
            currentBoltStep += amount;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 45, 0));
            transform.position += new Vector3(0, boltMoveAmount, 0);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}