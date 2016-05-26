using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EyeScript : MonoBehaviour
{
	public GameObject BossObject;
    public GameObject RightEye;
    public GameObject LeftEye;
    private Transform Player;
    private Transform LeftPupil;
    private Transform RightPupil;

    public float speed;
    public int radius;

    public int divider;

	private bool bossTurnOn;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        LeftPupil = LeftEye.transform.FindChild("EyePupil");
        RightPupil = RightEye.transform.FindChild("EyePupil");

		ParticleSystem cParts = GetComponent<ParticleSystem> ();
		if (cParts != null) {
			cParts.enableEmission = false;
			cParts.Clear ();
		}
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Player != null)
        {
            //Debug.DrawRay(LeftEye.transform.position, Player.transform.position - (transform.position - LeftEye.transform.position) - LeftEye.transform.position, Color.green);
            //Debug.DrawRay(RightEye.transform.position, Player.transform.position - (transform.position - LeftEye.transform.position) - LeftEye.transform.position, Color.green);


            Vector3 Diff = transform.position - LeftEye.transform.position;
			Vector2 Diffp = new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y);

			Vector3 Lvector = new Vector3(LeftEye.transform.position.x + (Diffp.x / divider), LeftEye.transform.position.y + (Diffp.y / divider), LeftPupil.transform.position.z);
			Vector3 Rvector = new Vector3(RightEye.transform.position.x + (Diffp.x / divider), RightEye.transform.position.y + (Diffp.y / divider), RightEye.transform.position.z);
            //Vector3 Rvector = new Vector3(RightEye.transform.position.x + ((Player.transform.position.x + Diff.x) / divider), (RightEye.transform.position.y + ((Player.transform.position.y))) / divider, RightPupil.transform.position.z);


            if (Vector3.Distance(LeftEye.transform.position, Lvector) * .9f < radius)
            {
                LeftPupil.transform.position = Vector3.Lerp(LeftPupil.transform.position, Lvector, speed * Time.deltaTime);
                RightPupil.transform.position = Vector3.Lerp(RightPupil.transform.position, Rvector, speed * Time.deltaTime);
            }
        }
        else
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }


		if (gameObject.name != "Boss" && GameObject.FindGameObjectWithTag ("PowerCrystal") == null) {
			//GameObject.Find ("Boss").SetActive(true);
			if (BossObject != null && !bossTurnOn) {
				StartCoroutine (SwitchToBoss ());
				bossTurnOn = true;
			}
		} else if (gameObject.name != "Boss"){
			if (LeftEye.activeSelf != true) {
				LeftEye.SetActive (true);
				LeftEye.GetComponent<FadeObjectInOut> ().FadeIn(0f);
				ParticleSystem cParts = LeftEye.GetComponent<ParticleSystem> ();
				cParts.enableEmission = true;
				Color newColor = cParts.startColor; newColor.a = 255f; cParts.startColor = newColor;
				cParts.Simulate (10.0f);
				cParts.Play ();
			}
			if (RightEye.activeSelf != true) {
				RightEye.SetActive (true);
				RightEye.GetComponent<FadeObjectInOut> ().FadeIn(0f);
				ParticleSystem cParts = RightEye.GetComponent<ParticleSystem> ();
				cParts.enableEmission = true;
				Color newColor = cParts.startColor; newColor.a = 255f; cParts.startColor = newColor;
				cParts.Simulate (10.0f);
				cParts.Play ();
			}
			//if ( GetComponent<ParticleSystem> ().enableEmission == true )
			//GetComponent<ParticleSystem> ().enableEmission = false;
		}
    }

	public IEnumerator SwitchToBoss()
	{
		ParticleSystem cParts = GetComponent<ParticleSystem> ();
		cParts.enableEmission = true;
		GetComponent<FadeObjectInOut> ().FadeIn(0f);
		Color newColor = cParts.startColor; newColor.a = 255f; cParts.startColor = newColor;

		LeftEye.GetComponent<ParticleSystem> ().enableEmission = false;
		RightEye.GetComponent<ParticleSystem> ().enableEmission = false;
		//yield return new WaitForSeconds (1f);
		LeftEye.GetComponent<FadeObjectInOut> ().FadeOut (2.0f);
		RightEye.GetComponent<FadeObjectInOut> ().FadeOut (2.0f);
		yield return new WaitForSeconds (2f);
		BossObject.SetActive (true);
		LeftEye.SetActive (false);
		RightEye.SetActive (false);
		GetComponent<FadeObjectInOut> ().FadeOut (1.0f);
		yield return new WaitForSeconds (1f);
		bossTurnOn = false;
		gameObject.SetActive (false);
	}
}
