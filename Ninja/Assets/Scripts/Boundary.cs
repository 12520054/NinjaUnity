using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {


    public void OnTriggerExit2D(Collider2D collider2D)
    {        
         Destroy(collider2D.gameObject);

    }

}
