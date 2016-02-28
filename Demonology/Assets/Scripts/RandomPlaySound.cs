using UnityEngine;
using System.Collections;

public class RandomPlaySound : MonoBehaviour {

	/*public AudioClip music1;
	public AudioClip music2; 
	public AudioClip music3; 
	private int randomMusic = 0;
	private float randomTime = 120.0f;
	private float timeCounter = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(timeCounter  > randomTime)
		{
			randomTime = Random.Range(60.0, 180.0);
			timeCounter = 0.0;
			GetComponent<AudioSource>().Stop();
			ChooseMusic();
			GetComponent<AudioSource>().Play();
		}
		
		timeCounter += Time.deltaTime;
	}

	void ChooseMusic()
	{   
		randomMusic = Random.Range(0, 3);
		
		switch (randomMusic)
		{
		case 0: 
			GetComponent<AudioSource>().clip = music1; 
			break;
		case 1: 
			GetComponent<AudioSource>().clip = music2; 
			break;
		case 2: 
			GetComponent<AudioSource>().clip = music3; 
			break;
		}
		
		Debug.Log( "Current Clip = music" + (randomMusic + 1) );
	}*/

}

