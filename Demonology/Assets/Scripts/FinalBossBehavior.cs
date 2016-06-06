using UnityEngine;
using System.Collections;

public class FinalBossBehavior : EnemyBehavior {

	public int startHitPoints = 10;
	public int hitPoints;
	public float attackRate = 2.5f;
	public float easyBatRate = 3f;
	public float mediumBatRate = 1.5f;
	public float hardBatRate = 1.0f;
	public float criticalBatRate = 0.6f;
	public int numCindersLeft = 4;
	public int numCindersRight = 4;
	private float timer = 0f;
	private float timer2 = 0f;
	//private GameObject player;
	public GameObject spawnAttack;
	public GameObject spawnBat;
	public GameObject EyeObject;
	private bool invincible;
	public AudioClip HitSound;
	public GameObject normalMusic;
	public GameObject bossMusic;

	public float shakeAmt = 0f;
	public float shakeTime = 0f;
	private bool dying = false;
	public GameObject exitPortal;
	public bool cheatCode = false;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		//timer = attackRate;
		timer = 2f;
		invincible = false;
		hitPoints = startHitPoints;

		/*if (GameObject.Find("Character") != null)
		{
			player = GameObject.Find("Character");
		}*/
	}
	
	// Update is called once per frame
	public override void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0f && !dying) {
			timer += attackRate;

			if (Random.Range (-5f, 5f) < 0) {
				LeftEyeAttack ();
			} else {
				RightEyeattack ();
			}
		}

		timer2 -= Time.deltaTime;
		if (timer2 <= 0f && !dying) {
			if (hitPoints >= 4 * startHitPoints / 5) {
				timer2 += easyBatRate;
			} else if (hitPoints >= 3 * startHitPoints / 5) {
				timer2 += mediumBatRate;
			} else if (hitPoints >= startHitPoints / 5) {
				timer2 += hardBatRate;
			} else {
				timer2 += criticalBatRate;
			}

			GameObject SpawnedObj = Instantiate(spawnBat) as GameObject;
			SpawnedObj.name = "BossBat";
			if (Random.Range (-5f, 5f) < 0) {
				SpawnedObj.transform.position = new Vector3 (transform.position.x - 20f,
					transform.position.y + Random.Range(-2f, 15f),
					0f);
			} else {
				SpawnedObj.transform.position = new Vector3 (transform.position.x + 20f,
					transform.position.y + Random.Range(-2f, 15f),
					0f);
			}
		}

		if (GameObject.FindGameObjectWithTag ("PowerCrystal") != null) {
			EyeObject.SetActive (true);
			ParticleSystem cParts = EyeObject.GetComponent<ParticleSystem> ();
			if (cParts != null) {
				cParts.enableEmission = false;
				cParts.Clear ();
			}
			if (bossMusic != null) {
				bossMusic.GetComponent<AudioSource> ().Stop ();
				normalMusic.GetComponent<AudioSource> ().Play ();
			}
			gameObject.SetActive (false);
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
		yield return new WaitForSeconds (1.2f*num/numCindersLeft + 0.5f);
		GameObject SpawnedObj = Instantiate(spawnAttack) as GameObject;
		SpawnedObj.transform.position = new Vector3 (transform.GetChild(0).position.x + 4f*Mathf.Cos(num*Mathf.PI/(numCindersLeft-1)),
			transform.GetChild(0).position.y - 4f*Mathf.Sin(num*Mathf.PI/(numCindersLeft-1)),
			0f);
		SpawnedObj.GetComponent<BossCinder>().initialDelay = 1.5f - 1.2f*num/numCindersLeft;
		//SpawnedObj.GetComponent<BossCinder>().initialDelay = 1.5f*num+1.5f - 0.3f*num;
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

	public IEnumerator HitSquint(float num)
	{
		invincible = true;
		ParticleSystem ps1 = transform.GetChild (0).GetChild (0).GetComponent<ParticleSystem> ();
		ParticleSystem ps2 = transform.GetChild (1).GetChild (0).GetComponent<ParticleSystem> ();
		ps1.startSize = 0.6f; ps1.Clear (); ps1.Simulate (2f); ps1.Play ();
		ps2.startSize = 0.6f; ps2.Clear (); ps2.Simulate (2f); ps2.Play ();
		yield return new WaitForSeconds (num);
		ps1.startSize = 2f; //ps1.Clear (); ps1.Simulate (2f); ps1.Play ();
		ps2.startSize = 2f; //ps2.Clear (); ps2.Simulate (2f); ps2.Play ();
		invincible = false;
	}

	public IEnumerator ScreenShake(float num)
	{
		if (num >= 0f) {
			//float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			//mainCamera.transform.position = originalCameraPosition;
			Vector3 pp = Camera.main.transform.position;
			pp.x += (Random.value * shakeAmt * 2 - shakeAmt) * (num/shakeTime);
			pp.y += (Random.value * shakeAmt * 2 - shakeAmt) * (num/shakeTime);
			if (num >= shakeTime *6/10) {
				pp.z -= Mathf.Abs (Random.value * shakeAmt * 2 - shakeAmt) * (num / 2 / shakeTime);
			} else {
				pp.z += Mathf.Abs (Random.value * shakeAmt * 2 - shakeAmt) * (num / 2 / shakeTime);
			}
			Camera.main.transform.position = pp;
			yield return new WaitForSeconds (0.05f);
			StartCoroutine (ScreenShake (num - 0.05f));
		}
	}

	public IEnumerator DeathSequence()
	{
		dying = true;
		GameObject[] killTheBat = GameObject.FindGameObjectsWithTag("enemy");
		for (int i = 0; i < killTheBat.Length; i++){
			if (killTheBat [i].name == "BossBat") {
				GameObject.Destroy (killTheBat [i]);
			}
		}
		StartCoroutine (ScreenShake (shakeTime));
		transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
		transform.GetChild (1).GetChild (1).gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		transform.GetChild (0).GetChild (0).GetComponent<ParticleSystem> ().enableEmission = false;
		transform.GetChild (1).GetChild (0).GetComponent<ParticleSystem> ().enableEmission = false;
		transform.GetChild (0).GetComponent<ParticleSystem> ().enableEmission = false;
		transform.GetChild (1).GetComponent<ParticleSystem> ().enableEmission = false;
		yield return new WaitForSeconds (2f);
		if (cheatCode) {
			Application.LoadLevel (14);
		}
		exitPortal.SetActive (true);
		OnDeath ();
	}


	public virtual void OnTriggerEnter2D(Collider2D other)
	{
		/*if (other.gameObject.tag != "cinder" && other.gameObject.tag != "imp") {
			//print ("BOOM!");
			print (other.gameObject.tag);
		}*/

		if (!invincible && ((other.gameObject.tag == "imp" && other.gameObject.GetComponent<ImpExp>() != null) ||
			(other.gameObject.tag == "impTrigger" && other.transform.parent.gameObject.GetComponent<ImpExp>() != null)) ) {
			//print ("boom");
			if (--hitPoints <= 0) {
				StopAllCoroutines ();
				//OnDeath ();
				StartCoroutine(DeathSequence());
			}
			//print (hitPoints);
			AudioSource.PlayClipAtPoint (HitSound, Camera.main.transform.position, 100.0f);
			StartCoroutine (HitSquint (2.0f));
		}
	}

	public override void OnRespawn()
	{
		base.OnRespawn ();
		timer = 2f;
		invincible = false;
		hitPoints = startHitPoints;
		dying = false;
		StopAllCoroutines ();

		ParticleSystem ps1 = transform.GetChild (0).GetChild (0).GetComponent<ParticleSystem> ();
		ParticleSystem ps2 = transform.GetChild (1).GetChild (0).GetComponent<ParticleSystem> ();
		ps1.startSize = 2f; ps1.Clear (); ps1.Simulate (2f); ps1.Play ();
		ps2.startSize = 2f; ps2.Clear (); ps2.Simulate (2f); ps2.Play ();
		ParticleSystem.ColorOverLifetimeModule col = ps1.colorOverLifetime; col.enabled = true;
		col = ps2.colorOverLifetime; col.enabled = true;
	}
}
