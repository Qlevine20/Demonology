using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterBehavior : DeadlyBehavior {
	
	public KeyCode Summon = KeyCode.Q;
	public KeyCode ShiftLeft = KeyCode.E;
	public KeyCode ShiftRight = KeyCode.R;
	
	public GameObject[] Minions;
	public int[] currentMats;
	public GameObject[] Demons;
	private int selected = 0;
	public static GameObject activeCheckpoint;
	public int maxMins = 5;
	public GameObject PlayerPrefab;
	
	//is the player touching the ground
	private bool isGrounded = false;
	
	//player rigidbody
	private Rigidbody2D rb;
	
	//player collider2D
	private BoxCollider2D bc;
	
	//player heights
	private float crouchHeight = 1;
	private float standHeight = 2;
	private float heightChange;
	
	//is the character currently crouched
	private bool isCrouched = false;
	
	
	public int speed = 10;//change in editor not here
	public int jumpspeed = 10;//change in editor not here
	
	public KeyCode jump = KeyCode.W;
	public KeyCode crouch = KeyCode.S;
	public KeyCode moveLeft = KeyCode.A;
	public KeyCode moveRight = KeyCode.D;
	public static Vector2 Dir;
	
	
	
	public override void Start () {
		base.Start ();
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
	}
	
	public override void OnDeath()
	{
		if (DeathAnim != null) 
		{
			DeathAnim.Play ();
		}
		transform.parent = null;
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
		
		if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="imp"|| other.gameObject.tag == "moving")
		{
			//Check to see if touching the floor
			isGrounded = true;
		}
		//base.OnCollisionEnter2D (other);
		
		if (other.gameObject.tag == "moving") 
		{
			transform.parent = other.transform;
		}
	}
	
	public virtual void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "moving") 
		{
			transform.parent = null;
		}
		if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="imp"|| other.gameObject.tag == "moving")
		{
			//Check to see if touching the floor
			isGrounded = false;
		}
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
	
	// Update is called once per frame
	void FixedUpdate () {
		
		//character movement with wasd
		if (Input.GetKey(moveRight))
		{
			transform.Translate(Vector2.right * speed * Time.deltaTime);
			Dir = Vector2.right;
		}
		if (Input.GetKey(moveLeft))
		{
			transform.Translate(Vector2.left * speed * Time.deltaTime);
			Dir = Vector2.left;
		}
		
		if (Input.GetKeyDown(jump) && isGrounded)
		{
			
			//Force added for up direction
			isGrounded = false;
			rb.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
			
		}
		
		if (Input.GetKeyDown(crouch) && !isCrouched)
		{
			//change the size and offset of the collider2D
			bc.size = new Vector2(bc.size.x, heightChange);
			bc.offset = new Vector2(bc.offset.x, bc.offset.y - (heightChange/2));
			isCrouched = true;
			
		}
		if (Input.GetKeyUp(crouch) && isCrouched)
		{
			bc.size = new Vector2(bc.size.x, (standHeight / crouchHeight) * bc.size.y);
			bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange/2));
			isCrouched = false;
			
		}
		
	}
	
	
}
