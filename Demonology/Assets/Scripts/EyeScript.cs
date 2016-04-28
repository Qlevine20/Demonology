using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EyeScript : MonoBehaviour
{

    public GameObject RightEye;
    public GameObject LeftEye;
    private Transform Player;
    private Transform LeftPupil;
    private Transform RightPupil;

    public float speed;
    public int radius;

    public int divider;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        LeftPupil = LeftEye.transform.FindChild("EyePupil");
        RightPupil = RightEye.transform.FindChild("EyePupil");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Player != null)
        {
            //Debug.DrawRay(LeftEye.transform.position, Player.transform.position - (transform.position - LeftEye.transform.position) - LeftEye.transform.position, Color.green);
            //Debug.DrawRay(RightEye.transform.position, Player.transform.position - (transform.position - LeftEye.transform.position) - LeftEye.transform.position, Color.green);


            Vector3 Diff = transform.position - LeftEye.transform.position;

            Vector3 Lvector = new Vector3(LeftEye.transform.position.x + ((Player.transform.position.x + Diff.x) / divider), LeftEye.transform.position.y + ((Player.transform.position.y)), LeftPupil.transform.position.z);
            Vector3 Rvector = new Vector3(RightEye.transform.position.x + ((Player.transform.position.x + Diff.x) / divider), RightEye.transform.position.y + ((Player.transform.position.y)), RightPupil.transform.position.z);


            if (Vector3.Distance(LeftEye.transform.position, Lvector) * .9f < radius)
            {
                LeftPupil.transform.position = Vector3.Lerp(LeftPupil.transform.position, Lvector, speed * Time.deltaTime);
                RightPupil.transform.position = Vector3.Lerp(RightPupil.transform.position, Rvector, speed * Time.deltaTime);
            }
        }
        else
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
