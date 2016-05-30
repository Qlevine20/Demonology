using UnityEngine;
using System.Collections;

public class EndTextScrollingScript : MonoBehaviour {

    public float speed;
    public Transform EndScroll;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 move = new Vector3(0, speed * Time.deltaTime, 0);
	    transform.Translate(move);
        EndScroll.transform.Translate(move);
        if (EndScroll.transform.localPosition.y > 0) 
        {
            Application.LoadLevel(16);
        }
	}
}
