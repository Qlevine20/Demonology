using UnityEngine;
using System.Collections;

public class DeadlyBehavior : MonoBehaviour {


	public Animation DeathAnim;
    //List of all game objects that kill player instantly
	public GameObject[] DeadlyObs;

	public static GameObject Player;
    private Animator DeadlyPlayerAnim;
    private GameObject spriteHolder;

	public virtual void Start()
	{
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            spriteHolder = GameObject.FindGameObjectWithTag("SpriteHolder");
            DeadlyPlayerAnim = spriteHolder.GetComponent<Animator>();
        }
	}

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
		CheckDeath (other.gameObject,DeadlyObs);
        
    }

	public virtual void Update()
	{
		Player = GameObject.FindGameObjectWithTag ("Player");
        spriteHolder = GameObject.FindGameObjectWithTag("SpriteHolder");
	}


    //Checks to see if the object has touched 
	public virtual void CheckDeath(GameObject other, GameObject[]DeadlyObs)
	{
        if (Player)
        {
            DeadlyPlayerAnim = spriteHolder.GetComponent<Animator>();
        }
		for (int i = 0; i < DeadlyObs.Length; i++)
		{
			if(DeadlyObs[i].gameObject.tag == other.tag)
			{
                if(DeadlyPlayerAnim)
                {
                    if(other.tag == "magma" || other.tag == "cinder")
                    {
						if (other.tag == "cinder") {
							// that's no magma, it's a gravity cinder!
							if (!CharacterBehavior.Dying)
							{
								DeadlyPlayerAnim.SetBool("EnemyDeath", true);//placeholder
								CharacterBehavior.Dying = true;
							}
							return;
						}
                        else if (!CharacterBehavior.Dying)
                        {
                            DeadlyPlayerAnim.SetBool("LavaDeath", true);
                            CharacterBehavior.Dying = true;
                        }
                    }
                    else if(other.tag == "spike")
                    {
                        if (!CharacterBehavior.Dying)
                        {
                            DeadlyPlayerAnim.SetBool("SpikeDeath", true);
                            CharacterBehavior.Dying = true;
                        }
                        
                    }
                    else if(other.tag == "enemy")
                    {
                        if (!CharacterBehavior.Dying)
                        {
                            DeadlyPlayerAnim.SetBool("EnemyDeath", true);
                            CharacterBehavior.Dying = true;
                        }
                    }
                }
				//OnDeath ();
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
