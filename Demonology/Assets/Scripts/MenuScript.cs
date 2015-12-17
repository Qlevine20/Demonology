using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {
	
	public Button startButton;
	public Button quitButton;
	// Use this for initialization
	void Start () {
		startButton = startButton.GetComponent<Button> ();
		quitButton = quitButton.GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
	
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
