using UnityEngine;
using System.Collections;

public class GravityCinder : EnemyBehavior {
	
	private ParticleSystem cParts;
    //private ParticleSystem.EmissionModule em;
	private Rigidbody2D rigid;
	public float pauseTime = 2f;
	public float initialDelay = -1f;
	public float lowerThresh;
	public float startVel = 0.0f;
	public float xVel = 0.0f;

	//private Vector2 startPos;
	private bool dead = false;
	private float gravitySave;


	// Use this for initialization
	public override void Start () {
		base.Start ();
        //em = cParts.emission;
		if (initialDelay < 0) {
			initialDelay = pauseTime;
		}

		//startPos = transform.position;
		cParts = GetComponent<ParticleSystem>();
		rigid = GetComponent<Rigidbody2D>();
		gravitySave = rigid.gravityScale;
		rigid.velocity = new Vector2(xVel, startVel);

		dead = true;
		rigid.gravityScale = 0.0f;
		rigid.velocity = new Vector2(0.0f, 0.0f);
		//em.enabled = false;
		cParts.enableEmission = false;
		StartCoroutine (Regen (initialDelay));
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (!dead && transform.position.y < lowerThresh) {
			StartCoroutine (ResetPos ());
		}
		if (dead) {
			gameObject.tag = "Untagged";
		} else {
			gameObject.tag = "cinder";
		}
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (!dead && (rigid.velocity.y <= 0.0f) ) {
			StartCoroutine (ResetPos ());
		}
	}

	public override void OnRespawn()
	{
		base.OnRespawn ();
		StopAllCoroutines ();

		dead = true;
		rigid.gravityScale = 0.0f;
		rigid.velocity = new Vector2(0.0f, 0.0f);
		//em.enabled = false;
		cParts.enableEmission = false;
		cParts.Clear ();

		StartCoroutine (Regen (initialDelay));
	}
	
	public IEnumerator ResetPos()
	{
		dead = true;
		rigid.gravityScale = 0.0f;
		rigid.velocity = new Vector2(0.0f, 0.0f);
		//em.enabled = false;
		cParts.enableEmission = false;
		yield return new WaitForSeconds (0.5f);
		transform.position = startPos;
		cParts.Clear ();
		StartCoroutine (Regen (pauseTime));
	}

	public IEnumerator Regen(float num)
	{
		yield return new WaitForSeconds (num);
		//em.enabled = true;
		cParts.enableEmission = true;
		yield return new WaitForSeconds (0.1f);
		rigid.gravityScale = gravitySave;
		rigid.velocity = new Vector2(xVel, startVel);
		dead = false;
	}

    public void StartCo() 
    {
        StartCoroutine(ResetPos());
    }

	public void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, lowerThresh));
	}
}
