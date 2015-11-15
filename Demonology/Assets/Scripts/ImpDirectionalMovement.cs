using UnityEngine;
using System.Collections;

public class ImpDirectionalMovement : ImpAI {

	public override void Movement(Ray2D ry)
	{
		//Casts a ray in front of the object to see if there are any obstacles blocking the path
		if (Physics2D.Raycast(ry.origin,ry.direction,wallDist,1<<8)) 
		{
			//Changes the direction the object faces to the opposite of its current direction
			CharacterMove.Dir = new Vector2(-CharacterMove.Dir.x,CharacterMove.Dir.y);
				
				
		}
			//Move forward
		transform.Translate(CharacterMove.Dir * speed * Time.deltaTime);
		//Draws the Raycast so it is viewable in the editor
		Debug.DrawRay (ry.origin, ry.direction,Color.red);
	}
}