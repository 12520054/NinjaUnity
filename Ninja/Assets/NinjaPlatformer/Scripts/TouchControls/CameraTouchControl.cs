using UnityEngine;

//CameraTouchControl Sends touch events to OnScreen buttons.

public class CameraTouchControl : MonoBehaviour
{

    private GameObject lastGo;
    public static Vector3[] inputHitPos;
	public static Vector3[] DragPos;
    public static GameObject goPointedAt { get; private set; }

	public LayerMask IncludeThisLayer;

	void Start(){
		inputHitPos = new Vector3[20];
		DragPos = new Vector3[20];
	}

    void Update()
    {

        if (Input.touchCount > 0)
        {
			for(int i = 0; i < Input.touchCount; i++){

      	   	   	Touch touch = Input.GetTouch(i);

	            if (touch.phase == TouchPhase.Began)
    	        {
					Press(touch.position, i);
				}
				if (touch.phase == TouchPhase.Ended)
        	    {
					Release(touch.position, i);
				}
				if (touch.phase == TouchPhase.Moved)
				{
					RaycastDragPosition(touch.position, i);
				}

			}

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Press(Input.mousePosition, 0);
        }else if (Input.GetMouseButtonUp(0))
        {
            Release(Input.mousePosition, 0);
        }else if (Input.GetMouseButton(0))
		{
			RaycastDragPosition(Input.mousePosition, 0);
		}
    }

    private void Press(Vector2 screenPos, int TouchNumber)
    {
        lastGo = RaycastObject(screenPos, TouchNumber);
        if (lastGo != null)
        {
            lastGo.SendMessage("OnPress_IE", TouchNumber, SendMessageOptions.DontRequireReceiver);
        }
    }

	private void Release(Vector2 screenPos, int TouchNumber)
	{
		lastGo = RaycastObject(screenPos, TouchNumber);

		if (lastGo != null)
        {
			GameObject currentGo = RaycastObject(screenPos, TouchNumber);
			if (currentGo == lastGo) lastGo.SendMessage("OnClick_IE", SendMessageOptions.DontRequireReceiver);
			lastGo.SendMessage("OnRelease_IE", TouchNumber, SendMessageOptions.DontRequireReceiver);
            lastGo = null;
        }
    }

	private GameObject RaycastObject(Vector2 screenPos, int TouchNumber)
	{
        RaycastHit info;
		if (Physics.Raycast(this.GetComponent<Camera>().ScreenPointToRay(screenPos), out info, 200,IncludeThisLayer))
		{
			inputHitPos[TouchNumber] = info.point;
			DragPos[TouchNumber] = info.point;
            return info.collider.gameObject;
        }

        return null;
    }

	private void RaycastDragPosition(Vector2 screenPos, int TouchNumber){
		
		RaycastHit info;
		if (Physics.Raycast(this.GetComponent<Camera>().ScreenPointToRay(screenPos), out info, 200,IncludeThisLayer))
		{
			DragPos[TouchNumber] = info.point;
		}
		
	}



}
