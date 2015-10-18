using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject play;
    public GameObject player;
    public Text jText;
    private int jScore = 0;
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && GlobalValues.isPlayerRunning == true)
        {
            jScore += 1;
            jText.GetComponent<Text>().text = jScore.ToString() + ".J";
        }
    }

    public void onPlayButtonClick()
    {
        GlobalValues.isPlayerRunning = true;
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(5000, 1000));
        play.SetActive(false);
    }
}
