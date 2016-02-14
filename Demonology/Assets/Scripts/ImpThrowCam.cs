using UnityEngine;
using System.Collections;

public class ImpThrowCam : MonoBehaviour {


    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (DeadlyBehavior.Player)
        {
            //Camera Move with player movement
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(DeadlyBehavior.Player.transform.position);
            Vector3 delta = DeadlyBehavior.Player.transform.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.35f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            CamDampMove(delta);
        }
	
	}




    //Dampened Camera Movement
    void CamDampMove(Vector3 delta) 
    {
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
    }
}
