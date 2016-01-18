using UnityEngine;
using System.Collections;

public class CloudSound : MonoBehaviour {

    public AudioClip[] lightningStrikes;


	// Use this for initialization
	void Start () {
        AudioSource.PlayClipAtPoint(
            lightningStrikes[Random.Range(0, lightningStrikes.Length)], transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
