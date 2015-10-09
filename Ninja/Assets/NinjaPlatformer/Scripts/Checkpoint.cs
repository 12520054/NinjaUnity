using UnityEngine;
using System.Collections;

//This script is only to hide the visual helpers of editor when you run the game. KillZone.cs handels player death and respawning.
public class Checkpoint : MonoBehaviour {

	public MeshRenderer MyMeshrenderer;
	
	void Start () {
		//Hide the green when you play the game.
		MyMeshrenderer.enabled = false;	
	}

	//If player walks past the checkpoint, make this as the active checkpoint.
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			other.GetComponent<NinjaMovementScript>().ActiveCheckpoint = this.gameObject;	
		}
	}


}
