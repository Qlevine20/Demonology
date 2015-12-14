using UnityEngine;
using System.Collections;

public class ImpAI : DemonBehavior {

    public Animation lavaDeath;
    public AudioClip[] impSummons;
    public AudioClip[] impDeaths;
	private Rigidbody2D rb;
	private BoxCollider2D bc;
	private float heightChange;

    // Use this for initialization
    public override void Start()
    {

		bc = GetComponent<BoxCollider2D> ();
		heightChange = (.5f) * bc.size.y;
		speed = 2;
        Physics2D.IgnoreLayerCollision(10, 9);
		Physics2D.IgnoreLayerCollision (10, 10);
        base.Start();
        AudioSource.PlayClipAtPoint(impSummons[Random.Range(0, impSummons.Length)], transform.position);
		rb = GetComponent<Rigidbody2D>();
    }
	// Update is called once per frame

	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "moving") 
		{
			transform.parent = other.transform;
		}

		if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="imp"|| other.gameObject.tag == "moving")
		{
			if ( rb.velocity.y <= -15.0f )
			{
				if (!dying)
				{
					dying = true;
					Anim.SetBool ("Death", true);
				}
				gameObject.layer = LayerMask.NameToLayer ("Ground");
				gameObject.GetComponent<BoxCollider2D>().enabled = true;
				speed = 0;
				gameObject.tag = "floor";
				HalveCollider(bc,heightChange);
				bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange/2));
				AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
			}
		}
	}


	public override void Update()
	{
		base.Update ();
		if (CharacterBehavior.Died) 
		{
			CharacterBehavior.Died = false;
			Destroy (gameObject);

		}
	}

	public virtual void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "moving") 
		{
			transform.parent = null;
		}
	}

	public override void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "magma") {
			if (!dying)
			{
				dying = true;
				Anim.SetBool ("Death", true);
			}
			gameObject.layer = LayerMask.NameToLayer ("Ground");
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			speed = 0;
			StartCoroutine (WaitTime (3f));
			gameObject.tag = "floor";
			Anim.SetBool ("Death", true);
            AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
			HalveCollider(bc,heightChange);
			bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange/2));
		} 
		else if (other.gameObject.tag == "spike") {

			if (!dying)
			{
				dying = true;
				Anim.SetBool ("Death", true);
			}
			gameObject.layer = LayerMask.NameToLayer ("Ground");
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			speed = 0;
			gameObject.tag = "floor";
			HalveCollider(bc,heightChange);
			bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange/2));
            AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
		} 
		else 
		{
			base.OnCollisionEnter2D (other);
		}
			
	}

	public override IEnumerator WaitTime(float num)
	{
		yield return new WaitForSeconds (num);
		OnDeath ();
	}
}
