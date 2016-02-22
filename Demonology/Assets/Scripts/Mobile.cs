using UnityEngine;
using System.Collections;

public class Mobile : DeadlyBehavior {

    public float speed = 0;
    public float wallDist = .1f;
	private Vector2 StartDir;
	public Animator Anim;
	public bool mobFacingRight;
	public LayerMask whatIsWall;
	public bool dying = false;
	public bool changeDir = false;

	//Direction of the entity
	public override void Start ()
	{
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
		//Casts a ray in front of the object to see if there are any obstacles blocking the path
		if (Physics2D.Raycast(ry.origin,ry.direction,wallDist,whatIsWall)) 
		{
			//Changes the Direction the object faces to the opposite of its current Direction
			Flip();

			StartDir = new Vector2(-StartDir.x,StartDir.y);


		}
		if(Anim!=null)
		{
				Anim.SetFloat ("Speed", speed);
		}
		//Move forward
		transform.Translate(StartDir * speed * Time.deltaTime);

		//Draws the Raycast so it is viewable in the editor
		Debug.DrawRay (ry.origin, ry.direction,Color.red);
    }

    //Flips the direction of the sprite
	public virtual void Flip()
	{
		mobFacingRight = !mobFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
