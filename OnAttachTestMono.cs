using UnityEngine;

namespace BSFDTestbed
{
    public class OnAttachTestMono : MonoBehaviour
    {
        Part part;
        // Use this for initialization
        void Start()
        {
            part = GetComponent<Part>();
            if (part)
            {
                part.OnAttach += BruhMoment;
                part.OnDetach += SyshSituation;
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Keypad5) && part)
            {
                foreach (var b in part.bolts) b.SetBoltStep(Random.Range(0, b.maxBoltSteps));
            }
        }

        void BruhMoment()
        {
            MSCLoader.ModConsole.Print("BRUH");
        }

        void SyshSituation()
        {
            MSCLoader.ModConsole.Print("SYSH");
        }
    }
}