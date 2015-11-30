using UnityEngine;
using System.Collections;

public class CameraHandle : MonoBehaviour {
	
	public GameObject background;
	public float sensitivity = 3f;
	public float MinZoom = 2.7f;
	public float MaxZoom = 6.5f;

	private float scroll;

	private bool translating;
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

		//Zooming
		scroll = Input.GetAxis ("Mouse ScrollWheel") * sensitivity;
		if (scroll != 0 ) {
			if(Camera.main.orthographicSize < MinZoom)
				Camera.main.orthographicSize = MinZoom;
			else if(Camera.main.orthographicSize > MaxZoom)
				Camera.main.orthographicSize = MaxZoom;
			Camera.main.orthographicSize -= scroll;
			CorrectBound(true);
		}

		//Translating by Pressing and Releasing ScrollWheel
		if (Input.GetMouseButtonUp (2)) {
			translating = false;
		}
		else if (Input.GetMouseButtonDown (2) || translating) {
			translating = true;
			originalPosition = transform.position;
			newX = transform.position.x + Input.GetAxis("Mouse X");
			newY = transform.position.y + Input.GetAxis("Mouse Y");
			transform.localPosition = new Vector3(newX,newY,transform.position.z);
			if(CorrectBound(false))
				transform.localPosition = originalPosition;
		}

	}
}
