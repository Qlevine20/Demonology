using UnityEngine;
using System.Collections;

public class Mobile : DeadlyBehavior {

    public float speed = 0;
    public float wallDist = .1f;
	protected Vector2 StartDir = CharacterBehavior.Dir;
	public Animator Anim;
	public bool mobFacingRight;
	public LayerMask whatIsWall;
	public bool dying = false;
	public bool changeDir = false;
    private Ray2D checkWall;
    public float checkWallDist = 1;
    public LayerMask checkMasks;
    private float right = 1;
    private RaycastHit2D feet_check;
    private RaycastHit2D check_empty;

	//Direction of the entity
	public override void Start ()
	{
        checkWall = new Ray2D(transform.position, (transform.right));
        //Grabs the direction the player is facing
		mobFacingRight = CharacterBehavior.FacingRight;

        //Flips the mobile objects sprite
		if (!mobFacingRight)
		{
			Flip ();
		}

        //Mobile Animator
		Anim = GetComponent<Animator> ();
		if (changeDir) {
			StartDir = -(CharacterBehavior.Dir);
		} else {
			StartDir = CharacterBehavior.Dir;
		}

        base.Start();
	}

    public override void Update() 
    {
        base.Update();
        feet_check = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - .2f), checkWall.direction, checkWallDist, checkMasks);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - .5f), checkWall.direction,Color.blue);
        if (feet_check.collider != null)
        {
            check_empty = Physics2D.Raycast(checkWall.origin, checkWall.direction, checkWallDist, checkMasks);
            Debug.DrawRay(checkWall.origin, checkWall.direction, Color.yellow);
            if (check_empty.collider == null && !dying)
            {
                //RaycastHit2D newLoc = Physics2D.Raycast(
                transform.position = new Vector3(transform.position.x, transform.position.y + .3f);
            }
        }
    }


	public virtual void FixedUpdate()
	{
		if (!dying)
		{
            //Move the mobile
			Movement (new Ray2D (transform.position, StartDir));
		}
	}

    //Moves the mobile and the mobile changes direction if it hits a wall
    public virtual void Movement(Ray2D ry)
    {
        Debug.DrawRay(ry.origin, ry.direction, Color.green);
        checkWall.direction = ry.direction;
		//Casts a ray in front of the object to see if there are any obstacles blocking the path
		if (Physics2D.Raycast(new Vector2(ry.origin.x,ry.origin.y-.5f),ry.direction,wallDist,whatIsWall)) 
		{
            
			//Changes the Direction the object faces to the opposite of its current Direction
			Flip();

			
		}
		if(Anim!=null)
		{
				Anim.SetFloat ("Speed", speed);
		}
		//Move forward
		transform.Translate(StartDir * speed * Time.deltaTime);

		//Draws the Raycast so it is viewable in the editor
		//Debug.DrawRay (ry.origin, ry.direction,Color.red);
    }

    //Flips the direction of the sprite
	public virtual void Flip()
	{
		mobFacingRight = !mobFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
        right = -right;
        StartDir = new Vector2(-(StartDir.x), StartDir.y);
        checkWall = new Ray2D(transform.position, StartDir);
       
        
	}

}
