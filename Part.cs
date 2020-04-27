using UnityEngine;
using System.Collections;

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
        public Collider partTrigger; // Trigger of part, used for collision test between attachmentTrigger.

        //part(thing you are attaching to) related variables
        public GameObject attachmentPoint; // GameObject, parent of Part upon attachment.
        public Collider attachmentTrigger; // Collider, Trigger, used for collision test between partTrigger.

        //events
        public delegate void AttachDelegate();
        public event AttachDelegate OnAttach;
        public delegate void DetachDelegate();
        public event DetachDelegate OnDetach;

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
            if (other == attachmentTrigger && canAttach())
            {
                Interaction.GUIAssemble.Value = true;
                if (Input.GetMouseButtonDown(0))
                {
                    Attach(true);
                    Interaction.GUIAssemble.Value = false;
                }
            }
        }

        bool canAttach() { return transform.IsChildOf(Interaction.ItemPivot) && attachmentTrigger.transform.childCount == 0 && !isFitted; }                 

        public void Attach(bool playAudio)
        {
            if (isFitted) return;

            transform.parent = attachmentPoint.transform;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            StartCoroutine(FixParent(attachmentPoint.transform));
            StartCoroutine(LateAttach(playAudio));
            boltParent.SetActive(true);
            OnAttach?.Invoke();
        }

        IEnumerator LateAttach(bool playAudio)
        {
            while(!rb.isKinematic || rb.useGravity)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                yield return new WaitForEndOfFrame();
            }

            if(playAudio) BSFDTestbed.assembleAudio.Play();
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
            rb.isKinematic = false;
            rb.useGravity = true;
            attachmentTrigger.enabled = true;
            isFitted = false;
            StartCoroutine(FixParent(null));
            StartCoroutine(LateDetach());
            boltParent.SetActive(false);
            OnDetach?.Invoke();
            UntightenAllBolts();
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
            foreach (var b in bolts) b.SetBoltStep(0);
        }
    }
}