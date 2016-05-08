using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour
{
    
    public GameObject PowerCrystal;
    //public GameObject Exit;
	private ParticleSystem cParts;
    private ParticleSystem.EmissionModule em;

    // Use this for initialization
    void Start()
    {
		cParts = GetComponent<ParticleSystem>();
        em = cParts.emission;
		em.enabled = false;
		transform.gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {
        PowerCrystal = GameObject.FindGameObjectWithTag("PowerCrystal");
        
		//once no more crystals, activate the exit
        if (PowerCrystal == null) {
			transform.gameObject.tag = "Finish";
			em.enabled = true;
		} else {
			transform.gameObject.tag = "Untagged";
			em.enabled = false;
		}

    }
}
