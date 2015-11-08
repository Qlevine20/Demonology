using UnityEngine;
using System.Collections;

public class MovingBlock : MonoBehaviour {
	
	public Vector2[] locs;
	public float speed;
	//Array Position
	private int Pos;
	private int ArrayDir;
	// Use this for initialization
	void Start () {
		locs [0] = transform.position;
		ArrayDir = 1;
		Pos = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (MoveBetweenPoints (locs [Pos])) 
		{
			Pos += ArrayDir;

			if (Pos >= locs.Length || Pos < 0) 
			{
				ArrayDir = -ArrayDir;
				Pos += ArrayDir*2;
				//print ("Change ArrayDir");
			}
			//print ("Increment Pos");


		}
	}



	bool MoveBetweenPoints(Vector2 p)
	{
		transform.position = Vector3.MoveTowards (transform.position,new Vector3(p.x,p.y,0.0f),speed*Time.deltaTime);
		//print ("movement");
		if (transform.position == new Vector3(p.x,p.y,0.0f)) 
		{
			return true;
		}

		return false;
	}
}
