using UnityEngine;
using System.Collections;

public class CinderBehavior : MovingBlock {

	public ParticleSystem cParts;
	public float pauseTime = 2f;
	private bool wait = false;

	// Update is called once per frame
	public override void Update () {
		if (!wait) {
			if (MoveBetweenPoints (locs [Pos])) {
				Pos++;
			
				if (Pos >= locs.Length) {
					Pos = 0;
					StartCoroutine (WaitTime ());
				}
			}
		}
	}

	public IEnumerator WaitTime()
	{
		wait = true;
		cParts.enableEmission = false;
		yield return new WaitForSeconds (0.5f);
		transform.position = locs [0];
		cParts.Clear ();
		yield return new WaitForSeconds (pauseTime);
		cParts.enableEmission = true;
		yield return new WaitForSeconds (0.1f);
		wait = false;
	}
}
