using UnityEngine;
using System.Collections;

public class StepSwitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        MoveDown();
        SomethingHappens();
    }

    public void MoveDown()
    {
        transform.Translate(Vector3.down, Space.World);
    }

    public void SomethingHappens()
    {
        //a door is opened?? idk how to do that yet
    }



    // Update is called once per frame
    void Update () {
	
	}
}
