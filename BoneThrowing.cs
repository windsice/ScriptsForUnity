using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoneThrowing : MonoBehaviour {
	
	[SerializeField] private Text levelIndication;
	[SerializeField] private Text SensitivityDisplay;
	[SerializeField] private Slider SensitivitySlider;
	public GameObject dog;
	public Canvas restartSreen;
	private Dog dogScript;

	private bool grounded;
	private bool rotate;

	public bool ReadyToThrow;
	public bool pastLevel;
	public bool GotCatch;
	public float groundColliderOffsetX;
	public float groundColliderOffsetY;
	public float rotation = 10f;
	public float MinTimeDelta = 0.1f;
	public float MaxTimeDelta = 0.5f;

	private float jumpForcex;
	private float jumpForcey;
	private float TimeStartThrow;
	
	private float mouseXi,mouseYi;
	private float sensitivity = 0.005f;

	private Rigidbody2D bone;

	void Start(){
		bone = GetComponent<Rigidbody2D> ();
		dogScript = dog.GetComponent<Dog> ();
	}

	void OnCollisionStay2D(Collision2D hit)
	{
		grounded = true;
	}

	void OnCollisionEnter2D(Collision2D hit)
	{
		rotate = false;
	}

	void OnCollisionExit2D(Collision2D hit)
	{
		grounded = false;
	}

	// Update is called once per frame
	void Update () {

		if (rotate) {
			if(bone.velocity.x < 0)
				this.transform.Rotate (new Vector3 (0, 0, rotation));
			else
				this.transform.Rotate (new Vector3 (0, 0, -rotation));
		}

		if (Input.GetMouseButtonDown (0) && ReadyToThrow) {
			mouseXi = Input.mousePosition.x;
			mouseYi = Input.mousePosition.y;
			TimeStartThrow = Time.time;
		}

		if (Input.GetMouseButtonUp (0) && ReadyToThrow) {
			if(grounded == true && Mathf.Abs(bone.velocity.x) < 0.01)
			{
				float timeDelta = Mathf.Lerp(MinTimeDelta,MaxTimeDelta,Time.time-TimeStartThrow);
				jumpForcex = (Input.mousePosition.x - mouseXi)*sensitivity/timeDelta;
				jumpForcey = (Input.mousePosition.y - mouseYi)*sensitivity/timeDelta;
				if(jumpForcey < 1) jumpForcey = 1;	//updating ground
			}
		}

		//Bone Missed
		if (grounded == true && Mathf.Abs(bone.velocity.x) < 0.01 && !GotCatch && !restartSreen.enabled) {
			dogScript.MissThrowFace.SetActive(true);
			GotCatch = true;	//just reuse the existing bool to prevent invoking the screen.
			Invoke("PopScreen",1f);
		}
	
		//update sensitivity display and value
		sensitivity = SensitivitySlider.value;
		SensitivityDisplay.text = "Sensitivity: " + sensitivity;
	}

	void PopScreen(){
		restartSreen.enabled = true;
	}

	void FixedUpdate(){
		if (jumpForcex > 0 || jumpForcey > 0 && ReadyToThrow) {
			bone.AddForce (new Vector2 (jumpForcex, jumpForcey), ForceMode2D.Impulse);
			jumpForcex = jumpForcey = 0;
			rotate = true;
			ReadyToThrow = false;
			GotCatch = false;
		}

		if (transform.position.y > levelIndication.transform.position.y)
			pastLevel = true;

	}
}
