using UnityEngine;
using System.Collections;

public class Mobile : DeadlyBehavior {

    public float speed = 1;
    public float wallDist = .04f;
	//Direction of the entity
	protected Vector2 Dir;


	public virtual void Start()
	{
		//Change later for now default to right
		Dir = Vector2.right;
	}
	public virtual void FixedUpdate()
	{
		Movement (new Ray2D(transform.position,Dir));
	}
    public virtual void Movement(Ray2D ry)
    {
		//Casts a ray in front of the object to see if there are any obstacles blocking the path
		if (Physics2D.Raycast(ry.origin,ry.direction,wallDist,1<<8)) 
		{
			//Changes the direction the object faces to the opposite of its current direction
			Dir = new Vector2(-Dir.x,Dir.y);


		}

		//Move forward
		transform.Translate(Dir * speed * Time.deltaTime);
		//Draws the Raycast so it is viewable in the editor
		Debug.DrawRay (ry.origin, ry.direction,Color.red);
    }

}
