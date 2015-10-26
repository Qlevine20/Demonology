using UnityEngine;
using System.Collections;

public class ImpAI : Mobile {

	// Use this for initialization
	public override void Start () {

		Physics2D.IgnoreLayerCollision(10,9);
		base.Start ();
	}
	// Update is called once per frame

	void OnDeath()
	{
		Physics2D.IgnoreLayerCollision (8, 9, false);
	}
}
