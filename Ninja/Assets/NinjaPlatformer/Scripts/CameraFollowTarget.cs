using UnityEngine;
using System.Collections;

public class CameraFollowTarget : MonoBehaviour {

	public GameObject FollowTargetOBJ;
	public float FollowSpeed;

	//Paralax layers

	private bool PlayerJustDied;

	void Update(){
		if (FollowTargetOBJ != null && GlobalValues.isPlayerRunning == true) {
            Vector3 NewPosition = FollowTargetOBJ.transform.position;
            this.transform.position = new Vector3 (NewPosition.x + 5, this.transform.position.y, this.transform.position.z);
		}
	}

	//Here you can set camera delay for the Ninja death.
	public void PlayerDied(){
		PlayerJustDied = true;
		Invoke ("BackToBusiness", 0.05f);
	}

	//Camera follow back ON.
	void BackToBusiness(){
		PlayerJustDied = false;
	}
}
