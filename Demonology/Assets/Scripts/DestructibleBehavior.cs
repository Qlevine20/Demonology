using UnityEngine;
using System.Collections;

public class DestructibleBehavior : EnemyBehavior {
	
	public override void Start()
	{
		base.Start ();
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "explosion") {
			OnDeath ();
		}
	}

	public override void OnRespawn()
	{
		base.OnRespawn ();
	}
}
