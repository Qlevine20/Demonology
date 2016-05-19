using UnityEngine;
using System.Collections;

public class FallingSpawner : MonoBehaviour {

	public GameObject spawnModel;
	private float timer = 0f;
	private float timer2 = 0f;
	public float spawnRate = 0.5f;

	Camera mainCamera;
	//Vector3 originalCameraPosition;
	public float shakeAmt = 0f;
	public float shakeTime = 0f;

	// Use this for initialization
    void Start () {
        //float timer = 0.0f;
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		//originalCameraPosition = mainCamera.transform.position;
    }
	
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

		timer2 -= Time.deltaTime;
		if (timer2 <= 0f) {
			timer2 += 4f + Random.Range (0f, 8f);
			//originalCameraPosition = mainCamera.transform.position;
			StartCoroutine(CameraShake(shakeTime));
		}
	}

	public IEnumerator CameraShake(float num)
	{
		if (num >= 0f) {
			//float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			//mainCamera.transform.position = originalCameraPosition;
			Vector3 pp = mainCamera.transform.position;
			pp.x += (Random.value * shakeAmt * 2 - shakeAmt) * (num/shakeTime);
			pp.y += (Random.value * shakeAmt * 2 - shakeAmt) * (num/shakeTime);
			//pp.z += (Random.value * shakeAmt * 2 - shakeAmt) * (num/shakeTime);
			mainCamera.transform.position = pp;
			yield return new WaitForSeconds (0.05f);
			StartCoroutine (CameraShake (num - 0.05f));
		} else {
			//mainCamera.transform.position = originalCameraPosition;
			StopAllCoroutines();
		}
	}
}
