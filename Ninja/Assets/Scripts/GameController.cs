using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using StartApp;

public class GameController : MonoBehaviour {
    
    public GameObject playButton;
    public GameObject player;
    public AudioClip startRun;
    public GameObject pausePanel;

    public void Start()
    {
        Time.timeScale = 1.0f;
#if UNITY_ANDROID
        StartAppWrapper.addBanner(
              StartAppWrapper.BannerType.AUTOMATIC,
              StartAppWrapper.BannerPosition.BOTTOM);
#endif
    }

    public void onPlayButtonClick()
    {
        GetComponent<AudioSource>().PlayOneShot(startRun);
        GlobalValues.isPlayerRunning = true;
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(10000, 0));
        playButton.SetActive(false);
    }

    public void Replay()
    {
        GlobalValues.isPlayerRunning = false;
        Application.LoadLevel(Application.loadedLevel);
    }
    public void ExitApp()
    {       
        Application.Quit();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }
    public void PauseGame()
    {
        if (GlobalValues.isPlayerRunning)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void RateApp()
    {
        Application.OpenURL("http://khuyenkhichsangtaoviet.net/chi_tiet_ung_dung_so_khao/58");
    }
}
