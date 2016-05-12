using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {

    public GameObject GameMenu;
    public KeyCode menuKey;
    public GameObject LevelsScreen;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    public void Update()
    {
        if (GameMenu != null && Input.GetKeyUp(menuKey))
        {
            UpdateMenu(GameMenu);
        }
    }


    public void UpdateMenu(GameObject menu)
    {
        if (LevelsScreen.activeSelf)
        {
            LevelsScreen.SetActive(false);
            menu.SetActive(true);
        }
        else
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                menu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void ExitToMainMenu()
    {
        Application.LoadLevel("Main Menu");
        Time.timeScale = 1.0f;
    }

    public void ToLevelsScreen(GameObject menu)
    {
        menu.SetActive(false);
        LevelsScreen.SetActive(true);

    }
    public void ExitLevelsScreen(GameObject menu)
    {
        UpdateMenu(GameMenu);
    }

    public void LevelButton(int level)
    {
        Time.timeScale = 1.0f;
        MenuScript.levelNum = level;
        Application.LoadLevel("LoadingScreen");
    }
}
