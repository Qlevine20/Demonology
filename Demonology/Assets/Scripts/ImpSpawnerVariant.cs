using UnityEngine;
using System;
using System.Collections;


public class ImpSpawnerVariant : MonoBehaviour{
	
	public GameObject Player;
	public GameObject Imp;
	public float startDelay = 0.0f;
	public float waitTime = 3.0f;
	private float counter = 0;
	private bool CheckCreate;
	public bool leftSpawner = false;
	public int maxSpawn = 0;
	public int spawnCount = 0;
	public float sinkingTime = 5;
	public int spawnPlayerAt = 3;
	
	
	void Start(){
		counter = waitTime - startDelay;
	}
	
	void Update(){
		counter += Time.deltaTime;
		CheckCreate = RandomSpawnImp(counter);
		if (CheckCreate) 
		{
			counter = 0;
		}
	}
	
	
	bool RandomSpawnImp(float counter) 
	{
		if (counter > waitTime && (maxSpawn == 0 || spawnCount < maxSpawn)) 
		{
			GameObject SpawnedImp;
			if (spawnCount == spawnPlayerAt) {
				SpawnedImp = Instantiate(Player, transform.position, transform.rotation) as GameObject;
			} else {
				SpawnedImp = Instantiate(Imp, transform.position, transform.rotation) as GameObject;
			}
			spawnCount++;
			if(SpawnedImp.GetComponent<ImpAI>() != null)
			{
				if(leftSpawner == CharacterBehavior.FacingRight){
					SpawnedImp.GetComponent<Mobile>().changeDir = true;
					SpawnedImp.GetComponent<Mobile>().Flip();
				}
				SpawnedImp.GetComponent<ImpAI>().SinkTime = sinkingTime;
				if(spawnCount == spawnPlayerAt+2)
				{
					SpawnedImp.GetComponent<Mobile>().speed = 20f;
					//SpawnedImp.GetComponent<Rigidbody2D>().mass = 1000f;
					SpawnedImp.transform.FindChild("ImpTrigger").GetComponent<CircleCollider2D>().enabled = true;
					//print ("SHOOT!");
				}
			}
			SpawnedImp.transform.parent = transform.parent;
			return true;
		}
		return false;
	}
}


