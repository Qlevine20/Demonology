using UnityEngine;
using System.Collections;

public class HelperText : MonoBehaviour {

	public GameObject tracking;
	public float xOffset = 2.3f;
	public float yOffset = 1.3f;
	
	// Use this for initialization
	void Start () {
		tracking = transform.parent.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Camera.main.WorldToScreenPoint (new Vector2 (tracking.transform.position.x + xOffset*1600f/Screen.width,
		                                                                  tracking.transform.position.y + yOffset*739f/Screen.height));
	}
}
