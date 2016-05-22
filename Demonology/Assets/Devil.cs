using UnityEngine;
using System.Collections;

public class Devil : MonoBehaviour {


    public float WalkingSpeed = 5;
    public float TimeWalking = 5;
    private float counter = 0;
    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (TimeWalking > counter)
        {
            counter+=Time.deltaTime;
            transform.Translate(new Vector3(-(WalkingSpeed * Time.deltaTime), 0.0f, 0.0f));
           
        }
        else if (anim.GetBool("Walking") == true) 
        {
            anim.SetBool("Walking",false);
        }
        
	}
}
