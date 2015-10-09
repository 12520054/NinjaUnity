using UnityEngine;
using System.Collections;

public class Enemy_FlyingA : MonoBehaviour {

	public float speed;

	private NinjaMovementScript PlayerScript;

	private bool EnemyAwake = false;
	private bool EnemyDead = false;

	//Distance who far player has to be to this enemy to wakeup
	private float AwakeDistance = 10f;
	
	//Here are reference slots for AnimationController and Player Sprite Object.
	public Animator AnimatorController;
	public GameObject MySpriteOBJ; 
	private Vector3 MySpriteOriginalScale;
	
	public ParticleSystem ParticleTrail;

	public AudioSource EnemyDiesAudio;

	void Start () {
		PlayerScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<NinjaMovementScript> ();
		//Start the distance checks. (When player gets close enough, Wake up. When he gets far enough, Go back to sleep.
		InvokeRepeating ("CheckPlayerDistance", 0.5f, 0.5f);

		MySpriteOriginalScale = MySpriteOBJ.transform.localScale;
		MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x,MySpriteOBJ.transform.localScale.y,1f);
		ParticleTrail.emissionRate = 0;

	}
	
	void FixedUpdate () {
		this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		//If you are awake. Move towards the player
		if (EnemyAwake == true && EnemyDead == false) {
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(this.transform.position, PlayerScript.transform.position, step);

			if(MySpriteOBJ.transform.localScale.x > 0 && transform.position.x > PlayerScript.transform.position.x){
				MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x,MySpriteOBJ.transform.localScale.y,1f);
			}

			if(MySpriteOBJ.transform.localScale.x < 0 && transform.position.x < PlayerScript.transform.position.x){
				MySpriteOBJ.transform.localScale = new Vector3(MySpriteOriginalScale.x,MySpriteOBJ.transform.localScale.y,1f);
			}
		}


		AnimatorController.SetBool ("Awake", EnemyAwake);
		AnimatorController.SetBool ("Dead", EnemyDead);
	}


	void OnCollisionEnter2D(Collision2D coll) {
	
		if (coll.gameObject.tag == "Player" && EnemyDead == false) {

			//Check who killed who. If contact happend from the top player killed the enemy. Else player died.
			if(	coll.contacts[0].normal.x > -1f && coll.contacts[0].normal.x < 1f && coll.contacts[0].normal.y < -0.35f){
				if (EnemyDiesAudio != null) {
					EnemyDiesAudio.Play ();
				}
				ParticleTrail.emissionRate = 0;
				coll.rigidbody.AddForce( new Vector2(0f,1500f));
				this.GetComponent<Rigidbody2D>().AddForce( new Vector2(0f,-200f));
				EnemyDead = true;
				Debug.Log ("Monster died");

				Invoke("iDied",0.15f);
			}else{
				PlayerScript.NinjaDies();
			}

		}
	}

	void iDied(){
		PlayerScript.NinjaKilledEnemy();
		Destroy (this.gameObject);
	}


	void CheckPlayerDistance(){

		if (Vector3.Distance (this.transform.position, PlayerScript.transform.position) <= AwakeDistance && EnemyAwake == false) {
//			Debug.Log("Close enough to wake up");
			EnemyAwake = true;
			ParticleTrail.emissionRate = 15;

		}

		if (Vector3.Distance (this.transform.position, PlayerScript.transform.position) > AwakeDistance && EnemyAwake == true) {
//			Debug.Log("Far enough to fall back sleep");
			EnemyAwake = false;
			ParticleTrail.emissionRate = 0;

		}

	}


}
