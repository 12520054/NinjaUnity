using UnityEngine;
using System.Collections;

//This is a simple script that can make objects auto-rotate

public class AutoRotator : MonoBehaviour {

	public float RotationSpeed;
	
	void Update () {
		this.transform.Rotate (new Vector3 (0f, 0f, RotationSpeed*Time.deltaTime));
	}
}
