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
	public GameObject CrystalPrefab;


	private List<GameObject> PickUpList= new List<GameObject>();
	public static int[] CheckPointMatsCount = new int[5];
	private bool WallColl;
	private bool TouchStickImp;

	
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
	
	public static Vector2 Dir;
	public static bool FacingRight;
	public static bool Died;
	

	
	public override void Start () {
		base.Start ();

		ImpSelect = GameObject.FindGameObjectWithTag ("ImpSelect");
		FacingRight = true;
		rb = GetComponent<Rigidbody2D> ();
		Dir = Vector2.right;
		rb = GetComponent<Rigidbody2D>();
		bc = GetComponent<Collider2D>() as BoxCollider2D;
		heightChange = (crouchHeight / standHeight) * bc.size.y;
		ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
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
			ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") > 0) 
		{
			ScrollUp();
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) 
		{
			ScrollDown();
		}

		if (Input.GetKeyDown (ShiftRight)) 
		{
			if (++selected >= Demons.Length)
			{
				selected = 0;
			}
			ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
		}
		if (Input.GetKeyDown (jump) && onGround ()||Input.GetKeyDown (jump) && TouchStickImp) 
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
		currentMats[0] = CheckPointMatsCount[0];
		Instantiate (PlayerPrefab, new Vector3 (activeCheckpoint.transform.position.x, activeCheckpoint.transform.position.y, 0.0f), Quaternion.identity);
		if (PickUpList != null) 
		{
			foreach (GameObject g in PickUpList) {
				g.SetActive(true);
				//When there are more than one type of crystal this breaks
			}

			PickUpList.Clear ();
		}
//		List<string> minNames = new List<>;
//		foreach (GameObject min in Minions) 
//		{
//			minNames.Add (min.name);
//			GameObject[] d = GameObject.FindGameObjectsWithTag (min.tag);
//			foreach(GameObject minkill in d)
//			{
//				Destroy (minkill);
//			}
//		}
//		GameObject[] des = GameObject.FindGameObjectsWithTag("floor");
//		foreach(GameObject f in des)
//		{
//			if(minNames.Contains(f.name))
//			{
//				Destroy (f);
//			}
//		}


		base.OnDeath();
		Died = true;


	}

	public void ScrollUp()
	{
		if (++selected >= Demons.Length)
		{
			selected = 0;
		}
		ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
	}

	public void ScrollDown()
	{
		if (--selected < 0)
		{
			selected = Demons.Length-1;
		}
		ImpSelect.GetComponent<Image> ().color = Demons [selected].GetComponent<SpriteRenderer> ().color;
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
		if(other.gameObject.tag == "checkpoint"){
			activeCheckpoint = other.gameObject;
			CheckPointMatsCount[0] = currentMats[0];
			PickUpList.Clear();
			other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
			other.gameObject.GetComponent<Checkpoint>().touched = true;
		}

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
			float xPos = transform.position.x;
			transform.SetParent(null);
			transform.position = new Vector3(xPos,transform.position.y,0.0f);
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


	public override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D (other);
		if (other.gameObject.tag == "floor" || other.gameObject.tag == "imp" || other.gameObject.tag == "moving") 
		{
			WallColl = true;
		}
		if (other.gameObject.tag == "stickImp") 
		{
			TouchStickImp = true;
		}
	}

	public virtual void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag == "floor" || other.gameObject.tag == "imp" || other.gameObject.tag == "moving") 
		{
			WallColl = false;
		}
		if (other.gameObject.tag == "stickImp") 
		{
			TouchStickImp = false;
		}
	}
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
	
	// FixedUpdate is called once per frame
	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");
		if(!WallColl || onGround())
		{
			rb.velocity = new Vector2(move * speed, rb.velocity.y);
		}
		
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
		
		if (jumpNow)
		{
			//Force added for up direction
			rb.velocity = new Vector2(rb.velocity.x,0);
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



	void Flip()
	{
		FacingRight = !FacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}	
}
