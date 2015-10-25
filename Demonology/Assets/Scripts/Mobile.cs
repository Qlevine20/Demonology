using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour {


    public float speed = 1;
    public float wallDist = 1.0f;
    protected bool forwardDir;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Movement()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        {
        forwardDir = !forwardDir;
        }
    }

    //public void startDir()
    //{

    //}
}
