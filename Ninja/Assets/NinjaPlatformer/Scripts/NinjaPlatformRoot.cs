using UnityEngine;
using System.Collections;

public class NinjaPlatformRoot : MonoBehaviour {

	public GameObject RootedTo;

	void Update () {
		if (RootedTo != null) {
			this.transform.position = RootedTo.transform.position;	
		}
	}
}
