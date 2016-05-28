using UnityEngine;
using System.Collections;

public class ImpUIScript : MonoBehaviour {

    private RectTransform devilT;
    private bool FRight;
    private RectTransform thisT;
    private float speed = 2;
    public static bool JumpTowardsDevil = false;
	// Use this for initialization
	void Start () {
        devilT = GameObject.FindGameObjectWithTag("Devil").GetComponent<RectTransform>();
        FRight = true;
        thisT = GetComponent<RectTransform>();

	}
	
	// Update is called once per frame
	void Update () {
        if (JumpTowardsDevil) 
        {
            JumpToDevil();
        }
        if (FRight && thisT.transform.position.x - devilT.transform.position.x > 0) 
        {
            FRight = false;
            Vector3 lScale = thisT.localScale;
            lScale.x *= -1;
            thisT.localScale = lScale;

        }
        else if (!FRight && thisT.position.x - devilT.position.x < 0) 
        {
            FRight = true;
            Vector3 lScale = thisT.localScale;
            lScale.x *= -1;
            thisT.localScale = lScale;
        }
	}

    void JumpToDevil() 
    {
        transform.position = Vector3.Lerp(transform.position, devilT.position, speed * Time.deltaTime);
        devilT.GetChild(1).gameObject.SetActive(true);
    }
}
