using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {


    public GameObject[] platform2D;

    private static int yPos = -8;
    private bool generateObject = false;

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player" && generateObject == false) {
            generateObject = true;
            Instantiate(platform2D[Random.Range(0, 5)], 
                new Vector3(transform.position.x + 40, yPos, 0), 
                transform.rotation);
        }
    }
}
