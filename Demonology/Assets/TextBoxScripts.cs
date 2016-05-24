using UnityEngine;
using System.Collections;

public class TextBoxScripts : MonoBehaviour {
    public float StartOffset = 5;
    public float DisplayTime = 5;
    public float NumTextBoxes;
    public float DevilTalkTime1;
    public float TabooTalkTime1;
    public float TabooTalkTime2;
    public float BackUpTalkTime1;
    private float counter = 0;
    private int Tbox = 0;
    private bool Started = false;
    private AudioSource Poololi;
    public GameObject Taboo;
    public GameObject Devil;
    public GameObject StartCam;
    public GameObject BackUp;
    public float TabooFieldOfView;
    public float DevilFieldOfView;
    private GameObject follow;
    private bool DevilTalking = false;
    private bool TabooTalking = false;
    private bool TabooTalking2 = false;
    private bool BackUpTalking = false;
    private float StartFieldOfView;
    
    
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
        if (Tbox < NumTextBoxes - 1 && !Input.GetKey(KeyCode.A))
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
        else if (Poololi.enabled == false)
        {
            transform.GetChild(Tbox).gameObject.SetActive(false);
            Poololi.enabled = true;
            counter = 0;
           

        }
        else 
        {
            //Debug.Log(counter);
            if (counter < DevilTalkTime1 && DevilTalking == false) 
            {
                DevilTalking = true;
                follow = Devil;
                Camera.main.fieldOfView = DevilFieldOfView;
            }
            else if ((counter > DevilTalkTime1 && counter < TabooTalkTime1 + DevilTalkTime1) && TabooTalking == false) 
            {
                TabooTalking = true;
                ActivateTabooPoolali();
            }
            else if ((counter > TabooTalkTime1 + DevilTalkTime1 && counter < (TabooTalkTime2 + TabooTalkTime1 + DevilTalkTime1)) && TabooTalking2 == false) 
            {
                TabooTalking2 = true;
                follow = StartCam;
                Camera.main.fieldOfView = StartFieldOfView;
                Debug.Log(StartCam.transform.position);
                
            }
            else if((counter > TabooTalkTime1 + DevilTalkTime1 + TabooTalkTime2 && counter < (TabooTalkTime1 + DevilTalkTime1 + TabooTalkTime2 + BackUpTalkTime1)) && BackUpTalking == false)
            {
                BackUpTalking = true;
                Debug.Log("BackUp");
                BackUp.GetComponent<Animator>().SetBool("YesSIR", true);
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
}
