using MSCLoader;
using UnityEngine;

namespace BSFDTestbed
{
    public class Part : MonoBehaviour
    {
        public GameObject[] bolts;  // Array of bolts, define in Unity inspector.
        public int tightness = 0;  // Current part tightness, calculated by UpdateItemTightness().
        public int MinTightness;  // If Tightness < MinTightness, part can be taken off by hand, recommended value for MinTightnes is ~60% of MaxTightness.
        public int MaxTightness; // Part will not fall off if tightness = MaxTightness
        public bool isAttached; // Self explanatory.

        // Use this for initialization
        void Start()
        {
            
            
        }

        void UpdateItemTightness()
        {
            foreach (var b in bolts)
            {
                tightness += b.GetComponent<Bolt>().currentBoltStep;
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}