using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterBehavior : DeadlyBehavior {
	
	public KeyCode Summon = KeyCode.Q;
	public KeyCode ShiftLeft = KeyCode.E;
	public KeyCode ShiftRight = KeyCode.R;
	public KeyCode jump = KeyCode.W;
	public KeyCode crouch = KeyCode.S;
	
	public GameObject[] Minions;
	public int[] currentMats;
	public GameObject[] Demons;
	private int selected = 0;
	public static GameObject activeCheckpoint;
	public int maxMins = 5;
	public GameObject PlayerPrefab;
	
	Animator anim;
	
	//player rigidbody and collider
	private Rigidbody2D rb;
	private BoxCollider2D bc;
	
	//player heights
	private float crouchHeight = 1;
	private float standHeight = 2;
	private float heightChange;
	
	// player crouch/jump info
	private bool isCrouched = false;
	private bool jumpNow = false;
	private float groundRadius = .1f;
	public LayerMask whatIsGrounded;
	public Transform[] groundChecks;
	public int speed = 10;//change in editor not here
	public int jumpspeed = 10;//change in editor not here
	
	public static Vector2 Dir;
	public static bool FacingRight;
	

	
	public override void Start () {
		base.Start ();
		FacingRight = true;
		rb = GetComponent<Rigidbody2D> ();
		Dir = Vector2.right;
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<Collider2D>() as BoxCollider2D;
		heightChange = (crouchHeight / standHeight) * bc.size.y;
	}
	
	
	// Use this for initialization
	
	
	public struct summMaterials
	{
		public GameObject mat;
		public int numOfMat;
	}
	
	
	public override void Update()
	{
		base.Update ();
		if (Input.GetKeyDown (Summon)) 
		{
			summon();
		}
		if (Input.GetKeyDown (ShiftLeft)) 
		{
			if (--selected < 0)
			{
				selected = Demons.Length-1;
			}
		}
		if (Input.GetKeyDown (ShiftRight)) 
		{
			if (++selected >= Demons.Length)
			{
				selected = 0;
			}
		}
		if (Input.GetKeyDown (jump) && onGround ()) 
		{
			jumpNow = true;
		}
	}
	
	public override void OnDeath()
	{
		if (DeathAnim != null) 
		{
			DeathAnim.Play ();
		}
		transform.SetParent(null);
		if (!FacingRight) 
		{
			Flip ();
		}
		Instantiate (PlayerPrefab, new Vector3 (activeCheckpoint.transform.position.x, activeCheckpoint.transform.position.y, 0.0f), Quaternion.identity);
		base.OnDeath();
	}
	
	public virtual void summon()
	{
		if(checkMaterials() && GameObject.FindGameObjectsWithTag(Demons[selected].tag).Length<maxMins)
		{
			Instantiate (Demons[selected], transform.position,transform.rotation);
			int[] reqMats = Demons[selected].GetComponent<DemonBehavior>().reqMats;
			for (int i=0; i<reqMats.Length; i++) 
			{
				currentMats[i] -= reqMats[i];
			}
		}
	}
	
	
	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "crystal") 
		{
			pickUpMat (other.gameObject);
		}
		if (other.gameObject.tag == "Finish") 
		{
			//Add go to next level code here
			Application.LoadLevel (Application.loadedLevel);
		}
		
		if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="imp"|| other.gameObject.tag == "moving")
		{
			//Check to see if touching the floor
			/*if ( rb.velocity.y <= 0.0f )
			{
				print(rb.velocity.y);
			}*/
			if ( rb.velocity.y <= -25.0f )
			{
				OnDeath ();
			}
		}
		//base.OnCollisionEnter2D (other);
		
		if (other.gameObject.tag == "moving") 
		{
			transform.SetParent(other.transform);
		}
	}
	
	public virtual void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "moving") 
		{
			transform.SetParent(null);
		}
		if (other.gameObject.tag == "DeathBoundary") 
		{
			OnDeath ();
		}
		//if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="imp"|| other.gameObject.tag == "moving")
		//{
		//	//Check to see if touching the floor
		//}
	}
	
	public virtual void pickUpMat(GameObject pickUp)
	{
		int[] newMats = pickUp.GetComponent<CrystalScript>().newMats;
		for (int i=0; i<currentMats.Length; i++) 
		{
			currentMats[i] += newMats[i];
		}
		Destroy (pickUp);
	}
	
	public virtual bool checkMaterials()
	{
		int[] reqMats = Demons[selected].GetComponent<DemonBehavior>().reqMats;
		for (int i=0; i<reqMats.Length; i++) 
		{
			if(currentMats[i] < reqMats[i])
			{
				return false;
			}
		}
		return true;
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {
		
		float move = Input.GetAxis ("Horizontal");
		rb.velocity = new Vector2(move * speed, rb.velocity.y);
		
		if (move > 0 && !FacingRight) {
			Flip ();
		} else if (move < 0 && FacingRight) {
			Flip ();
		}
		
		if (CharacterBehavior.FacingRight) {
			Dir = Vector2.right;
		} else {
			Dir = Vector2.left;
		}
		//character movement with wasd
		
		//		if (Input.GetKey(moveRight))
		//		{
		//			rb.velocity = (Vector2.right * speed *);
		//			Dir = Vector2.right;
		//		}
		//		if (Input.GetKey(moveLeft))
		//		{
		//			transform.Translate(Vector2.left * speed * Time.deltaTime);
		//			Dir = Vector2.left;
		//		}
		
		if (jumpNow)
		{
			//Force added for up direction
			rb.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
			jumpNow = false;
			//anim.SetBool ("Ground", false);
		}
		
		if (Input.GetKeyDown(crouch) && !isCrouched)
		{
			//change the size and offset of the collider2D
			isCrouched = true;
			HalveCollider(bc,heightChange);
		}
		if (Input.GetKeyUp(crouch) && isCrouched)
		{
			isCrouched = false;
			DoubleCollider(bc,standHeight/crouchHeight);
		}
	}
	private bool onGround()
	{
		if (rb.velocity.y <= 0) 
		{
			foreach(Transform t in groundChecks)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(t.position,groundRadius,whatIsGrounded);

				for(int i =0;i<colliders.Length;i++)
				{
					if(colliders[i].gameObject!=gameObject)
					{
						return true;
					}
				}
			} 
		}
		return false;
	}


	void Flip()
	{
		FacingRight = !FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}	
}
