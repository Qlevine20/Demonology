using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
    private bool PowerCrystal;
	// Use this for initialization
	void Start () {
        PowerCrystal = GameObject.FindGameObjectWithTag("PowerCrystal");

        if (PowerCrystal == false)
        {
            tag = "Finish";
        } else {
            tag = "floor";
        }
	}
	
	// Update is called once per frame
	void Update () {
        PowerCrystal = GameObject.FindGameObjectWithTag("PowerCrystal");
        if (PowerCrystal == false)
        {
            tag = "Finish";
        } else {
            tag = "floor";
        }
	}
}
