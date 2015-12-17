using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dog : MonoBehaviour {

	private const string line = "--------------------------------------------------------------------------";
	
	[SerializeField] private Text levelIndication;
	[SerializeField] private Text ScoreDisplay;
	[SerializeField] private Text BestScoreDisplay;
	public GameObject bone;
	private BoneThrowing boneScript;
	public GameObject background;
	private BoxCollider2D boneCollider;
	private BoxCollider2D dogCollider;

	public GameObject MissThrowFace;

	public float PlaceBoneOffsetY = 0.6f;
	public float PlaceBoneOffsetX = 1.1f;
	public float PlaceBoneTime = 0.27f;
	public float MovementSpeed = 5.0f;

	private Animator anim;
	private bool GotBone;
	
	private float rightBound;
	private float leftBound;
	private float Randomized_X;
	private float DogBodyLength = 1.2f;
	private float AcceptableRange = 0.1f;

	private int level;
	private int score;
	private const int MAXLEVEL = 10;
	private const int SCORESINLEVEL = 3;
	private const float LEVELHEIGHT = 0.8f;

	void Start(){
		anim = GetComponent<Animator>();
		boneCollider = bone.GetComponent<BoxCollider2D> ();
		boneScript = bone.GetComponent<BoneThrowing> ();
		dogCollider = GetComponent<BoxCollider2D> ();

		reStart ();

		leftBound = background.GetComponent<SpriteRenderer>().bounds.center.x - background.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f + DogBodyLength;
		rightBound = background.GetComponent<SpriteRenderer>().bounds.center.x + background.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f - DogBodyLength;
	}

	void PlaceBone(){
		//Place bone in the new location
		bone.transform.rotation = new Quaternion ();
		if(transform.localScale.x < 0)
			bone.transform.position = new Vector3 
				(transform.position.x-PlaceBoneOffsetX, transform.position.y-PlaceBoneOffsetY, transform.position.z);
		else
			bone.transform.position = new Vector3 
				(transform.position.x+PlaceBoneOffsetX, transform.position.y-PlaceBoneOffsetY, transform.position.z);
		bone.SetActive (true);

		// Get Ready to Run
		// 1. get next location
		// 2. update animation
		// 3. ignore collision
		Randomized_X = Random.Range (leftBound, rightBound);
		while(Mathf.Abs(Randomized_X - bone.transform.position.x) < DogBodyLength*3 || Mathf.Abs(Randomized_X - bone.transform.position.x) > 10f+level*2.0f)
			Randomized_X = Random.Range (leftBound, rightBound);
		GotBone = true;

		anim.SetTrigger ("Run");
		
		Physics2D.IgnoreCollision (boneCollider, dogCollider, true);
	}

	void OnCollisionEnter2D(Collision2D hit)
	{
		//if the dog got the bone, and the bone did past the height level
		if (hit.gameObject == bone && boneScript.pastLevel) {

			anim.SetTrigger("GotBone");
			bone.SetActive(false);
			Invoke("PlaceBone",PlaceBoneTime);
			boneScript.GotCatch = true;

			//increment score, update levelIndication if score met
			score++;
			if(score % SCORESINLEVEL == 0 && level <= MAXLEVEL && score != 0){
				level++;
				levelIndication.transform.position = new Vector3(levelIndication.transform.position.x,levelIndication.transform.position.y + LEVELHEIGHT,levelIndication.transform.position.z);
			}

			//update score in the restart screen
			ScoreDisplay.text = "Score: " + score;
			BestScoreDisplay.text = "Best: " + score;
		}
	}

	void Run(){
		//running
		if (transform.position.x > Randomized_X) {
			transform.Translate (new Vector3 (-Time.deltaTime * MovementSpeed, 0, 0));
			transform.localScale = new Vector3 (-Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);
		} else {
			transform.Translate (new Vector3 (Time.deltaTime * MovementSpeed, 0, 0));
			transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}

		//stop
		if (Mathf.Abs (transform.position.x - Randomized_X) < AcceptableRange) {
			GotBone = false;
			anim.SetTrigger("GotBone");
			Physics2D.IgnoreCollision (boneCollider, dogCollider, false);
			boneScript.pastLevel = false;
			boneScript.ReadyToThrow = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if (GotBone) {
			Run ();
			return;
		}

		//Facing the bone
		if (bone.transform.position.x > transform.position.x)
			transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);
		else
			transform.localScale = new Vector3 (-Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);

		//update levelIndication
		if (level <= MAXLEVEL)
			levelIndication.text = "Level " + level + " " + line;
		else
			levelIndication.text = score + " " + line;
	}

	public void reStart(){
		bone.transform.position = new Vector3 (Randomized_X, -3.35f, bone.transform.position.z);
		transform.position = new Vector3 (Randomized_X, -7.635269f, transform.position.z);
		score = -1;
		level = 1;
		levelIndication.transform.position = new Vector3 (-11f,-8.7f,levelIndication.transform.position.z);
		boneScript.GotCatch = true;
		MissThrowFace.SetActive (false);
		boneScript.restartSreen.enabled = false;
	}
}

