using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {

	//You need to assign CheckPoint to each killzone. CheckPoint can be just an empty gameobject. It is used only to check where player should appear after dying.
	public GameObject CheckPoint;
	public MeshRenderer MyMeshrenderer;
	
	private NinjaMovementScript NinjaScript;
	
	void Start () {
		//Hide the red when you play the game.
		MyMeshrenderer.enabled = false;	
	}

	//If player enters deadzone. Go back to Checkpoint.
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {

			if(NinjaScript == null){
				NinjaScript = GameObject.FindGameObjectWithTag("Player").GetComponent<NinjaMovementScript>();
			}
			NinjaScript.NinjaDies();

		}
	}


}
