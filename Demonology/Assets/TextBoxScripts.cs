using UnityEngine;
using System.Collections;

public class TextBoxScripts : MonoBehaviour {
    public float StartOffset = 5;
    public float DisplayTime = 5;
    public float NumTextBoxes;
    private float counter = 0;
    private int Tbox = 0;
    private bool Started = false;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Tbox < NumTextBoxes - 1)
        {
            counter += Time.deltaTime;
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
    }
}
