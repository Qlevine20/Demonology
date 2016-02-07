using UnityEngine;
using System.Collections;

public class ImpDirectionalMovement : ImpAI {

	Vector2 OldDir;
	CharacterBehavior player;
	private Rigidbody2D rb;
	public int jumpspeed = 5;

	public override void Start ()
	{
		base.Start ();
		OldDir = CharacterBehavior.Dir;
		player = GameObject.Find("Character").GetComponent<CharacterBehavior>();
		rb = GetComponent<Rigidbody2D> ();
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

	public override void OnTriggerEnter2D (Collider2D other){
		base.OnTriggerEnter2D (other);

		// If the imp collides with a crystal, pick it up for the player
		if (other.gameObject.tag == "crystal") {
			if (gameObject != null) {
				AudioSource.PlayClipAtPoint (player.crystalPickupSound, Camera.main.transform.position, 75.0f);
			}
			player.pickUpMat (other.gameObject);
		}
	}

	public override void FixedUpdate () {
		base.FixedUpdate ();
		float fall = Input.GetAxis ("Vertical");
		if (fall > 0 && rb.velocity.y >= 0.0f && player.onGround()) {
			rb.AddForce (new Vector2 (0, jumpspeed), ForceMode2D.Impulse);
		}
	}
}
