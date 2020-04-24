using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace BSFDTestbed
{
    public class Bolt : MonoBehaviour
    {
        public float insAmount;
        public int currentBoltStep;
        public int maxBoltSteps = 8;
        public float boltSize;
        public float boltMoveAmount;
        bool mouseOver = false;

        float boltTimeDelay;
        Interaction boltInteraction;
        bool GUIuse;
        Collider selfCollider;
        FsmFloat gameToolID;


        // Use this for initialization
        void Start()
        {
            boltInteraction = BSFDTestbed.boltInteraction;
            GUIuse = BSFDTestbed.GUIuse;
            selfCollider = GetComponent<Collider>();
            boltTimeDelay = Interaction.boltTimeDelay;
            gameToolID = BSFDTestbed.gameToolID;
        }

        void BoltEventDown()
        {
            currentBoltStep -= 1;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0, 0, 45));
            transform.position += new Vector3(0, boltMoveAmount, 0);
        }

        void BoltEventUp()
        {
            currentBoltStep += 1;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 45));
            transform.position += new Vector3(0, -boltMoveAmount, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (boltInteraction.GetHit(selfCollider) & gameToolID.Value > 0f & boltSize == gameToolID.Value)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.time >= boltTimeDelay && currentBoltStep < maxBoltSteps) // Scroll Up
                {
                    BoltEventUp();
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.time >= boltTimeDelay && currentBoltStep > 0) // Scroll Down
                {
                    BoltEventDown();
                }
            }
        }
    }
}