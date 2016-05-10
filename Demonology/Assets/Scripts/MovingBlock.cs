using UnityEngine;
using System.Collections;

public class MovingBlock : EnemyBehavior {
	
	public Vector2[] locs;
	public float speed;
	public bool autoStart = false;
	public bool pauseEachPoint = false;

	//Array Position
	protected int Pos;
	protected int ArrayDir;
	protected bool Moving;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		locs [0] = transform.position;
		ArrayDir = 1;
		Pos = 0;
		Moving = autoStart;
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		if (Moving && MoveBetweenPoints (locs [Pos])) 
		{
			if((!autoStart && Pos == 0 && (ArrayDir < 0)) || (pauseEachPoint && Pos != 0))
			{
				Moving = false;
				//print ("check");
			}

			Pos += ArrayDir;

			if (Pos >= locs.Length || Pos < 0) 
			{
				ArrayDir = -ArrayDir;
				Pos += ArrayDir*2;
			}
		}
	}

	public void OnCollisionEnter2D(Collision2D other)
	{
		if ((!autoStart || pauseEachPoint) && (other.gameObject.tag == "imp" || other.gameObject.tag == "Player")) {
			Moving = true;
		}
	}

    public void OnDrawGizmos()
    {
        if (locs.Length < 1)
        {
            return;
        }

        for (int i = 1; i < locs.Length; i++)
        {
            Gizmos.DrawCube(locs[i - 1], new Vector3(1, 1, 0));
            Gizmos.DrawLine(locs[i - 1], locs[i]);
        }
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


	public void OnTriggerEnter2D(Collider2D other)
	{
		if ((other.gameObject.tag == "floor" || other.gameObject.tag == "spike")&& other.gameObject.layer == 15) {
			ArrayDir = -ArrayDir;
			Pos += ArrayDir;
			if (Pos >= locs.Length || Pos < 0) 
			{
				ArrayDir = -ArrayDir;
				Pos += ArrayDir*2;
			}
		}
	}


	public override void OnRespawn () 
	{
		base.OnRespawn ();
		ArrayDir = 1;
		Pos = 0;
		Moving = autoStart;
	}
}
