using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace BSFDTestbed
{
    public class BSFDTestbed : Mod
    {
        public override string ID => "BSFDTestbed"; //Your mod ID (unique)
        public override string Name => "BSFDTestbed"; //You mod name
        public override string Author => "eps"; //Your Username
        public override string Version => "1.0"; //Version

        // Set this to true if you will be load custom assets from Assets folder.
        // This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => true;
        public GameObject PLAYER;
        public static BSFDinteraction boltInteraction;

        public override void OnNewGame()
        {
            // Called once, when starting a New Game, you can reset your saves here
        }

        public override void OnLoad()
        {
            PLAYER = GameObject.Find("PLAYER");
            boltInteraction = PLAYER.AddComponent<BSFDinteraction>();
            AssetBundle ab = LoadAssets.LoadBundle(this, "bsfd.unity3d");
            GameObject boltboxtest = ab.LoadAsset("boltbox.prefab") as GameObject;
            GameObject parts = ab.LoadAsset("PARTS.prefab") as GameObject;
            GameObject st = ab.LoadAsset("StressTest.prefab") as GameObject;
            
            GameObject box = GameObject.Instantiate(boltboxtest);
            GameObject paarts = GameObject.Instantiate(parts);
            
            box.transform.position = new Vector3(2.98f, 0.6999f, 0.96f);

            ab.Unload(false);

        }

        public override void ModSettings()
        {
            // All settings should be created here. 
            // DO NOT put anything else here that settings.
        }

        public override void OnSave()
        {
            // Called once, when save and quit
            // Serialize your save file here.
        }

        public override void OnGUI()
        {
            // Draw unity OnGUI() here
        }

        public override void Update()
        {
            // Update is called once per frame
        }

    }
}
