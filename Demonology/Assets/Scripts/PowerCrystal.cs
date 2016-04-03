using UnityEngine;
using System.Collections;

public class PowerCrystal : EnemyBehavior
{
	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnDeath();
        }
	}
}
