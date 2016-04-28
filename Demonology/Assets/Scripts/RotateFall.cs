using UnityEngine;
using System.Collections;

public class RotateFall : MonoBehaviour {

	private Quaternion baseRot;
	public float rotateSpeed;
	public float lifeTime = 6f;

	// Use this for initialization
	void Start () {
		rotateSpeed = Random.Range (-1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0,0,360*rotateSpeed*Time.deltaTime);
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0f) {
			Destroy(gameObject);
		}
	}
}
