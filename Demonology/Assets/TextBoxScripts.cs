using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBoxScripts : MonoBehaviour {
    public float StartOffset = 5;
    public float DisplayTime = 5;
    public float NumTextBoxes;
    public float DevilTalkTime1;
    public float TabooTalkTime1;
    public float TabooTalkTime2;
    public float BackUpTalkTime1;
    public float BackUpTalkTime2;
    private float counter = 0;
    private int Tbox = 0;
    private bool Started = false;
    private AudioSource Poololi;
    public GameObject Taboo;
    public GameObject Devil;
    public GameObject StartCam;
    public GameObject BackUp;
    public GameObject ImpFallSpawner;
    public float TabooFieldOfView;
    public float DevilFieldOfView;
    private GameObject follow;
    private bool DevilTalking = false;
    private bool TabooTalking = false;
    private bool TabooTalking2 = false;
    private bool BackUpTalking = false;
    private bool BackUpTalking2 = false;
    private float StartFieldOfView;
    
    private float totTime;
    
    
    
	// Use this for initialization
	void Start () {
        Poololi = Camera.main.GetComponent<AudioSource>();
        follow = StartCam;
        StartFieldOfView = Camera.main.fieldOfView;
        
	}
	
	// Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        Camera.main.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y + 20.0f, Camera.main.transform.position.z);
        if (Tbox < NumTextBoxes-1 && !Input.GetKey(KeyCode.A))
        {

            if (StartOffset < counter && Started == false) 
            {
                Started = true;
                counter = 0;
                transform.GetChild(Tbox).gameObject.SetActive(true);
            }
            else if (DisplayTime < counter)
            {
                counter = 0;
                transform.GetChild(Tbox).gameObject.SetActive(false);
                Tbox++;
                transform.GetChild(Tbox).gameObject.SetActive(true);
            }
        }
        else if (Poololi.enabled == false && DisplayTime < counter)
        {
            transform.GetChild(Tbox).gameObject.SetActive(false);
            Poololi.enabled = true;
            counter = 0;


        }
        else if(Poololi.enabled == true)
        {
            //Debug.Log(counter);
            if (counter < DevilTalkTime1 && DevilTalking == false)
            {
                GetComponent<AudioSource>().enabled = false;
                DevilTalking = true;
                follow = Devil;
                Camera.main.fieldOfView = DevilFieldOfView;
                totTime = DevilTalkTime1;
                
            }
            else if (TimeChange(counter, totTime,TabooTalkTime1, TabooTalking)) 
            {
                TabooTalking = true;
                ActivateTabooPoolali();
            }
            else if (TimeChange(counter, totTime,TabooTalkTime2, TabooTalking2)) 
            {
                TabooTalking2 = true;
                follow = StartCam;
                Camera.main.fieldOfView = StartFieldOfView;
                Debug.Log(StartCam.transform.position);
            }
            else if (TimeChange(counter,totTime,BackUpTalkTime1,BackUpTalking))
            {
                BackUpTalking = true;
                Debug.Log("BackUp");
                BackUp.GetComponent<Animator>().SetBool("YesSIR", true);
            }
            else if(TimeChange(counter,totTime,BackUpTalkTime2,BackUpTalking2))
            {
                BackUpTalking2 = true;
                ImpFallSpawner.SetActive(true);
            }
        }
    }

    void ActivateTabooPoolali() 
    {
        Taboo.GetComponent<Devil>().WalkOnAwake = true;
        foreach (Animator d in Taboo.GetComponentsInChildren<Animator>())
        {
            d.SetBool("Walking", true);
        }
        follow = Taboo;
        Camera.main.fieldOfView = TabooFieldOfView;
    }

    bool TimeChange(float countC, float countStart, float countEnd, bool StartCheck) 
    {
        if (!StartCheck) 
        {
            if(countC > countStart)
            {
                    if (countC < countEnd + countStart) 
                    {
                        totTime += countEnd;
                        return true;
                    }
            }
        }
        return false;
    }
}
