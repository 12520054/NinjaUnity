using UnityEngine;
using System.Collections;

public class NinjaCollectible : MonoBehaviour {

	private bool collected;
	public GameObject SoulSprites;
	public ParticleSystem SoulParticles;

	private MainEventsLog MainEventsLog_script;
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player" && collected == false) {
			collected = true;
			SoulSprites.SetActive(false);
			SoulParticles.Emit(10);
			Invoke ("DestroyObject", 1f);

			//Send message to MainEventsLog. First checks if the reference path is set. If not, it will MainEventsLog from the scene.
			if(MainEventsLog_script == null){
				MainEventsLog_script = GameObject.FindGameObjectWithTag("MainEventLog").GetComponent<MainEventsLog>();
			}
			MainEventsLog_script.PlayerCollectedSoul();
		}
	}

	void DestroyObject(){
		Destroy (this.gameObject);
	}



}
