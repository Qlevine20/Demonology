using UnityEngine;
using System.Collections;

public class ImpExp : ImpAI {

	public float TimeToExp;
	public GameObject Psyst;
	private float CurrTime;
	// Use this for initialization
	public override void Start () 
	{
		CurrTime = 0;
		base.Start ();
		WaitTime (TimeToExp);
	}

	public override void OnDeath()
	{
		Instantiate (Psyst, transform.position, Quaternion.identity);
		base.OnDeath ();

	}

	public override void Update()
	{
		base.Update ();
		CurrTime += Time.deltaTime;
		CheckExp ();
	}

	public void CheckExp()
	{
		if (CurrTime >= TimeToExp) 
		{
			OnDeath ();
		}
	}
}
