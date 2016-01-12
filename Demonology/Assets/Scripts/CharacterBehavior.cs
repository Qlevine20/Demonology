using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterBehavior : DeadlyBehavior {

	// KeyCode Assignments
	public KeyCode Summon = KeyCode.Q;
	public KeyCode ShiftLeft = KeyCode.E;
	public KeyCode ShiftRight = KeyCode.R;
	public KeyCode jump = KeyCode.W;
	public KeyCode crouch = KeyCode.S;
	public KeyCode grab = KeyCode.G;

	public GameObject[] Minions;
	public int[] currentMats;
	public GameObject[] Demons;
	private int selected = 0;
	public static GameObject activeCheckpoint;
	public int maxMins = 5;
	public GameObject PlayerPrefab;
	public GameObject CrystalPrefab;
	public LayerMask IgnorePlayerLayer;
    public AudioClip crystalPickupSound;
	
	
	private List<GameObject> PickUpList= new List<GameObject>();
	public static int[] CheckPointMatsCount = new int[5];
	private Ray2D mP;
	//private bool WallColl;
	private string HoldingImp;
	
	
	Animator anim;
	
	//player rigidbody and collider
	private Rigidbody2D rb;
	private BoxCollider2D bc;
	
	//player heights
	private float crouchHeight = 1;
	private float standHeight = 2;
	private float heightChange;
	private GameObject ImpSelect;
	
	// player crouch/jump info
	private bool isCrouched = false;
	private bool jumpNow = false;
	private float groundRadius = .1f;
	public LayerMask whatIsGrounded;
	public Transform[] groundChecks;
	public int speed = 10;//change in editor not here
	public int jumpspeed = 10;//change in editor not here
	public float ForceMult;
	
	public static Vector2 Dir;
	public static bool FacingRight;
	public static bool Died;

    private Animator PlayerAnim;
    private Collider2D GrabbingImp = null;
    private float Throwing = 0.0f;




	// struct for summoning materials
	public struct summMaterials
	{
		public GameObject mat;
		public int numOfMat;
	}


	//
	// FUNCTIONS START HERE
	//

	// Use this for initialization
	public override void Start () {
		base.Start ();
        PlayerAnim = GetComponent<Animator>();
		mP = new Ray2D (new Vector2 (0, 0), new Vector2 (0, 0));
		HoldingImp = "";
		ImpSelect = GameObject.FindGameObjectWithTag ("ImpSelect");
		FacingRight = true;
		rb = GetComponent<Rigidbody2D> ();
		Dir = Vector2.right;
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<Collider2D>() as BoxCollider2D;
		heightChange = (crouchHeight / standHeight) * bc.size.y;
		ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
	}


	//
	// UPDATE FUNCTIONS START HERE
	//

	// Update is called once per frame
	public override void Update()
	{
		base.Update ();

		// Code for holding and throwing imps
		if (HoldingImp!="") 
		{
			if(transform.childCount > 3)
			{
	//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	//			Gizmos.DrawRay (ray);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				mP = new Ray2D (new Vector2 (ray.origin.x, ray.origin.y-2.5f), new Vector2 (ray.direction.x, ray.direction.y));
				if(Physics2D.Raycast (mP.origin,mP.direction,3.0f,IgnorePlayerLayer).collider == null)
				{
					transform.GetChild (3).transform.position = new Vector3(mP.GetPoint (2.0f).x,mP.GetPoint (2.0f).y,0.0f);
				}
				else
				{
                    if (!transform.GetChild(3).GetComponent<ImpAI>().dead)
                    {
                        float Change = Physics2D.Raycast(mP.origin, mP.direction, 1.0f, IgnorePlayerLayer).distance;
                        transform.GetChild(3).transform.position = new Vector3(mP.GetPoint(Change - 0.5f).x, mP.GetPoint(Change - 0.5f).y, 0.0f);
                    }
                    else 
                    {
                        float Change = Physics2D.Raycast(mP.origin, mP.direction, 1.0f, IgnorePlayerLayer).distance;
                        transform.GetChild(3).transform.position = new Vector3(mP.GetPoint(Change - 0.5f).x, mP.GetPoint(Change - 0.5f).y, 0.0f);
                    }

				}
			}
			else
			{
				HoldingImp = "";
			}
		}

		// Code for recieving button input
		if (Input.GetKeyDown (Summon)) 
		{
			// Summon a demon
			summon();
		}
		if (Input.GetKeyDown (ShiftLeft)) 
		{
			// Cycle the demon selection wheel to the left
			if (--selected < 0)
			{
				selected = Demons.Length-1;
			}
			ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
		}
		if (Input.GetKeyDown (ShiftRight)) 
		{
			// Cycle the demon selection wheel to the right
			if (++selected >= Demons.Length)
			{
				selected = 0;
			}
			ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
		}
		if (Input.GetKeyDown (jump) && onGround ()||Input.GetKeyDown (jump) && HoldingImp == "stickImp") 
		{
			// Jump (this actually gets processed later, in FixedUpdate
			jumpNow = true;
		}

		if (Input.GetMouseButtonDown (0) || Throwing != 0.0f) 
		{
			// THROW THE IMP!
			if(HoldingImp!="" && Throwing == 0.0f)
			{
                ThrowImp(ForceMult);
            }
            if(Throwing != 0.0f)
            {
                Throwing = 0.0f;
                ThrowImp(Throwing);

			}
		}

        if (Input.GetMouseButtonDown(1)) 
        {
            if (HoldingImp != "") 
            {
                Transform heldImp = transform.GetChild(3);
                ThrowImp(ForceMult);
                heldImp.GetComponent<ImpAI>().KillImp();
                GrabImp(heldImp.FindChild("ImpTrigger").GetComponent<CircleCollider2D>());

                
            }

        }
	}

    public void ThrowImp(float FM)
    {
        HoldingImp = "";
        GameObject childImp = transform.GetChild(3).gameObject;
        childImp.transform.parent = null;
        Rigidbody2D childRb = childImp.GetComponent<Rigidbody2D>();
        childRb.isKinematic = false;
        childRb.AddForce(mP.direction * FM, ForceMode2D.Impulse);
        childImp.transform.localScale = ImpAI.ImpScale;
        if (childImp.GetComponent<ImpAI>().dead == false)
        {
            childImp.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            childImp.transform.FindChild("ImpTrigger").GetComponent<CircleCollider2D>().enabled = true;
        }

    }

	// FixedUpdate is called once per frame
	void FixedUpdate () {     
		float move = Input.GetAxis ("Horizontal");
		//		if(!WallColl || onGround())
		//		{
		rb.velocity = new Vector2(move * speed, rb.velocity.y);
        //		}

        //Animations for moving left and right
        if (PlayerAnim)
        {
            if (FacingRight && move != 0)
            {
                PlayerAnim.SetBool("moveRight", true);
                PlayerAnim.SetBool("moveLeft", false);
            }
            else if (!FacingRight && move != 0)
            {
                PlayerAnim.SetBool("moveRight", false);
                PlayerAnim.SetBool("moveLeft", true);
            }
            else
            {
                PlayerAnim.SetBool("moveRight", false);
                PlayerAnim.SetBool("moveLeft", false);
            }
        }

        if (Input.GetKeyDown(grab))
        {
            if (HoldingImp != "")
            {
                HoldingImp = "";
                Throwing = 0.01f;

            }
            if (GrabbingImp)
            {
                GrabImp(GrabbingImp);
            }
        }

		// Check if you're holding a sticky imp
		if (move != 0) 
		{
			CheckHoldingStickImp();
		}

		// Check if the sprite needs to flip
		if (move > 0 && !FacingRight) {
			Flip ();
		} else if (move < 0 && FacingRight) {
			Flip ();
		}

		// Dictate the player's direction
		if (CharacterBehavior.FacingRight) {
			Dir = Vector2.right;
		} else {
			Dir = Vector2.left;
		}
		
		// If the player needs to jump now, JUMP!
		if (jumpNow)
		{
			CheckHoldingStickImp();
			//Force added for up direction
			rb.velocity = new Vector2(rb.velocity.x,0);
			rb.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
			jumpNow = false;
			//anim.SetBool ("Ground", false);
		}

		// Crouch (this doesn't do anything now, slated for removal
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


	//
	// COLLISION FUNCTIONS START HERE!
	//
	
	// Collision code for entering a "trigger" colldier
	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		// If you collide with a checkpoint...
		if(other.gameObject.tag == "checkpoint"){
			// activate it
			activeCheckpoint = other.gameObject;
			CheckPointMatsCount[0] = currentMats[0];
			PickUpList.Clear();
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			other.gameObject.GetComponent<Checkpoint>().touched = true;
		}
		// If you collide with a crystal...
		if (other.gameObject.tag == "crystal") 
		{
			//play a sound
            if (gameObject != null) {
                AudioSource.PlayClipAtPoint(crystalPickupSound, Camera.main.transform.position, 100.0f);
            }
            // pick it up
			pickUpMat (other.gameObject);
		}
		// If you collide with the end of the stage...
		if (other.gameObject.tag == "Finish") 
		{
			//Add go to next level code here
			Application.LoadLevel (Application.loadedLevel);
		}
		// If you collide with a "hard" object...
		if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="impTrigger"|| other.gameObject.tag == "moving")
		{
			// die if you're falling too quickly
			/*if ( rb.velocity.y <= 0.0f )
			{
				print(rb.velocity.y);
			}*/
			if ( rb.velocity.y <= -25.0f )
			{
				OnDeath ();
			}
		}
		// If you collide with a moving platform..
		if (other.gameObject.tag == "moving") 
		{
			// attach the character to it so they move with it
			transform.SetParent(other.transform);
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
			OnDeath ();
		}
	}

	// Collision code that runs as long as you're touching a "trigger" collider
	public void OnTriggerStay2D(Collider2D other)
	{
		// This whole function is for dealing with grabbing imps
        if(other.gameObject.tag == "impTrigger")
        {
             GrabbingImp = other;
        }
           
		if (other.gameObject.tag == "stickImp") 
		{
			if(Input.GetKey (grab) && HoldingImp == "")
			{
				HoldingImp = other.transform.parent.gameObject.tag;
				StickToImp(other);
			}
		}
	}
	
	// Collision code for making contact with an object
	public override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D (other);
		/*if (other.gameObject.tag == "floor" || other.gameObject.tag == "imp" || other.gameObject.tag == "moving") 
		{
			WallColl = true;
		}*/
	}

	// Collision code for ceasing contact with an object
	public virtual void OnCollisionExit2D(Collision2D other)
	{
		/*if (other.gameObject.tag == "floor" || other.gameObject.tag == "imp" || other.gameObject.tag == "moving") 
		{
			WallColl = false;
		}*/

	}
	

	//
	// OTHER FUNCTIONS START HERE!
	//
	
	// Check if the player is on the ground (and therefore allowed to jump)
	private bool onGround()
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
		// Reset the materials to the last checkpoint and respawn the player
		currentMats[0] = CheckPointMatsCount[0];
		Instantiate (PlayerPrefab, new Vector3 (activeCheckpoint.transform.position.x, activeCheckpoint.transform.position.y+2, 0.0f), Quaternion.identity);
		if (PickUpList != null) 
		{
			foreach (GameObject g in PickUpList) {
				g.SetActive(true);
				//When there are more than one type of crystal this breaks
			}
			
			PickUpList.Clear ();
		}
		
		// Finish killing the player
		base.OnDeath();
		Died = true;
	}

	// Summon a demon!
	public virtual void summon()
	{
		// Make sure that you have the necessary materials for demon summoning
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

	// Pick up a material
	public virtual void pickUpMat(GameObject pickUp)
	{
		int[] newMats = pickUp.GetComponent<CrystalScript>().newMats;
		for (int i=0; i<currentMats.Length; i++) 
		{
			currentMats[i] += newMats[i];
		}
		if (PickUpList!=null) 
		{
			PickUpList.Add (pickUp);
			pickUp.SetActive(false);
		}
	}

	// Check is you have enough materials to summon the selected demon
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

	// Grab an imp
	void GrabImp(Collider2D imp)
	{

        HoldingImp = imp.transform.parent.gameObject.tag;
        Vector2 origScale = imp.transform.parent.localScale;
        imp.transform.parent.transform.parent = transform;
        imp.transform.parent.localScale = origScale;
        imp.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        imp.transform.GetComponent<CircleCollider2D>().enabled = false;
		imp.transform.parent.transform.parent = transform;
		imp.transform.parent.GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	// Check if you're holding a sticky imp
	void CheckHoldingStickImp()
	{
		if(HoldingImp == "stickImp")
		{
			HoldingImp = "";
			rb.isKinematic = false;
			
		}
	}
	
	// Stick to the sticky imp!
	void StickToImp(Collider2D imp)
	{
		rb.isKinematic = true;
	}

	// Flip your direction
	void Flip()
	{
		FacingRight = !FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}	

	// Convenience function
	public Vector3 ConvertVector3(Vector3 prefix, float x, float y, float z)
	{
		return new Vector3 (prefix.x + x,prefix.y + y,prefix.z + z);
	}

	// Draw gizmos for imp throwing (doesn't occur during runtime)
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay (new Vector3(mP.origin.x,mP.origin.y,0.0f),new Vector3(mP.direction.x,mP.direction.y,0.0f));
		
	}
}
