using UnityEngine;
using System.Collections;

public class PowerCrystal : EnemyBehavior
{
    public GameObject PartEffect;
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
}
