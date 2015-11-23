using UnityEngine;
using System.Collections;

public class ImpDirectionalMovement : ImpAI {
	Vector2 OldDir;

	public override void Start ()
	{
		base.Start ();
		OldDir = CharacterBehavior.Dir;
	}
	public override void Movement(Ray2D ry)
	{

		transform.Translate(CharacterBehavior.Dir * speed * Time.deltaTime);
		if (CharacterBehavior.Dir != OldDir) 
		{
			OldDir = CharacterBehavior.Dir;
			Flip ();
		}

	}
}