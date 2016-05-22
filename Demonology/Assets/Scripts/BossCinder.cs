using UnityEngine;
using System.Collections;

public class BossCinder : MonoBehaviour {

	private ParticleSystem cParts;
    //private ParticleSystem.EmissionModule em;
	private bool dead = false;
	private Vector3 targetPos;
	public float speed = 5f;
	public float initialDelay = 0.5f;
	
	
	// Use this for initialization
	public void Start () {
		cParts = GetComponent<ParticleSystem>();
        //em = cParts.emission;
		//targetPos = GameObject.FindGameObjectWithTag ("Player").transform.position;

		dead = true;
        //em.enabled = false;
		cParts.enableEmission = false;
		StartCoroutine (Fire (initialDelay));
	}
	
	// Update is called once per frame
	public void Update () {
		if (!dead) {
			SmartMove (transform.position, transform.position-targetPos, speed*Time.deltaTime);
		}
	}

	public virtual void LateUpdate()
	{
		if (CharacterBehavior.Died) 
		{
			Destroy(gameObject);
		}
	}
	
	//public void OnCollisionEnter2D(Collision2D other)
	public void OnTriggerEnter2D(Collider2D other)
	{
		//print ("triggercollide");
		if (!dead && 
			(other.tag == "floor" ||
			//other.tag == "imp" ||
			other.tag == "spike" ||
			other.tag == "moving" ||
			//other.tag == "enemy" ||
			other.tag == "PowerCrystal" ||
			other.tag == "DeadImp")) {
			//print ("die");
			StartCoroutine (FadeOut ());
		}
	}

	private void SmartMove(Vector3 oldPos, Vector3 moveToPos, float moveDist)
	{
		Vector3 newPos = Vector3.MoveTowards (oldPos, moveToPos, moveDist);
		transform.position = newPos;
	}
	
	public IEnumerator Fire(float num)
	{
        
		//em.enabled = true;
		cParts.enableEmission = true;
		yield return new WaitForSeconds (num);
		targetPos = transform.position - GameObject.FindGameObjectWithTag ("Player").transform.position;
		targetPos = targetPos.normalized;
		dead = false;
	}
	
	public IEnumerator FadeOut()
	{
		dead = true;
		//em.enabled = false;
		cParts.enableEmission = false;
		gameObject.tag = "Untagged";
		yield return new WaitForSeconds (0.5f);
		cParts.Clear ();
		Destroy (gameObject);
	}

	public void StartCo() 
	{
		StartCoroutine(FadeOut());
	}
}
