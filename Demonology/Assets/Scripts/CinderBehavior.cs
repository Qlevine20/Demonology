using UnityEngine;
using System.Collections;

public class CinderBehavior : MovingBlock {

	public ParticleSystem cParts;
    //private ParticleSystem.EmissionModule em;
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
        //em = cParts.emission;
		//em.enabled = false;
		cParts.enableEmission = false;
		yield return new WaitForSeconds (0.5f);
		transform.position = locs [0];
		cParts.Clear ();
		yield return new WaitForSeconds (pauseTime);
		//em.enabled = true;
		cParts.enableEmission = true;
		yield return new WaitForSeconds (0.1f);
		wait = false;
	}
}
