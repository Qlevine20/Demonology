using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICrystalDisplay : MonoBehaviour {
	
	private Text crystalText;
	private int[] playerMats;
	public GameObject GameMenu;
	public KeyCode menuKey;
	public int crystalId;

	void Start () {
		crystalText = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		if(DeadlyBehavior.Player)
		playerMats = DeadlyBehavior.Player.GetComponent<CharacterBehavior>().currentMats;
		crystalText.text = playerMats[crystalId].ToString();

		if (GameMenu != null && Input.GetKeyUp (menuKey)) 
		{
			UpdateMenu (GameMenu);
		}
	}

	public void UpdateMenu(GameObject menu)
	{
		if(menu.activeSelf)
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

	public void ExitToMainMenu()
	{
		Application.LoadLevel ("Main Menu");
		Time.timeScale = 1.0f;
	}
}