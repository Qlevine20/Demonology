using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICrystalDisplay : MonoBehaviour {
	
	public Text crystalText;
	private int[] playerMats;

	// Update is called once per frame
	void Update () {
		if(DeadlyBehavior.Player)
		playerMats = DeadlyBehavior.Player.GetComponent<CharacterBehavior>().currentMats;
		crystalText.text = playerMats[0].ToString();
	}
}