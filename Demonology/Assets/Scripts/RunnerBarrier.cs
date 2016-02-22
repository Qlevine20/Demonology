using UnityEngine;
using System.Collections;

public class RunnerBarrier : MonoBehaviour {

	public bool goRight = true;

	public void OnDrawGizmos()
	{
		Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0));
	}
}
