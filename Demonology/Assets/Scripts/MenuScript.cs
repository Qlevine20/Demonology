﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {
	
	public Button startButton;
	public Button quitButton;
    public Button controlsButton;
    public Button levelsButton;
    public Button creditsButton;
    public GameObject ControlsScreen;
    public GameObject MainMenu;
    public GameObject LevelsScreen;
    public static int levelNum;
    
	// Use this for initialization
	void Start () {
        //RectTransform lvlButt = levelsButton.GetComponent<RectTransform>();
        //RectTransform CredButt = creditsButton.GetComponent<RectTransform>();
        //lvlButt.position = new Vector3(lvlButt.position.x + Screen.width / 5, lvlButt.position.y);
        //CredButt.position = new Vector3(CredButt.position.x - Screen.width / 5, CredButt.position.y);

		startButton = startButton.GetComponent<Button> ();
		quitButton = quitButton.GetComponent<Button> ();
        controlsButton = controlsButton.GetComponent<Button>();
        levelsButton = levelsButton.GetComponent<Button>();
        creditsButton = creditsButton.GetComponent<Button>();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BackToMenu() 
    {
        ControlsScreen.SetActive(false);
        LevelsScreen.SetActive(false);
        startButton.interactable = true;
        quitButton.interactable = true;
        controlsButton.interactable = true;
        levelsButton.interactable = true;
        creditsButton.interactable = true;

    }

    public void ControlsMenuButton() 
    {
        ControlsScreen.SetActive(true);
        startButton.interactable = false;
        quitButton.interactable = false;
        controlsButton.interactable = false;
        levelsButton.interactable = false;
        creditsButton.interactable = false;


    }

    public void LevelsMenuButton() 
    {
        LevelsScreen.SetActive(true);
        startButton.interactable = false;
        quitButton.interactable = false;
        controlsButton.interactable = false;
        levelsButton.interactable = false;
        creditsButton.interactable = false;

    }

    public void CreditsMenuButton() 
    {
        Application.LoadLevel(16);
    }

    public void LoadLevelButton(int level) 
    {
        levelNum = level;
        Application.LoadLevel("LoadingScreen");
    }

	public void StartLevel()
	{
        levelNum = 0;
        Application.LoadLevel(18);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

}
