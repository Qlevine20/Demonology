using UnityEngine;
using System.Collections;

public class LevelReScale : MonoBehaviour {

    public GameObject[] Buttons;
    
	// Use this for initialization
	void Start () {
        int levelNum = 0;
        foreach (GameObject b in Buttons) 
        {
            levelNum++;
            //RectTransform buttonRect = b.GetComponent<RectTransform>();
            ////buttonRect.localScale = new Vector3(buttonRect.localScale.x * Screen.width / buttonRect.rect.width/Buttons.Length,buttonRect.localScale.y * Screen.height / buttonRect.rect.height/Buttons.Length, 1);
            //if (currC > columnSize-1) 
            //{
            //    currC = 0;
            //    currR--;
            //}
            //if (currC < (columnSize)/2)
            //{
            //    buttonRect.anchoredPosition = new Vector2(-buttonRect.rect.width * (((columnSize)/2) - currC) + Screen.width/columnSize, (buttonRect.rect.height * currR) - Screen.height/rowSize);
            //}
            //else if (currC == (columnSize-1) / 2.0f)
            //{
                
            //    buttonRect.anchoredPosition = new Vector2(0, buttonRect.rect.height * currR);
            //}
            //else
            //{
                
            //    buttonRect.anchoredPosition = new Vector2(buttonRect.rect.width * (currC - (columnSize/2)) + Screen.width/columnSize, (buttonRect.rect.height * currR) - Screen.height/rowSize);
            //}
            if (PersistantBehavoir.levelsEarned < levelNum) 
            {
                b.SetActive(false);
            }


            //currC++;
            
        }
	
	}
	
	// Update is called once per frame

}

