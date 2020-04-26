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

        public static Material defaultMaterial;
        public static Material activeMaterial;

        Renderer renderer;
        bool isDelay = false;

        // Use this for initialization
        void Start()
        {
            renderer = GetComponent<Renderer>();
            if(defaultMaterial == null) defaultMaterial = Instantiate(renderer.material) as Material;
        }

        void BoltTightenEvent(bool down, float delayTime)
        {
            if( (down && currentBoltStep > 0) || (!down && currentBoltStep < maxBoltSteps))
            {
                StartCoroutine(Delay(delayTime));
                Interaction.audioBoltScrew.Play();
                currentBoltStep += down ? -1 : 1;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, down ? -45 : 45));
                transform.localPosition += new Vector3(0, down ? boltMoveAmount : -boltMoveAmount, 0);
            }
        }

        // Update is called from Interaction.cs
        public void UpdateBolt()
        {
            if (boltSize == BSFDTestbed.gameToolID.Value)
            {
                // Set active material
                if (renderer.material != activeMaterial) SetActiveMaterial(true);

                if (Input.GetAxis("Mouse ScrollWheel") != 0 && !isDelay)
                {
                    // Rachet Logic
                    if (Interaction.ratchetFsm.Active) BoltTightenEvent(!Interaction.ratchetSwitch.Value, 0.1f);

                    // Spanner Logic                      
                    else BoltTightenEvent(Input.GetAxis("Mouse ScrollWheel") > 0 ? false : true, 0.28f);
                }
            }
            else
            {
                Exit();
            }
        }

        public void Exit()
        {
            if (renderer.material != defaultMaterial) SetActiveMaterial(false);
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