using UnityEngine;
using System.Collections;

public class FinalBossBehavior : EnemyBehavior {

	public int hitPoints = 10;
	public float attackRate = 2.5f;
	public int numCindersLeft = 4;
	public int numCindersRight = 4;
	private float timer = 0f;
	private GameObject player;
	public GameObject spawnAttack;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		//timer = attackRate;
		timer = 2f;

		if (GameObject.Find("Character") != null)
		{
			player = GameObject.Find("Character");
		}
	}
	
	// Update is called once per frame
	public override void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0f) {
			timer += attackRate;

			if (Random.Range (-5f, 5f) < 0) {
				LeftEyeAttack ();
			} else {
				RightEyeattack ();
			}
		}
	}


	public void LeftEyeAttack()
	{
		StartCoroutine (EyeFlash (0));
		for (int i=0; i<numCindersLeft; i++)
		{
			/*Vector2 direction = new Vector2(0f, 0f);
				direction.x = Random.Range (-5f, 5f);
				direction.y = 2f;
				direction = direction.normalized*8;*/
			/*SpawnedObj.transform.position = new Vector3(player.transform.position.x + direction.x,
			player.transform.position.y + direction.y,
			player.transform.position.z);*/

			/*GameObject SpawnedObj = Instantiate(spawnAttack) as GameObject;
			SpawnedObj.transform.position = new Vector3 (transform.GetChild(0).position.x + 4f*Mathf.Cos(i*Mathf.PI/(numCinders-1)),
				transform.GetChild(0).position.y - 4f*Mathf.Sin(i*Mathf.PI/(numCinders-1)),
				0f);
			SpawnedObj.GetComponent<BossCinder>().initialDelay = 1.5f*i+1f;*/

			StartCoroutine (CinderShotLeft (i));
		}
	}


	public void RightEyeattack()
	{
		StartCoroutine (EyeFlash (1));
		for (int i=0; i<numCindersRight; i++)
		{
			StartCoroutine (CinderShotRight (i));
		}
	}


	public IEnumerator CinderShotLeft(float num)
	{
		yield return new WaitForSeconds (0.3f*num + 0.5f);
		GameObject SpawnedObj = Instantiate(spawnAttack) as GameObject;
		SpawnedObj.transform.position = new Vector3 (transform.GetChild(0).position.x + 4f*Mathf.Cos(num*Mathf.PI/(numCindersLeft-1)),
			transform.GetChild(0).position.y - 4f*Mathf.Sin(num*Mathf.PI/(numCindersLeft-1)),
			0f);
		SpawnedObj.GetComponent<BossCinder>().initialDelay = 1.5f*num+1.5f - 0.3f*num;
	}

	public IEnumerator CinderShotRight(float num)
	{
		yield return new WaitForSeconds (0.2f*num + 0.5f);
		GameObject SpawnedObj = Instantiate(spawnAttack) as GameObject;
		SpawnedObj.transform.position = new Vector3 (transform.GetChild(1).GetChild(0).position.x,
			transform.GetChild(1).GetChild(0).position.y,
			0f);
		SpawnedObj.GetComponent<BossCinder>().initialDelay = 1.5f;
	}

	public IEnumerator EyeFlash(int num)
	{
		ParticleSystem ps = transform.GetChild (num).GetChild (0).GetComponent<ParticleSystem> ();
		ParticleSystem.ColorOverLifetimeModule col = ps.colorOverLifetime;
		col.enabled = false;
		yield return new WaitForSeconds (0.5f);
		col.enabled = true;
	}


	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		/*if (other.gameObject.tag != "cinder" && other.gameObject.tag != "imp") {
			//print ("BOOM!");
			print (other.gameObject.tag);
		}*/

		if ((other.gameObject.tag == "imp" && other.gameObject.GetComponent<ImpExp>() != null) ||
			(other.gameObject.tag == "impTrigger" && other.transform.parent.gameObject.GetComponent<ImpExp>() != null)) {
			//print ("boom");
			if (--hitPoints <= 0) {
				OnDeath ();
			}
			print (hitPoints);
		}
	}
}
