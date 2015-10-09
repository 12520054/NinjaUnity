using UnityEngine;
using System.Collections;

//MainEvents can keep track of Souls and Player deaths.

public class MainEventsLog : MonoBehaviour
{

    public int SoulsCollected;

    public NinjaMovementScript NinjaMovScript;
    public GameObject TouchControls;

    public CameraFollowTarget CameraFollowScript;

    public AudioSource AudioSource_Collectible;
    public AudioSource AudioSource_Death;



    public void PlayerCollectedSoul()
    {
        AudioSource_Collectible.Play();
        Debug.Log("Soul collected");
        SoulsCollected += 1;
    }


    public void PlayerDied()
    {

        //Camera will stop for a second when the player dies.
        CameraFollowScript.PlayerDied();

        AudioSource_Death.Play();
        Debug.Log("Player died");
    }
}
