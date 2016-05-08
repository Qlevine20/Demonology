using UnityEngine;
using System.Collections;

public class FallingSpawner : MonoBehaviour {

	public GameObject spawnModel;
	private float timer = 0f;
	public float spawnRate = 0.5f;

	// Use this for initialization
    //void Start () {
    //    float timer = 0.0f;
    //}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0f) {
			timer += spawnRate + Random.Range(-spawnRate/2, spawnRate/2);
			GameObject SpawnedObj = Instantiate(spawnModel) as GameObject;
			Vector3 newPos = GameObject.FindGameObjectWithTag("Player").transform.position;
			Vector3 newScale = SpawnedObj.transform.localScale;

			newPos.x += Random.Range (-25f, 25f);
			newPos.y += 40f;
			newPos.z += Random.Range(-13f, 13f);
			float randDir = ((float)Random.Range(0,2)-0.5f)*2f;
			newScale.x *= randDir;
			SpawnedObj.transform.position = newPos;
			SpawnedObj.transform.localScale = newScale;

			if( newPos.z >= 0 ) {
				SpawnedObj.GetComponent<SpriteRenderer>().sortingLayerName = "DisplayBehind";
			}
			else {
				SpawnedObj.GetComponent<SpriteRenderer>().sortingLayerName = "DisplayFront";
			}
		}
	}
}
