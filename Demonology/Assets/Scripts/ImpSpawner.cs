using UnityEngine;
using System;
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
    public bool LoadingScreen = false;

	public float activeRange = 15.0f;


	void Start(){
		//waitTime = 3.0f;
		Player = GameObject.FindGameObjectWithTag("Player");
		counter = waitTime;
	}

	void Update(){
		if (Player == null && !LoadingScreen) 
		{
			Player = GameObject.FindGameObjectWithTag("Player");
		}

		if (LoadingScreen || DistanceBetween (transform.position, Player.transform.position) < activeRange) {
			counter += Time.deltaTime;
			CheckCreate = RandomSpawnImp(counter);
			if (CheckCreate) 
			{
				counter = 0;
			}
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
			if (Player!=null || LoadingScreen)
			{
				GameObject SpawnedImp = Instantiate(Imp, transform.position, transform.rotation) as GameObject;
				spawnCount++;
				if(leftSpawner == CharacterBehavior.FacingRight){
                    SpawnedImp.GetComponent<Mobile>().changeDir = true;
                    SpawnedImp.GetComponent<Mobile>().Flip();
                    
                    //Transform spriteHolder = SpawnedImp.transform;
                    //Vector3 theScale = spriteHolder.localScale;
                    //theScale.x *= -1;
                    //spriteHolder.localScale = theScale;

                    //SpawnedImp.GetComponent<Mobile>().changeDir = true;
				}
				SpawnedImp.GetComponent<ImpAI>().SinkTime = sinkingTime;
				return true;
			}
		}
		return false;
	}


	// Find the distance between two points
	public float DistanceBetween (Vector2 pos1, Vector2 pos2)
	{
		float xPos = pos1.x - pos2.x;
		float yPos = pos1.y - pos2.y;
		return (float)Math.Sqrt (xPos*xPos + yPos*yPos);
	}
}


