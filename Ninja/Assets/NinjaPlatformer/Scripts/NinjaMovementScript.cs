using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NinjaMovementScript : MonoBehaviour {
	
	//Player speed and JumpForce. You can tweak these to change the game dynamics. 
	public float PlayerSpeed;
	public float JumpForce;

	//Do you want player to have double jump? Then make this DoubleJump boolean true :)
	public bool DoubleJump;
	

	//These variables are for the code. They track the current events of the player character.
	//You don't need to change or worry about them :)
	private MainEventsLog MainEventsLog_script;
	private bool DJ_available;
	private float JumpForceCount;
	private bool IsGrounded;

	public List<GameObject> GroundedToObjectsList;
	public List<GameObject> WalledToObjectsList;

	private float walljump_count;
	private bool WallTouch;
	private bool WallGripJustStarted;

	private bool PlayerLooksRight;

	private bool NoNeedForSafeJump_bool = false;

	private int JustPressedSpace;
	private int FixStateTimer = 0; 
	private int OnCollisionStayCounter = 0;
	private int OnCollisionBugThreshold = 0;
	private int UpInTheAir_Counter;
	
	private float Ground_X_MIN;
	private float Ground_X_MAX;
	private float Ground_Y_MIN;
	private float Ground_Y_MAX;

	//This is to make sure Ninja moves along moving platforms when standing on them.
	public GameObject NinjaPlatformRoot_PREFAB;
	private NinjaPlatformRoot NinjaPlatformRoot;
	public GameObject NinjaVisualRoot;

	//Checkpoint related things:
	public GameObject ActiveCheckpoint;



	//These booleans keep track which button is being pressed or not.
	private bool Btn_Left_bool;
	private bool Btn_Right_bool;
	private bool Btn_Jump_bool;

	//Here are reference slots for AnimationController and Player Sprite Object.
	public Animator AnimatorController;
	public GameObject MySpriteOBJ;
	private Vector3 MySpriteOriginalScale;

	//Here are reference slots for Player Particle Emitters
	public ParticleSystem WallGripParticles;
	private int WallGripEmissionRate;
	public ParticleSystem JumpParticles_floor;
	public ParticleSystem JumpParticles_wall;
	public ParticleSystem JumpParticles_doublejump;
	public ParticleSystem Particles_DeathBoom;


	//AudioSources play the audios of the scene.
	public AudioSource AudioSource_Jump;



	
	// Use this for initialization
	void Start () {

		GroundedToObjectsList = new List<GameObject> ();
		WalledToObjectsList = new List<GameObject> ();

		//These define when collision is detected as a floor and when as a wall.
		Ground_X_MIN = -0.75f;
		Ground_X_MAX = 0.75f;
		Ground_Y_MIN = 0.5f;
		Ground_Y_MAX = 1f;

		//NinjaRootOBJ makes sure that Ninja together with moving platforms
		GameObject NinjaROOTOBJ = Instantiate (NinjaPlatformRoot_PREFAB, Vector3.zero, Quaternion.identity) as GameObject;
		NinjaPlatformRoot = NinjaROOTOBJ.GetComponent<NinjaPlatformRoot> ();
		NinjaROOTOBJ.name = "NINJA_PlatformRoot";

		//Just some default values for WallGrip Particle Emitter.
		WallGripEmissionRate = 10;
		WallGripParticles.emissionRate = 0;

		//Player characters looks right in the start of the scene.
		PlayerLooksRight = true;
		MySpriteOriginalScale = MySpriteOBJ.transform.localScale;

	}



	// Update is called once per frame
	void Update () {

		//Button commands from the keyboard
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Button_Left_press();		
		}
		if(Input.GetKeyUp (KeyCode.LeftArrow)) {
			Button_Left_release();		
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Button_Right_press();		
		}
		if(Input.GetKeyUp (KeyCode.RightArrow)) {
			Button_Right_release();		
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			JustPressedSpace = 2;
			Button_Jump_press();		
		}
		if (Input.GetKeyUp (KeyCode.Space)) {
			Button_Jump_release();		
		}

		if (Input.GetKeyDown (KeyCode.A)) {
			Button_Jump_press();		
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			Button_Jump_release();		
		}

		if (walljump_count >= 0) {
			walljump_count -= Time.deltaTime;		
		}
	}




	void FixedUpdate(){

		//The actual Left/Right movement happens here.

		//This checks is the player pressing left or right button.
		//if(Btn_Left_bool == true && Btn_Right_bool == false){
		//	if(PlayerLooksRight == true && WallTouch == false)
  //          {
		//		PlayerLooksRight = false;
		//		MySpriteOBJ.transform.localScale = new Vector3(-MySpriteOriginalScale.x,MySpriteOriginalScale.y,MySpriteOriginalScale.z);
		//	}

		//	this.GetComponent<Rigidbody2D>().AddForce(new Vector2(NinjaVisualRoot.transform.right.x,NinjaVisualRoot.transform.right.y)*-PlayerSpeed*Time.deltaTime);

		//}else if(Btn_Left_bool == false && Btn_Right_bool == true)
        {
			if(PlayerLooksRight == false && WallTouch == false){
				PlayerLooksRight = true;
				MySpriteOBJ.transform.localScale = MySpriteOriginalScale;
			}
			this.GetComponent<Rigidbody2D>()
                .AddForce(
                new Vector2(NinjaVisualRoot.transform.right.x,
                NinjaVisualRoot.transform.right.y) * PlayerSpeed * Time.deltaTime);
		}

		//this makes sure player is not sliding on slobes
		if (IsGrounded == true && WallTouch == false) {
			this.GetComponent<Rigidbody2D>().gravityScale = 0f;
		} else {
			if(this.GetComponent<Rigidbody2D>().gravityScale != 1f){
				this.GetComponent<Rigidbody2D>().gravityScale = 1f;
			}
		}

		//Slowdown the player fall if touching a wall.
		if (IsGrounded == false && WallTouch == true) {
			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (this.GetComponent<Rigidbody2D>().velocity.x, Physics2D.gravity.y * 0.01f);
		}

		//If Ninja is in the air. Start to totate him back upwards after few frames.
		UpInTheAir_Counter += 1;
		if(UpInTheAir_Counter > 5){
			if (IsGrounded == false && WallTouch == false) {
				Vector2 RealDirectionV2 = new Vector2(this.transform.up.x,this.transform.up.y);
				Vector2 WorldUpVec = new Vector2(0f,1f);
				float TorqueTo = Vector2.Angle (WorldUpVec, RealDirectionV2);
				if (WorldUpVec.normalized.x > RealDirectionV2.normalized.x) {
					TorqueTo = TorqueTo * (-1);
				}
				if (-WorldUpVec.normalized.y > RealDirectionV2.normalized.y) {
					TorqueTo = TorqueTo * (-1);
				}
				this.GetComponent<Rigidbody2D>().AddTorque (TorqueTo * 400f * Time.deltaTime);
			}
		}

		//Lift player up if jump is happening.
		if (Btn_Jump_bool == true && JumpForceCount > 0) {
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,JumpForce);
			JumpForceCount -= 0.1f*Time.deltaTime;			
		}


		//This if-statement makes sure Ninja is not grounded to any platform when there is no collision detections. (In some cases OnCollisionExit messages might be lost. This makes sure that it will not cause a bug.)
		//START-------
		if (OnCollisionStayCounter == 0) {
			OnCollisionBugThreshold += 1;
		} else {
			OnCollisionStayCounter = 0;
		}
		
		if (OnCollisionBugThreshold > 4 && (IsGrounded == true || WallTouch == true)) {
			DJ_available = true;
			IsGrounded = false;
			WallTouch = false;
			this.transform.parent = null;
			GroundedToObjectsList.Clear();
			WalledToObjectsList.Clear();
			WallGripParticles.emissionRate = 0;
			FixStateTimer = 0;
			OnCollisionBugThreshold = 0;
			OnCollisionStayCounter = 1;
		}
		//--------END


		float AnimVelY = this.GetComponent<Rigidbody2D>().velocity.y;
		float AnimVelX = this.GetComponent<Rigidbody2D>().velocity.sqrMagnitude * 4f;

		if (JustPressedSpace > 0) {
			AnimVelX = 0f;
			JustPressedSpace -= 1;
		}

		if(Btn_Jump_bool == false && IsGrounded == true){
			//-- Set to zero to get run animation instead of fall animation
			AnimVelY = 0f;
		}

		//Send variables to Animation Controller
		AnimatorController.SetFloat ("HorizontalSpeed", AnimVelX);
		AnimatorController.SetFloat ("VerticalSpeed", AnimVelY);
		AnimatorController.SetBool ("Grounded", IsGrounded);
		AnimatorController.SetBool ("Walled", WallTouch);

	}






	void OnCollisionEnter2D(Collision2D coll) {

		if(coll.gameObject.CompareTag("Enemy")){
			return;
		}

		//This makes sure Ninja doesn't slide from previous force when hitting platform. Unless player is holding Left or Right button.
		if (IsGrounded == false && Btn_Left_bool == false && Btn_Right_bool == false) {
			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (this.GetComponent<Rigidbody2D>().velocity.x * 0.25f, -0.01f);
		} else if (IsGrounded == false) {
			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (this.GetComponent<Rigidbody2D>().velocity.x, -0.01f);	
		}

		OnCollisionStayCounter += 1;
		OnCollisionBugThreshold = 0;
		UpInTheAir_Counter = 0;

		foreach (ContactPoint2D contact in coll.contacts) {

			//If Ninja hits his head to the roof. Stop Jump Force.
			if (0.1f > contact.normal.y && ((contact.normal.x*contact.normal.x) < (0.85f*0.85f))) {
				JumpForceCount = 0f;
			}

			//If it wasn't the roof. Was it a ground perhaps?
			else if (contact.normal.x >= Ground_X_MIN && contact.normal.x <= Ground_X_MAX && contact.normal.y >= Ground_Y_MIN && contact.normal.y <= Ground_Y_MAX) {

				int CountHappenings = 0;
				foreach(GameObject GroundedObject in GroundedToObjectsList){
					if(contact.collider.gameObject.GetInstanceID() == GroundedObject.GetInstanceID()){
						CountHappenings += 1;
					}
				}

				//Is the platform already listed in GroundedObjects? If not Add it to the list.
				if(CountHappenings == 0){
					DJ_available = false;
					GroundedToObjectsList.Add(contact.collider.gameObject);
					//Move NinjaPlatformRoot to the new platform.
					this.transform.parent = null;
					NinjaPlatformRoot.transform.position = contact.collider.gameObject.transform.position;
					NinjaPlatformRoot.RootedTo = contact.collider.gameObject;
					this.transform.parent = NinjaPlatformRoot.transform;

					IsGrounded = true;

					this.GetComponent<Rigidbody2D>().AddForce (contact.normal * (-300f));

					if(WallTouch == true){
						WallGripParticles.emissionRate = 0;
						FixStateTimer = 0;
					}
				}

			//If it wasnt a roof or a ground it must have been wall. No need for Normal check anymore.
			}else{
				//Ninja must be faling downwards to grab the wall.
				if (this.GetComponent<Rigidbody2D>().velocity.y < 0f && IsGrounded == false) {
					this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					//is the Object already listed in WalledObjects?
					int CountHappenings = 0;
					foreach(GameObject WalledObject in WalledToObjectsList){
						if(contact.collider.gameObject.GetInstanceID() == WalledObject.GetInstanceID()){
							CountHappenings += 1;
						}
					}
					//if not. Lets list it.
					if(CountHappenings == 0){
						DJ_available = false;
						WalledToObjectsList.Add(contact.collider.gameObject);
						this.transform.parent = null;
						NinjaPlatformRoot.transform.position = contact.collider.gameObject.transform.position;
						NinjaPlatformRoot.RootedTo = contact.collider.gameObject;
						this.transform.parent = NinjaPlatformRoot.transform;
					
						WallTouch = true;
					
						//Check that the player is facing to the right direction
						if (contact.normal.x > 0) {
							PlayerLooksRight = true;
							MySpriteOBJ.transform.localScale = MySpriteOriginalScale;
						} else {
							PlayerLooksRight = false;
							MySpriteOBJ.transform.localScale = new Vector3 (-MySpriteOriginalScale.x, MySpriteOriginalScale.y, MySpriteOriginalScale.z);
						}
					
						//Start emiting smoke particles when touching the wall
						WallGripParticles.emissionRate = WallGripEmissionRate;

					}
				}
			}
		}
	}



	void OnCollisionStay2D(Collision2D coll) {

		OnCollisionStayCounter += 1;
		UpInTheAir_Counter = 0;

		//This is making sure that when Ninja is colliding with something it is always registered.
		if (IsGrounded == false && WallTouch == false) {
			FixStateTimer += 1;
			if(FixStateTimer > 4){
				foreach (ContactPoint2D contact in coll.contacts) {
					if (0.1f > contact.normal.y && ((contact.normal.x*contact.normal.x) < (0.85f*0.85f))) {
						JumpForceCount = 0f;
					}
					else if (contact.normal.x >= Ground_X_MIN && contact.normal.x <= Ground_X_MAX && contact.normal.y >= Ground_Y_MIN && contact.normal.y <= Ground_Y_MAX) {
						FixStateTimer = 0;
						DJ_available = false;
						GroundedToObjectsList.Add(contact.collider.gameObject);
						IsGrounded = true;
					}else{
						
						if (this.GetComponent<Rigidbody2D>().velocity.y < 0f) {
							FixStateTimer = 0;
							DJ_available = false;
							WalledToObjectsList.Add(contact.collider.gameObject);
							WallTouch = true;

							this.transform.parent = null;
							NinjaPlatformRoot.transform.position = contact.collider.gameObject.transform.position;
							NinjaPlatformRoot.RootedTo = contact.collider.gameObject;
							this.transform.parent = NinjaPlatformRoot.transform;
							
							if (contact.normal.x > 0) {
								PlayerLooksRight = true;
								MySpriteOBJ.transform.localScale = MySpriteOriginalScale;
							} else {
								PlayerLooksRight = false;
								MySpriteOBJ.transform.localScale = new Vector3 (-MySpriteOriginalScale.x, MySpriteOriginalScale.y, MySpriteOriginalScale.z);
							}
							
							//Start emiting smoke particles when touching the wall
							WallGripParticles.emissionRate = WallGripEmissionRate;
						}
					}
				}
			}
		}


		//OnStay Ground Events:
		else if (IsGrounded == true) {
			Vector2 NinjaStandDirection = Vector2.zero;
			foreach (ContactPoint2D contact in coll.contacts) {
				int CountHappenings = 0;
				foreach (GameObject GroundedObject in GroundedToObjectsList) {
					if (contact.collider.gameObject.GetInstanceID () == GroundedObject.GetInstanceID ()) {
						NinjaStandDirection += contact.normal;
						CountHappenings += 1;
					}
				}
				if (CountHappenings > 0) {
					NinjaStandDirection = NinjaStandDirection/CountHappenings;
					//This makes sure that Ninja doesn't walk on the walls.
					if((NinjaStandDirection.x > Ground_X_MAX || NinjaStandDirection.x < Ground_X_MIN) && (NinjaStandDirection.y > Ground_Y_MAX || NinjaStandDirection.y < Ground_Y_MIN)){
						this.GetComponent<Rigidbody2D>().AddForce(NinjaStandDirection * 100f);
					}else{
						//this Rotates the Ninja to allign with platform.
						Vector2 RealDirectionV2 = new Vector2(this.transform.up.x,this.transform.up.y);
						float TorqueTo = Vector2.Angle (NinjaStandDirection, RealDirectionV2);
						if (NinjaStandDirection.normalized.x > RealDirectionV2.normalized.x) {
							TorqueTo = TorqueTo * (-1);
						}
						if (-NinjaStandDirection.normalized.y > RealDirectionV2.normalized.y) {
							TorqueTo = TorqueTo * (-1);
						}
						this.GetComponent<Rigidbody2D>().AddTorque (TorqueTo * 1000f * Time.deltaTime);

						this.GetComponent<Rigidbody2D>().AddForce (NinjaStandDirection * (-300f));
					}
				}
			}


		//OnStay Wall Events:
		} else if (WallTouch == true) {

			foreach (ContactPoint2D contact in coll.contacts) {
				Vector2 NinjaWallDirection = Vector2.zero;
				int CountHappenings = 0;
				foreach (GameObject WallObject in WalledToObjectsList) {
					if (contact.collider.gameObject.GetInstanceID () == WallObject.GetInstanceID ()) {
						NinjaWallDirection += contact.normal;
						CountHappenings += 1;
					}
				}
				
				if (CountHappenings > 0) {
					NinjaWallDirection = NinjaWallDirection/CountHappenings;
					if((NinjaWallDirection.x > Ground_X_MAX || NinjaWallDirection.x < Ground_X_MIN) && (NinjaWallDirection.y > Ground_Y_MAX || NinjaWallDirection.y < Ground_Y_MIN)){

						if((Btn_Left_bool == false && PlayerLooksRight == false) || (Btn_Right_bool == false && PlayerLooksRight == true)){
							this.GetComponent<Rigidbody2D>().AddForce (NinjaWallDirection * -100f);
						}
						//this Rotates the Ninja to allign with the wall.
						Vector2 RealDirectionV2 = new Vector2(this.transform.up.x,this.transform.up.y);

						if(PlayerLooksRight == false){
							RealDirectionV2 = RotateThisVector(RealDirectionV2,1.35f);
						}else{
							RealDirectionV2 = RotateThisVector(RealDirectionV2,-1.35f);
						}

						float TorqueTo = Vector2.Angle (NinjaWallDirection, RealDirectionV2);
						if (contact.normal.x > RealDirectionV2.normalized.x) {
							TorqueTo = TorqueTo * (-1);
						}
						if (-contact.normal.y > RealDirectionV2.normalized.y) {
							TorqueTo = TorqueTo * (-1);
						}
						this.GetComponent<Rigidbody2D>().AddTorque (TorqueTo * 450f * Time.deltaTime);
					}else{
						if((Btn_Left_bool == false && PlayerLooksRight == false) || (Btn_Right_bool == false && PlayerLooksRight == true)){
							this.GetComponent<Rigidbody2D>().AddForce (NinjaWallDirection * 100f);
						}
					}
				}
			}
		}
	}
		

	//Here we check if the player is jumping or moving away from the wall or ground.
	void OnCollisionExit2D(Collision2D coll) {
		OnCollisionStayCounter = 0;
		foreach (ContactPoint2D contact in coll.contacts) {
			int CountHappenings = 0;
			int CountHappeningsWALL = 0;
			foreach(GameObject GroundedObject in GroundedToObjectsList){
				if(contact.collider.gameObject.GetInstanceID() == GroundedObject.GetInstanceID()){
					CountHappenings += 1;
				}
			}
			foreach(GameObject WalledObject in WalledToObjectsList){
				if(contact.collider.gameObject.GetInstanceID() == WalledObject.GetInstanceID()){
					CountHappeningsWALL += 1;
				}
			}

			//was the object one of the grounded to objects?
			if(CountHappenings > 0){
				GroundedToObjectsList.Remove(contact.collider.gameObject);
				if(GroundedToObjectsList.Count == 0){
					DJ_available = true;
					IsGrounded = false;
					this.transform.parent = null;
					FixStateTimer = 0;
				}
			}

			//was the object one of the wall?
			if(CountHappeningsWALL > 0){
				WalledToObjectsList.Remove(contact.collider.gameObject);
				if(WalledToObjectsList.Count == 0){
					if(NoNeedForSafeJump_bool == false){
						//This makes the walljump a bit easier. Player is able to do the wall jump even few miliseconds after he let go of the wall.
						walljump_count = 0.16f;
					}
					NoNeedForSafeJump_bool = false;
					DJ_available = true;
					this.transform.parent = null;
					WallTouch = false;
					WallGripParticles.emissionRate = 0;
					FixStateTimer = 0;
				}
			}
		}
	}


	public void NinjaDies(){
		Particles_DeathBoom.Emit (50);
	
		this.gameObject.transform.position = ActiveCheckpoint.transform.position;
		this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	
		//Send message to MainEventsLog. First checks if the reference path is set. If not, it will MainEventsLog from the scene.
		if(MainEventsLog_script == null){
			MainEventsLog_script = GameObject.FindGameObjectWithTag("MainEventLog").GetComponent<MainEventsLog>();
		}
		MainEventsLog_script.PlayerDied();
	}

	public void NinjaKilledEnemy(){
		GroundedToObjectsList.Clear ();
		WalledToObjectsList.Clear ();
	}

	//This region is for Button events. (These same events are called from Keyboard and Touch Buttons)
	#region ButtonVoids

	public void Button_Left_press(){
		Btn_Left_bool = true;
	}

	public void Button_Left_release(){
		Btn_Left_bool = false;
	}

	public void Button_Right_press(){
		Btn_Right_bool = true;
	}
		
	public void Button_Right_release(){
		Btn_Right_bool = false;
	}

	public void Button_Jump_press(){

		Btn_Jump_bool = true;
	
		//If you are on the ground. Do the Jump.
		if (IsGrounded == true) {
			DJ_available = true;
			AudioSource_Jump.Play();
			JumpForceCount = 0.02f;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,0f) + new Vector2(NinjaVisualRoot.transform.up.x,NinjaVisualRoot.transform.up.y)*JumpForce;

			JumpParticles_floor.Emit(20);

		//If you are in the air and DoubleJump is available. Do it!
		}else if(DoubleJump == true && DJ_available == true && WallTouch == false){
			DJ_available = false;
			AudioSource_Jump.Play();
			JumpForceCount = 0.02f;
			this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,JumpForce);
			JumpParticles_doublejump.Emit(10);
		}


		//If you touch the wall or just let go. And are defenitly not in the ground. Do the Wall Jump!
		if ((WallTouch == true || walljump_count > 0f) && IsGrounded == false) {

			//This is to fix the bug where Ninja was sometimes able to do double jump when leaving the wall.
			if(walljump_count <= 0f){
				NoNeedForSafeJump_bool = true;
			}

			DJ_available = true;
			AudioSource_Jump.Play();
			WallTouch = false;
			JumpForceCount = 0.02f;
			JumpParticles_wall.Emit(20);
			this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			if(PlayerLooksRight == false){
				this.GetComponent<Rigidbody2D>().AddForce (new Vector2 (-JumpForce*32f, 0f));
			}else{
				this.GetComponent<Rigidbody2D>().AddForce (new Vector2 (JumpForce*32f, 0f));
			}
		}
	}

	public void Button_Jump_release(){
		JumpForceCount = 0f;
		Btn_Jump_bool = false;
	}

	//This is a vector rotator. It can be used to rotate a Vector2 with an angle valua.
	private Vector3 RotateThisVector( this Vector2 v, float angle )
	{
		float sin = Mathf.Sin( angle );
		float cos = Mathf.Cos( angle );

		float tx = v.x;
		float ty = v.y;
		v.x = (cos * tx) - (sin * ty);
		v.y = (cos * ty) + (sin * tx);
		
		return v;
	}
	
	#endregion


}
