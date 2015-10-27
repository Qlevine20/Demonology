using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterBehavior : DeadlyBehavior {

	public KeyCode Summon = KeyCode.Q;
	public GameObject[] Minions;
	public int[] reqMats;
	private Dictionary<string,int> currentMats = new Dictionary<string,int >();
	private Dictionary<string,int> neededMats = new Dictionary<string,int >();
	public int maxMins = 5;

	public struct summMaterials
	{
		public GameObject mat;
		public int numOfMat;
	}

	
	public virtual void Update()
	{
		if (Input.GetKeyDown (Summon)) 
		{
			summon();
		}
	}
	public override void OnDeath()
	{
		if (DeathAnim != null) 
		{
			DeathAnim.Play ();
		}
		//Put in code to go to checkpoint here
		Application.LoadLevel (Application.loadedLevel);
	}

	public virtual void summon()
	{
		foreach (GameObject min in Minions) 
		{
			if(checkMaterials() && GameObject.FindGameObjectsWithTag(min.tag).Length<maxMins)
			{
				Instantiate (min, transform.position,transform.rotation);

			}
		}

	}


	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "crystal") 
		{
			pickUpMat (other.gameObject);
		}

	}

	public virtual void pickUpMat(GameObject pickUp)
	{
		Destroy (pickUp);
		print (currentMats.ContainsKey(pickUp.name));
		if(currentMats.ContainsKey(pickUp.name))
		{
			currentMats[pickUp.name] += 1;
		}
		else
		{
			currentMats[pickUp.name] = 1;
		}
	}

	public virtual bool checkMaterials()
	{
		foreach (string key in neededMats.Keys) 
		{
			if(currentMats[key] < neededMats[key])
			{
				return false;
			}
		}
		return true;
	}

}
