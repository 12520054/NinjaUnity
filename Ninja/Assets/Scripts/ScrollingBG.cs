using UnityEngine;
using System.Collections;

public class ScrollingBG : MonoBehaviour
{

    public float speed;
    public int bg_count;
    public float bg_width;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalValues.isPlayerRunning == true)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            Vector3 pos = transform.localPosition;
            if (pos.x < -bg_width)
            {
                pos.x = bg_width * bg_count;
                transform.localPosition = pos;
            }
        }
    }
}
