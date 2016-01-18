using UnityEngine;
using System.Collections;

public class CloudMove : MonoBehaviour {

    private Animator cloudAnim;
    public float speed;
    private float count;
    private float rand;
	// Use this for initialization
	void Start () {
        count = 0;
        rand = RandWait();
        cloudAnim = GetComponent<Animator>();
        
	
	}
	
	// Update is called once per frame
	void Update () {
        count += Time.deltaTime;
        transform.Translate(-(speed * Time.deltaTime),0,0);
        if (transform.position.x < CloudsSpawner.leftX) 
        {
            Destroy(gameObject);
        }
        if (count > rand) 
        {
            if (cloudAnim)
            {
                cloudAnim.SetBool("Flash", true);
                count = 0;
                rand = RandWait();
                //cloudAnim.SetBool("Flash", false);
            }
        }

	}



    int RandWait()
    {
        return Random.Range(2, 4);
    }
}
