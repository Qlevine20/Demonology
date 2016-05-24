using UnityEngine;
using System.Collections;

public class Devil : MonoBehaviour {


    public float WalkingSpeed = 5;
    public float TimeWalking = 5;
    public bool WalkOnAwake;
    public bool BackUp = false;
    private float counter = 0;
    private Animator anim;
    private float counter2 = 0;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.S)) 
        {
            counter2 += Time.deltaTime;
            Debug.Log(counter2);
        }
        if (Input.GetKeyUp(KeyCode.S)) 
        {
            counter2 = 0;
        }
        if (TimeWalking > counter && WalkOnAwake)
        {
            counter += Time.deltaTime;
            transform.Translate(new Vector3(-(WalkingSpeed * Time.deltaTime), 0.0f, 0.0f));
           
        }
        else if (anim.GetBool("Walking") == true && !BackUp) 
        {
            anim.SetBool("Walking",false);
        }
        else if (BackUp) 
        {
            anim.SetBool("Walking", transform.parent.GetComponent<Animator>().GetBool("Walking"));
        }
        
	}
}
