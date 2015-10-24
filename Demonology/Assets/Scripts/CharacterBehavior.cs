using UnityEngine;
using System.Collections;

public class CharacterBehavior : MonoBehaviour {


    //List of all game objects that kill player instantly
    public GameObject[] DeadlyObs;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D other)
    {


        for (int i = 0; i < DeadlyObs.Length; i++)
        {
            if(DeadlyObs[i].gameObject.tag == other.gameObject.tag)
            {

                //place load checkpoint here when we figure that part out
                Application.LoadLevel(Application.loadedLevel);
                break;
            }

        }
        
    }
}
