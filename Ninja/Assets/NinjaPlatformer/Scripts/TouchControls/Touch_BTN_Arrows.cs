using UnityEngine;
using System.Collections;

//Touch control for 2 arrows is a little more complex than the Jump button.
//I wanted buttons to work even when the player is too fast to lift his finger.

public class Touch_BTN_Arrows : MonoBehaviour {

	public NinjaMovementScript NinjaMovScript;

	private bool IsPressed;
	private bool RightSide;
	private bool LeftSide;

	public Color ColorON;
	public Color ColorOFF;
	
	public SpriteRenderer ButtonLeft_EnableSprite;
	public SpriteRenderer ButtonRight_EnableSprite;

	private int StoredTouchID;


	void Start(){
		IsPressed = false;
		RightSide = false;
		LeftSide = false;
	}

	void Update(){

		//Checks if the finger drags from Left button to Right button or other way.
		if (IsPressed == true) {
		
			if(RightSide == true && CameraTouchControl.DragPos[StoredTouchID].x < this.transform.position.x){
				RightSide = false;
				LeftSide = true;
				ButtonRight_EnableSprite.color = ColorON;
				ButtonLeft_EnableSprite.color = ColorOFF;

				NinjaMovScript.Button_Right_release ();
				NinjaMovScript.Button_Left_press ();

			}else if(RightSide == false && CameraTouchControl.DragPos[StoredTouchID].x > this.transform.position.x){
				RightSide = true;
				LeftSide = false;
				ButtonRight_EnableSprite.color = ColorOFF;
				ButtonLeft_EnableSprite.color = ColorON;

				NinjaMovScript.Button_Left_release ();
				NinjaMovScript.Button_Right_press ();

			}
		}	
	}



	public void OnPress_IE(int TouchID){

		StoredTouchID = TouchID;

		IsPressed = true;

		if (CameraTouchControl.inputHitPos[StoredTouchID].x < this.transform.position.x) {
			ButtonRight_EnableSprite.color = ColorON;
			LeftSide = true;
			NinjaMovScript.Button_Left_press ();
		} else {
			ButtonLeft_EnableSprite.color = ColorON;
			RightSide = true;
			NinjaMovScript.Button_Right_press ();
		}

	}
	
	public void OnRelease_IE(int TouchID){
			
		if (CameraTouchControl.inputHitPos [StoredTouchID].x < this.transform.position.x) {
			LeftSide = false;
			NinjaMovScript.Button_Left_release ();
			ButtonRight_EnableSprite.color = ColorOFF;

			
		} else {
			RightSide = false;
			NinjaMovScript.Button_Right_release ();
			ButtonLeft_EnableSprite.color = ColorOFF;

		}

		if (RightSide == false && LeftSide == false) {
			IsPressed = false;	
		}

	}

}
