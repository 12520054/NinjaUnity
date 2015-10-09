using UnityEngine;
using System.Collections;

public class Touch_BTN_Jump : MonoBehaviour {

	public NinjaMovementScript NinjaMovScript;

	public Color ColorON;
	public Color ColorOFF;

	public SpriteRenderer ButtonEnabledSprite;
	
	public void OnPress_IE(){
		NinjaMovScript.Button_Jump_press ();
		ButtonEnabledSprite.color = ColorON;
	}

	public void OnRelease_IE(){
		NinjaMovScript.Button_Jump_release ();
		ButtonEnabledSprite.color = ColorOFF;
	}

}
