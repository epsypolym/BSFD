using MSCLoader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Net.NetworkInformation;

namespace BSFDTestbed
{
    public class Interaction : MonoBehaviour
    {
        public RaycastHit hitInfo;
        private Part raycastedPart;

        public bool hasHit = false;
        public float rayDistance = 1.35f;
        public int layerMaskBolts;
        public int layerMaskParts;

        public static AudioSource audioBoltScrew;

        public static PlayMakerFSM ratchetFsm;
        public static FsmBool ratchetSwitch;
        public static FsmBool GUIAssemble;
        public static FsmBool GUIDisassemble;

        private Bolt activeBolt;

        // Use this for initialization
        void Start()
        {
            layerMaskBolts = LayerMask.GetMask("Bolts");
            layerMaskParts = LayerMask.GetMask("Parts");

            hitInfo = new RaycastHit();
            audioBoltScrew = GameObject.Find("MasterAudio/CarBuilding/bolt_screw").GetComponent<AudioSource>();
            ratchetFsm = GameObject.Find("PLAYER/Pivot/AnimPivot/Camera/FPSCamera/2Spanner/Pivot/Ratchet").GetComponent<PlayMakerFSM>();
            ratchetSwitch = ratchetFsm.FsmVariables.FindFsmBool("Switch");
            GUIAssemble = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIassemble");
            GUIDisassemble = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIdisassemble");          
        }

        void FixedUpdate()
        {
            // Detach Parts
            RaycastHit hit;          
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, rayDistance, layerMaskParts))
            {
                raycastedPart = hit.collider.GetComponent<Part>();
                if(raycastedPart)
                {
                    if(raycastedPart.isFitted && raycastedPart.tightness == 0)
                    {
                        GUIDisassemble.Value = true;
                        if (Input.GetMouseButtonDown(1)) raycastedPart.Detach();
                    }
                }
            }

            // don't update raycast, if not in tool mode
            if (BSFDTestbed.gameToolID.Value == 0)
            {
                // deactivate bolt.
                if (activeBolt)
                {
                    activeBolt.Exit();
                    activeBolt = null;
                }
                // exit from the loop.
                return;
            }

            // do raycast
            if (Camera.main != null) hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, rayDistance, layerMaskBolts);

            // set active bolt
            if (hasHit && !activeBolt)
            {
                activeBolt = hitInfo.collider.GetComponent<Bolt>();
            }
            // deactivate bolt
            else if (!hasHit && activeBolt)
            {
                activeBolt.Exit();
                activeBolt = null;
            }
        }

        void Update()
        {
            if (activeBolt) activeBolt.UpdateBolt();
        }

        public bool GetHit(Collider collider) => hasHit && hitInfo.collider == collider;
        public bool GetHitAny(Collider[] colliders) => hasHit && colliders.Any(collider => collider == hitInfo.collider);
        public bool GetHitAny(List<Collider> colliders) => hasHit && colliders.Any(collider => collider == hitInfo.collider);
    }
}