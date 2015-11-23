using UnityEngine;
using System.Collections;

public class Mobile : DeadlyBehavior {

    public float speed = 0;
    public float wallDist = .04f;
	private Vector2 StartDir;
	public Animator Anim;
	public bool mobFacingRight;

	//Direction of the entity
	public override void Start ()
	{
		mobFacingRight = CharacterBehavior.FacingRight;

		if (!mobFacingRight)
		{
			Flip ();
		}

		Anim = GetComponent<Animator> ();
		base.Start ();
		StartDir = CharacterBehavior.Dir;
	}

	public virtual void FixedUpdate()
	{
		Movement (new Ray2D(transform.position,StartDir));
	}
    public virtual void Movement(Ray2D ry)
    {
		//Casts a ray in front of the object to see if there are any obstacles blocking the path
		if (Physics2D.Raycast(ry.origin,ry.direction,wallDist,1<<8)) 
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

	public virtual void Flip()
	{
		mobFacingRight = !mobFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
