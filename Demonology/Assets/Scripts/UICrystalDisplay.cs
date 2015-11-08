using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICrystalDisplay : MonoBehaviour {
	
	public Text crystalText;
	public GameObject Player;
	private int[] playerMats;

	// Update is called once per frame
	void Update () {
		playerMats = Player.GetComponent<CharacterBehavior>().currentMats;
		crystalText.text = playerMats[0].ToString();
		print (playerMats [0]);
	}
}