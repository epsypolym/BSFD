using MSCLoader;
using System.IO.Pipes;
using UnityEngine;

namespace BSFDTestbed
{
    public class OnAttachTestMono : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            gameObject.GetComponent<Part>().OnAttach += BruhMoment;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void BruhMoment()
        {
            Destroy(gameObject); //IT JUST WORKS
        }

    }
}