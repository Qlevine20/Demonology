using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	// Update is called once per frame
	void Update () 
	{
		if (DeadlyBehavior.Player)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(DeadlyBehavior.Player.transform.position);
			Vector3 delta = DeadlyBehavior.Player.transform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.35f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
	}
}