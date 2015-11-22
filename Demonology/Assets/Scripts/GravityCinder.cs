using UnityEngine;
using System.Collections;

public class GravityCinder : MonoBehaviour {
	
	private ParticleSystem cParts;
	private Rigidbody2D rigid;
	public float pauseTime = 2f;
	public float lowerThresh;
	public float startVel = 0.0f;

	private Vector2 startPos;
	private bool dead = false;
	private float gravitySave;

	// Use this for initialization
	public void Start () {
		startPos = transform.position;
		cParts = GetComponent<ParticleSystem>();
		rigid = GetComponent<Rigidbody2D>();
		gravitySave = rigid.gravityScale;
		rigid.velocity = new Vector2(0.0f, startVel);
	}

	// Update is called once per frame
	public void Update () {
		if (!dead && transform.position.y < lowerThresh) {
			StartCoroutine (ResetPos ());
		}
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (!dead && (rigid.velocity.y <= 0.0f) ) {
			StartCoroutine (ResetPos ());
		}
	}
	
	public IEnumerator ResetPos()
	{
		dead = true;
		rigid.gravityScale = 0.0f;
		rigid.velocity = new Vector2(0.0f, 0.0f);
		cParts.enableEmission = false;
		yield return new WaitForSeconds (0.5f);
		transform.position = startPos;
		cParts.Clear ();
		yield return new WaitForSeconds (pauseTime);
		cParts.enableEmission = true;
		yield return new WaitForSeconds (0.1f);
		rigid.gravityScale = gravitySave;
		rigid.velocity = new Vector2(0.0f, startVel);
		dead = false;
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, lowerThresh));
	}
}
