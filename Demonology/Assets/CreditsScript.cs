using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour {

    public float timePerCredit;
    public GameObject[] credits;
    private Color currColor;
    private float counter;
    private int currCredit = 0;
    public float changeAlpha;
    public KeyCode QuitCredits;
    
	// Use this for initialization
	void Start () {
        counter = 0;
        foreach (GameObject credit in credits) 
        {
            currColor = credit.GetComponent<Text>().color;
            currColor.a = 0;
            credit.GetComponent<Text>().color = currColor;
        }
        credits[currCredit].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if(Input.GetMouseButton(0) || Input.GetKey(QuitCredits))
        {
            Application.LoadLevel(0);
        }
        if (currCredit < credits.Length) 
        {
            currColor = credits[currCredit].GetComponent<Text>().color;
            currColor.a += changeAlpha * Time.deltaTime;
            credits[currCredit].GetComponent<Text>().color = currColor;
        }
        if (counter > timePerCredit)
        {
            counter = 0;
            credits[currCredit].SetActive(false);
            currCredit++;
            if (currCredit < credits.Length)
            {
                credits[currCredit].SetActive(true);
            }
            else 
            {
                Application.LoadLevel(0);
            }
        }
	}
}
