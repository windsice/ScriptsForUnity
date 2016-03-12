using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Throwing : MonoBehaviour {

	[SerializeField]
	private Text MikuStatus;

	private bool grounded = false;
	private Animator anim;
	public bool jump = false;
	public float jumpForcex = 3;
	public float jumpForcey = 5;
	public int movementSpeed = 4;

	private float mouseXi,mouseYi;



	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	void OnCollisionEnter2D(Collision2D hit)
	{
		grounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.touchCount == 1){
		if (Input.GetMouseButtonDown (0)) {
			mouseXi = Input.mousePosition.x;
			mouseYi = Input.mousePosition.y;
		}

		//if(Input.GetButtonDown("Fire1")){
		if (Input.GetMouseButtonUp (0)) {
			if(grounded == true && GetComponent<Rigidbody2D>().velocity.x < 0.01)
			{
				//jumpForcex = Mathf.Abs (Input.GetTouch (0).deltaPosition.x + 3);
				//jumpForcey = Mathf.Abs (Input.GetTouch (0).deltaPosition.y + 5);
				jumpForcex = Mathf.Abs (Input.mousePosition.x - mouseXi)/40;
				jumpForcey = Mathf.Abs (Input.mousePosition.y - mouseYi)/40;
				if(jumpForcey < 1) jumpForcey = 1;	//updating ground
				jump = true;
				grounded = false;
				anim.SetTrigger("Jump");
			}
		}
	}

	void FixedUpdate(){
		if (jump) {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(jumpForcex,jumpForcey),ForceMode2D.Impulse);
			//GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.acceleration.x,Input.acceleration.y),ForceMode2D.Impulse);
			jump = false;
		}
		else if (grounded) {
			if(GetComponent<Rigidbody2D>().velocity.x > 3)
			GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,0),ForceMode2D.Impulse);
		}

		MikuStatus.text = "X Velocity: " + GetComponent<Rigidbody2D> ().velocity.x + "\n";
		MikuStatus.text += "Y Velocity: " + GetComponent<Rigidbody2D> ().velocity.y + "\n";
	}
}
