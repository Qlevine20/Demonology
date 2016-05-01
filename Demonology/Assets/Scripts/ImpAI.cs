using UnityEngine;
//using System;
using System.Collections;

public class ImpAI : DemonBehavior {
    //Animations once we have them
    public Animation lavaDeath;

    //Audio Components
    public AudioClip[] impSummons;
    public AudioClip[] impDeaths;

    //Imp Components
	protected Rigidbody2D rb;
	protected BoxCollider2D bc;

    //dead Imp information
	public float heightChange;
    private bool lava = false;
	public float SinkTime = 3.0f;
    public static Vector2 ImpScale;
    public bool dead = false;
    public float sinkDiv = 1;

	public CharacterBehavior player;
	public bool persist = false;
	public FadeObjectInOut fader;

    // Use this for initialization
    public override void Start()
    {
        //Save localScale of imp
        ImpScale = transform.localScale;

        //Imp Components
		bc = GetComponent<BoxCollider2D> ();
        rb = GetComponent<Rigidbody2D>();
		fader = GetComponent<FadeObjectInOut> ();

        //Height change of collider after Imp dies
		heightChange = (.5f) * bc.size.y;

		speed = 2;

        //Ignore Collisions with these layers
        Physics2D.IgnoreLayerCollision(10, 9);
		Physics2D.IgnoreLayerCollision (10, 10);

        //Play sound when created
        AudioSource.PlayClipAtPoint(impSummons[Random.Range(0, impSummons.Length)], transform.position);

		//Obtain player
		player = GameObject.Find("Character").GetComponent<CharacterBehavior>();

        //Call Parent class Start() funciton
        base.Start();
    }


