using UnityEngine;
using System.Collections;

public class Mobile : MonoBehaviour {


    public float speed = 1;
    public float wallDist = 1.0f;
    protected int forwardDir = 1;

    public void Movement()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime * forwardDir);
    }
}
