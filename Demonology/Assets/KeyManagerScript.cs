using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class KeyManagerScript : MonoBehaviour {

    public static Dictionary<string, KeyCode> keys = new Dictionary<string,KeyCode>();
    public Text Mod, Jump, Summon, ChangeImp, SummonHand, PushImp, LookDown, KillYourself;
    private GameObject currentKey;
    private Color32 currColor = new Color32(10, 10, 255, 255);
    private Color32 clicked = new Color32(10, 10, 255, 255);
	// Use this for initialization
	void Start () {
        if (keys.Count == 0)
        {
            keys.Add("Mod", KeyCode.LeftShift);
            keys.Add("Jump", KeyCode.W);
            keys.Add("Summon", KeyCode.Q);
            keys.Add("ChangeImp", KeyCode.E);
            keys.Add("SummonHand", KeyCode.Q);
            keys.Add("PushImp", KeyCode.RightShift);
            keys.Add("LookDown", KeyCode.S);
            keys.Add("KillYourself", KeyCode.K);
            currColor = GameObject.Find("Jump").GetComponent<Image>().color;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Jump != null)
        {
            Mod.text = keys["Mod"].ToString();
            Jump.text = keys["Jump"].ToString();
            Summon.text = keys["Summon"].ToString();
            ChangeImp.text = keys["ChangeImp"].ToString();
            SummonHand.text = keys["SummonHand"].ToString();
            PushImp.text = keys["PushImp"].ToString();
            LookDown.text = keys["LookDown"].ToString();
            KillYourself.text = keys["KillYourself"].ToString();
        }
	}

    void OnGUI() 
    {
        if (currentKey != null) 
        {
            Event keyE = Event.current;
            if (keyE.isKey) 
            {
                keys[currentKey.name] = keyE.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = keyE.keyCode.ToString();
                currentKey.GetComponent<Image>().color = currColor;
                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject key) 
    {
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = currColor;
        }
        currentKey = key;
        currColor = key.GetComponent<Image>().color;
        currentKey.GetComponent<Image>().color = clicked;
        
    }
}
