using UnityEngine;
using System.Collections;

public class CameraHandle : MonoBehaviour {
	
	public GameObject background;
	public float sensitivity = 3f;
	public float MinZoom = 3f;

	private float scroll;

	private bool translating;
	private Vector3 originalPosition;
	private float newX,newY;
	
	private float CameraScaleY;
	private float CameraScaleX;
	private float rightBound;
	private float leftBound;
	private float topBound;
	private float bottomBound;

	void Start() {

		leftBound = background.GetComponent<SpriteRenderer>().bounds.center.x - background.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f;
		rightBound = background.GetComponent<SpriteRenderer>().bounds.center.x + background.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f;
		bottomBound = background.GetComponent<SpriteRenderer>().bounds.center.y - background.GetComponent<SpriteRenderer>().bounds.size.y / 2.0f;
		topBound = background.GetComponent<SpriteRenderer>().bounds.center.y + background.GetComponent<SpriteRenderer>().bounds.size.y / 2.0f;

		CameraScaleY = 1.0f;
		CameraScaleX = CameraScaleY * Camera.main.aspect;
	
		Debug.Log (leftBound);
		Debug.Log (rightBound);
		Debug.Log (bottomBound);
		Debug.Log (topBound);
	}
	
	//Correct for view out of boundary
	bool OutOfBound(){
		if (Camera.main.orthographicSize * CameraScaleX + transform.position.x > Mathf.Abs(rightBound))
			return true;
		if (Camera.main.orthographicSize * CameraScaleX - transform.position.x > Mathf.Abs(leftBound))
			return true;
		if (Camera.main.orthographicSize * CameraScaleY + transform.position.y > Mathf.Abs(topBound))
			return true;
		if (Camera.main.orthographicSize * CameraScaleY - transform.position.y > Mathf.Abs(bottomBound))
			return true;
		return false;
	}

	// Update is called once per frame
	void Update () {

		//Zooming
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0 ) {
			Camera.main.orthographicSize -= scroll * sensitivity;
			if(OutOfBound())
				Camera.main.orthographicSize += scroll * sensitivity;
			else if(Camera.main.orthographicSize < MinZoom)
				Camera.main.orthographicSize = MinZoom;
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
			if(OutOfBound())
				transform.localPosition = originalPosition;
		}

	}
}
