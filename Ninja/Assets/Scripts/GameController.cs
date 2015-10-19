using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    
    public GameObject playButton;
    public GameObject player;
    
    public void onPlayButtonClick()
    {
        GetComponent<AudioSource>().Play();
        GlobalValues.isPlayerRunning = true;
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(10000, 0));
        playButton.SetActive(false);
    }

    public void Replay()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void ExitApp()
    {       
        Application.Quit();
    }
}
