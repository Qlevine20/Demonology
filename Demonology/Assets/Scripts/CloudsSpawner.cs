using UnityEngine;
using System.Collections;

public class CloudsSpawner : MonoBehaviour {

    public GameObject Cloud;
    public GameObject Player;

    public float left;
    public float right;
    public static float leftX;
    public static float rightX;
    private float counter = 0;
    private bool CheckCreate;
    private int wait_time;
	// Use this for initialization
    void Start() 
    {
        leftX = left;
        rightX = right;
        wait_time = RandWait();
    }
	// Update is called once per frame
	void Update () {
        if (Player == null) 
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        counter += Time.deltaTime;
        CheckCreate = RandomSpawnCloud(counter);
        if (CheckCreate) 
        {
            wait_time = RandWait();
            counter = 0;
        }
	    
	}

    bool RandomSpawnCloud(float counter) 
    {
        if (counter > wait_time) 
        {
            if (Player!=null)
            {
                Instantiate(Cloud, new Vector3(rightX, Player.transform.position.y + 8, transform.position.z), Quaternion.identity);
                return true;
            }
        }
        return false;
    }

    int RandWait() 
    {
        return Random.Range(0, 8);
    }
}
