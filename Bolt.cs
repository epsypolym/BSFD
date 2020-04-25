using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;
using System.Collections;

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

        public static Material defaultMaterial;
        public static Material activeMaterial;
        Renderer renderer;

        bool isDelay = false;

        // Use this for initialization
        void Start()
        {
            boltInteraction = BSFDTestbed.boltInteraction;
            GUIuse = BSFDTestbed.GUIuse;
            selfCollider = GetComponent<Collider>();
            boltTimeDelay = Interaction.boltTimeDelay;
            gameToolID = BSFDTestbed.gameToolID;
            renderer = GetComponent<Renderer>();
            if(defaultMaterial == null) defaultMaterial = Instantiate(renderer.material) as Material;
        }

        void BoltEventDown()
        {
            StartCoroutine(Delay(0.28f));
            Interaction.audioBoltScrew.Play();
            currentBoltStep -= 1;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0, 0, 45));
            transform.localPosition += new Vector3(0, boltMoveAmount, 0);
        }

        void BoltEventUp()
        {
            StartCoroutine(Delay(0.28f));
            Interaction.audioBoltScrew.Play();
            currentBoltStep += 1;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 45));
            transform.localPosition += new Vector3(0, -boltMoveAmount, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if (boltInteraction.GetHit(selfCollider) & gameToolID.Value > 0f & boltSize == gameToolID.Value)
            {
                if (renderer.material != activeMaterial) SetActiveMaterial(true);

                if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.time >= boltTimeDelay && currentBoltStep < maxBoltSteps && !isDelay) // Scroll Up
                {
                    BoltEventUp();
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f && Time.time >= boltTimeDelay && currentBoltStep > 0 && !isDelay) // Scroll Down
                {
                    BoltEventDown();
                }
            } else
            {
                if (renderer.material != defaultMaterial) SetActiveMaterial(false);
            }
        }

        void SetActiveMaterial(bool active)
        {
            if (renderer && activeMaterial && defaultMaterial)
                renderer.material = active ? activeMaterial : defaultMaterial;
            else
                ModConsole.Print("Error when setting bolt material!");
        }

        IEnumerator Delay(float time)
        {
            isDelay = true;
            yield return new WaitForSeconds(time);
            isDelay = false;
        }       
    }
}