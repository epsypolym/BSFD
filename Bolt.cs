using UnityEngine;
using System.Collections;

namespace BSFDTestbed
{
    public class Bolt : MonoBehaviour
    {
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
                BSFDinteraction.audioBoltScrew.Play();
                BSFDinteraction.audioBoltScrew.gameObject.transform.position = transform.position;
                currentBoltStep += down ? -1 : 1;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, down ? -45 : 45));
                transform.localPosition += new Vector3(0, down ? boltMoveAmount : -boltMoveAmount, 0);
            }
        }

        public void SetBoltStep(int newBoltStep)
        {
            if(newBoltStep < 0 || newBoltStep > maxBoltSteps)
            {
                MSCLoader.ModConsole.Print("BSFD: Tried set BoltStep to "+newBoltStep+". BoltStep should be in range 0 - "+maxBoltSteps + ".");
                return;
            }

            int steps = 0;
            bool down = newBoltStep < currentBoltStep;
            steps = Mathf.Abs(currentBoltStep - newBoltStep);

            if (steps == 0) return; // we are already in target step -> quit.

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, down ? -45 * steps : 45 * steps));
            transform.localPosition += new Vector3(0, down ? boltMoveAmount * steps : -boltMoveAmount * steps, 0);
            currentBoltStep = newBoltStep;
        }

        // Update is called from Interaction.cs
        public void UpdateBolt()
        {
            if (boltSize == BSFDinteraction.gameToolID.Value)
            {
                // Set active material
                if (renderer.material != activeMaterial) SetActiveMaterial(true);

                if (Input.GetAxis("Mouse ScrollWheel") != 0 && !isDelay)
                {
                    // Rachet Logic
                    if (BSFDinteraction.ratchetFsm.Active) BoltTightenEvent(!BSFDinteraction.ratchetSwitch.Value, 0.1f);

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
                MSCLoader.ModConsole.Print("BSFD: Error when setting bolt material!");
        }

        IEnumerator Delay(float time)
        {
            isDelay = true;
            yield return new WaitForSeconds(time);
            isDelay = false;
        }       
    }
}
