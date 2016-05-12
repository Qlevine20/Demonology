using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICrystalDisplay : MonoBehaviour
{

    private Text crystalText;
    private int[] playerMats;

    public int crystalId;

    void Start()
    {
        crystalText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DeadlyBehavior.Player)
        {
            playerMats = DeadlyBehavior.Player.GetComponent<CharacterBehavior>().currentMats;
			if (playerMats[crystalId] < 9000) {
				crystalText.text = playerMats [crystalId].ToString ();
			}
			else {
				crystalText.text = "∞";
			}
        }
    }
}