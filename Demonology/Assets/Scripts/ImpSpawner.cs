using UnityEngine;
using System.Collections;


public class ImpSpawner : MonoBehaviour{
	
	public GameObject Player;
	public GameObject Imp;
	public float waitTime;
	public float counter = 0;
	private bool CheckCreate;
	public bool leftSpawner = false;


	void Start(){
		waitTime = 3.0f;
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
				GameObject SpawnedImp = Instantiate(Imp, transform.position, transform.rotation) as GameObject;
				if(leftSpawner){
					Transform spriteHolder = SpawnedImp.transform;
					Vector3 theScale = spriteHolder.localScale;
					theScale.x *= -1;
					spriteHolder.localScale = theScale;


					SpawnedImp.GetComponent<Mobile>().changeDir = true;
				}
				return true;
			}
		}
		return false;
	}



}


