using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    
    public GameObject playButton;
    public GameObject player;
    public AudioClip startRun;
    public GameObject pausePanel;

    public void onPlayButtonClick()
    {
        GetComponent<AudioSource>().PlayOneShot(startRun);
        GlobalValues.isPlayerRunning = true;
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(10000, 0));
        playButton.SetActive(false);
    }

    public void Replay()
    {
        Time.timeScale = 1.0f;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void ExitApp()
    {       
        Application.Quit();
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void PauseGame()
    {
        if (GlobalValues.isPlayerRunning)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
