using UnityEngine;
using System.Collections;

public class DeadlyBehavior : MonoBehaviour {


    //List of all game objects that kill player instantly
	public Animation DeathAnim;
	public GameObject[] DeadlyObs;
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
		CheckDeath (other.gameObject,DeadlyObs);

        
    }

	public virtual void CheckDeath(GameObject other, GameObject[]DeadlyObs)
	{
		for (int i = 0; i < DeadlyObs.Length; i++)
		{
			if(DeadlyObs[i].gameObject.tag == other.tag)
			{
				OnDeath ();
			}
			
		}

	}

	public virtual void OnDeath()
	{
		if (DeathAnim != null) 
		{
			DeathAnim.Play ();
		}
		Destroy (gameObject);
	}

	public virtual IEnumerator WaitTime(float num)
	{
		print (Time.time);
		yield return new WaitForSeconds (num);
		print (Time.time);
	}


}
