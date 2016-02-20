using UnityEngine;
using System.Collections;


public class ImpSpawner : MonoBehaviour{

	public GameObject Spawned;
	public GameObject Player;
	public int waitTime;
	public float counter = 0;
	private bool CheckCreate;


	void Start(){
		waitTime = 3f;
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


	bool RandomSpawnImp(float counter) 
	{
		if (counter > waitTime) 
		{
			if (Player!=null)
			{
				//This is where I need to fix the "actually choosing an imp to spawn" problem
				Instantiate(CharacterBehavior.Demons[0], transform.position, transform.rotation);
				return true;
			}
		}
		return false;
	}



}


