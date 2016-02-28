using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour
{

    public GameObject PowerCrystal;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (PowerCrystal == null)
        {
            PowerCrystal = GameObject.FindGameObjectWithTag("PowerCrystal");
        }

        //once no more crystals, activate the exit

    }
}
