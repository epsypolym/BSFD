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

        //part(self) related variables
        public bool isFitted; // Self explanatory
        public AudioSource soundSource; // Source of assembleSound and disassembleSound.
        public Collider partTrigger; // Trigger of part, used for collision test between attachmentTrigger.

        //part(thing you are attaching to) related variables
        public GameObject attachmentPoint; // GameObject, parent of Part upon attachment.
        public Collider attachmentTrigger; // Collider, Trigger, used for collision test between partTrigger.

        Rigidbody rb;

        // Use this for initialization
        void Start()
        {
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
                Interaction.GUIAssemble.Value = true;
                if (Input.GetMouseButtonDown(0))
                {
                    Attach();
                    Interaction.GUIAssemble.Value = false;
                }
            }
        }

        void Attach()
        {
            if (isFitted) return;
            transform.parent = attachmentPoint.transform;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            StartCoroutine(FixParent(attachmentPoint.transform));
            StartCoroutine(LateAttach());
            boltParent.SetActive(true);
        }

        IEnumerator LateAttach()
        {
            while(!rb.isKinematic || rb.useGravity)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                yield return new WaitForEndOfFrame();
            }
            BSFDTestbed.assembleAudio.Play();
            partTrigger.enabled = false;
            attachmentTrigger.enabled = false;
            gameObject.tag = "Untagged";
            isFitted = true;
        }

        public void Detach()
        {
            if (!isFitted) return;

            BSFDTestbed.disassembleAudio.Play();
            gameObject.tag = "PART";
            transform.parent = null;
            StartCoroutine(FixParent(null));
            StartCoroutine(LateDetach());
            boltParent.SetActive(false);
            //TODO: ResetBoltStatus();
        }

        IEnumerator LateDetach()
        {
            while (rb.isKinematic || !rb.useGravity)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                yield return new WaitForEndOfFrame();
            }
            attachmentTrigger.enabled = true;
            isFitted = false;
        }

        void UntightenAllBolts()
        {
            //TODO: untightening event 

        }
    }
}