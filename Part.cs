using MSCLoader;
using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
using System.Net.Mail;

namespace BSFDTestbed
{
    public class Part : MonoBehaviour
    {
        //Bolt related variables
        public GameObject boltParent; // GameObject, Child of Part, Parent of ALL BOLTS.
        public Bolt[] bolts;          // Array of bolts, define in Unity inspector
        public int tightness = 0;    // Current part tightness, calculated by UpdatePartTightness()
        public int MaxTightness;    // Part will not fall off if tightness = MaxTightness

        //Sound related variables
        AudioClip assembleSound; // Sound to be played on item attachment.
        AudioClip disassembleSound; // Sound to be played on item detachment.

        //part(self) related variables
        public bool isFitted; // Self explanatory
        public AudioSource soundSource; // Source of assembleSound and disassembleSound.
        public Collider partTrigger; // Trigger of part, used for collision test between attachmentTrigger.

        //part(thing you are attaching to) related variables
        public GameObject attachmentPoint; // GameObject, parent of Part upon attachment.
        public Collider attachmentTrigger; // Collider, Trigger, used for collision test between partTrigger.
        //References
        FsmBool GUIAssemble;
        FsmBool GUIDisassemble;
        Rigidbody rb;

        // Use this for initialization
        void Start()
        {
            //TODO: add sound init
            GUIAssemble = Interaction.GUIAssemble;
            GUIDisassemble = Interaction.GUIDisassemble;
            rb = gameObject.GetComponent<Rigidbody>();
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

        void OnTriggerStay(Collider other)
        {
            if (!isFitted && other == attachmentTrigger)
            {
                GUIAssemble.Value = true;
                if (Input.GetMouseButtonDown(0))
                {
                    Attach();
                    GUIAssemble.Value = false;
                }
            }
        }

        void Attach()
        {
            isFitted = true;
            //TODO: play attachment sound code
            partTrigger.enabled = false;
            attachmentTrigger.enabled = false;
            rb.isKinematic = true;
            gameObject.tag = "Untagged";
            gameObject.transform.SetParent(attachmentPoint.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            StartCoroutine(FixParent(attachmentPoint.transform));
            boltParent.SetActive(true);
        }

        void Detach()
        {
            isFitted = false;
            //TODO: play detachment sound code
            gameObject.tag = "PART";
            gameObject.transform.SetParent(transform.root);
            attachmentTrigger.enabled = true;
            rb.isKinematic = false;
            boltParent.SetActive(false);
            //TODO: ResetBoltStatus();
        }

        void UntightenAllBolts()
        {
            //TODO: untightening event 

        }
    }
}