using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EyeScript : MonoBehaviour {

    public GameObject RightEye;
    public GameObject LeftEye;
    private Transform Player;
    private Transform LeftPupil;
    private Transform RightPupil;
    private Vector3 LeftStart;
    private Vector3 RightStart;
    public float speed;
    public float mult;
    private List<Vector2> CollPointsLeft;
    private List<Vector2> CollPointsRight;
    private int LeftIndex;
    private int RightIndex;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        LeftPupil = LeftEye.transform.FindChild("EyePupil");
        RightPupil = RightEye.transform.FindChild("EyePupil");
        LeftStart = LeftPupil.position;
        RightStart = RightPupil.position;
        
        
        CollPointsLeft = new List<Vector2>(LeftEye.GetComponentInChildren<PolygonCollider2D>().points);
        CollPointsRight = new List<Vector2>(RightEye.GetComponentInChildren<PolygonCollider2D>().points);
        CollPointsLeft.Sort(delegate(Vector2 a, Vector2 b)
            {
                return (a.x).CompareTo(b.x);
            });
        CollPointsRight.Sort(delegate(Vector2 a, Vector2 b)
        {
            return (a.x).CompareTo(b.x);
        });


        foreach (Vector2 point in CollPointsLeft) 
        {
         //   Debug.Log(point);
        }

        LeftIndex = FindClosestIndexStart(CollPointsLeft);
        RightIndex = FindClosestIndexStart(CollPointsRight);


        Vector2 LeftChange = CollPointsLeft[LeftIndex];
        Debug.Log(LeftChange);
        Vector3 NewLeftPos = new Vector3(LeftChange.x * LeftEye.transform.localScale.x + LeftStart.x, LeftChange.y * LeftEye.transform.localScale.y + LeftStart.y, LeftPupil.transform.position.z);
        LeftPupil.transform.position = Vector3.Lerp(LeftPupil.transform.position, NewLeftPos, speed * Time.deltaTime);



        Vector2 RightChange = CollPointsRight[RightIndex];
        Vector3 NewRightPos = new Vector3(RightChange.x * RightEye.transform.localScale.x + RightStart.x, RightChange.y * RightEye.transform.localScale.y + RightStart.y, RightPupil.transform.position.z);
        RightPupil.transform.position = Vector3.Lerp(RightPupil.transform.position, NewRightPos, speed * Time.deltaTime);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Player != null)
        {

            Vector2 LeftChange = FindClosestPoint(CollPointsLeft, LeftIndex, true);
            //Debug.Log(LeftChange);

            Vector3 NewLeftPos = new Vector3(LeftChange.x * LeftEye.transform.localScale.x + LeftStart.x, LeftChange.y * LeftEye.transform.localScale.y + LeftStart.y, LeftPupil.transform.position.z);

            //Debug.Log(NewLeftPos);
            LeftPupil.transform.position = Vector3.Lerp(LeftPupil.transform.position, NewLeftPos, speed * Time.deltaTime);

            //Debug.Log(RightIndex);
            Vector2 RightChange = FindClosestPoint(CollPointsRight, RightIndex, false);

            Vector3 NewRightPos = new Vector3(RightChange.x * RightEye.transform.localScale.x + RightStart.x, RightChange.y * RightEye.transform.localScale.y + RightStart.y, RightPupil.transform.position.z);

            RightPupil.transform.position = Vector3.Lerp(RightPupil.transform.position, NewRightPos, speed * Time.deltaTime);
        }
        else 
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
	}


    //public float Offset(bool left)
    //{
    //    if (left)
    //    {
    //        return -(transform.position.x - Player.transform.position.x) / (transform.position.z / speed);
    //    }
    //    return -(transform.position.x - Player.transform.position.x) / (transform.position.z / speed);
    //}

    public Vector2 FindClosestPoint(List<Vector2> CollPoints, int indexLoc, bool left) 
    {

        Vector2 minVec = CollPoints[indexLoc];

        if (indexLoc + 1 < CollPoints.Count && Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.y), new Vector2(CollPoints[indexLoc + 1].x + LeftStart.x, CollPoints[indexLoc + 1].y + LeftStart.y)) < Vector2.Distance(Player.transform.position, new Vector2(minVec.x + LeftStart.x, minVec.y + LeftStart.y)))
        {

            if (left)
            {
                LeftIndex++;
            }
            else
            {
                RightIndex++;
            }
            //Debug.Log("Change");
            return CollPoints[indexLoc + 1];
        }
        if (indexLoc - 1 >= 0 && Vector2.Distance(new Vector2(Player.transform.position.x/mult,Player.transform.position.y), new Vector2(CollPoints[indexLoc - 1].x + LeftStart.x, CollPoints[indexLoc - 1].y + LeftStart.y)) < Vector2.Distance(Player.transform.position/mult, new Vector2(minVec.x + LeftStart.x, minVec.y + LeftStart.y)))
        {

            if (left)
            {
                LeftIndex--;
            }
            else
            {
                RightIndex--;
            }
            //Debug.Log("Change");
            return CollPoints[indexLoc - 1];
        }
        return minVec;


    }


    public int FindClosestIndexStart(List<Vector2> CollPoints) 
    {
        int minIndex = 0;
        int count = 0;
        foreach (Vector2 point in CollPoints)
        {
            //Debug.Log(point);
            if (Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.y), new Vector2((point.x) * mult + LeftStart.x, point.y * mult + LeftStart.y)) < Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.y), new Vector2(CollPoints[minIndex].x * mult + LeftStart.x, CollPoints[minIndex].y * mult + LeftStart.y)))
            {
                minIndex = count;
            }
            count++;
        }
        //Debug.Log(minIndex);
        return minIndex;
    }
}
