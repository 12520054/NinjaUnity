using UnityEngine;
using System.Collections;

//Add this script to the platform you want to move.
public class MovingPlatform : MonoBehaviour {

	//Platform movement speed.
	public float speed;

	//This is the position where the platform will move.
	public Transform MovePosition;

	//Some private variables making the code work :)
	private Vector3 StartPosition;
	private Vector3 EndPosition;
	private bool OnTheMove;

	// Use this for initialization
	void Start () {
		//Store the start and the end position. Platform will move between these two points.
		StartPosition = this.transform.position;
		EndPosition = MovePosition.position;
	}
	
	void FixedUpdate () {
	
		float step = speed * Time.deltaTime;

		if (OnTheMove == false) {
			this.transform.position = Vector3.MoveTowards (this.transform.position, EndPosition, step);
		}else{
			this.transform.position = Vector3.MoveTowards (this.transform.position, StartPosition, step);
		}

		//When the platform reaches end. Start to go into other direction.
		if (this.transform.position.x == EndPosition.x && this.transform.position.y == EndPosition.y && OnTheMove == false) {
			OnTheMove = true;
		}else if (this.transform.position.x == StartPosition.x && this.transform.position.y == StartPosition.y && OnTheMove == true) {
			OnTheMove = false;
		}
	}
	
	

}
