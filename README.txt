
Instructions

BSFD was designed with simplicity at it's core, so most if not all of it's functions are configured within the Unity Editor.

== Adding BSFD to the Unity Editor ==

Step 0: Download the MSC Asset Bundles project from the MSCLoader GitHub page.
Step 1: Add the BSFD Libraries to your mod project in Visual Studio.
Step 2: Compile your mod project.
Step 3: Drag and drop your compiled mod DLL into the Unity Editor of the MSC Asset Bundles project with any dependencies* you might be using. 
- * (such as: MSCLoader.dll, PlayMaker.dll, cInput.dll, etc.)
Step 4: You should now be able to add BSFD Monobehaviours onto your gameobjects.

== Using BSFD compared to other attachment systems ==

Due to the way Unity works, you cannot load your parts individually. You have to load all your parts with their respective references with a parent GameObject.

Example hierarchy configuration:

PARTS(empty GameObject)
 > Part1(GameObject)
 > Part2(GameObject)
 > Part3(GameObject)
 > etc...

Afterwards, you create a prefab from the PARTS GameObject you have created, and then you load said PARTS prefab. This way, your parts will retain their attachment
references. Do note, this step is not required for parts that do not attach to another GameObject you are loading, as you will have to target it's attachment
configuration in code regardless.

== Creating BSFD Prefabs and GameObjects ==

=Bolt Prefab Setup Instructions=
- A functional BSFD bolt only requires three parts:
	- a Mesh(any mesh you want + your desired Material/Texture)
	- a BoxCollider (to scale of Mesh and or autoscaled to mesh bounds by Unity)
	- the "Bolt.cs" monobehavior. 

=Part Prefab Setup Instruction=
- A functional BSFD part requires this list of parts:
	- Mesh Collider (for world collisions)
	- Box Collider (set to Trigger, used for attaching with other items)
	- "Part.cs" Monobehaviour.
	- AudioSource
	- a RigidBody component.

= Bolt Configuration for Parts =

Adding bolts to your Part is very simple.
Step1: create a "Bolts" child of your Part GameObject.

Example:

Part1(GameObject)
 > Bolts(empty GameObject)
   > bolt
   > bolt 1
   > etc...

Step2: in the Part monobehavior, assign the bolts to the bolts array of your desired size.
Step3: calculate your Part maximum tightness (8 bolts with 8 bolting steps = 8*8 = 64, Max Tightness would be 64)
[Do note: do not change the "tightness" variable, that one is calculated in-game]


at this point i have run out of patience so just ask me on discord for the rest if you're too dumb to figure it out
lmao
