using UnityEngine;
using System.Collections;

public class ImpAI : DemonBehavior {
    //Animations once we have them
    public Animation lavaDeath;

    //Audio Components
    public AudioClip[] impSummons;
    public AudioClip[] impDeaths;

    //Imp Components
	private Rigidbody2D rb;
	private BoxCollider2D bc;

    //dead Imp information
	private float heightChange;
	public float SinkTime = 3.0f;
    public static Vector2 ImpScale;
    public bool dead = false;

    // Use this for initialization
    public override void Start()
    {
        //Save localScale of imp
        ImpScale = transform.localScale;

        //Imp Components
		bc = GetComponent<BoxCollider2D> ();
        rb = GetComponent<Rigidbody2D>();

        //Height change of collider after Imp dies
		heightChange = (.5f) * bc.size.y;


		speed = 2;

        //Ignore Collisions with these layers
        Physics2D.IgnoreLayerCollision(10, 9);
		Physics2D.IgnoreLayerCollision (10, 10);

        //Play sound when created
        AudioSource.PlayClipAtPoint(impSummons[Random.Range(0, impSummons.Length)], transform.position);


        //Call Parent class Start() funciton
        base.Start();
        
    }
	// Update is called once per frame

	public virtual void OnTriggerEnter2D(Collider2D other)
	{
        //When on a moving platform
		if (other.gameObject.tag == "moving") 
		{
			transform.parent = other.transform;
		}

        //Fall Death
		if (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="imp"|| other.gameObject.tag == "moving")
		{
			if ( rb.velocity.y <= -15.0f )
			{
                KillImp();
			}
		}
	}


	public override void Update()
	{
        base.Update();
		
        //Check if player is dead and kill Imp() if player died
		if (CharacterBehavior.Died) 
		{
			OnDeath ();

		}
	}


	public virtual void LateUpdate()
	{
        //Check if player dead and make sure player Died is false when player respawns
		if (CharacterBehavior.Died) 
		{
			CharacterBehavior.Died = false;
		}
	}

	public virtual void OnTriggerExit2D(Collider2D other)
	{

        //When exiting a platform the imp must de-parent platform
        //Potential bug when player holding imp on moving platform
		if (other.gameObject.tag == "moving") 
		{
			transform.parent = null;
		}
	}

	public override void OnCollisionEnter2D(Collision2D other)
	{
        //When colliding with magma kill imp, but body stays for a SinkTime
		if (other.gameObject.tag == "magma" || other.gameObject.tag == "enemy") {
			if (!dying)
			{
				dying = true;
				Anim.SetBool ("Death", true);
			}
			gameObject.layer = LayerMask.NameToLayer ("DeadImp");
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			speed = 0;
			StartCoroutine (WaitTime (SinkTime));
			//gameObject.tag = "floor";
			Anim.SetBool ("Death", true);
            if (!dead)
            {
                AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
                HalveCollider(bc, heightChange);
                bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange / 2));
            }
		} 
        //Collision with Spike kills imp
		else if (other.gameObject.tag == "spike") 
        {
            KillImp();
		} 
		else 
		{
			base.OnCollisionEnter2D (other);
		}
	}


    //Functions
	public override IEnumerator WaitTime(float num)
	{
		yield return new WaitForSeconds (num);
		OnDeath ();
	}

	public override void OnDeath()
	{
		for (int i=0; i<transform.childCount; i++) {
			if ( transform.GetChild(i).tag == "enemy" ){
				transform.GetChild (i--).parent = null;
			}
		}
		Destroy (gameObject);
	}

    public virtual void KillImp() 
    {
        if (!dying)
        {
            dying = true;
            Anim.SetBool("Death", true);
            AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
        }
        gameObject.layer = LayerMask.NameToLayer("DeadImp");
        transform.FindChild("ImpTrigger").gameObject.layer = LayerMask.NameToLayer("DeadImp");
        speed = 0;
        dead = true;
        transform.FindChild("ImpTrigger").GetComponent<CircleCollider2D>().enabled = true;
        rb.freezeRotation = false;
        bc.enabled = false;
    }
}
