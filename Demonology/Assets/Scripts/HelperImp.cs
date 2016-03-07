using UnityEngine;
using System;
using System.Collections;

public class HelperImp : MonoBehaviour {

	public GameObject Poof;
	private GameObject Player;
	private SpriteRenderer sprite;
	public float activeRange = 15.0f;
	private bool active;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag("Player");
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		transform.FindChild("OtherCanvas").gameObject.SetActive(false);
		sprite.enabled = false;
		active = false;
	}

	// Update is called once per frame
	void Update () {
		if (Player == null) 
		{
			Player = GameObject.FindGameObjectWithTag("Player");
		}

		if (!active && DistanceBetween (transform.position, Player.transform.position) < activeRange) {
			transform.FindChild("OtherCanvas").gameObject.SetActive(true);
			sprite.enabled = true;
			active = true;
			Instantiate (Poof, transform.position, Quaternion.identity);
		} else if (active && DistanceBetween (transform.position, Player.transform.position) >= activeRange) {
			transform.FindChild("OtherCanvas").gameObject.SetActive(false);
			sprite.enabled = false;
			active = false;
			Instantiate (Poof, transform.position, Quaternion.identity);
		}
	}


	// Find the distance between two points
	public float DistanceBetween (Vector2 pos1, Vector2 pos2)
	{
		float xPos = pos1.x - pos2.x;
		float yPos = pos1.y - pos2.y;
		return (float)Math.Sqrt (xPos*xPos + yPos*yPos);
	}
}
