using MSCLoader;
using UnityEngine;
using System.Collections;

namespace BSFDTestbed
{
    public class Part : MonoBehaviour
    {
        //public GameObject[] bolts;  // Array of bolts, define in Unity inspector.
        public Bolt[] bolts;
        public int tightness = 0;  // Current part tightness, calculated by UpdatePartTightness().
        public int MinTightness;  // If Tightness < MinTightness, part can be taken off by hand, recommended value for MinTightnes is ~60% of MaxTightness.
        public int MaxTightness; // Part will not fall off if tightness = MaxTightness
        public bool isFitted; // Self explanatory.
        public GameObject pivotPoint;
        AudioClip assembleSound;
        AudioClip disassembleSound;
        public Collider pivotCollider;
        ConfigurableJoint configJoint;
        public GameObject boltsParent;
        public AudioSource soundSource;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(UpdatePartTightness());
        }

        IEnumerator UpdatePartTightness()
        {
            while (true)
            {
                int _tightness = 0;
                foreach (var b in bolts) _tightness += b.currentBoltStep;
                tightness = _tightness;
                yield return new WaitForSeconds(3f);
            }
        }

        public void Attach(bool playSound = true)
        {
            isFitted = true;
            if (playSound) PlaySound(assembleSound);

            pivotCollider.enabled = false;
            gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;

            gameObject.tag = "Untagged";
            gameObject.transform.parent = pivotPoint.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            StartCoroutine(FixParent(pivotPoint.transform));

            configJoint = gameObject.AddComponent<ConfigurableJoint>();
            configJoint.xMotion = ConfigurableJointMotion.Limited;
            configJoint.yMotion = ConfigurableJointMotion.Limited;
            configJoint.zMotion = ConfigurableJointMotion.Limited;
            configJoint.angularXMotion = ConfigurableJointMotion.Limited;
            configJoint.angularYMotion = ConfigurableJointMotion.Limited;
            configJoint.angularZMotion = ConfigurableJointMotion.Limited;
            configJoint.connectedBody = pivotPoint.transform.parent.gameObject.GetComponent<Rigidbody>();
            configJoint.breakForce = 100;
            configJoint.breakTorque = 50;

            boltsParent.SetActive(true);
            
        }

        IEnumerator FixParent(Transform parent)
        {
            yield return new WaitForEndOfFrame();
            while (transform.parent != parent)
            {
                transform.parent = parent;
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
                yield return new WaitForEndOfFrame();
            }
        }

        public void Detach()
        {
            isFitted = false;
            PlaySound(disassembleSound);

            gameObject.tag = "PART";
            gameObject.transform.SetParent(transform.root);
            pivotPoint.GetComponent<Collider>().enabled = true;
            gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

            foreach (var b in bolts)
            {
                b.currentBoltStep = 0;
            }

            boltsParent.SetActive(false);
            
        }

        void PlaySound(AudioClip clip, float pitch = 1f)
        {
            if (!soundSource.isPlaying)
            {
                soundSource.pitch = pitch;
                soundSource.PlayOneShot(clip);
            }
        }
        // Update is called once per frame
        void Update()
        { 
            
        }
    }
}