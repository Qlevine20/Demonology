using UnityEngine;
using System.Collections;

public class PowerCrystal : EnemyBehavior
{
	public int baubleNumber = 0;
	public GameObject PartEffect;
	public GameObject bauble;
	Animator anim;

	public override void Start()
	{
		base.Start ();
		switch (baubleNumber) {
		case 0:
			bauble = GameObject.Find("Bauble (0)");
			break;
		case 1:
			bauble = GameObject.Find("Bauble (1)");
			break;
		case 2:
			bauble = GameObject.Find("Bauble (2)");
			break;
		case 3:
			bauble = GameObject.Find("Bauble (3)");
			break;
		case 4:
			bauble = GameObject.Find("Bauble (4)");
			break;
		case 5:
			bauble = GameObject.Find("Bauble (5)");
			break;
		default:
			bauble = GameObject.Find("Bauble (0)");
			break;
		}
		bauble.GetComponent<Animator> ().SetInteger ("ActiveColor", 1);
	}

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Instantiate(PartEffect, transform.position,Quaternion.identity);
            OnDeath();
        }

		if (other.gameObject.tag == "imp" && !other.gameObject.GetComponent<ImpAI>().dead)
		{
			Instantiate(PartEffect, transform.position,Quaternion.identity);
			OnDeath();
		}
	}

	public override void OnDeath ()
	{
		base.OnDeath ();
		bauble.GetComponent<Animator> ().SetInteger ("ActiveColor", 2);
	}

	public override void OnRespawn ()
	{
		base.OnRespawn ();
		bauble.GetComponent<Animator> ().SetInteger ("ActiveColor", 1);
	}
}
