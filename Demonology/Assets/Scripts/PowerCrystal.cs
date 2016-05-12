using UnityEngine;
using System.Collections;

public class PowerCrystal : EnemyBehavior
{
	public int baubleNumber = 0;
	public GameObject PartEffect;
	public GameObject bauble;
	Animator anim;
	public AudioClip shatterSound;
	public GameObject impFaller;

	public override void Start()
	{
		base.Start ();
		switch (baubleNumber) {
		case 0:
			bauble = GameObject.Find("Bauble (0)");
			break;
		case 1:
			bauble = GameObject.Find("Bauble (1)");
			break;
		case 2:
			bauble = GameObject.Find("Bauble (2)");
			break;
		case 3:
			bauble = GameObject.Find("Bauble (3)");
			break;
		case 4:
			bauble = GameObject.Find("Bauble (4)");
			break;
		case 5:
			bauble = GameObject.Find("Bauble (5)");
			break;
		default:
			bauble = GameObject.Find("Bauble (0)");
			break;
		}
		StartCoroutine (PhaseOn ((baubleNumber+1) * 0.5f));
		//bauble.GetComponent<Animator> ().SetInteger ("ActiveColor", 1);
	}

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Instantiate(PartEffect, transform.position,Quaternion.identity);
            OnDeath();
        }

		if (other.gameObject.tag == "imp" && !other.gameObject.GetComponent<ImpAI>().dead)
		{
			Instantiate(PartEffect, transform.position,Quaternion.identity);
			OnDeath();
		}
	}

	public override void OnDeath ()
	{
		base.OnDeath ();
		AudioSource.PlayClipAtPoint(shatterSound, Camera.main.transform.position);
		bauble.GetComponent<Animator> ().SetInteger ("ActiveColor", 2);

		if (GetComponent<CrystalScript> () != null) {
			int[] newMats = GetComponent<CrystalScript> ().newMats;
			CharacterBehavior player = GameObject.Find("Character").GetComponent<CharacterBehavior>();;
			for (int i=0; i<player.currentMats.Length; i++) {
				player.currentMats [i] += newMats [i];
			}
			if(impFaller != null) {
				impFaller.SetActive(true);
			}
			//transform.GetChild(0).parent = null;
		}
	}

	public override void OnRespawn ()
	{
		base.OnRespawn ();
		bauble.GetComponent<Animator> ().SetInteger ("ActiveColor", 1);
		if(impFaller != null) {
			impFaller.SetActive(false);
		}
	}

	public IEnumerator PhaseOn(float num)
	{
		yield return new WaitForSeconds (num-0.02f);
		bauble.transform.GetChild (0).gameObject.SetActive (true);
		yield return new WaitForSeconds (0.02f);
		bauble.GetComponent<Animator> ().SetInteger ("ActiveColor", 1);
	}
}
