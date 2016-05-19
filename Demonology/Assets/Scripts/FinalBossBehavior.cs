using UnityEngine;
using System.Collections;

public class FinalBossBehavior : EnemyBehavior {

	public float attackRate = 2.5f;
	public int numCinders = 4;
	private float timer = 0f;
	private GameObject player;
	public GameObject spawnAttack;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		timer = attackRate;

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

			for (int i=0; i<numCinders; i++)
			{
				Vector2 direction = new Vector2(0f, 0f);
				direction.x = Random.Range (-5f, 5f);
				direction.y = 2f;
				direction = direction.normalized*8;

				GameObject SpawnedObj = Instantiate(spawnAttack) as GameObject;
				SpawnedObj.transform.position = new Vector3(player.transform.position.x + direction.x,
				                                            player.transform.position.y + direction.y,
				                                            player.transform.position.z);
				SpawnedObj.GetComponent<BossCinder>().initialDelay = 1.5f*i+1f;
			}
		}
	}
}
