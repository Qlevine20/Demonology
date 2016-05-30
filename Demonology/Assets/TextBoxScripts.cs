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
    public float BackUpTalkTime3;
    public float EndOfScene;
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
    private bool BackUpTalking3 = false;
    private bool FinishScene = false;
    private float StartFieldOfView;
    
    private int currBool = 0;
    //private bool[] boolL;
    
    
	// Use this for initialization
	void Start () {
        //boolL = new bool[7] { DevilTalking, TabooTalking, TabooTalking2, BackUpTalking, BackUpTalking2, BackUpTalking3, FinishScene };
        Poololi = Camera.main.GetComponent<AudioSource>();
        follow = StartCam;
        StartFieldOfView = Camera.main.fieldOfView;
        
	}
	
	// Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        Camera.main.transform.position = new Vector3(follow.transform.position.x, follow.transform.position.y + 20.0f, Camera.main.transform.position.z);
        if (Tbox < NumTextBoxes-1 && !Input.GetKey(KeyCode.A) && Poololi.enabled == false)
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
        else if (Poololi.enabled == true)
        {
            //Debug.Log(counter);
            if (counter < DevilTalkTime1 && DevilTalking == false)
            {
                GetComponent<AudioSource>().enabled = false;
                DevilTalking = true;
                follow = Devil;
                Camera.main.fieldOfView = DevilFieldOfView;
                counter = 0;

            }
            else if (TimeChange(counter, TabooTalkTime1, TabooTalking) && currBool == 0)
            {

                TabooTalking = true;
                currBool++;
                counter = 0;
                ActivateTabooPoolali();
            }
            else if (TimeChange(counter, TabooTalkTime2, TabooTalking2) && currBool == 1)
            {
                TabooTalking2 = true;
                currBool++;
                counter = 0;
                follow = StartCam;
                Camera.main.fieldOfView = StartFieldOfView;
            }
            else if (TimeChange(counter,  BackUpTalkTime1, BackUpTalking) && currBool == 2)
            {
                BackUpTalking = true;
                currBool++;
                counter = 0;
                BackUp.GetComponent<Animator>().SetBool("YesSIR", true);
            }
            else if (TimeChange(counter, BackUpTalkTime2, BackUpTalking2) && currBool == 3)
            {
                BackUpTalking2 = true;
                counter = 0;
                currBool++;
                ImpFallSpawner.SetActive(true);
                BackUp.GetComponent<Animator>().SetBool("Raining", true);
            }
            else if (TimeChange(counter,  BackUpTalkTime3, BackUpTalking3) && currBool == 4)
            {
                BackUpTalking3 = true;
                currBool++;
                counter = 0;
                ImpFallSpawner.GetComponent<FallingSpawner>().Shake = true;
                ImpUIScript.JumpTowardsDevil = true;
            }
            else if (TimeChange(counter,EndOfScene, FinishScene) && currBool == 5)
            {
                FinishScene = true;
                MenuScript.levelNum = Application.loadedLevel + 1;
                Application.LoadLevel("LoadingScreen");
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

    bool TimeChange(float countC, float countEnd, bool StartCheck) 
    {
        if (!StartCheck) 
        {

            if (countC > countEnd) 
            {
                return true;
            }
        }
        return false;
    }
}
