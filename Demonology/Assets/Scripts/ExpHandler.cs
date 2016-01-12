using UnityEngine;
using System.Collections;

public class ExpHandler : MonoBehaviour {

	private ParticleSystem expParts;

	// Use this for initialization
	void Start () {
		expParts = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!expParts.IsAlive ()) {
			Destroy (gameObject);
		}
	}
}
