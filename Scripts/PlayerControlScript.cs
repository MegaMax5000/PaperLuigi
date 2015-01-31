using UnityEngine;
using System.Collections;

public class PlayerControlScript : MonoBehaviour 
{
	
	CharacterController cc;

	public GameObject luigi;
	public GameObject skin;
	public Renderer leftboot;
	public Renderer rightboot;
	public Renderer rightfist;
	public GameObject dust;

	public bool isGrounded;
	public float slowMovementSpeed = 6;
	public float jumpPower = 1f;
	float movementSpeed;
	float forwardSpeed = 0.0f;
	float sideSpeed = 0.0f;
	float vertVelocity = 0;
	float hammerCooldown = 0;

	bool running = false;
	bool jumpPressed = false;
	bool hammerPressed = false;
	bool canMove = true;
	string currDirection = "left";
	
	// Use this for initialization
	void Start () 
	{
		cc = GetComponent<CharacterController> ();

		isGrounded = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (hammerCooldown > 0)
			hammerCooldown -= Time.deltaTime;
		if(luigi.transform.localRotation.y > .5f)
			currDirection = "right";
		else
			currDirection = "left";
		if(canMove)
		{
			if (cc.isGrounded)
				isGrounded = true;
			else
				isGrounded = false;
			
			luigi.GetComponent<AnimationControl> ().SetAnimationBool ("GROUNDED", isGrounded);
			
			// Movement

			if (Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f
			    || Input.GetAxis ("Horizontal") > 0.1f || Input.GetAxis ("Horizontal") < -0.1f) 
			{
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUN", true);
			} 
			else
			{
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUN", false);
			}

			//// Player runs fast if left shift is down
			if(!Input.GetKey(KeyCode.LeftShift) && luigi.GetComponent<AnimationControl> ().GetAnimationBool("RUN"))
			{
				movementSpeed = slowMovementSpeed * 1.6f;
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUNFAST", true);
			}
			else
			{
				movementSpeed = slowMovementSpeed;
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUNFAST", false);
			}

			//// Sets forward and side speed
			/// 
			if(Input.GetKey("w"))
				forwardSpeed = movementSpeed * Time.deltaTime; 
			else if(Input.GetKey("s"))
				forwardSpeed = -1f * movementSpeed * Time.deltaTime;
			else
				forwardSpeed = 0f;
			if(Input.GetKey("a") && Input.GetKey("d"))
			    sideSpeed = 0;
			else if(Input.GetKey("d"))
			{
				sideSpeed = movementSpeed * Time.deltaTime;
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RIGHT", true);
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("LEFT", false);
			}
			else if(Input.GetKey("a"))
			{
				sideSpeed = -1f * movementSpeed * Time.deltaTime;
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("LEFT", true);
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RIGHT", false);
			}
			else if(!Input.GetKeyDown("a") && !Input.GetKeyDown("d"))
			{
				sideSpeed = 0f;
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RIGHT", false);
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("LEFT", false);
			}
			// If player is on the ground, it can initiate jump or hammer
			if(isGrounded)
			{
				vertVelocity = -.1f;
				if(Input.GetKeyDown("space") && !jumpPressed)
				{
					jumpPressed = true;
					luigi.GetComponent<AnimationControl> ().SetAnimationBool ("JUMP", true);
					vertVelocity = jumpPower;
				}
				if(Input.GetButtonDown("Fire1") && !hammerPressed && hammerCooldown <= 0f)
				{
					hammerPressed = true;
					StartCoroutine(Hammer ());
				}
			}
			else
			{
				jumpPressed = false;
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("JUMP", false);
				vertVelocity += Physics.gravity.y * Time.deltaTime;
			}

			// Direction Player is facing

			if (Input.GetKey("a") && Input.GetKey("d")){
				luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUN", false);
			}
			else if(sideSpeed < 0)
			{
				if(currDirection == "right")
				{
					StartCoroutine(FaceLeft ());
					currDirection = "left";
				}
			} 
			else if(sideSpeed > 0)
			{
				if(currDirection == "left")
				{
					StartCoroutine(FaceRight ());
					currDirection = "right";
				}
			}

			// Double check

			if(sideSpeed > 0 && currDirection == "left")
				StartCoroutine(MakeLeft());
			else if(sideSpeed < 0 && currDirection == "right")
				StartCoroutine(MakeRight());

			// Moving the Player

			Vector3 speed = new Vector3 (sideSpeed, vertVelocity, forwardSpeed);
			speed = transform.rotation * speed;
			cc.Move(speed);
		}
	}
	
	IEnumerator Hammer()
	{
		rightfist.sortingOrder = 17;
		rightboot.sortingOrder = 0;
		leftboot.sortingOrder = 25;
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("HAMMER", true);
		canMove = false;
		yield return new WaitForSeconds (.25f);
		rightfist.sortingOrder = 7;
		rightboot.sortingOrder = 9;
		leftboot.sortingOrder = 20;
		canMove = true;
		hammerPressed = false;
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("HAMMER", false);
		hammerCooldown = .5f;
	}
	
	IEnumerator FaceLeft()
	{
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUN", false);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("TURN", true);
		yield return new WaitForSeconds (.1f);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUN", true);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("TURN", false);
	}
	
	IEnumerator FaceRight()
	{
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUN", false);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("TURN", true);
		yield return new WaitForSeconds (.1f);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RUN", true);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("TURN", false);
	}

	IEnumerator MakeLeft()
	{
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("LEFT", true);
		yield return new WaitForSeconds (.25f);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("LEFT", false);
	}

	IEnumerator MakeRight()
	{
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RIGHT", true);
		yield return new WaitForSeconds (.25f);
		luigi.GetComponent<AnimationControl> ().SetAnimationBool ("RIGHT", false);
	}
}






