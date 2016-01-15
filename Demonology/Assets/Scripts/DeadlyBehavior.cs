using UnityEngine;
using System.Collections;

public class DeadlyBehavior : MonoBehaviour {


	public Animation DeathAnim;
    //List of all game objects that kill player instantly
	public GameObject[] DeadlyObs;

	public static GameObject Player;
    private Animator PlayerAnim;

	public virtual void Start()
	{
		Player = GameObject.FindGameObjectWithTag ("Player");
        Player.GetComponent<Animator>();
	}

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
		CheckDeath (other.gameObject,DeadlyObs);
        
    }

	public virtual void Update()
	{
		Player = GameObject.FindGameObjectWithTag ("Player");
	}


    //Checks to see if the object has touched 
	public virtual void CheckDeath(GameObject other, GameObject[]DeadlyObs)
	{
		for (int i = 0; i < DeadlyObs.Length; i++)
		{
			if(DeadlyObs[i].gameObject.tag == other.tag)
			{
                if(PlayerAnim)
                {
                    if(other.tag == "magma")
                    {
                        PlayerAnim.SetBool("LavaDeath",true);
                    }
                    else if(other.tag == "spike")
                    {
                        PlayerAnim.SetBool("SpikeDeath",true);
                    }
                    else if(other.tag == "enemy")
                    {
                        PlayerAnim.SetBool("EnemyDeath",true);
                    }
                }
				OnDeath ();
			}
		}
	}

	public virtual void OnDeath()
	{
		Destroy (gameObject);
	}

	public virtual void HalveCollider(BoxCollider2D bc,float heightChange)
	{
		bc.size = new Vector2(bc.size.x, heightChange);
		bc.offset = new Vector2(bc.offset.x, bc.offset.y - (heightChange/2));
	}

	public virtual void DoubleCollider(BoxCollider2D bc, float heightChange)
	{
		bc.size = new Vector2(bc.size.x, heightChange * bc.size.y);
		bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange*bc.size.y/8));
	}

	public virtual IEnumerator WaitTime(float num)
	{
		print (Time.time);
		yield return new WaitForSeconds (num);
		print (Time.time);
	}


}
