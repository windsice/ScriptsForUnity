using UnityEngine;
using System.Collections;

public class CameraHandle : MonoBehaviour {

	public GameObject background;
	public const float SENSITIVITY = 3f;
	public const float MINZOOM = 2.7f;
	public const float MAXZOOM = 6.5f;
	private const float ZOOMSPEED = 5.0f;
	private const float TRANSLATESPEED = 5.0f;

	private float scroll;
	private float scrollTarget;

	public bool translating;
	private Vector3 originalPosition;
	private float newX,newY;
	
	private float CameraScaleY;
	private float CameraScaleX;
	private float BGrightBound,CameraRightBound;
	private float BGleftBound,CameraLeftBound;
	private float BGtopBound,CameraTopBound;
	private float BGbottomBound,CameraBottomBound;

	void Start() {

		BGleftBound = background.GetComponent<SpriteRenderer>().bounds.center.x - background.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f;
		BGrightBound = background.GetComponent<SpriteRenderer>().bounds.center.x + background.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f;
		BGbottomBound = background.GetComponent<SpriteRenderer>().bounds.center.y - background.GetComponent<SpriteRenderer>().bounds.size.y / 2.0f;
		BGtopBound = background.GetComponent<SpriteRenderer>().bounds.center.y + background.GetComponent<SpriteRenderer>().bounds.size.y / 2.0f;

		CameraScaleY = 1.0f;
		CameraScaleX = CameraScaleY * Camera.main.aspect;

		scrollTarget = Camera.main.orthographicSize;
		translating = false;
		newX = transform.position.x;
		newY = transform.position.y;
	
	}
	
	//Correct for view out of boundary
	//when not CorrectingBound, function simply tell now is out of bound of not.
	//	otherwise, tell and correct.
	bool CorrectBound(bool CorrectingBound){
		bool OutOfBound = false;
		CameraRightBound = Camera.main.orthographicSize * CameraScaleX + transform.position.x;
		CameraLeftBound = Camera.main.orthographicSize * CameraScaleX - transform.position.x;
		CameraTopBound = Camera.main.orthographicSize * CameraScaleY + transform.position.y;
		CameraBottomBound = Camera.main.orthographicSize * CameraScaleY - transform.position.y;

		if ( CameraRightBound > Mathf.Abs(BGrightBound)){
			if(CorrectingBound)
				transform.position = new Vector3(
					transform.position.x - (CameraRightBound - Mathf.Abs(BGrightBound)),
					transform.position.y,transform.position.z);
			OutOfBound =  true;
		}
		if (CameraLeftBound > Mathf.Abs (BGleftBound)) {
			if(CorrectingBound)
				transform.position = new Vector3(
					transform.position.x + (CameraLeftBound - Mathf.Abs(BGleftBound)),
					transform.position.y,transform.position.z);
			OutOfBound =  true;
		}
		if (CameraTopBound > Mathf.Abs (BGtopBound)) {
			if(CorrectingBound)
				transform.position = new Vector3(
					transform.position.x,
					transform.position.y - (CameraTopBound - Mathf.Abs(BGtopBound)),
					transform.position.z);
			OutOfBound =  true;
		}
		if (CameraBottomBound > Mathf.Abs (BGbottomBound)) {			
			if(CorrectingBound)
			transform.position = new Vector3(
				transform.position.x,
					transform.position.y + (CameraBottomBound - Mathf.Abs(BGbottomBound)),
				transform.position.z);
			OutOfBound =  true;
		}
		return OutOfBound;
	}

	// Update is called once per frame
	void Update () {

		if (!Input.touchSupported) {
			//Zooming
			scroll = Input.GetAxis ("Mouse ScrollWheel") * SENSITIVITY;
			if (scroll != 0) {
		
				scrollTarget = Camera.main.orthographicSize - scroll*ZOOMSPEED;

			}else if(Mathf.Abs(scrollTarget - Camera.main.orthographicSize) > 0.1f)
			{
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,scrollTarget,Time.deltaTime*ZOOMSPEED);
				
				if (Camera.main.orthographicSize < MINZOOM)
					Camera.main.orthographicSize = MINZOOM;
				else if (Camera.main.orthographicSize > MAXZOOM)
					Camera.main.orthographicSize = MAXZOOM;
				
				CorrectBound (true);
				newX = transform.position.x;
				newY = transform.position.y;
			}
			 
			//Translating by Pressing and Releasing ScrollWheel
			if (Input.GetMouseButtonUp (2)) {
				translating = false;
			} else if (Input.GetMouseButtonDown (2)) {
				translating = true;
			} else if(translating){
				newX = transform.position.x + Input.GetAxis ("Mouse X")*TRANSLATESPEED;
				newY = transform.position.y + Input.GetAxis ("Mouse Y")*TRANSLATESPEED;
			}

			if(Mathf.Abs(transform.position.x - newX) > 0.1f || Mathf.Abs(transform.position.y - newY) > 0.1f)
			{
				originalPosition = transform.position;
				transform.localPosition = new Vector3 (Mathf.Lerp(transform.position.x,newX,Time.deltaTime*TRANSLATESPEED), 
				                                       Mathf.Lerp(transform.position.y,newY,Time.deltaTime*TRANSLATESPEED), 
				                                       transform.position.z);
				if (CorrectBound (false))
					transform.localPosition = originalPosition;
			}

		} else {
			if(Input.touchCount == 2){
				translating = true;
				originalPosition = transform.position;
				Touch touch0 = Input.GetTouch(0);
				Touch touch1 = Input.GetTouch(1);

				//Determine is zooming or translating
				//if two fingers are moving in the same vector then is translating,
				Vector2 translateTolerance;
				translateTolerance.x = translateTolerance.y = 1.0f;
				if(Mathf.Abs(touch0.deltaPosition.x - touch1.deltaPosition.x) < translateTolerance.x &&
				   Mathf.Abs(touch0.deltaPosition.y - touch1.deltaPosition.y) < translateTolerance.y)
				{
					newX = transform.position.x + touch0.deltaPosition.x * 0.5f;
					newY = transform.position.y + touch1.deltaPosition.y * 0.5f;
					transform.localPosition = new Vector3(newX,newY,transform.position.z);
					if (CorrectBound (false))
						transform.localPosition = originalPosition;
				}

				//else is zooming.
				else {
					//Get difference in magniture
					Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
					Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

					float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
					float touchDeltaMag = (touch0.position-touch1.position).magnitude;

					float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
					
					Camera.main.orthographicSize += deltaMagnitudeDiff * 0.02f;

					if (Camera.main.orthographicSize < MINZOOM)
						Camera.main.orthographicSize = MINZOOM;
					else if (Camera.main.orthographicSize > MAXZOOM)
						Camera.main.orthographicSize = MAXZOOM;

					CorrectBound (true);
				}


			} else if(Input.touchCount == 0){
				translating = false;
			}
		}

	}
}
