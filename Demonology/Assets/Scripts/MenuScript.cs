using UnityEngine;
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
    public GameObject CreditsScreen;
	// Use this for initialization
	void Start () {
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
        CreditsScreen.SetActive(false);
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
        CreditsScreen.SetActive(true);
        startButton.interactable = false;
        quitButton.interactable = false;
        controlsButton.interactable = false;
        levelsButton.interactable = false;
        creditsButton.interactable = false;
    }

    public void LoadLevelButton(int level) 
    {
        Application.LoadLevel(level);
    }

	public void StartLevel()
	{
		Application.LoadLevel (1);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

}
