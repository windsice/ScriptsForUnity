using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	public GameObject character;

	// Update is called once per frame
	void Update () {
	
		if (character.transform.position.x > transform.position.x) {
			transform.position = new Vector2(character.transform.position.x,transform.position.y);
		}
	}
}
