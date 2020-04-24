using HutongGames.PlayMaker;
using MSCLoader;
using System.Net.NetworkInformation;
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
        public static Interaction boltInteraction;
        public static bool GUIuse;
        public static FsmFloat gameToolID;
        public override void OnNewGame()
        {
            // Called once, when starting a New Game, you can reset your saves here
        }

        public override void OnLoad()
        {
            GUIuse = PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value;
            gameToolID = PlayMakerGlobals.Instance.Variables.GetFsmFloat("ToolWrenchSize");
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
