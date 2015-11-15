using UnityEngine;
using System.Collections;

public class Mobile : DeadlyBehavior {

    public float speed = 1;
    public float wallDist = .04f;
	private Vector2 StartDir;
	//Direction of the entity
	public virtual void Start ()
	{
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
			//Changes the direction the object faces to the opposite of its current direction
			StartDir = new Vector2(-StartDir.x,StartDir.y);


		}
		//Move forward
		transform.Translate(StartDir * speed * Time.deltaTime);
		//Draws the Raycast so it is viewable in the editor
		Debug.DrawRay (ry.origin, ry.direction,Color.red);
    }

}
