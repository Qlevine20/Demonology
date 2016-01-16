using UnityEngine;
using System.Collections;

public class RunnerBehavior : EnemyBehavior {

	public float speed;
	private float defaultSpeed;

	public float wallDist;
	public float targetDist;
	private Vector2 facingDir;
	private Vector2 defaultDir;
	public bool mobFacingRight;
	public LayerMask whatIsWall;
	public LayerMask whatIsTarget;

	private bool pause = false;
	private bool charging = false;
	private Rigidbody2D rb;

	public GameObject Demons;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		defaultSpeed = speed;
		rb = GetComponent<Rigidbody2D>();
		if (mobFacingRight) {
			defaultDir = facingDir = Vector2.right;
		} else {
			defaultDir = facingDir = Vector2.left;
		}
	}

	// Update is called once per frame
	public override void Update () 
	{
		base.Update ();
		if (CharacterBehavior.Died) 
		{
			facingDir = Vector2.right;
			pause = false;
			charging = false;
			speed = defaultSpeed;
			facingDir = defaultDir;
		}
	}
	
	public void FixedUpdate()
	{
		if (!pause)
		{
			//Move the mobile
			Movement (new Ray2D (transform.position, facingDir));
			if (!charging){
				Seek (new Ray2D (transform.position, facingDir));
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		//When on a moving platform
		if (other.gameObject.tag == "moving") 
		{
			transform.parent = other.transform;
		}
		
		//Fall Death
		if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="imp"|| other.gameObject.tag == "moving")
		{
			if ( rb.velocity.y <= -25.0f )
			{
				OnDeath();
			}
		}
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		//When colliding with magma kill imp, but body stays for a SinkTime
		if (other.gameObject.tag == "magma" || other.gameObject.tag == "spike") {
			OnDeath ();
		}
		
	}

	//Moves the mobile and the mobile changes direction if it hits a wall
	public void Movement(Ray2D ry)
	{
		//Casts a ray in front of the object to see if there are any obstacles blocking the path
		if (Physics2D.Raycast(ry.origin,ry.direction,wallDist,whatIsWall)) 
		{
			//Changes the Direction the object faces to the opposite of its current Direction
			Flip();
			facingDir = new Vector2(-facingDir.x,facingDir.y);
			charging = false;
			speed = defaultSpeed;
		}

		//Move forward
		transform.Translate(facingDir * speed * Time.deltaTime);
		
		//Draws the Raycast so it is viewable in the editor
		Debug.DrawRay (ry.origin, ry.direction*wallDist, Color.red);
	}

	//Look for prey!
	public void Seek(Ray2D ry)
	{
		if (Physics2D.Raycast (ry.origin, ry.direction, targetDist, whatIsTarget)) 
		{
			pause = true;
			StartCoroutine (WaitTime(0.4f));
		}
		Debug.DrawRay (ry.origin, ry.direction*targetDist, Color.blue);
	}
	
	//Flips the direction of the sprite
	public void Flip()
	{
		mobFacingRight = !mobFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public IEnumerator WaitTime(float num)
	{
		yield return new WaitForSeconds (num);
		pause = false;
		charging = true;
		speed = 12;
	}
}
