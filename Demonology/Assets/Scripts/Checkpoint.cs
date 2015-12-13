using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {

	Animator animator;
	public bool active = false; // False until player activates
								// Used for enabling animation

	// Use this for initialization
	public GameObject player;
	//set for crystals[]
	//Keep track of crystals picked up until you hit a checkpoint, and then erase the set and start over.
	//If you respawn at a checkpoint, reinstantiate all the crystals in the set

	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {



	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.tag == "Player") {
			animator.SetBool ("active", true);
		}
	}
}
