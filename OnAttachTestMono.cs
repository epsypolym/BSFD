using UnityEngine;

namespace BSFDTestbed
{
    public class OnAttachTestMono : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GetComponent<Part>().OnAttach += BruhMoment;
            GetComponent<Part>().OnDetach += SyshSituation;
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