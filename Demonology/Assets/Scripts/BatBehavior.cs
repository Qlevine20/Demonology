using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BatBehavior : EnemyBehavior {

	public Vector2[] locs;
	public float speed;
	protected int Pos;
	protected int ArrayDir;

	public override void Start()
	{
		base.Start ();
		locs [0] = transform.position;
		ArrayDir = 1;
		Pos = 0;
	}

	// Update is called once per frame
	public override void Update ()
	{
		if (CharacterBehavior.Died) 
		{
			transform.position = startPos;
			ArrayDir = 1;
			Pos = 0;
		}

		if (transform.parent != null) {
			return;
		}

		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 4.0f);
		bool foundTarget = false;
		if (hitColliders.Length != 0) {
			GameObject closestTarget = hitColliders[0].gameObject;
			float closestDist = 100.0f;
			float dist;
			int i = 0;
			while (i < hitColliders.Length) {
				if (hitColliders[i].gameObject.tag == "Player" || hitColliders[i].gameObject.tag == "imp"){
					dist = DistanceBetween (transform.position, hitColliders[i].gameObject.transform.position);
					if (dist < closestDist){
						closestDist = dist;
						closestTarget = hitColliders[i].gameObject;
					}
				}
				i++;
			}

			if (closestDist < 100.0f){
				foundTarget = true;
				Vector2 p = closestTarget.transform.position;
				transform.position = Vector3.MoveTowards (transform.position, new Vector3(p.x,p.y,0.0f), speed*Time.deltaTime);
			}
		}
		if (!foundTarget) {
			if (MoveBetweenPoints (locs [Pos])) 
			{
				Pos += ArrayDir;
			
				if (Pos >= locs.Length || Pos < 0) {
					ArrayDir = -ArrayDir;
					Pos += ArrayDir * 2;
				}
			}
		}
	}


	public virtual void LateUpdate()
	{
		if (CharacterBehavior.Died) 
		{
			CharacterBehavior.Died = false;
		}
	}


	public void OnCollisionStay2D(Collision2D other)
	{
		if (other.gameObject.tag == "imp") {
			transform.SetParent(other.transform, true);
			transform.localScale = new Vector3(7.0F/transform.parent.localScale.x, 7.0F/transform.parent.localScale.y, 0.0F);
		}
	}


	public float DistanceBetween (Vector2 pos1, Vector2 pos2)
	{
		float xPos = pos1.x - pos2.x;
		float yPos = pos1.y - pos2.y;
		return (float)Math.Sqrt (xPos*xPos + yPos*yPos);
	}


	public bool MoveBetweenPoints(Vector2 p)
	{
		transform.position = Vector3.MoveTowards (transform.position,new Vector3(p.x,p.y,0.0f),speed*Time.deltaTime);
		if (transform.position == new Vector3(p.x,p.y,0.0f)) 
		{
			return true;
		}
		
		return false;
	}

	public void OnDrawGizmos()
	{
		if (locs.Length < 1)
		{
			return;
		}
		
		for (int i = 1; i < locs.Length; i++)
		{
			Gizmos.DrawLine(locs[i - 1], locs[i]);
		}
	}
}
