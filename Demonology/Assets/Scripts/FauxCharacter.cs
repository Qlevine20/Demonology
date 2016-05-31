using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FauxCharacter : DeadlyBehavior {
	
	public GameObject PlayerPrefab;
	public GameObject CrystalPrefab;
	public LayerMask IgnorePlayerLayer;
	public LayerMask checkMasks;
	public AudioClip crystalPickupSound;
	public AudioClip crystalFizzleSound;
	public AudioClip altarActivateSound;
	public AudioClip playerImpFogSound;
	public AudioClip playerPlayerFogSound;
	public Camera ImpThrowCam;
	
	//private Ray2D mP;

	public string HoldingImp;
	private RaycastHit2D rayhit;
	private RaycastHit2D feet_check;
	//private GameObject cdImp;
	
	Animator anim;
	private Rigidbody2D rb;
	
	// player jump info
	public LayerMask whatIsGrounded;
	public Transform[] GroundedEnds;
	public int speed = 10;//change in editor not here
	public int jumpspeed = 10;//change in editor not here
	public float ForceMult;
	public float fallCheck;
	
	public Vector2 Dir;
	public bool FacingRight = true;
	
	private Animator PlayerAnim;
	//private float Throwing = 0.0f;
	private bool PressMove;
	private Ray2D checkWall;
	public float checkWallDist = 1;
	public LayerMask wallMasks;
	private float right = 1;
	//private float holdDown = 0;

	public bool Dying;
	public float moveNow = 0f;
	public float timer = 0f;
	bool slowDown;

	
	// Use this for initialization
	public override void Start () {
		Dying = false;

		checkWall = new Ray2D(transform.position, (transform.right));
		base.Start ();
		PlayerAnim = transform.FindChild("CharSpriteHolder").GetComponent<Animator>();
		//mP = new Ray2D (new Vector2 (0, 0), new Vector2 (0, 0));
		HoldingImp = "";

		if (FacingRight == true) {
			Dir = Vector2.right;
		} else {
			Dir = Vector2.left;
			transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y);
		}
		rb = GetComponent<Rigidbody2D> ();

		fallCheck = transform.position.y;
		transform.GetChild (4).gameObject.SetActive (false);
		slowDown = false;
	}
	
	
	//
	// UPDATE FUNCTIONS START HERE
	//
	
	// Update is called once per frame
	public override void Update()
	{
		timer += Time.deltaTime;
		if (timer >= 2.15f && !slowDown) {
			Time.timeScale = 0.1f;
			slowDown = true;
		}

		// new fall death code
		if (onGround ()) {
			float newGround = transform.position.y;
			if (fallCheck - 10 > newGround) {
				PlayerAnim.SetBool ("FallDeath", true);
			}
			fallCheck = newGround;
		}
		
		if (Dying) {
			transform.GetChild (4).gameObject.SetActive (true);
		}

		base.Update ();
		
		checkWall.origin = transform.position;
		rayhit = Physics2D.Raycast(checkWall.origin, checkWall.direction, checkWallDist, wallMasks);
		feet_check = Physics2D.Raycast(new Vector2(checkWall.origin.x , checkWall.origin.y - 1.2f), checkWall.direction, checkWallDist,checkMasks);
		// Debug.DrawRay(checkWall.origin,checkWall.direction);
		//Debug.DrawRay(new Vector2(checkWall.origin.x , checkWall.origin.y - 1.2f), checkWall.direction);
		if (feet_check.collider != null) 
		{
			//Debug.Log(feet_check.point);
			//Debug.Log(feet_check.collider.gameObject.transform.position.y);
			if (feet_check.collider.gameObject.tag != "impTrigger")
			{
				RaycastHit2D check_empty = Physics2D.Raycast(checkWall.origin, checkWall.direction, checkWallDist, checkMasks);
				if (check_empty.collider == null && Mathf.Abs(Input.GetAxis("Horizontal")) > .5f)
				{
					//RaycastHit2D newLoc = Physics2D.Raycast(
					transform.position = new Vector3(transform.position.x, transform.position.y + .3f);
				}
			}
		}
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {   

		float move = moveNow;
		if (moveNow == 0f) {
			return;
		}

		if (move > 0 && FacingRight || move < 0 && !FacingRight)
		{
			if (rayhit.distance == checkWallDist)
			{
				rb.velocity = new Vector2(0.0f, rb.velocity.y);
			}
			else if (rayhit.distance != 0)
			{
				rb.velocity = new Vector2(right * (-1.0f * rayhit.distance), rb.velocity.y);
			}
			else
			{
				rb.velocity = new Vector2(move * speed, rb.velocity.y);
			}
		}
		else
		{
			if (rayhit.distance == checkWallDist)
			{
				
				rb.velocity = new Vector2(0.0f, rb.velocity.y);
			}
			else if (rayhit.distance != 0)
			{
				rb.velocity = new Vector2(right * (-1.0f * rayhit.distance), rb.velocity.y);
			}
			else
			{
				rb.velocity = new Vector2(move * (speed / 2), rb.velocity.y);
			}
		}
		
		//Animations for moving left and right
		if (PlayerAnim)
		{
			if (move!=0 && onGround())
			{
				PlayerAnim.SetBool("Move", true);
			}
			else if (!onGround() && rb.velocity.y <= 0 && !PlayerAnim.GetBool("Fall")) 
			{
				PlayerAnim.SetBool("Fall", true);
			}
			else
			{
				PlayerAnim.SetBool("Move", false);
			}
		}
		
		// Check if the sprite needs to flip
		if (move > 0 && !FacingRight) {
			if (onGround())
			{
				Flip();
			}
		} 
		else if (move < 0 && FacingRight) 
		{
			if (onGround())
			{
				Flip();
			}
		}
		
		// Dictate the player's direction
		if (FacingRight) {
			Dir = Vector2.right;
		} else {
			Dir = Vector2.left;
		}
	}
	
	
	//
	// COLLISION FUNCTIONS START HERE!
	//
	
	// Collision code for entering a "trigger" colldier
	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		// If you collide with the end of the stage...
		if (other.gameObject.tag == "Finish") {
			Time.timeScale = 1.0f;
			//Add go to next level code here
            Application.LoadLevel(1);
		}
		// If you collide with a moving platform..
		if (other.gameObject.tag == "moving") {
			// attach the character to it so they move with it
			transform.SetParent (other.transform);
		}
		if (other.gameObject.tag == "cinder") {
			// it's a cinder! it always kills
			if (!Dying){
				PlayerAnim.SetBool ("EnemyDeath", true);
				Dying = true;
			}
			//OnDeath ();
		}
		// If you collide with deadly fog...
		if (other.gameObject.tag == "playerkiller") {
			//die, obviously
			if (!Dying)
			{
				AudioSource.PlayClipAtPoint(playerPlayerFogSound, transform.position);
				PlayerAnim.SetBool("EnemyDeath", true);
				Dying = true;
			}
		}
	}
	
	// Collision code for exiting a "trigger" colldier
	public virtual void OnTriggerExit2D(Collider2D other)
	{
		// If you exit a moving platform, detach from it
		if (other.gameObject.tag == "moving") 
		{
			float xPos = transform.position.x;
			transform.SetParent(null);
			transform.position = new Vector3(xPos,transform.position.y,0.0f);
		}
		// If you exit the stage boundaries, DIE
		if (other.gameObject.tag == "DeathBoundary") 
		{
			//OnDeath ();
			if (!Dying){
				PlayerAnim.SetBool ("FallDeath", true);
				Dying = true;
			}
		}
	}
	
	// Collision code for making contact with an object
	public override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D (other);

		if (onGround()) 
		{
			PlayerAnim.SetBool("Jump", false);
			PlayerAnim.SetBool("Fall", false);
		}
        //if (other.gameObject.layer == 12) 
        //{
        //    cdImp = other.gameObject;
        //}
		if (other.gameObject.tag == "imp" || other.gameObject.tag == "impTrigger") {
			rb.AddForce((Vector2.up*0.7f+Dir*5f) * 80000.0f);
			moveNow = 0f;
			//Time.timeScale = 1.0f;
		}
	}
	
	
	//
	// OTHER FUNCTIONS START HERE!
	//
	
	// Check if the player is on the ground (and therefore allowed to jump)
	public bool onGround()
	{
		foreach (Transform t in GroundedEnds) 
		{
			if(Physics2D.Linecast(this.transform.position, t.position, whatIsGrounded))
				return true;
		}
		return false;
	}
	
	// Run this when the character "dies"
	public override void OnDeath()
	{
		// Run the death animation (if it exists)
		if (DeathAnim != null) 
		{
			DeathAnim.Play ();
		}
		transform.SetParent(null);
		if (!FacingRight) 
		{
			Flip ();
		}

		base.OnDeath();
		print (timer);
	}
	
	// Flip your direction
	void Flip()
	{
		
		FacingRight = !FacingRight;
		Transform spriteHolder = transform.GetChild(0);
		Vector3 theScale = spriteHolder.localScale;
		theScale.x *= -1;
		spriteHolder.localScale = theScale;
		right = -right;
		checkWall = new Ray2D(transform.position, right * (transform.right));
	}	
	
	// Convenience function
	public Vector3 ConvertVector3(Vector3 prefix, float x, float y, float z)
	{
		return new Vector3 (prefix.x + x,prefix.y + y,prefix.z + z);
	}
}
