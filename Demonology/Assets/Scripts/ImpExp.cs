using UnityEngine;
using System.Collections;

public class ImpExp : ImpAI {

	public float TimeToExp;
	public GameObject Psyst;
	private float CurrTime;
	public LayerMask whatIsTrigger;

	// Use this for initialization
	public override void Start () 
	{
		CurrTime = 0;
		base.Start ();
		WaitTime (TimeToExp);
	}

	public override void OnDeath()
	{
		Instantiate (Psyst, transform.position, Quaternion.identity);
		base.OnDeath ();
	}

	public override void Update()
	{
		base.Update ();
		CurrTime += Time.deltaTime;
		CheckExp ();
	}

	public void CheckExp()
	{
		if (CurrTime >= TimeToExp) 
		{
			OnDeath ();
		}
	}

	//Moves the mobile and the mobile changes direction if it hits a wall
	public override void Movement(Ray2D ry)
	{
		//Casts a ray in front of the object to see if there are any obstacles blocking the path
		if (Physics2D.Raycast (ry.origin, ry.direction, wallDist, whatIsTrigger))
		{
			// BOOM!
			OnDeath ();
		}
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
}
