using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Help : MonoBehaviour {

	public Button helpButton;
	public Button playButton;

	public GameObject currentPicture;
	public GameObject nextPicture;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void initHelp() {
		helpButton.interactable = false;
		playButton.interactable = false;
		currentPicture.SetActive (true);
	}

	public void nextHelp(){
		currentPicture.SetActive (false);
		nextPicture.SetActive (true);
	}

	public void endHelp(){
		currentPicture.SetActive (false);
		helpButton.interactable = true;
		playButton.interactable = true;
	}
}
