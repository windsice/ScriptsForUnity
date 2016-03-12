using UnityEngine;
using System.Collections;

public class BackgroundShift : MonoBehaviour {

	public GameObject character;
	public GameObject positionObject;
	public float shift = 33.26f;

	// Update is called once per frame
	void Update () {
		//when character pass certain postion, this background shift
		if (character.transform.position.x > positionObject.transform.position.x)
			transform.position = new Vector2 (transform.position.x + shift, transform.position.y);
	}
}
