using UnityEngine;
using System.Collections;

public class CharacterMove : DeadlyBehavior {
    //character movement speed
    public int speed = 5;//change in editor not here
    public int jumpspeed = 5;//change in editor not here

    public KeyCode jump = KeyCode.W;
    public KeyCode crouch = KeyCode.S;
    public KeyCode moveLeft = KeyCode.D;
    public KeyCode moveRight = KeyCode.A;


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


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<Collider2D>() as BoxCollider2D;
        heightChange = (crouchHeight / standHeight) * bc.size.y;
    }
	
	// Update is called once per frame
	void Update () {

        //character movement with wasd
        if (Input.GetKey(moveLeft))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(moveRight))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
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

	override public void OnDeath()
	{
		if (DeathAnim != null) 
		{
			DeathAnim.Play ();
		}
		//Put in code to go to checkpoint here
		Application.LoadLevel (Application.loadedLevel);
	}


    override public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "floor")
        {
            //Check to see if touching the floor
            isGrounded = true;
        }
		base.OnCollisionEnter2D (other);
    }
}