	// Update is called once per frame
	public virtual void OnTriggerEnter2D(Collider2D other)
	{
        //When on a moving platform
		if (other.gameObject.tag == "moving" && transform.parent == null) 
		{
			transform.parent = other.transform;
		}

        //Fall Death
		if (!dead && (other.gameObject.tag == "floor"  ||  other.gameObject.tag=="impTrigger"|| other.gameObject.tag == "moving"))
		{
			if ( rb.velocity.y <= -15.0f )
			{
				HalveCollider(bc, heightChange);
				bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange / 2));
				KillImp();
				//print ("Imp death via falling!");
			}
		}

		//Fog death
		if (other.gameObject.tag == "impkiller" /*&& (Mathf.Abs(transform.position.x-other.transform.position.x) <= 1.1f)*/) {
			if (!dying)
			{
				dying = true;
				Anim.SetBool ("Death", true);
			}
			gameObject.layer = LayerMask.NameToLayer ("DeadImp");
			//transform.FindChild("ImpTrigger").gameObject.layer = LayerMask.NameToLayer("DeadImp");
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			StartCoroutine (WaitTime (SinkTime));
			Anim.SetBool ("Death", true);
			if (!dead)
			{
				AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
				HalveCollider(bc, heightChange);
				bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange / 2));
				transform.position = new Vector3(transform.position.x, transform.position.y - heightChange, transform.position.z);
				//print ("Imp death via fog!");
			}
		}
	}


	public override void Update()
	{
        base.Update();
        if (dying && lava) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (Time.deltaTime)/sinkDiv, transform.position.z);
        }
        //Check if player is dead and kill Imp() if player died
		/*if (CharacterBehavior.Died) 
		{
			CharacterBehavior.Died = false;
		}*/
	}


	public virtual void LateUpdate()
	{
        //Check if player dead and make sure player Died is false when player respawns
		if (CharacterBehavior.Died && !persist) 
		{
			OnDeath ();
		}
	}

	public virtual void OnTriggerExit2D(Collider2D other)
	{

        //When exiting a platform the imp must de-parent platform
        //Potential bug when player holding imp on moving platform
		if (other.gameObject.tag == "moving" && transform.parent != null && transform.parent.tag == "moving") 
		{
			transform.parent = null;
		}
	}
	public override void OnCollisionEnter2D(Collision2D other)
	{
        //When colliding with magma kill imp, but body stays for a SinkTime
		if (other.gameObject.tag == "magma" || other.gameObject.tag == "enemy" || other.gameObject.tag == "impkiller" || other.gameObject.tag == "cinder") {
			RunnerBehavior isARunner = other.gameObject.GetComponent<RunnerBehavior>();
			if (isARunner != null) {
				KillImp ();
				return;
			}

			if (!dying && other.gameObject.tag != "magma")
			{
                Anim.SetBool("Death", true);
				dying = true;
			}
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
			//speed = 0;
			StartCoroutine (WaitTime (SinkTime));
			//gameObject.tag = "floor";
			//Anim.SetBool ("Death", true);
            if (!dead && other.gameObject.tag != "magma")
            {
                AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
				HalveCollider(bc, heightChange);
				bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange / 2));
				transform.position = new Vector3(transform.position.x, transform.position.y - heightChange, transform.position.z);
            }
            if (other.gameObject.tag == "magma")
            {
				//print ("Imp death via magma!");

				//Anim.SetBool("Death", true);
                dying = true;
                //if (!dead) {
                //    HalveCollider(bc, heightChange);
                //    bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange / 2));
                //    transform.position = new Vector3(transform.position.x, transform.position.y - heightChange, transform.position.z);
                //}
                Anim.SetBool("lava", true);
                lava = true;
                AudioSource.PlayClipAtPoint(impDeaths[Random.Range(0, impDeaths.Length)], transform.position);
                gameObject.layer = LayerMask.NameToLayer("DeadLavaImp");
               
                rb.isKinematic = true;
            }
		} 
        //Collision with Spike kills imp
		else if (other.gameObject.tag == "spike") 
        {
			if(!dead)
			{
				//print ("Imp death via spike!");

				rb.velocity = Vector3.zero;
				if(other.gameObject.layer != 15)
				{
					rb.isKinematic = true;
				}
				HalveCollider(bc, heightChange);
				bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange / 2));
				transform.position = new Vector3(transform.position.x, transform.position.y - heightChange, transform.position.z);
				KillImp();
			}
		} 
		else 
		{
			base.OnCollisionEnter2D (other);
		}
	}


    //Functions
	public override IEnumerator WaitTime(float num)
	{
		yield return new WaitForSeconds (num-0.3f);
		fader.FadeOut (0.3f);
		yield return new WaitForSeconds (0.3f);
		OnDeath ();
	}

	public override void OnDeath()
	{
		for (int i=0; i<transform.childCount; i++) {
			if ( transform.GetChild(i).tag == "enemy" || transform.GetChild (i).tag == "impkiller" ){
				transform.GetChild (i--).parent = null;
			}
		}
		if (transform.parent != null && transform.parent.tag == "Player") {
			CharacterBehavior playerChar = transform.parent.GetComponent<CharacterBehavior>();
			//playerChar.GrabbingImp = null;
			playerChar.HoldingImp = "";
			transform.parent = null;
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
        //transform.FindChild("ImpTrigger").gameObject.layer = LayerMask.NameToLayer("DeadImp");
        //speed = 0;
        dead = true;
        //transform.FindChild("ImpTrigger").GetComponent<CircleCollider2D>().enabled = true;
        //Roll Imps:
        //rb.freezeRotation = false;
        //bc.enabled = false;
    }


	public virtual void OnMouseDown()
	{
		Vector3 mouse_po = Input.mousePosition;
		mouse_po = Camera.main.ScreenToWorldPoint (new Vector3 (mouse_po.x, mouse_po.y, Camera.main.nearClipPlane));
		mouse_po.z = 0f;

		if ((player.HoldingImp == "") && 
			DistanceBetween (player.transform.position, transform.position) <= 4.0f)
		{
			player.GrabImp (transform.GetChild(0).GetComponent<Collider2D>());
			player.mouseDelay = true;
		}
	}

	// Find the distance between two points
	public float DistanceBetween (Vector2 pos1, Vector2 pos2)
	{
		float xPos = pos1.x - pos2.x;
		float yPos = pos1.y - pos2.y;
		return (float)System.Math.Sqrt (xPos*xPos + yPos*yPos);
	}
}
