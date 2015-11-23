using UnityEngine;
using System.Collections;

public class ImpDirectionalMovement : ImpAI {
	
	public override void Movement(Ray2D ry)
	{
		transform.Translate(CharacterBehavior.Dir * speed * Time.deltaTime);
		//Draws the Raycast so it is viewable in the editor
	}
}