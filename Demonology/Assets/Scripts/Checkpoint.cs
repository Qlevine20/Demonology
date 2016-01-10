using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {


	public bool touched; //Bool to check if player touched Checkpoint
	private Animator Anim; //Checkpoints Animator

	void Start()
	{
		Anim = GetComponent<Animator> ();
	}
	void Update()
	{
		if (touched) 
		{
			if(Anim!=null)
			{
                //Activates the Animator on the checkpoint game object when touched.
				Anim.SetBool ("active",true);
			}
		}
	}
}
