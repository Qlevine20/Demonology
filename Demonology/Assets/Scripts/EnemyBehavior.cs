using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	protected Vector3 startPos;
	protected Quaternion startRot;

	// Use this for initialization
	public virtual void Start () 
	{
		startPos = transform.position;
		startRot = transform.rotation;
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
		/*if (CharacterBehavior.Died) 
		{
			CharacterBehavior.Died = false;
		}*/
	}
	
	
	public virtual void LateUpdate()
	{
		if (CharacterBehavior.Died) 
		{
			OnRespawn ();
		}
	}

	public virtual void OnDeath()
	{
		CharacterBehavior.KilledEnemies.Add (gameObject);
		gameObject.SetActive (false);
	}

	public virtual void OnRespawn()
	{
		transform.position = startPos;
		transform.rotation = startRot;
	}
}
