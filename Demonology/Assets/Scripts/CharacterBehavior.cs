using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterBehavior : DeadlyBehavior {

	public KeyCode Summon = KeyCode.Q;
	public GameObject[] Minions;
	public int[] currentMats;
	public GameObject Selected;
	//private Dictionary<string,int> currentMats = new Dictionary<string,int >();
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
		 
		if(checkMaterials() && GameObject.FindGameObjectsWithTag(Selected.tag).Length<maxMins)
		{
			Instantiate (Selected, transform.position,transform.rotation);
			int[] reqMats = Selected.GetComponent<DemonBehavior>().reqMats;
			for (int i=0; i<reqMats.Length; i++) 
			{
				currentMats[i] -= reqMats[i];
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
		int[] newMats = pickUp.GetComponent<CrystalScript>().newMats;
		for (int i=0; i<currentMats.Length; i++) 
		{
			currentMats[i] += newMats[i];
		}
		Destroy (pickUp);
	}

	public virtual bool checkMaterials()
	{
		int[] reqMats = Selected.GetComponent<DemonBehavior>().reqMats;
		for (int i=0; i<reqMats.Length; i++) 
		{
			if(currentMats[i] < reqMats[i])
			{
				return false;
			}
		}

		return true;
	}

}
