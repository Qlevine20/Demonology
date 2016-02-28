using UnityEngine;
using System.Collections;


public class ImpSpawner : MonoBehaviour{
	
	public GameObject Player;
	public GameObject Imp;
	public float waitTime = 3.0f;
	private float counter = 0;
	private bool CheckCreate;
	public bool leftSpawner = false;
	public int maxSpawn = 0;
	public int spawnCount = 0;
	public int sinkingTime = 5;


	void Start(){
		//waitTime = 3.0f;
	}

	void Update(){
		if (Player == null) 
		{
			Player = GameObject.FindGameObjectWithTag("Player");
		}

		counter += Time.deltaTime;
		CheckCreate = RandomSpawnImp(counter);
		if (CheckCreate) 
		{
			counter = 0;
		}
	}


	public virtual void LateUpdate()
	{
		if (CharacterBehavior.Died) 
		{
			spawnCount = 0;
		}
	}


	bool RandomSpawnImp(float counter) 
	{
		if (counter > waitTime && (maxSpawn == 0 || spawnCount < maxSpawn)) 
		{
			if (Player!=null)
			{
				GameObject SpawnedImp = Instantiate(Imp, transform.position, transform.rotation) as GameObject;
				spawnCount++;
				if(leftSpawner == CharacterBehavior.FacingRight){
					Transform spriteHolder = SpawnedImp.transform;
					Vector3 theScale = spriteHolder.localScale;
					theScale.x *= -1;
					spriteHolder.localScale = theScale;

					SpawnedImp.GetComponent<Mobile>().changeDir = true;
				}
				SpawnedImp.GetComponent<ImpAI>().SinkTime = sinkingTime;
				return true;
			}
		}
		return false;
	}



}


