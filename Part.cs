using UnityEngine;
using System.Collections;
using System.Linq;
using System;

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
        public bool disableColliders = false;
        public bool destroyRigidbody = false;
        public Collider partTrigger; // Trigger of part, used for collision test between attachmentTrigger.

        //part(thing you are attaching to) related variables
        public GameObject attachmentPoint; // GameObject, parent of Part upon attachment.
        public Collider attachmentTrigger; // Collider, Trigger, used for collision test between partTrigger.

        //events
        public event Action OnAttach;
        public event Action OnDetach;

        Rigidbody rb;
        float mass;
        CollisionDetectionMode collmode;
        RigidbodyInterpolation interpolationmode;

        // Use this for initialization
        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            mass = rb.mass;
            collmode = rb.collisionDetectionMode;
            interpolationmode = rb.interpolation;

            if (bolts.Length != 0 && boltParent != null)
            {
                StartCoroutine(UpdatePartTightness());
            }
        }

        void FixedUpdate()
        {
            if (isFitted) PartAttached();
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
                BSFDinteraction.GUIAssemble.Value = true;
                if (Input.GetMouseButtonDown(0))
                {
                    Attach(true);
                    BSFDinteraction.GUIAssemble.Value = false;
                }
            }
        }

        bool canAttach() { return transform.IsChildOf(BSFDinteraction.ItemPivot) && attachmentTrigger.transform.childCount == 0 && !isFitted; }

        public void Attach(bool playAudio)
        {
            if (isFitted) return;

            transform.parent = attachmentPoint.transform;
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            StartCoroutine(FixParent(attachmentPoint.transform));
            StartCoroutine(LateAttach(playAudio));
            if (boltParent != null)
            {
                boltParent.SetActive(true);
            }
            OnAttach?.Invoke();
        }

        IEnumerator LateAttach(bool playAudio)
        {
            if (!destroyRigidbody)
            {
                while (!rb.isKinematic || rb.useGravity)
                {
                    rb.isKinematic = true;
                    rb.useGravity = false;

                    if (disableColliders) { rb.detectCollisions = false; }
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                Component.Destroy(rb);
            }

            if (playAudio) MasterAudio.PlaySound3DAtTransform("CarBuilding", partTrigger.transform, 1f, 1f, 0f, "assemble");
            partTrigger.enabled = false;
            attachmentTrigger.enabled = false;
            gameObject.tag = "Untagged";
            isFitted = true;
        }

        void PartAttached()
        {
            if (boltParent == null)
            {
                partTrigger.enabled = true;
            }
            else
            {
                if (tightness >= MaxTightness)
                {
                    partTrigger.enabled = false;
                }
                else if (tightness <= 0)
                {
                    partTrigger.enabled = true;
                }
            }
        }

        public void Detach()
        {
            if (!isFitted) return;

            MasterAudio.PlaySound3DAtTransform("CarBuilding", partTrigger.transform, 1f, 1f, 0f, "disassemble");
            gameObject.tag = "PART";
            transform.parent = null;
            if (!destroyRigidbody)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.detectCollisions = true;
            }
            attachmentTrigger.enabled = true;
            isFitted = false;
            StartCoroutine(FixParent(null));
            if (disableColliders && !destroyRigidbody) { rb.detectCollisions = true; }
            StartCoroutine(LateDetach());
            if (boltParent != null)
            {
                boltParent.SetActive(false);
            }
            OnDetach?.Invoke();
            if (boltParent != null && bolts.Length != 0)
            {
                UntightenAllBolts();
            }
        }

        IEnumerator LateDetach()
        {
            if (!destroyRigidbody)
            {
                while (rb.isKinematic || !rb.useGravity)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    if (disableColliders) { rb.detectCollisions = true; }
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.mass = mass;
                rb.collisionDetectionMode = collmode;
                rb.interpolation = interpolationmode;
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