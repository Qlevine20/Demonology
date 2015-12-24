using UnityEngine;
using System.Collections;

public class MovingBlock : MonoBehaviour {
	
	public Vector2[] locs;
	public float speed;
	//Array Position
	protected int Pos;
	protected int ArrayDir;

	// Use this for initialization
	public virtual void Start () {
		locs [0] = transform.position;
		ArrayDir = 1;
		Pos = 0;
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		if (MoveBetweenPoints (locs [Pos])) 
		{
			Pos += ArrayDir;

			if (Pos >= locs.Length || Pos < 0) 
			{
				ArrayDir = -ArrayDir;
				Pos += ArrayDir*2;
			}
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
}
