using UnityEngine;
using System.Collections;

public class StickImp : ImpAI {

	private Rigidbody2D Rb;
	public override void Start () {
		base.Start ();
		Rb = GetComponent<Rigidbody2D> ();
		if (CharacterBehavior.FacingRight) {
			Rb.AddForce (new Vector2 (.2f, .3f) * speed * 7, ForceMode2D.Impulse);
		} else 
		{
			Rb.AddForce (new Vector2 (-.2f, .3f) * speed * 7, ForceMode2D.Impulse);

		}
	}

	public override void FixedUpdate()
	{

	}

	public override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D(other);
		if (other.gameObject.tag == "floor") 
		{
			Rb.isKinematic = true;

		}
	}

}