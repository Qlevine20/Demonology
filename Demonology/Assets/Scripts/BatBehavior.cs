using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BatBehavior : EnemyBehavior {

	public Vector2[] locs;
	public float speed;
	public float targetRange = 4.0f;
	public bool mobFacingRight;
	protected int Pos;
	protected int ArrayDir;
	protected Vector3 startScale;

	public override void Start()
	{
		base.Start ();
		locs [0] = transform.position;
		ArrayDir = 1;
		Pos = 0;
		startScale = transform.localScale;

		if (!mobFacingRight) {
			Flip ();
		}
	}

	// Update is called once per frame
	public override void Update ()
	{
		base.Update ();

		if (transform.parent != null && transform.parent.gameObject.tag == "imp") {
			return;
		} else {
			gameObject.tag = "enemy";
		}

		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, targetRange);
		bool foundTarget = false;
		if (hitColliders.Length != 0) {
			GameObject closestTarget = hitColliders[0].gameObject;
			float closestDist = 100.0f;
			float dist;
			int i = 0;
			while (i < hitColliders.Length) {
				if (hitColliders[i].gameObject.tag == "Player" || 
				    hitColliders[i].gameObject.tag == "imp" || 
				    hitColliders[i].gameObject.tag == "stickImp"){
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
				//transform.position = Vector3.MoveTowards (transform.position, new Vector3(p.x,p.y,0.0f), speed*Time.deltaTime);
				SmartMove (transform.position, new Vector3(p.x,p.y,0.0f), speed*Time.deltaTime);
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


	// 
	public void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "imp" || other.gameObject.tag == "stickImp") {
			transform.SetParent(other.transform, true);
			transform.localScale = new Vector3(startScale.x/transform.parent.localScale.x, 
			                                   startScale.y/transform.parent.localScale.y, 
			                                   0.0F);
			gameObject.tag = "impkiller";
		}
		if (other.gameObject.tag == "Player") {
			if(transform.parent != null && transform.parent.gameObject.tag == "imp"){
				OnDeath ();
			}
		}
		if (other.gameObject.tag == "explosion" || other.gameObject.tag == "magma") {
			OnDeath ();
		}
	}


	// Find the distance between two points
	public float DistanceBetween (Vector2 pos1, Vector2 pos2)
	{
		float xPos = pos1.x - pos2.x;
		float yPos = pos1.y - pos2.y;
		return (float)Math.Sqrt (xPos*xPos + yPos*yPos);
	}


	// Move in the direction of the given point, return true if you reach that point
	public bool MoveBetweenPoints(Vector2 p)
	{
		//transform.position = Vector3.MoveTowards (transform.position,new Vector3(p.x,p.y,0.0f),speed*Time.deltaTime);
		SmartMove (transform.position, new Vector3 (p.x, p.y, 0.0f), speed * Time.deltaTime);
		if (transform.position == new Vector3(p.x,p.y,0.0f)) 
		{
			return true;
		}
		
		return false;
	}


	// Draw lines for the bat's preprogrammed flight path
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

	public override void OnRespawn()
	{
		base.OnRespawn ();
		ArrayDir = 1;
		Pos = 0;
	}

	//Flips the direction of the sprite
	public virtual void Flip()
	{
		mobFacingRight = !mobFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void SmartMove(Vector3 oldPos, Vector3 moveToPos, float moveDist)
	{
		Vector3 newPos = Vector3.MoveTowards (oldPos, moveToPos, moveDist);
		if ((mobFacingRight && (newPos.x < oldPos.x)) || (!mobFacingRight && (newPos.x > oldPos.x))) {
			Flip ();
		}
		transform.position = newPos;
	}
}
