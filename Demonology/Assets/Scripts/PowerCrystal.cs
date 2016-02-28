using UnityEngine;
using System.Collections;

public class PowerCrystal : EnemyBehavior
{
    public void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            OnDeath();
        }

    }
}
