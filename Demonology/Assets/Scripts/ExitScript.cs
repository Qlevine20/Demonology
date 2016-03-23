using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour
{

    public GameObject PowerCrystal;
    public GameObject Exit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PowerCrystal = GameObject.FindGameObjectWithTag("PowerCrystal");
        
        if (PowerCrystal == null)
        {
            transform.gameObject.tag = "Finish";
        }

        //once no more crystals, activate the exit

    }
}
