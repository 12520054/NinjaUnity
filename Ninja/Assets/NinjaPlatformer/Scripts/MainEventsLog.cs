using UnityEngine;
using System.Collections;

//MainEvents can keep track of Souls and Player deaths.

public class MainEventsLog : MonoBehaviour {

	public int SoulsCollected;

	public NinjaMovementScript NinjaMovScript;
	public GameObject TouchControls;

	public CameraFollowTarget CameraFollowScript;

	public AudioSource AudioSource_Collectible;
	public AudioSource AudioSource_Death;


	
	public void PlayerCollectedSoul(){
		AudioSource_Collectible.Play ();
		Debug.Log ("Soul collected");
		SoulsCollected += 1;
	}


	public void PlayerDied(){

		//Camera will stop for a second when the player dies.
		CameraFollowScript.PlayerDied ();

		AudioSource_Death.Play ();
		Debug.Log ("Player died");
	}






	//All the Unity UI stuff is here.
	private bool VisibleGUI = true;
	private string GUIText_DoubleJump = "Activate Double Jump";
	private string GUIText_TouchControls = "Hide Touch Controls";

	void OnGUI () {
		if(VisibleGUI == true){
			if(GUI.Button(new Rect(10,10,200,40), GUIText_DoubleJump)) {
				if(NinjaMovScript.DoubleJump == true){
					NinjaMovScript.DoubleJump = false;
					GUIText_DoubleJump = "Activate Double Jump";
				}else{
					NinjaMovScript.DoubleJump = true;
					GUIText_DoubleJump = "DeActivate Double Jump";
				}
			}

			if(GUI.Button(new Rect(10,60,200,40), GUIText_TouchControls)) {
				if(TouchControls.activeSelf == true){
					TouchControls.SetActive(false);
					GUIText_TouchControls = "Show Touch Controls";
				}else{
					TouchControls.SetActive(true);
					GUIText_TouchControls = "Hide Touch Controls";
				}
			}
			if(GUI.Button(new Rect(10,110,100,40), "Hide this UI")) {
				VisibleGUI = false;
			}
		
		}else{
			if(GUI.Button(new Rect(10,10,75,20), "Show UI")) {
				VisibleGUI = true;
			}
		}
	}

}
