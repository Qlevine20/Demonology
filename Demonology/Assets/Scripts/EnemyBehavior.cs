using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	protected Vector3 startPos;

	// Use this for initialization
	public virtual void Start () 
	{
		startPos = transform.position;
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
		if (CharacterBehavior.Died) 
		{
			transform.position = startPos;
		}
	}

	public virtual void OnDeath()
	{
		CharacterBehavior.KilledEnemies.Add (gameObject);
		gameObject.SetActive (false);
	}
}
