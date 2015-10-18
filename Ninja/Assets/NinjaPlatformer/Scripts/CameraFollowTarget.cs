﻿using UnityEngine;
using System.Collections;

public class CameraFollowTarget : MonoBehaviour {

	public GameObject FollowTargetOBJ;
	public float FollowSpeed;

	//Paralax layers

	private bool PlayerJustDied;

	void Update(){
		if (PlayerJustDied == false && FollowTargetOBJ != null) {
						//Smoothly Follow Target
						Vector3 PositionBefore = this.transform.position;
            //Vector3 NewPosition = Vector3.Lerp (this.transform.position, FollowTargetOBJ.transform.position, FollowSpeed * Time.deltaTime);
            Vector3 NewPosition = FollowTargetOBJ.transform.position;
            this.transform.position = new Vector3 (NewPosition.x + 0.7f, this.transform.position.y, this.transform.position.z);
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
