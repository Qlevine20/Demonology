using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterBehavior : DeadlyBehavior {

	// KeyCode Assignments
	public KeyCode Summon = KeyCode.Q;
	public KeyCode ShiftLeft = KeyCode.E;
	public KeyCode ShiftRight = KeyCode.R;
	public KeyCode[] jumpButton;
	public KeyCode grabButton = KeyCode.F;
    public KeyCode killSelf = KeyCode.K;
    public KeyCode climb = KeyCode.C;
    public KeyCode modifier = KeyCode.LeftShift;

	public int[] currentMats;
	public GameObject[] Demons;
	public int selected = 0;
	public static GameObject activeCheckpoint;
	//public int maxMins = 5;
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


    public static int ImpsKilled = 0;
    public static int TimesPlayerDied = 0;
	
	
	private List<GameObject> PickUpList= new List<GameObject>();
	public static List<GameObject> KilledEnemies= new List<GameObject>();
	public static int[] CheckPointMatsCount = new int[5];
	private Ray2D mP;
	//private bool WallColl;
	public string HoldingImp;
    private RaycastHit2D rayhit;
    private RaycastHit2D feet_check;
    //private GameObject cdImp;
	
	
	Animator anim;
	
	//player rigidbody and collider
	private Rigidbody2D rb;
	//private BoxCollider2D bc;
	
	private GameObject ImpSelect;
	
	// player jump info
    //private float groundRadius = .2f;
	public LayerMask whatIsGrounded;
	//public Transform[] groundChecks;
    public Transform[] GroundedEnds;
	public int speed = 10;//change in editor not here
	public int jumpspeed = 10;//change in editor not here
	public float ForceMult;
	public float fallCheck;
	
	public static Vector2 Dir;
	public static bool FacingRight;
    public static bool Dying;
	public static bool Died;

    private Animator PlayerAnim;
    private float Throwing = 0.0f;
    private bool PressMove;
    private Ray2D checkWall;
    public float checkWallDist = 1;
    public LayerMask wallMasks;
    public Vector2 BoxVec;
    private float right = 1;
    private float holdDown = 0;

	public bool mouseDelay = false;




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
        if (Application.loadedLevelName != "LoadingScreen")
        {
            MenuScript.levelNum = Application.loadedLevel;
        }
        Dying = false;
        checkWall = new Ray2D(transform.position, (transform.right));
		base.Start ();
        PlayerAnim = transform.FindChild("CharSpriteHolder").GetComponent<Animator>();
		mP = new Ray2D (new Vector2 (0, 0), new Vector2 (0, 0));
		HoldingImp = "";
		ImpSelect = GameObject.FindGameObjectWithTag ("ImpSelect");
		FacingRight = true;
		rb = GetComponent<Rigidbody2D> ();
		Dir = Vector2.right;
		//bc = GetComponent<Collider2D>() as BoxCollider2D;
		if (ImpSelect != null) {
			ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
		}
        if (GameObject.Find("ImpThrowCam") != null)
        {
            ImpThrowCam = GameObject.Find("ImpThrowCam").GetComponent<Camera>();
        }
		fallCheck = transform.position.y;
		transform.GetChild (4).gameObject.SetActive (false);
	}

    
	//
	// UPDATE FUNCTIONS START HERE
	//

	// Update is called once per frame
	public override void Update()
	{
		// new fall death code
		if (onGround ()) {
			float newGround = transform.position.y;
			if (fallCheck - 10 > newGround) {
				CharacterBehavior.Dying = true;
				PlayerAnim.SetBool ("FallDeath", true);
			}
			fallCheck = newGround;
		}

		if (Dying) {
			transform.GetChild (4).gameObject.SetActive (true);
		}

        foreach (KeyCode k in jumpButton)
        {
            if (Input.GetKeyDown(k) || Input.GetKeyDown(KeyManagerScript.keys["Jump"]))
            {
                if (onGround())
                {
                    rb.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
                    PlayerAnim.SetBool("Jump", true);
                    break;
                }
            }
        }
		base.Update ();
        //foreach (Transform t in GroundedEnds) 
        //{
        //    //Debug.DrawLine(this.transform.position, t.position, Color.green);
        //}
        
        checkWall.origin = transform.position;
        rayhit = Physics2D.BoxCast(new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), BoxVec, 0.0f, checkWall.direction, checkWallDist, wallMasks);
        feet_check = Physics2D.Raycast(new Vector2(checkWall.origin.x , checkWall.origin.y - 1.2f), checkWall.direction, checkWallDist,checkMasks);
        //Debug.DrawRay(checkWall.origin,checkWall.direction,Color.green);
        //Debug.DrawRay(new Vector2(checkWall.origin.x , checkWall.origin.y - 1.2f), checkWall.direction);
        if (feet_check.collider != null) 
        {
            //Debug.Log(feet_check.point);
            //Debug.Log(feet_check.collider.gameObject.transform.position.y);
            if (feet_check.collider.gameObject.tag != "impTrigger" && !(Input.GetKey(climb) || Input.GetKeyDown(KeyManagerScript.keys["PushImp"])))
            {
                RaycastHit2D check_empty = Physics2D.Raycast(checkWall.origin, checkWall.direction, checkWallDist, checkMasks);
                if (check_empty.collider == null && Mathf.Abs(Input.GetAxis("Horizontal")) > .5f)
                {
                    //RaycastHit2D newLoc = Physics2D.Raycast(
                    transform.position = new Vector3(transform.position.x, transform.position.y + .3f);
                }
            }
        }
		// Code for holding and throwing imps
		if (HoldingImp!="") 
		{
			if(transform.childCount > 4)
			{
				Ray ray = ImpThrowCam.ScreenPointToRay(Input.mousePosition);
				mP = new Ray2D (new Vector2 (ray.origin.x, ray.origin.y-2.5f), new Vector2 (ray.direction.x, ray.direction.y));
				if(Physics2D.Raycast (mP.origin,mP.direction,3.0f,IgnorePlayerLayer).collider == null)
				{
					transform.GetChild(5).transform.position = new Vector3(mP.GetPoint (2.0f).x,mP.GetPoint (2.0f).y,0.0f);
				}
				else
				{
                    if (!transform.GetChild(5).GetComponent<ImpAI>().dead)
                    {
                        float Change = Physics2D.Raycast(mP.origin, mP.direction, 1.0f, IgnorePlayerLayer).distance;
                        transform.GetChild(5).transform.position = new Vector3(mP.GetPoint(Change - 0.5f).x, mP.GetPoint(Change - 0.5f).y, 0.0f);
                    }
                    else 
                    {
                        float Change = Physics2D.Raycast(mP.origin, mP.direction, 1.0f, IgnorePlayerLayer).distance;
                        transform.GetChild(5).transform.position = new Vector3(mP.GetPoint(Change - 0.5f).x, mP.GetPoint(Change - 0.5f).y, 0.0f);
                    }

				}
			}
			else
			{
				HoldingImp = "";
			}
		}

        if (HoldingImp != "" && transform.GetChild(transform.childCount - 1).GetComponent<ImpAI>().DropKill == true) 
        {
            
            HoldingImp = "";
            Throwing = 0.01f;
            
        }

		// Code for recieving button input
		if ((Input.GetKeyDown (Summon) || Input.GetKeyDown(KeyManagerScript.keys["Summon"])) && (Input.GetKey(modifier) || Input.GetKeyDown(KeyManagerScript.keys["Mod"])) && HoldingImp=="") 
		{
            summon(true);
			// Summon a demon
		}
        if ((Input.GetKeyDown(Summon) || Input.GetKeyDown(KeyManagerScript.keys["Summon"])) && !(Input.GetKey(modifier) || Input.GetKeyDown(KeyManagerScript.keys["Mod"])) && HoldingImp == "") 
        {
            summon(false);
        }
		if (Input.GetKeyDown (ShiftRight)) 
		{
			// Cycle the demon selection wheel to the left (NOW REVERSED!)
			if (--selected < 0)
			{
				selected = Demons.Length-1;
			}
			ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
		}
        if (Input.GetKeyDown(ShiftLeft) || Input.GetKeyDown(KeyManagerScript.keys["ChangeImp"])) 
		{
			// Cycle the demon selection wheel to the right (reversed, CONSIDER REMOVING!)
			if (++selected >= Demons.Length)
			{
				selected = 0;
			}
			ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
		}

        if (Input.GetKey(killSelf) || Input.GetKeyDown(KeyManagerScript.keys["KillYourself"])) 
        {
            holdDown += Time.deltaTime;
            if (holdDown > 1.0f) 
            {
                //OnDeath();
				if (!CharacterBehavior.Dying){
					PlayerAnim.SetBool("EnemyDeath", true);
					CharacterBehavior.Dying = true;
					transform.GetChild (4).gameObject.SetActive (true);
				}
            }
        }
        if (Input.GetKeyUp(killSelf) || Input.GetKeyDown(KeyManagerScript.keys["KillYourself"])) 
        {
            holdDown = 0;
        }

        //if (Input.GetKeyDown(KeyCode.U)) 
        //{
        //    Debug.Log("Imp Deaths");
        //    Debug.Log(ImpsKilled);
        //    Debug.Log("Player Deaths");
        //    Debug.Log(TimesPlayerDied);
        //}

		//Grab an Imp or drop it
        if (Input.GetKeyDown(grabButton))
		{
			if (HoldingImp != "" && HoldingImp != "stickImp")
			{
				HoldingImp = "";
				Throwing = 0.01f;
				
			}
			else if (HoldingImp != "stickImp")
			{
				summon (true);
			}
		}


		if ((Input.GetMouseButtonDown (0) && !mouseDelay) || Throwing != 0.0f) 
		{
			// THROW THE IMP!
			if(HoldingImp!="" && Throwing == 0.0f)
			{
                ThrowImp(ForceMult);
            }
            if(Throwing != 0.0f)
            {
                Throwing = 0.0f;
                if (HoldingImp == "stickImp")
                {
                    transform.GetChild(5).GetComponent<StickImp>().Thrown = true;
                }
                ThrowImp(Throwing);
			}
		}

        if (Input.GetMouseButtonDown(1)) 
        {
            if (HoldingImp != "") 
            {
                Transform heldImp = transform.GetChild(5);
				if (heldImp != null) {
					ThrowImp (ForceMult);
					heldImp.GetComponent<ImpAI> ().KillImp ();
					GrabImp (heldImp.FindChild ("ImpTrigger").GetComponent<BoxCollider2D> ());

					BoxCollider2D bc = heldImp.GetComponent<ImpAI> ().GetComponent<BoxCollider2D> ();
					heldImp.GetComponent<ImpAI> ().HalveCollider (bc, heldImp.GetComponent<ImpAI> ().heightChange);
					bc.offset = new Vector2 (bc.offset.x, bc.offset.y + (heldImp.GetComponent<ImpAI> ().heightChange / 2));
				}
            }
        }

		if (mouseDelay) {
			mouseDelay = false;
		}

		if (Died) 
		{
			Died = false;
		}
	}

    public void ThrowImp(float FM)
    {
        HoldingImp = "";
        GameObject childImp = transform.GetChild(5).gameObject;
        childImp.GetComponent<ImpAI>().held = false;
        childImp.transform.parent = null;
        Rigidbody2D childRb = childImp.GetComponent<Rigidbody2D>();
        childRb.isKinematic = false;
        childRb.AddForce(mP.direction * FM, ForceMode2D.Impulse);
        //if (childImp.GetComponent<ImpAI>().dead == false)
        //{
            childImp.GetComponent<BoxCollider2D>().enabled = true;
        //}
        //else
        //{
            //childImp.transform.FindChild("ImpTrigger").GetComponent<CircleCollider2D>().enabled = true;
        //}
            if (childImp.GetComponent<ImpAI>().DropKill == true) 
            {
                Destroy(childImp);
            }

    }

	// FixedUpdate is called once per frame
	void FixedUpdate () {   
		//		if(!WallColl || onGround())
		//		{
        //if (HoldingStickImp)
        //{
        //    HoldingStickImp = false;
        //    Input.ResetInputAxes();
        //}
        float move = Input.GetAxis("Horizontal");
        //Physics2D.Raycast(checkWall.origin, checkWall.direction, checkWallDist, wallMasks).collider
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
            //else if (!onGround() && rb.velocity.y>0 && !PlayerAnim.GetBool("Jump")) 
            //{
            //    PlayerAnim.SetBool("Jump", true);
            //}
            else if (!onGround() && rb.velocity.y <= -2f && !PlayerAnim.GetBool("Fall")) 
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
		if (CharacterBehavior.FacingRight) {
			Dir = Vector2.right;
		} else {
			Dir = Vector2.left;
		}
		
		// If the player needs to jump now, JUMP!
        //if (jumpNow)
        //{
        //    CheckHoldingStickImp();
        //    ////Force added for up direction
        //    //rb.velocity = new Vector2(rb.velocity.x,0);
        //    //rb.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
        //    //jumpNow = false;
        //    //anim.SetBool ("Ground", false);
        //}
	}


	//
	// COLLISION FUNCTIONS START HERE!
	//
	
	// Collision code for entering a "trigger" colldier
	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		// If you collide with a checkpoint...
		if (other.gameObject.tag == "checkpoint") {
			// activate it
			activeCheckpoint = other.gameObject;
			for(int i=0; i<currentMats.Length; i++){
				CheckPointMatsCount[i] = currentMats[i];
			}
			//CheckPointMatsCount = currentMats;
			PickUpList.Clear ();
			KilledEnemies.Clear ();
			other.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			other.gameObject.GetComponent<Checkpoint> ().touched = true;
			if (gameObject != null) {
				AudioSource.PlayClipAtPoint (altarActivateSound, ImpThrowCam.transform.position, 75.0f);
			}
		}
		// If you collide with a crystal...
		if (other.gameObject.tag == "crystal") {
			//play a sound
			if (gameObject != null) {
				AudioSource.PlayClipAtPoint (crystalPickupSound, ImpThrowCam.transform.position, 75.0f);
			}
			// pick it up
			pickUpMat (other.gameObject);
		}
		// If you collide with the end of the stage...
		if (other.gameObject.tag == "Finish") {
			//Add go to next level code here
            if (Application.loadedLevel+1 < Application.levelCount)
            {
                MenuScript.levelNum = Application.loadedLevel + 1;
                Application.LoadLevel("LoadingScreen");
            }
            else 
            {
                Application.LoadLevel(0);
            }
		}
		// If you collide with a "hard" object...
		/*if (other.gameObject.tag == "floor" || other.gameObject.tag == "impTrigger" || other.gameObject.tag == "moving") {
			// fall death if you're falling too quickly
			if ( rb.velocity.y <= 0.0f )
			{
				print(rb.velocity.y);
			}
			if (rb.velocity.y <= -25.0f) {
				if (PlayerAnim && !CharacterBehavior.Dying) {
					PlayerAnim.SetBool ("FallDeath", true);
					CharacterBehavior.Dying = true;
				}
				//OnDeath ();
			}
		}*/
		// If you collide with a moving platform..
		if (other.gameObject.tag == "moving") {
			// attach the character to it so they move with it
			transform.SetParent (other.transform);
		}
		if (other.gameObject.tag == "cinder") {
			// it's a cinder! it always kills
			if (!CharacterBehavior.Dying){
				PlayerAnim.SetBool ("EnemyDeath", true);
				CharacterBehavior.Dying = true;
			}
			//OnDeath ();
		}
		// If you collide with deadly fog...
		if (other.gameObject.tag == "playerkiller") {
			//die, obviously
			if (!CharacterBehavior.Dying)
			{
                AudioSource.PlayClipAtPoint(playerPlayerFogSound, transform.position);
				PlayerAnim.SetBool("EnemyDeath", true);
				CharacterBehavior.Dying = true;
			}
		}
		// If you collide with imp-killing fog...
		if (other.gameObject.tag == "impkiller") {
			//all of your crystals vanish! bwahahaha!
			BatBehavior isABat = other.gameObject.GetComponent<BatBehavior> ();
			if (isABat == null) {
				for(int i=0; i<currentMats.Length; i++){
					currentMats[i] = 0;
				}
                AudioSource.PlayClipAtPoint(playerImpFogSound, transform.position);
				return;
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
			if (!CharacterBehavior.Dying){
				PlayerAnim.SetBool ("FallDeath", true);
				CharacterBehavior.Dying = true;
			}
		}
	}

	// Collision code for making contact with an object
    public override void OnCollisionEnter2D(Collision2D other)
    {
		base.OnCollisionEnter2D (other);

		/*if (other.gameObject.tag == "floor" || other.gameObject.tag == "impTrigger" || other.gameObject.tag == "moving") {
			// fall death if you're falling too quickly
			//print(other.relativeVelocity.y);
			if (other.relativeVelocity.y <= -25.0f) {
				if (PlayerAnim && !CharacterBehavior.Dying) {
					//print ("Death by falling!  Eeheehee!");
					PlayerAnim.SetBool ("FallDeath", true);
					CharacterBehavior.Dying = true;
				}
			}
		}*/
		if (onGround()) 
		{
		   PlayerAnim.SetBool("Jump", false);
		   PlayerAnim.SetBool("Fall", false);
		}
        //if (other.gameObject.layer == 12) 
        //{
        //   cdImp = other.gameObject;
        //}
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
	public bool onGround()
    {
        foreach (Transform t in GroundedEnds) 
        {
            if(Physics2D.Linecast(this.transform.position, t.position, whatIsGrounded))
                return true;
        }
        return false;
        

    //    foreach(Transform t in groundChecks)
    //    {
    //        Collider2D[] colliders = Physics2D.OverlapCircleAll(t.position,groundRadius,whatIsGrounded);
				
    //        for(int i =0;i<colliders.Length;i++)
    //        {
    //            if(colliders[i].gameObject!=gameObject)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
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
		for(int i=0; i<currentMats.Length; i++){
			currentMats[i] = CheckPointMatsCount[i];
		}
		//currentMats = CheckPointMatsCount;
        if (!activeCheckpoint)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab, new Vector3(activeCheckpoint.transform.position.x, activeCheckpoint.transform.position.y + 2, 0.0f), Quaternion.identity);
            if (PickUpList != null)
            {
                foreach (GameObject g in PickUpList)
                {
                    g.SetActive(true);
                    //When there are more than one type of crystal this breaks
                }

                PickUpList.Clear();
            }
            if (KilledEnemies != null)
            {
                foreach (GameObject g in KilledEnemies)
                {
                    g.SetActive(true);
                }
                KilledEnemies.Clear();
            }

            // Finish killing the player
            Died = true;
            newPlayer.name = "Character";
			newPlayer.gameObject.GetComponent<CharacterBehavior>().selected = selected;
            TimesPlayerDied++;
            base.OnDeath();
        }
	}

	// Summon a demon!
	public virtual void summon(bool Grab)
	{
		// Make sure that you have the necessary materials for demon summoning
		if(checkMaterials() /*&& GameObject.FindGameObjectsWithTag(Demons[selected].tag).Length<maxMins*/)
		{
			GameObject newImp = Instantiate (Demons[selected], transform.position,transform.rotation) as GameObject;
			int[] reqMats = Demons[selected].GetComponent<DemonBehavior>().reqMats;
			for (int i=0; i<reqMats.Length; i++) 
			{
				if (currentMats[i] < 9000) {
					currentMats[i] -= reqMats[i];
				}
			}
            if (Grab)
            {
                GrabImp(newImp.transform.GetChild(0).GetComponent<BoxCollider2D>());
            }
        }   
        else {
            AudioSource.PlayClipAtPoint(crystalFizzleSound, transform.position);
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
	public void GrabImp(Collider2D imp)
	{
        HoldingImp = imp.transform.parent.gameObject.tag;
        imp.transform.parent.gameObject.GetComponent<ImpAI>().held = true;
        Vector2 origScale = imp.transform.parent.localScale;
        imp.transform.parent.transform.parent = transform;
        imp.transform.parent.localScale = origScale;
        imp.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        imp.transform.GetComponent<CircleCollider2D>().enabled = false;
        imp.transform.parent.transform.parent = transform;
        imp.transform.parent.GetComponent<Rigidbody2D>().isKinematic = true;
        
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

	// Draw gizmos for imp throwing (doesn't occur during runtime)
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		//Gizmos.DrawRay (new Vector3(mP.origin.x,mP.origin.y,0.0f),new Vector3(mP.direction.x,mP.direction.y,0.0f));
        //Gizmos.DrawRay(transform.position,checkWall.direction);
		
	}
}
